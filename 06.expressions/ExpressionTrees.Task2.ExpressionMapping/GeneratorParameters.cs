using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class GeneratorParameters
    {
        public IDictionary<string, IMappingConverter> SourceConverters { get; set; }

        public IDictionary<string, IMappingConverter> DestinationConverters { get; set; }
    }
}
