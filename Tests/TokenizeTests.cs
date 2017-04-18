using System.Linq;
using InputParser.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{

    [TestClass]
    public class TokenizeTests
    {
        [TestMethod]
        public void TokenizeNumbers()
        {
            var str = "10+-10+10+-10-10";
            var output = "10|+|~|10|+|10|+|~|10|-|10";
            var tokens = Tokenizer.Tokenize(str);
            Assert.AreEqual(output, tokens.Aggregate("", (x, y) => x + "|" + y.Value).Substring(1));
        }

        [TestMethod]
        public void TokenizeSimpleWithLetters()
        {
            var str = "a+b+c";
            var output = "a|+|b|+|c";
            var tokens = Tokenizer.Tokenize(str);
            Assert.AreEqual(output, tokens.Aggregate("", (x, y) => x + "|" + y.Value).Substring(1));
        }
        [TestMethod]
        public void TokenizeAllWithLetters()
        {
            var str = "a*b/c^2 + e - f*-g";
            var output = "a|*|b|/|c|^|2|+|e|-|f|*|~|g";
            var tokens = Tokenizer.Tokenize(str);
            Assert.AreEqual(output, tokens.Aggregate("", (x, y) => x + "|" + y.Value).Substring(1));
        }
    }

}
