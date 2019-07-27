using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DllCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            Work();
        }

        static void Work()
        {
            // アセンブリバージョンを取得
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var csFile = Directory
                .EnumerateFiles(baseDirectory, "*.cs", SearchOption.AllDirectories)
                .FirstOrDefault(x => x.EndsWith("AssemblyInfo.cs"));

            var line = File
                .ReadAllLines(csFile, new UTF8Encoding(true))
                .FirstOrDefault(x => x.StartsWith("[assembly: AssemblyVersion("));

            // ライブラリ名とアセンブリバージョン名を合体
            var value = line.Replace(@"[assembly: AssemblyVersion(""", string.Empty).Replace(@""")]", string.Empty);
            value = $"ObjectVisualization_{value}";

            var dllDirectory = Path.Combine(baseDirectory, value);
            if (Directory.Exists(dllDirectory))
                Directory.Delete(dllDirectory, true);
            
            Directory.CreateDirectory(dllDirectory);

            var dirs = Directory
                .EnumerateDirectories(baseDirectory, "*", SearchOption.AllDirectories)
                .Where(x => x.EndsWith(@"\bin\Debug"))
                .Where(x => !x.Contains(@"\BuildTools\"))
                .ToList();

            dirs.ForEach(x => CopyDirectory(baseDirectory, x, dllDirectory));
        }

        static void CopyDirectory(string baseDirectory, string sourceDirectory, string destDirectory, bool isFirstCall = true)
        {
            if (isFirstCall)
            {
                var dirName = sourceDirectory.Replace(baseDirectory, string.Empty);
                dirName = dirName.Substring(0, dirName.IndexOf('\\'));
                destDirectory = Path.Combine(destDirectory, dirName);
            }

            if (!Directory.Exists(destDirectory))
                Directory.CreateDirectory(destDirectory);

            // *.dll, *.pdb を想定
            var anyFiles = Directory.EnumerateFiles(sourceDirectory, "*.*");
            foreach (var anyFile in anyFiles)
            {
                var destName = new FileInfo(anyFile).Name;
                var destFile = Path.Combine(destDirectory, destName);
                File.Copy(anyFile, destFile);
            }

            // サブフォルダもコピー
            var subDirs = Directory
                .EnumerateDirectories(sourceDirectory, "*", SearchOption.AllDirectories)
                .ToList();

            if (0 < subDirs.Count)
            {
                foreach (var subDir in subDirs)
                {
                    var dirName = new DirectoryInfo(subDir).Name;
                    var dirPath = Path.Combine(destDirectory, dirName);
                    CopyDirectory(baseDirectory, subDir, dirPath, false);
                }
            }
            

        }
    }
}
