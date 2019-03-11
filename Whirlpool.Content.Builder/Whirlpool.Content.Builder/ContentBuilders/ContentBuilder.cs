using System;
using System.IO;
using System.Linq;
using Whirlpool.Shared;

namespace Whirlpool.Content.Builder
{
    public enum LogStatus
    {
        Warning,
        Error,
        General,
        Success
    }

    public class ContentBuilder : IDisposable
    {
        bool verbose = false;
        string directory;
        public Package package;
        public ContentBuilder(string directory, bool verbose)
        {
            this.verbose = verbose;
            this.directory = directory;
            package = new Package();
        }

        public void Log(string str, LogStatus status = LogStatus.General)
        {
            switch (status)
            {
                case LogStatus.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogStatus.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogStatus.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
            if (verbose)
                Console.WriteLine("[" + status.ToString() + "] " + str);

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Compile(bool firstTimeCompilation, bool forceRebuild)
        {
            if (firstTimeCompilation)
            {
                Log("Package didn't already exist; compiling all files", LogStatus.Error);
            }
            else
            {
                Log("Package exists", LogStatus.Success);
                package = new Package(directory + ".wpak");
            }

            CompileDirectory(directory, firstTimeCompilation, forceRebuild);
            if (!firstTimeCompilation)
            {
                // Check against all files in directory; if packagemodified is before currentmodified, recompile the file
                var directoryFiles = Directory.GetFiles(directory);
                if (directoryFiles.Length != package.files.Count)
                {
                    int deleteIndex = 0;
                    // Find any files that were deleted
                    PackageFile[] files = new PackageFile[package.files.Count];
                    package.files.CopyTo(files);
                    foreach (PackageFile file in files)
                    {
                        if (!System.IO.File.Exists(file.sourceFileName))
                        {
                            Log("Removed file " + file.fileName + " (" + file.sourceFileName + ")", LogStatus.Warning);
                            if (deleteIndex > 0 && deleteIndex < package.files.Count)
                                package.files.RemoveAt(deleteIndex);
                            else
                                Log("Couldn't remove the file - was it in the package?", LogStatus.Error);
                        }
                        deleteIndex++;
                    }
                }
            }
        }

        private ContentType GetFileContentType(string fileName)
        {
            var s = fileName.Split('.').Last().ToLower();
            switch (s)
            {
                case "glsl":
                    return ContentType.Shader;
                case "ttf":
                    return ContentType.Font;
                default:
                    return ContentType.Misc;
            }
        }

        private void CompileFile(string fileName, bool firstTimeCompilation)
        {
            GetFileContentType(fileName).buildClass.Build(ref package, fileName, fileName.Remove(0, directory.Length));
        }

        private void CompileDirectory(string directoryName, bool firstTimeCompilation, bool forceRebuild)
        {
            foreach (string file in Directory.GetFiles(directoryName))
            {
                Log("Compiling " + file);
                if (firstTimeCompilation)
                    CompileFile(file, firstTimeCompilation);
                else
                {

                    var existingFile = package.files.Find(item => item.fileName == file.Remove(0, directory.Length));
                    if (file.EndsWith(".ttf"))
                    {
                        // compiled font file, check again
                        existingFile = package.files.Find(item => item.fileName == file.Remove(0, directory.Length) + ".png");
                    }
                    if (existingFile != null)
                    {
                        if (existingFile.modified.Ticks < System.IO.File.GetLastWriteTimeUtc(file).Ticks && !forceRebuild)
                        {
                            Log("File " + file + " was modified; recompiling", LogStatus.Success);
                            CompileFile(file, firstTimeCompilation);
                        }
                        else
                        {
                            Log("Skipping unmodified file " + file, LogStatus.Success);
                        }
                    }
                    else
                    {
                        // The package didn't contain the file
                        Log("New file " + file, LogStatus.Success);
                        CompileFile(file, firstTimeCompilation);
                    }
                }
            }
            foreach (string subDirectory in Directory.GetDirectories(directoryName))
                CompileDirectory(subDirectory, firstTimeCompilation, forceRebuild);
        }

        public void Dispose()
        {
            package.Save(directory + ".wpak");
        }
    }
}
