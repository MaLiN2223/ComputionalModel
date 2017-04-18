using System.Collections.Generic;
using InputParser.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class EquationTests
    {
        [TestMethod]
        public void EvalPureTest()
        {
            Equation equation = new Equation("b = 2*3^2");
            Assert.AreEqual(18, equation.Evaluate());
        }
        [TestMethod]
        public void EvalPureTest1()
        {
            Equation equation = new Equation("a = (-(2 + 3)* (1 + 2)) * 4 ^ 2");
            Assert.AreEqual(-240, equation.Evaluate());
        }
        [TestMethod]
        public void EvalPureTest4()
        {
            Equation equation = new Equation("a = 2+2*2");
            Assert.AreEqual(6, equation.Evaluate());
            equation = new Equation("a = 2+2/2");
            Assert.AreEqual(3, equation.Evaluate());
            equation = new Equation("a = (2+2)/2");
            Assert.AreEqual(2, equation.Evaluate());
            equation = new Equation("a = (2+2)/(2*2)");
            Assert.AreEqual(1, equation.Evaluate());
            equation = new Equation("a = 3 / (1 * 3)");
            Assert.AreEqual(1, equation.Evaluate());
            equation = new Equation("a = (5 / 2) * 4 ");
            Assert.AreEqual(10, equation.Evaluate());
        }

        [TestMethod]
        public void EvalPureTest2()
        {
            Equation equation = new Equation("c = -5^3^2*2-1");
            Assert.AreEqual(-3906251, equation.Evaluate());
        }
        [TestMethod]
        public void EvalPureTest3()
        {
            Equation equation = new Equation("c = -5^3^2*2-1");
            Assert.AreEqual(-3906251, equation.Evaluate());
        }

        [TestMethod]
        public void EvalLettersWithNumbers()
        {
            Equation equation = new Equation("c = a + 3");
            var dict = new Dictionary<string, double> { { "a", 1 } };
            Assert.AreEqual(4, equation.Evaluate(dict));
        }

        [TestMethod]
        public void EvalLettersWithNumbers1()
        {
            Equation equation = new Equation("a = (b*c^2)/2 + d - 2");
            var dict = new Dictionary<string, double>
            {
                { "b", 5 },
                { "c", 3 },
                { "d", -2 },
            };
            Assert.AreEqual(18.5, equation.Evaluate(dict));
        }

        [TestMethod]
        public void Transform()
        {
            Equation equation = new Equation("a = b+c");
            equation.TransformFor("b");
            string expected = "b = a - c".Replace(" ", "");
            Assert.AreEqual(expected, equation.Symbolic().Replace(" ", ""));


        }
        [TestMethod]
        public void Transform2()
        {
            Equation equation = new Equation("a = (b*c^2)/2 + d - 2");
            equation.TransformFor("d");
            string expected = "d= a + 2 - (b*c^2)/2".Replace(" ", "");
            Assert.AreEqual(expected, equation.Symbolic().Replace(" ", ""));
            var dict = new Dictionary<string, double>
            {
                { "b", 4 },
                { "c", 3 },
                { "a", 7 },
            };
            Assert.AreEqual(-9, equation.Evaluate(dict));
        }

        [DataTestMethod]
        [DataRow(2, 4, 5, 40)]
        [DataRow(2, 1, 0.5, 1)]
        public void RealTest2(double m, double g, double h, double P)
        {
            Equation equation = new Equation("P=m*g*h");
            string expected = "P=m*g*h".Replace(" ", "");
            Assert.AreEqual(expected, equation.Symbolic().Replace(" ", ""));

            Assert.AreEqual(P, equation.Evaluate(new Dictionary<string, double>()
            {
                { "m", m },
                { "g", g },
                { "h", h }
            }));
            equation.TransformFor("m");
            expected = "m = P/(g*h)".Replace(" ", "");
            Assert.AreEqual(expected, equation.Symbolic().Replace(" ", ""));

            Assert.AreEqual(m, equation.Evaluate(new Dictionary<string, double>()
            {
                { "P", P },
                { "g", g },
                { "h", h }
            }));
            equation.TransformFor("g");
            expected = "g = P/m/h".Replace(" ", "");
            Assert.AreEqual(expected, equation.Symbolic().Replace(" ", ""));

            Assert.AreEqual(g, equation.Evaluate(new Dictionary<string, double>()
            {
                { "P", P },
                { "m", m },
                { "h", h }
            }));
            equation.TransformFor("h");
            expected = "h = P/m/g".Replace(" ", "");
            Assert.AreEqual(expected, equation.Symbolic().Replace(" ", ""));

            Assert.AreEqual(h, equation.Evaluate(new Dictionary<string, double>()
            {
                { "P", P },
                { "m", m },
                { "g", g }
            }));
            equation.TransformFor("P");
            expected = "P=h*g*m".Replace(" ", "");
            Assert.AreEqual(expected, equation.Symbolic().Replace(" ", ""));

            Assert.AreEqual(P, equation.Evaluate(new Dictionary<string, double>()
            {
                { "m", m },
                { "g", g },
                { "h", h }
            }));
        }

        [TestMethod]
        public void RealTest1()
        {
            double m = 10;
            double v = 3;
            double K = 45;
            Equation equation = new Equation("K=m*v^2/2");
            string expected = "K=(m*v^2)/2".Replace(" ", "");
            Assert.AreEqual(expected, equation.Symbolic().Replace(" ", ""));

            Assert.AreEqual(K, equation.Evaluate(new Dictionary<string, double>()
            {
                { "m", m },
                { "v", v }
            }));
            equation.TransformFor("m");
            expected = "m =  (K*2) / v ^ 2  ".Replace(" ", "");
            Assert.AreEqual(expected, equation.Symbolic().Replace(" ", ""));
            Assert.AreEqual(m, equation.Evaluate(new Dictionary<string, double>()
            {
                { "K", K },
                { "v", v }
            }));
            equation.TransformFor("v");
            expected = "v=((K*2)/m) & 2".Replace(" ", ""); // TODO : problem z mnozeniem gdy ma po prawej zostac 1 / x
            Assert.AreEqual(expected, equation.Symbolic().Replace(" ", ""));
            Assert.AreEqual(v, equation.Evaluate(new Dictionary<string, double>()
            {
                { "K", K },
                { "m", m }
            }));
        }

    }

}
