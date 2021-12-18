using ExpressionTrees.Task2.ExpressionMapping.Annotations;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests.Models
{
    internal class Bar
    {
        string _field;

        public string commonField;

        private double PropertyPrivate { get; set; }

        public int Id { get; set; }

        public string Label { get; set; }

        public string Date { get; set; }

        public int Year { get; }

        [Ignore]
        public string BarIgnored { get; set; }

        public string FooIgnored { get; set; }

        [SourceMember("FooOnly")]
        public int BarOnly { get; set; }

        public int DestinationBar { get; set; }

        public double Sum { get; set; }

        [SourceMember("Count")]
        public double Count { get; set; }
    }
}
