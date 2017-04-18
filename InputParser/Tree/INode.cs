using System.Security.Cryptography.X509Certificates;

namespace InputParser.Tree
{
    public partial class ExpressionTree
    {
        public interface INode
        {
            double Evaluate();
            string Symbolic();
            INode Left { get; set; }
            INode Right { get; set; }
            bool Contains(string a);
        }



    }
}