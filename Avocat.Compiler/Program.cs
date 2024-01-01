using System;
using System.IO;
using System.Linq;

namespace Avocat
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var content = File.ReadAllText("..\\..\\..\\main.av");
                var tokenizer = new Tokenizer.Tokenizer(content);
                var tokens = tokenizer.GetTokens();

                var parser = new Parser.Parser(tokens);

                foreach (var token in parser.Parse())
                {
                    Console.WriteLine(token);
                }
            } catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
