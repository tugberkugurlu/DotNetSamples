using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionSample
{
    class Program
    {
        // http://msdn.microsoft.com/en-us/library/bb397951.aspx
        static void Main(string[] args)
        {
            // Create an expression tree.
            Expression<Func<int, bool>> expressionTree = num => num < 5;

            // Decompose the expression tree.
            ParameterExpression param = expressionTree.Parameters[0];
            BinaryExpression operation = (BinaryExpression)expressionTree.Body;
            ParameterExpression left = (ParameterExpression)operation.Left;
            ConstantExpression right = (ConstantExpression)operation.Right;

            Console.WriteLine("Decomposed expression: {0} => {1} {2} {3}",
                              param.Name, left.Name, operation.NodeType, right.Value);

            Expression<Func<Person, bool>> prExpTree = person => person.Name == "Tugberk";
        }
    }

    public static class AzureSearchQueryBuilder<TEntity> where TEntity : class
    {
        public static IQuery Equals(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public static IQuery And(IQuery firstQuery, IQuery secondQuery)
        {
            throw new NotImplementedException();
        }

        public static IQuery Or(IQuery firstQuery, IQuery secondQuery)
        {
            throw new NotImplementedException();
        }
    }

    public interface IQuery
    {
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
