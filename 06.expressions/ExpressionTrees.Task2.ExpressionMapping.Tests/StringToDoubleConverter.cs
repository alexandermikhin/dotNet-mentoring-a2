namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    class StringToDoubleConverter : IMappingConverter
    {
        public object Convert(object value)
        {
            var str = value as string;
            var result = !string.IsNullOrEmpty(str) ? double.Parse(str) : default;
            return result;
        }
    }
}
