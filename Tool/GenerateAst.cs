using System;
using System.Collections.Generic;
using System.IO;

namespace Tool
{
    public class Tool
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: generate_ast <output directory>");
            }

            string outputDir = args[0];

            DefineAst(outputDir, "Expr", new List<string> { 
                "Binary   : Expr left, Token operator, Expr right",
                "Grouping : Expr expression",
                "Literal  : Object value",
                "Unary    : Token operator, Expr right" 
            });
        }

        private static void DefineAst(string outputDir, string baseName, List<string> types)
        {
            string path = outputDir + "/" + baseName + ".cs";
            StreamWriter writer = new StreamWriter(path);

            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine();
            writer.WriteLine("namespace Lox");
            writer.WriteLine("{");
            writer.WriteLine();
            writer.WriteLine($"\tpublic abstract class {baseName}");
            writer.WriteLine("\t{");

            // The AST classes.
            foreach (string type in types) 
            {
                string className = type.Split(":")[0].Trim();
                string fields = type.Split(":")[1].Trim(); 
                DefineType(writer, baseName, className, fields);
            }

            writer.WriteLine();
            writer.WriteLine("\t}");
            writer.WriteLine("}");
            writer.Close();
        }

        private static void DefineType(StreamWriter writer, string baseName, string className, string fieldList)
        {
           
        }
    }
}
            /*
            writer.WriteLine($"\t\tpublic class {className} : {baseName}");
            writer.WriteLine("\t\t{");

            // constructor
            if (fieldList != null)
            {
                writer.WriteLine($"\t\t\tpublic {className} ({fieldList})");
                writer.WriteLine("\t\t\t{");

                string[] fields = fieldList.Split(", ");
                
                foreach (string field in fields)
                {
                    string name = field.Split(" ")[1];
                    writer.WriteLine($"\t\t\tthis.{name} = {name};");
                }

                writer.WriteLine("\t\t\t}");
                writer.WriteLine();

                // Fields
                foreach (string field in fields)
                {
                    writer.WriteLine($"\t\t\tpublic readonly {field};");
                }
            }
            else
            {
                writer.WriteLine($"\t\t\tpublic {className} () ");
                writer.WriteLine("{}");
            }
            
            // Visitor
            writer.WriteLine($"\t\t\tpublic override T Accept<T>(IVisitor<T> visitor) => visitor.Visit{className}{baseName}(this);");
            writer.WriteLine("\t\t}");

            */