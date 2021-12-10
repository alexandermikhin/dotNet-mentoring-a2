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
    }
}
