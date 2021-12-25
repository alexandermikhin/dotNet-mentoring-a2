namespace FeedManager.Task1.FeedValidators
{
    public class ErrorCode
    {
        public static string IdIsNotValidMessage { get; } = "Identifier should be bigger than 1";

        public static string PriceIsNotValid { get; } = "Should be positive and 2 digits after decimal point";

        public static string NotValidIsin { get; } = "Should be valid Isin";

        public static string PropertyRangeError(string name, decimal minVal, decimal maxVal)
        {
            return $"{name} should be between {minVal} and {maxVal}";
        }
    }
}
