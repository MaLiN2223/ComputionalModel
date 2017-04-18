using InputParser.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class TreeTests
    {
        [DataTestMethod]
        [DataRow("a = b + c", "a = b + c")]
        [DataRow("a = (b / c) * q ", " a = (b / c) * q")]
        [DataRow("a = b / (c * q) ", "a = b / (c * q)")]
        [DataRow("a = (b * c ^ 2) / 2 + d - 2", "a = (b * c ^ 2) / 2 + d - 2")]
        [DataRow("a = (2-3)/(2+3)", "a = (2-3)/(2+3)")]
        public void SymbolicExpression1(string input, string expected)
        {
            expected = expected.Replace(" ", "");
            ExpressionTree tree = new ExpressionTree(input);
            Assert.AreEqual(expected, tree.Symbolic().Replace(" ", ""));
        } 


    }

}
