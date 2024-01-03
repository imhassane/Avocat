using Avocat.Exceptions;
using Avocat.Tokenizer;
using NUnit.Framework;
using System.Linq;

namespace Avocat.Tests
{
    public class TokenizerTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Declare_Integer_Variable_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = 30");
            var tokens = tokenizer.GetTokens().ToList();

            Assert.AreEqual(5, tokens.Count());

            Assert.AreEqual(ETokenType.VAR, tokens[0].Type);
            Assert.AreEqual(string.Empty, tokens[0].Value);

            Assert.AreEqual(ETokenType.IDENTIFIER, tokens[1].Type);
            Assert.AreEqual("age", tokens[1].Value);

            Assert.AreEqual(ETokenType.INTEGER, tokens[3].Type);
            Assert.AreEqual("30", tokens[3].Value);
        }

        [Test]
        public void Declare_Float_Variable_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age = 30.5");
            var tokens = tokenizer.GetTokens().ToList();

            Assert.AreEqual(5, tokens.Count());

            Assert.AreEqual(ETokenType.VAR, tokens[0].Type);
            Assert.AreEqual(string.Empty, tokens[0].Value);

            Assert.AreEqual(ETokenType.IDENTIFIER, tokens[1].Type);
            Assert.AreEqual("age", tokens[1].Value);

            Assert.AreEqual(ETokenType.FLOAT, tokens[3].Type);
            Assert.AreEqual("30.5", tokens[3].Value);
        }

        [Test]
        public void Declare_String_Variable_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec nom = \"test\"");
            var tokens = tokenizer.GetTokens().ToList();

            Assert.AreEqual(5, tokens.Count());

            Assert.AreEqual(ETokenType.VAR, tokens[0].Type);
            Assert.AreEqual(string.Empty, tokens[0].Value);

            Assert.AreEqual(ETokenType.IDENTIFIER, tokens[1].Type);
            Assert.AreEqual("nom", tokens[1].Value);

            Assert.AreEqual(ETokenType.STRING, tokens[3].Type);
            Assert.AreEqual("test", tokens[3].Value);
        }

        [Test]
        public void Declare_String_Variable_With_Non_Closing_Quote_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec nom = \"test");

            Assert.Throws<InvalidSyntaxException>(() => {
                tokenizer.GetTokens().ToList();
            });
        }

        [Test]
        public void Declare_String_Variable_With_Non_Closing_Quote_And_New_LineTest()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec nom = \"test\n");

            Assert.Throws<InvalidSyntaxException>(() => {
                tokenizer.GetTokens().ToList();
            });
        }

        [Test]
        public void Declare_String_Variable_With_Escaped_Quote_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec exemple = \"\\\"\"");
            var tokens = tokenizer.GetTokens().ToList();

            Assert.AreEqual(5, tokens.Count());

            Assert.AreEqual(ETokenType.VAR, tokens[0].Type);
            Assert.AreEqual(string.Empty, tokens[0].Value);

            Assert.AreEqual(ETokenType.IDENTIFIER, tokens[1].Type);
            Assert.AreEqual("exemple", tokens[1].Value);

            Assert.AreEqual(ETokenType.STRING, tokens[3].Type);
            Assert.AreEqual("\\\"", tokens[3].Value);
        }

        [Test]
        public void Use_Parenthesis_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("()(");
            var tokens = tokenizer.GetTokens().ToList();

            Assert.AreEqual(4, tokens.Count());

            Assert.AreEqual(ETokenType.OPEN_PARENT, tokens[0].Type);
            Assert.AreEqual(string.Empty, tokens[0].Value);

            Assert.AreEqual(ETokenType.CLOSE_PARENT, tokens[1].Type);
            Assert.AreEqual(string.Empty, tokens[1].Value);

            Assert.AreEqual(ETokenType.OPEN_PARENT, tokens[2].Type);
            Assert.AreEqual(string.Empty, tokens[2].Value);
        }

        [Test]
        public void Use_Equal_And_Parenthesis_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("()(=");
            var tokens = tokenizer.GetTokens().ToList();

            Assert.AreEqual(5, tokens.Count());

            Assert.AreEqual(ETokenType.OPEN_PARENT, tokens[0].Type);
            Assert.AreEqual(string.Empty, tokens[0].Value);

            Assert.AreEqual(ETokenType.CLOSE_PARENT, tokens[1].Type);
            Assert.AreEqual(string.Empty, tokens[1].Value);

            Assert.AreEqual(ETokenType.OPEN_PARENT, tokens[2].Type);
            Assert.AreEqual(string.Empty, tokens[2].Value);

            Assert.AreEqual(ETokenType.EQUAL, tokens[3].Type);
            Assert.AreEqual(string.Empty, tokens[3].Value);
        }

        [Test]
        public void Comment_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("# Hello");
            var tokens = tokenizer.GetTokens().ToList();

            Assert.AreEqual(1, tokens.Count());
        }

        [Test]
        public void Declare_Typed_String_Variable_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec hello: chaine = \"Hello\"");
            var tokens = tokenizer.GetTokens().ToList();

            Assert.AreEqual(7, tokens.Count());
            Assert.AreEqual(ETokenType.VAR, tokens[0].Type);
            Assert.AreEqual(string.Empty, tokens[0].Value);

            Assert.AreEqual(ETokenType.IDENTIFIER, tokens[1].Type);
            Assert.AreEqual("hello", tokens[1].Value);

            Assert.AreEqual(ETokenType.TWO_POINTS, tokens[2].Type);
            Assert.AreEqual(string.Empty, tokens[2].Value);

            Assert.AreEqual(ETokenType.TYPE_STRING, tokens[3].Type);
            Assert.AreEqual(string.Empty, tokens[3].Value);

            Assert.AreEqual(ETokenType.EQUAL, tokens[4].Type);
            Assert.AreEqual(string.Empty, tokens[4].Value);

            Assert.AreEqual(ETokenType.STRING, tokens[5].Type);
            Assert.AreEqual("Hello", tokens[5].Value);
        }

        [Test]
        public void Declare_Typed_Integer_Variable_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec age: entier = 5");
            var tokens = tokenizer.GetTokens().ToList();

            Assert.AreEqual(7, tokens.Count());
            Assert.AreEqual(ETokenType.VAR, tokens[0].Type);
            Assert.AreEqual(string.Empty, tokens[0].Value);

            Assert.AreEqual(ETokenType.IDENTIFIER, tokens[1].Type);
            Assert.AreEqual("age", tokens[1].Value);

            Assert.AreEqual(ETokenType.TWO_POINTS, tokens[2].Type);
            Assert.AreEqual(string.Empty, tokens[2].Value);

            Assert.AreEqual(ETokenType.TYPE_INTEGER, tokens[3].Type);
            Assert.AreEqual(string.Empty, tokens[3].Value);

            Assert.AreEqual(ETokenType.EQUAL, tokens[4].Type);
            Assert.AreEqual(string.Empty, tokens[4].Value);

            Assert.AreEqual(ETokenType.INTEGER, tokens[5].Type);
            Assert.AreEqual("5", tokens[5].Value);
        }

        [Test]
        public void Declare_Typed_Float_Variable_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec price: flottant = 10.0");
            var tokens = tokenizer.GetTokens().ToList();

            Assert.AreEqual(7, tokens.Count());
            Assert.AreEqual(ETokenType.VAR, tokens[0].Type);
            Assert.AreEqual(string.Empty, tokens[0].Value);

            Assert.AreEqual(ETokenType.IDENTIFIER, tokens[1].Type);
            Assert.AreEqual("price", tokens[1].Value);

            Assert.AreEqual(ETokenType.TWO_POINTS, tokens[2].Type);
            Assert.AreEqual(string.Empty, tokens[2].Value);

            Assert.AreEqual(ETokenType.TYPE_FLOAT, tokens[3].Type);
            Assert.AreEqual(string.Empty, tokens[3].Value);

            Assert.AreEqual(ETokenType.EQUAL, tokens[4].Type);
            Assert.AreEqual(string.Empty, tokens[4].Value);

            Assert.AreEqual(ETokenType.FLOAT, tokens[5].Type);
            Assert.AreEqual("10.0", tokens[5].Value);
        }

        [Test]
        public void Declare_Typed_Char_Variable_Test()
        {
            var tokenizer = new Tokenizer.Tokenizer("dec a: caractere = 'a'");
            var tokens = tokenizer.GetTokens().ToList();

            Assert.AreEqual(7, tokens.Count());
            Assert.AreEqual(ETokenType.VAR, tokens[0].Type);
            Assert.AreEqual(string.Empty, tokens[0].Value);

            Assert.AreEqual(ETokenType.IDENTIFIER, tokens[1].Type);
            Assert.AreEqual("a", tokens[1].Value);

            Assert.AreEqual(ETokenType.TWO_POINTS, tokens[2].Type);
            Assert.AreEqual(string.Empty, tokens[2].Value);

            Assert.AreEqual(ETokenType.TYPE_CHAR, tokens[3].Type);
            Assert.AreEqual(string.Empty, tokens[3].Value);

            Assert.AreEqual(ETokenType.EQUAL, tokens[4].Type);
            Assert.AreEqual(string.Empty, tokens[4].Value);

            Assert.AreEqual(ETokenType.CHAR, tokens[5].Type);
            Assert.AreEqual("a", tokens[5].Value);
        }
    }
}
