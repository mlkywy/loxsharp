using System;
using System.Collections.Generic;
using System.IO;

namespace Lox
{
    public class Lox
    {
        static bool HadError = false;

        public static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: loxsharp [script]");
            }
            else if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunPrompt();
            }
        }

        /// <summary>
        /// Reads and executes the provided file path.
        /// </summary>
        private static void RunFile(string path)
        {
            StreamReader sr = new(path);
            Run(sr.ReadToEnd());
        }

        /// <summary>
        /// Reads and executes provided code one line at a time.
        /// </summary>
        private static void RunPrompt()
        {
            for (;;)
            {
                Console.Write("> ");

                string line = Console.ReadLine();
                if (line == null) return;
                Run(line);

                HadError = false;
            }
        }

        /// <summary>
        /// Prints out the tokens to the console.
        /// </summary>
        private static void Run(string source)
        {
            if (HadError) return;

            Scanner scanner = new(source);
            List<Token> tokens = scanner.ScanTokens();

            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }
        }

        /// <summary>
        /// Logs error to the console.
        /// </summary>
        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        /// <summary>
        /// Helper function that outputs where the error has occured on a given line.
        /// </summary>
        private static void Report(int line, string where, string message)
        {
            Console.Error.WriteLine($"[line {line} Error {where}: {message}");
            HadError = true;
        }
    }
}
