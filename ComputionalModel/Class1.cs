using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InputParser;

namespace ComputionalModel
{
    public class Model
    {
        public Model()
        {
            Base = InputParser.InputParser.ReadBase();
        }
        public KnowledgeData Base { get; set; }

        public double Calculate(string variable, Dictionary<string, double> values)
        {
            var tmp = Base.Equations.FirstOrDefault(x => x.Variable == variable);
            if (tmp != null)
            {
                return tmp.Evaluate(values);
            }
        }
    }
}
