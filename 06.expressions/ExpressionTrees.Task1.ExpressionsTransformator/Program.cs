/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            var visitor = new IncDecExpressionVisitor();
            Expression<Func<int, int>> incExpression = val => val + 1;
            var incTransformResult = visitor.Transform(incExpression);
            Console.WriteLine("Increment operation expression");
            Console.WriteLine("\tSource: " + incExpression);
            Console.WriteLine("\tResult: " + incTransformResult);

            Expression<Func<int, int>> decExpression = val => val - 1;
            var decTransformResult = visitor.Transform(decExpression);
            Console.WriteLine("Decrement operation expression");
            Console.WriteLine("\tSource: " + decExpression);
            Console.WriteLine("\tResult: " + decTransformResult);

            Console.ReadLine();
        }
    }
}
