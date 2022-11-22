
namespace Calculator.Helper
{
    public static class CalculatorHelper
    {
        public static double PerformCalculation(double leftValue, double rightValue, string sOperation)
        {
            switch (sOperation)
            {
                case "+": return leftValue + rightValue;
                case "-": return leftValue - rightValue;
                case "*": return leftValue * rightValue;
                case "/": return leftValue / rightValue;
                default: throw new ArgumentException("Uknown Operator!");
            }
        }
    }
}
