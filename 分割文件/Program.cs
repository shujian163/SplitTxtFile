using System.Diagnostics;
using System.Text;

namespace 分割文件
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("============================================");
            Console.WriteLine("  版权信息: 本程序由数字星球提供");
            Console.WriteLine("  访问我们的官网: https://xingqiu.pro");
            Console.WriteLine("============================================");
            for (int i = 3; i > 0; i--)
            {
                Console.Write($"\r {i} s ");
                Thread.Sleep(1300);
            }
            Console.Write($"\r");
            //Console.WriteLine("按 空格 键打开官网...");
            //if (Console.ReadKey().Key == ConsoleKey.Spacebar)
            //{
            //    Process.Start(new ProcessStartInfo { FileName = "https://xingqiu.pro", UseShellExecute = true });
            //}
            if (args.Length == 0)
            {
                Console.WriteLine("请将要分割的文本文件拖到此程序上运行。");
                return;
            }

            string filePath = args[0];
            if (!File.Exists(filePath))
            {
                Console.WriteLine("文件不存在！");
                return;
            }

            Console.Write("请输入要分割的文件数（默认 2）：");
            string input = Console.ReadLine();
            int parts = 2;
            if (!string.IsNullOrWhiteSpace(input) && !int.TryParse(input, out parts))
            {
                Console.WriteLine("输入无效，使用默认值 2。");
                parts = 2;
            }

            string directory = Path.GetDirectoryName(filePath);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            string headLine = "";

            int totalLines = 0;
            var line = "";
            using (var reader = new StreamReader(filePath))
            {
                while ( (line =reader.ReadLine()) != null)
                {
                    if (totalLines == 0)
                    {
                        // 如果是csv文件，第一行为表头
                        if (extension == ".csv") 
                        { 
                            headLine = line  ;
                        }
                    }
                    totalLines++;
                }
            }

            int linesPerPart = (int)Math.Ceiling((double)totalLines / parts);


            using (var reader = new StreamReader(filePath))
            {
                for (int i = 0; i < parts; i++)
                {
                    string newFilePath = Path.Combine(directory, $"{fileNameWithoutExt}_part{i + 1}{extension}");
                    using (var writer = new StreamWriter(newFilePath,false,new UTF8Encoding(true)))
                    {
                        if (i != 0 && headLine != "")
                        {
                            writer.WriteLine(headLine);
                        }
                        for (int j = 0; j < linesPerPart; j++)
                        {
                            if (reader.EndOfStream) break;
                            writer.WriteLine(reader.ReadLine());
                        }
                    }
                    Console.WriteLine($"生成文件: {newFilePath}");
                }
            }

            Console.WriteLine("分割完成！");
        }
    }
    
}
