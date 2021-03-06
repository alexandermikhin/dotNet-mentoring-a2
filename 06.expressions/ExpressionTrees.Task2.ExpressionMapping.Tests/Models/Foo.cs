using System;
using System.ComponentModel;
using ExpressionTrees.Task2.ExpressionMapping.Annotations;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests.Models
{
    internal class Foo
    {
        string _field;
        int _year;

        public string commonField;

        private double PropertyPrivate { get; set; }

        public int Id { get; set; }

        public string Name => "name";

        public DateTime Date { get; set; }

        public int Year { set { _year = value; } }

        public string BarIgnored { get; set; }

        [Ignore]
        public string FooIgnored { get; set; }

        public int FooOnly { get; set; }

        [DestinationMember("DestinationBar")]
        public int SourceFoo { get; set; }

        [DestinationMember("Sum")]
        public string Sum { get; set; }

        public string Count { get; set; }
    }
}
