using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Whirlpool.Shared;

namespace Whirlpool.Content.Builder
{
    class Font
    {
        public int charSize;
        public int lowestChar;
        public int highestChar;
        public int tile;
    }

    public class FontBuilder : IContentBuilderTypeCompiler
    {
        public void Build(ref Package package, string fileName, string internalName) // TODO: font metadata in .wfnt file, export metadata
        {
            // initiate font build
            Font font = new Font();
            font.charSize = 128;
            var tempDir = Path.GetRandomFileName();
            Directory.CreateDirectory(tempDir);

            font.lowestChar = 0x0020;
            font.highestChar = 0x017F;
            for (int i = font.lowestChar; i < font.highestChar; ++i)
            {
                // Build character
                Process charProcess = new Process();
                string args = @"-range 1 -size " + font.charSize + " " + font.charSize + " -autoframe -font " + fileName + " " + i + " -o " + tempDir + "\\" + i + ".png";
                charProcess.StartInfo = new ProcessStartInfo("msdfgen.exe", args);
                charProcess.StartInfo.CreateNoWindow = true;
                charProcess.StartInfo.UseShellExecute = false;
                charProcess.Start();
            }

            // Concatenate characters
            string concatCommand = "";
            for (int i = font.lowestChar; i < font.highestChar; ++i)
            {
                concatCommand += tempDir + "\\" + i + ".png ";
            }
            var result = tempDir + "\\result.png";
            font.tile = (int)Math.Ceiling(Math.Sqrt(font.highestChar - font.lowestChar));
            Process process = new Process();
            var procArgs = concatCommand + " -tile " + font.tile + "x -geometry +0+0 " + result;
            process.StartInfo = new ProcessStartInfo("montage.exe", procArgs);
            process.Start();
            process.WaitForExit();
            package.AddFile(fileName, internalName + ".png", result);

            List<byte> buffer = new List<byte>();
            var serializedFont = JsonConvert.SerializeObject(font);
            var text = Encoding.ASCII.GetBytes(serializedFont);
            foreach (byte b in text)
                buffer.Add(b);
            package.AddData(fileName, internalName + ".json", buffer.ToArray(), DateTime.Now);
            Directory.Delete(tempDir, recursive: true);
        }
    }
}
