using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyVersionUp
{
    class Program
    {
        static void Main(string[] args)
        {
            // 指定無しの場合は、ビルドバージョンを上げる
            var item = new BuildInfo() { Types = VersionTypes.Build };
            
            for (var i = 0; i < args.Length; i++)
            {
                var key = args[0].ToLower();
                if (key == "-folder" || key == "/folder")
                {
                    var folder = args[1];
                    if (Directory.Exists(folder))
                        item.CurrentDirectory = folder;
                }
                else if (key == "-majar" || key == "/majar")
                {
                    item.Types = VersionTypes.Majar;
                }
                else if (key == "-minor" || key == "/minor")
                {
                    item.Types = VersionTypes.Minor;
                }
                else if (key == "-build" || key == "/build")
                {
                    item.Types = VersionTypes.Build;
                }
                else if (key == "-revision" || key == "/revision")
                {
                    item.Types = VersionTypes.Revision;
                }
                else if (key == "-reset" || key == "/reset")
                {
                    item.Types = VersionTypes.Reset;
                }
            }

            // パスの指定が無い場合は、本exe のディレクトリ以下にある AssemblyInfo.cs を対象にする
            if (string.IsNullOrEmpty(item.CurrentDirectory))
                item.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            Work(item);
        }

        static void Work(BuildInfo item)
        {
            var csFiles = Directory
                .EnumerateFiles(item.CurrentDirectory, "*.cs", SearchOption.AllDirectories)
                .Where(x => x.EndsWith("AssemblyInfo.cs"))
                .ToList();

            foreach (var csFile in csFiles)
            {
                var fi = new FileInfo(csFile);
                Console.WriteLine($"{fi.Directory.Name}/{fi.Name}");

                var sb = new StringBuilder();
                var lines = File.ReadLines(csFile, new UTF8Encoding(true));
                foreach (var line in lines)
                {
                    // [assembly: AssemblyVersion("1.0.0.0")]
                    if (line.StartsWith($"[assembly: AssemblyVersion("))
                    {
                        var tag = "AssemblyVersion";
                        var value = VersionUp(item, line, tag);
                        sb.AppendLine(value);
                        continue;
                    }

                    // [assembly: AssemblyFileVersion("1.0.0.0")]
                    if (line.StartsWith("[assembly: AssemblyFileVersion("))
                    {
                        var tag = "AssemblyFileVersion";
                        var value = VersionUp(item, line, tag);
                        sb.AppendLine(value);
                        continue;
                    }

                    sb.AppendLine(line);
                }

                File.SetAttributes(csFile, FileAttributes.Normal);
                File.WriteAllText(csFile, sb.ToString(), new UTF8Encoding(true));
            }
        }

        static string VersionUp(BuildInfo item, string line, string tag)
        {
            Console.WriteLine($"before: {line}");            
            var value = line.Replace($@"[assembly: {tag}(""", string.Empty).Replace(@""")]", string.Empty);

            if (item.Types == VersionTypes.Majar)
            {
                var splits = value.Split('.');
                splits[0] = (int.Parse(splits[0]) + 1).ToString();
                splits[1] = "0";
                splits[2] = "0";
                splits[3] = "0";
                value = string.Join(".", splits);
            }
            else if (item.Types == VersionTypes.Minor)
            {
                var splits = value.Split('.');
                splits[1] = (int.Parse(splits[1]) + 1).ToString();
                splits[2] = "0";
                splits[3] = "0";
                value = string.Join(".", splits);
            }
            else if (item.Types == VersionTypes.Build)
            {
                var splits = value.Split('.');
                splits[2] = (int.Parse(splits[2]) + 1).ToString();
                splits[3] = "0";
                value = string.Join(".", splits);
            }
            else if (item.Types == VersionTypes.Revision)
            {
                var splits = value.Split('.');
                splits[3] = (int.Parse(splits[3]) + 1).ToString();
                value = string.Join(".", splits);
            }
            else if (item.Types == VersionTypes.Reset)
            {
                var splits = value.Split('.');
                splits[0] = "1";
                splits[1] = "0";
                splits[2] = "0";
                splits[3] = "0";
                value = string.Join(".", splits);
            }

            value = $@"[assembly: {tag}(""{value}"")]";
            Console.WriteLine($"after:  {value}");

            return value;
        }
    }

    enum VersionTypes
    {
        Majar,
        Minor,
        Build,
        Revision,
        Reset
    }
    
    class BuildInfo
    {
        public string CurrentDirectory;
        public VersionTypes Types;
    }
}
