/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            CheckIncTransform();
            CheckDecTransform();
            CheckValueSubstituteTransform();

            Console.ReadLine();
        }

        static void CheckIncTransform()
        {
            var visitor = new IncDecExpressionVisitor();
            Expression<Func<int, double, double>> incExpression = (iVal, dVal) => (iVal + 1) * (iVal + 5) * (dVal + 1) * (dVal + 5) * Math.Sin(iVal);
            var incTransformResult = visitor.Transform(incExpression);
            Console.WriteLine("Increment operation expression");
            Console.WriteLine("\tSource: " + incExpression);
            Console.WriteLine("\tResult: " + incTransformResult);
        }

        static void CheckDecTransform()
        {
            var visitor = new IncDecExpressionVisitor();
            Expression<Func<int, double, double>> decExpression = (iVal, dVal) => (iVal - 1) * (iVal - 5) * (dVal - 1) * (dVal - 5) * Math.Sin(iVal);
            var decTransformResult = visitor.Transform(decExpression);
            Console.WriteLine("Decrement operation expression");
            Console.WriteLine("\tSource: " + decExpression);
            Console.WriteLine("\tResult: " + decTransformResult);
        }

        static void CheckValueSubstituteTransform()
        {
            var visitor = new IncDecExpressionVisitor();
            Expression<Func<int, double, double>> expression = (iVal, dVal) => (iVal - 1) + (dVal + 1) * (dVal - 1) - (iVal - 3) * Math.Sin(iVal) * Math.Cos(dVal);
            var values = new Dictionary<string, object>() { { "iVal", 2 } };
            var decTransformResult = visitor.Transform(expression, values);
            Console.WriteLine("Value substitute operation expression");
            Console.WriteLine("\tSource: " + expression);
            Console.WriteLine("\tResult: " + decTransformResult);
        }
    }
}
