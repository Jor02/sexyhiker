using System;
using System.IO;

namespace Sexyhiker
{

    class Program
    {
        static bool verbose = false;
        static string path = string.Empty;

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Usage();
                Console.WriteLine();
                Console.Write("Press any key to continue...");
                Console.CursorVisible = false;
                Console.ReadKey();
                Console.CursorVisible = true;
                return;
            }

            if (args[0] == "-h" || args[0] == "--help")
            {
                Usage();
                return;
            }

            if (args.Length > 1)
                for (int i = 1; i < args.Length; i++)
                    switch (args[i])
                    {
                        case "-h":
                        case "--help":
                            Usage();
                            return;
                        case "-v":
                        case "--verbose":
                            verbose = true;
                            break;
                        case "-o":
                        case "--output":
                            if (args.Length < i + 1) return;
                            i++;
                            path = args[i];
                            break;
                    }

            if (path == string.Empty)
                path = Path.ChangeExtension(args[0], "gmd");

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("Input file does not exist");
                return;
            }
            
            new Decompiler(verbose).Decompile(args[0], path);
        }

        static void Usage()
        {
            string fileName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            Console.WriteLine($"Usage: {fileName} FILE [options]");
            Console.WriteLine();
            Console.WriteLine(
                "Options:" + Environment.NewLine +
                "   -h      prints this screen" + Environment.NewLine +
                "   -v      enables verbose logging"
                );
        }
    }
}
