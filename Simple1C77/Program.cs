using System;
using System.IO;
using System.Collections.Generic;

namespace Simple1C77
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dir = @"D:\Work\dotNET\Simple1C77\Simple1C77\Tests";
            string code, content;

            var filesNames = Directory.EnumerateFiles(dir, "*.1s7", SearchOption.AllDirectories);

            foreach(string file in filesNames)
            {
                Console.WriteLine("Processing file: " + file);
                code = File.ReadAllText(file);
                Lexer lexer = new Lexer(code);
                Parser parser = new Parser(lexer);
                AST tree = parser.Parse();
                GenerateASTForDOT viz = new GenerateASTForDOT();
                content = viz.GenerateDOT(tree);
                //dot_file = open('ast.dot', mode = 'w', encoding = 'utf-8')
                //dot_file.write(content)
                File.WriteAllText(file + ".dot", content);

            };
            //Console.ReadLine();
        }
    }
}
