using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using InputParser.Expressions;
using InputParser.Tree;

namespace InputParser
{
    public static class InputParser
    {
        public static KnowledgeData ReadBase()
        {
            var xml = XDocument.Parse("KnowledgeBase/base.xml");
            var data = new KnowledgeData
            {
                Contants = xml.Descendants("consts").Select(item => new
                {
                    Name = item.Attribute("name").Value,
                    Value = double.Parse(item.Value)
                }).ToDictionary(c => c.Name, c => c.Value),

                Equations = (from item in xml.Descendants("equations")
                             select new Equation(item.Value)).ToList()
            };
            return data;
        }
    }


    public class KnowledgeData
    {
        public Dictionary<string, double> Contants { get; set; }
        public List<Equation> Equations { get; set; }
    }
}
