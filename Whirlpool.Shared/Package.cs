using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Whirlpool.Shared
{

    public class PackageFile
    {
        public string sourceFileName;
        public string fileName;
        public byte[] fileData;
        public DateTime modified;
    }

    public class Package
    {
        public List<PackageFile> files;

        /// <summary>
        /// Create an empty package.
        /// </summary>
        public Package()
        {
            files = new List<PackageFile>();
        }


        /// <summary>
        /// Load an existing package from a path.
        /// </summary>
        /// <param name="location">The location of the package.</param>
        public Package(string location)
        {
            using (StreamReader sr = new StreamReader(location))
            {
                byte[] buf = new byte[sr.BaseStream.Length];
                sr.BaseStream.Read(buf, 0, (int)sr.BaseStream.Length);

                InitPackageFromData(buf);
            }
        }

        /// <summary>
        /// Load a package using a buffer.
        /// </summary>
        /// <param name="data">The buffer to load from.</param>
        public Package(byte[] data)
        {
            InitPackageFromData(data);
        }

        private void InitPackageFromData(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryReader br = new BinaryReader(ms);
            char[] magicNumber = new char[] { 'W', 'P', 'A', 'K' };
            for (int i = 0; i < 4; ++i)
            {
                if (br.ReadChar() != magicNumber[i])
                    throw new Exception("Magic number was not met!");
            }


            files = new List<PackageFile>();

            int numFiles = br.ReadInt32();
            bool filesCompressed = br.ReadBoolean();

            for (int i = 0; i < numFiles; ++i)
            {
                PackageFile file = new PackageFile();
                file.modified = new DateTime(br.ReadInt64());
                int fileNameLength = br.ReadInt32();
                byte[] fileNameB = br.ReadBytes(fileNameLength);
                file.fileName = Encoding.ASCII.GetString(fileNameB);
                int srcFileNameLength = br.ReadInt32();
                byte[] srcFileNameB = br.ReadBytes(srcFileNameLength);
                file.sourceFileName = Encoding.ASCII.GetString(srcFileNameB);
                int fileContentLength = br.ReadInt32();
                file.fileData = br.ReadBytes(fileContentLength);
                files.Add(file);
            }
            br.Close();
            ms.Close();
        }


        /// <summary>
        /// Adds a file to the package.
        /// </summary>
        /// <param name="fileName">The filename to use inside the package.</param>
        /// <param name="filePath">The location of the file on disk.</param>
        public void AddFile(string fileName, string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                byte[] buf = new byte[sr.BaseStream.Length];
                sr.BaseStream.Read(buf, 0, (int)sr.BaseStream.Length);

                AddData(filePath, fileName, buf, System.IO.File.GetLastWriteTimeUtc(filePath));
            }
        }

        public void AddFile(string sourceFileName, string fileName, string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                byte[] buf = new byte[sr.BaseStream.Length];
                sr.BaseStream.Read(buf, 0, (int)sr.BaseStream.Length);

                AddData(sourceFileName, fileName, buf, System.IO.File.GetLastWriteTimeUtc(filePath));
            }
        }

        public void AddData(string sourceFileName, string fileName, MemoryStream memoryStream, DateTime modified)
        {
            byte[] buf = new byte[memoryStream.Length];
            memoryStream.Read(buf, 0, (int)memoryStream.Length);
            AddData(sourceFileName, fileName, buf, modified);
        }

        public void AddData(string sourceFileName, string fileName, byte[] data, DateTime modified)
        {
            // Check if file already exists
            var existingFile = files.Find(item => item.fileName == fileName);
            if (existingFile != null)
            {
                files.Remove(existingFile);
            }
            files.Add(new PackageFile()
            {
                sourceFileName = sourceFileName,
                fileName = fileName,
                fileData = data,
                modified = modified
            });
        }

        /// <summary>
        /// Save the contents of a package to a destination.
        /// </summary>
        /// <param name="destination">The desination to save to.</param>
        public void Save(string destination)
        {
            StreamWriter sr = new StreamWriter(destination);
            WriteBytes(ref sr, "WPAK");
            WriteBytes(ref sr, files.Count);
            WriteBytes(ref sr, false);

            foreach (PackageFile file in files)
            {
                WriteBytes(ref sr, file.modified.Ticks);
                WriteBytes(ref sr, file.fileName.Length);
                WriteBytes(ref sr, file.fileName);
                WriteBytes(ref sr, file.sourceFileName.Length);
                WriteBytes(ref sr, file.sourceFileName);
                WriteBytes(ref sr, file.fileData.Length);
                WriteBytes(ref sr, file.fileData);
            }
            sr.Close();
        }

        /// <summary>
        /// Unpack all files
        /// </summary>
        /// <param name="destination"></param>
        public void Extract(string destination)
        {
            foreach (PackageFile file in files)
            {
                string destFilePath = destination + file.fileName;
                string fileDir = Path.GetDirectoryName(destFilePath);
                if (!Directory.Exists(fileDir))
                    Directory.CreateDirectory(fileDir);
                using (StreamWriter sw = new StreamWriter(destFilePath))
                    sw.BaseStream.Write(file.fileData, 0, file.fileData.Length);
            }
        }

        private void WriteBytes(ref StreamWriter sr, string content) => WriteBytes(ref sr, Encoding.ASCII.GetBytes(content));
        private void WriteBytes(ref StreamWriter sr, int content) => WriteBytes(ref sr, BitConverter.GetBytes(content));
        private void WriteBytes(ref StreamWriter sr, bool content) => WriteBytes(ref sr, BitConverter.GetBytes(content));
        private void WriteBytes(ref StreamWriter sr, long content) => WriteBytes(ref sr, BitConverter.GetBytes(content));

        private void WriteBytes(ref StreamWriter sr, byte[] content)
        {
            sr.BaseStream.Write(content, 0, content.Length);
        }
    }
}
