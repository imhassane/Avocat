using Avocat.Exceptions;
using Avocat.Tokenizer;
using NUnit.Framework;
using System.Linq;

namespace Avocat.Tests
{
    public class ParserTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Declare_Variable_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = 18");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(1, statements.Count());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.FirstOrDefault());
        }

        [Test]
        public void Declare_Variable_With_Two_Equals_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age == 18");
            var parser = new Parser.Parser(tokenizer.GetTokens());

            Assert.Throws<InvalidSyntaxException>(() =>
            {
                parser.Parse().ToList();
            });
        }

        [Test]
        public void Invalid_Start_Of_Program_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("age = 18");
            var parser = new Parser.Parser(tokenizer.GetTokens());

            Assert.Throws<InvalidSyntaxException>(() =>
            {
                parser.Parse().ToList();
            });
        }

        [Test]
        public void Declare_Two_Variables_Inline_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age == 18 dec age = 32");
            var parser = new Parser.Parser(tokenizer.GetTokens());

            Assert.Throws<InvalidSyntaxException>(() =>
            {
                parser.Parse().ToList();
            });
        }

        [Test]
        public void Declare_Two_Variables_Correctly_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = 18\ndec age = 32");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(2, statements.Count());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.FirstOrDefault());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.LastOrDefault());
        }

        [Test]
        public void Declare_Two_Variables_With_Parenthesis_Correctly_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = (18)\ndec age = 32");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(2, statements.Count());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.FirstOrDefault());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.LastOrDefault());
        }

        [Test]
        public void Declare_Two_Variables_With_Parenthesis_And_Operation_Correctly_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = (18)\ndec age = 32 + 1");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(2, statements.Count());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.FirstOrDefault());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.LastOrDefault());
        }

        [Test]
        public void Declare_Two_Variables_With_Parenthesis_And_Operation_Correctly_2_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = (18)\ndec age = (32 + 1)");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(2, statements.Count());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.FirstOrDefault());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.LastOrDefault());
        }

        [Test]
        public void Declare_Multiple_Types_Of_Variables_Correctly_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = (18)\ndec age = (32 + 1)\ndec nom = \"Hilton\"");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(3, statements.Count());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.FirstOrDefault());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.LastOrDefault());
        }

        [Test]
        public void Declare_String_Variables_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec name = \"Sow\"");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(1, statements.Count());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.FirstOrDefault());
            Assert.AreEqual(ETokenType.IDENTIFIER, statements.FirstOrDefault().Token.Type);
            Assert.AreEqual("name", statements.FirstOrDefault().Token.Value);
            Assert.AreEqual("Sow", statements.FirstOrDefault().Expression.Token.Value);
            Assert.IsInstanceOf<Expressions.ExpressionString>(statements.FirstOrDefault().Expression);
        }

        [Test]
        public void Declare_Integer_Variables_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = 10");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(1, statements.Count());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.FirstOrDefault());
            Assert.AreEqual(ETokenType.IDENTIFIER, statements.FirstOrDefault().Token.Type);
            Assert.AreEqual("age", statements.FirstOrDefault().Token.Value);
            Assert.AreEqual("10", statements.FirstOrDefault().Expression.Token.Value);
            Assert.IsInstanceOf<Expressions.ExpressionInteger>(statements.FirstOrDefault().Expression);
        }

        [Test]
        public void Declare_Float_Variables_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec prix = 10.5");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(1, statements.Count());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.FirstOrDefault());
            Assert.AreEqual(ETokenType.IDENTIFIER, statements.FirstOrDefault().Token.Type);
            Assert.AreEqual("prix", statements.FirstOrDefault().Token.Value);
            Assert.AreEqual("10.5", statements.FirstOrDefault().Expression.Token.Value);
            Assert.IsInstanceOf<Expressions.ExpressionFloat>(statements.FirstOrDefault().Expression);
        }

        [Test]
        public void Declare_Variable_With_Parenthesis_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = (10)");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(1, statements.Count());
            Assert.IsInstanceOf<Statements.StatementVar>(statements.FirstOrDefault());
            Assert.AreEqual(ETokenType.IDENTIFIER, statements.FirstOrDefault().Token.Type);
            Assert.AreEqual("age", statements.FirstOrDefault().Token.Value);
            Assert.AreEqual("10", statements.FirstOrDefault().Expression.Token.Value);
        }

        [Test]
        public void Declare_Variable_With_Missing_Closing_Parenthesis_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = (10");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            
            Assert.Throws<InvalidSyntaxException>(() =>
            {
                parser.Parse().ToList();
            });
        }

        [Test]
        public void Declare_Variable_With_Missing_Opening_Parenthesis_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = 10)");
            var parser = new Parser.Parser(tokenizer.GetTokens());

            Assert.Throws<InvalidSyntaxException>(() =>
            {
                parser.Parse().ToList();
            });
        }

        [Test]
        public void Declare_Basic_Sum_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = 10 + 1");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(1, statements.Count());
            Assert.IsInstanceOf<Expressions.ExpressionOperation>(statements.FirstOrDefault().Expression);
            Assert.AreEqual(ETokenType.PLUS, statements.FirstOrDefault().Expression.Token.Type);
            Assert.AreEqual("age", statements.FirstOrDefault().Token.Value);
        }

        [Test]
        public void Declare_Basic_Multiplication_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = 10 * 1");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(1, statements.Count());
            Assert.IsInstanceOf<Expressions.ExpressionOperation>(statements.FirstOrDefault().Expression);
            Assert.AreEqual(ETokenType.MULTIPLY, statements.FirstOrDefault().Expression.Token.Type);
            Assert.AreEqual("age", statements.FirstOrDefault().Token.Value);
        }

        [Test]
        public void Declare_Basic_Soustraction_Minus_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = 10 - 1");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(1, statements.Count());
            Assert.IsInstanceOf<Expressions.ExpressionOperation>(statements.FirstOrDefault().Expression);
            Assert.AreEqual(ETokenType.MINUS, statements.FirstOrDefault().Expression.Token.Type);
            Assert.AreEqual("age", statements.FirstOrDefault().Token.Value);
        }

        public void Declare_Basic_Sum_With_Parenthesis_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = (10 + 1)");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(1, statements.Count());
            Assert.IsInstanceOf<Expressions.ExpressionOperation>(statements.FirstOrDefault().Expression);
            Assert.AreEqual(ETokenType.PLUS, statements.FirstOrDefault().Expression.Token.Type);
            Assert.AreEqual("age", statements.FirstOrDefault().Token.Value);
        }

        [Test]
        public void Declare_Basic_Multiplication_With_Parenthesis_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = (10 * 1)");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(1, statements.Count());
            Assert.IsInstanceOf<Expressions.ExpressionOperation>(statements.FirstOrDefault().Expression);
            Assert.AreEqual(ETokenType.MULTIPLY, statements.FirstOrDefault().Expression.Token.Type);
            Assert.AreEqual("age", statements.FirstOrDefault().Token.Value);
        }

        [Test]
        public void Declare_Basic_Soustraction_Minus_With_Parenthesis_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = (10 - 1)");
            var parser = new Parser.Parser(tokenizer.GetTokens());
            var statements = parser.Parse().ToList();

            Assert.AreEqual(1, statements.Count());
            Assert.IsInstanceOf<Expressions.ExpressionOperation>(statements.FirstOrDefault().Expression);
            Assert.AreEqual(ETokenType.MINUS, statements.FirstOrDefault().Expression.Token.Type);
            Assert.AreEqual("age", statements.FirstOrDefault().Token.Value);
        }
    }
}
