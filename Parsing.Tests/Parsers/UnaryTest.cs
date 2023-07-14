using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Parsing.Parsers.Unary;

namespace Parsing.Tests.Parsers
{
    public class UnaryTest
    {
        public static readonly object?[][] VariableTestData =
        {
            new object[] { "abc", true, (Expression)Expression.Parameter(typeof(double), "abc"), "" },
            new object[] { "a0_b1", true, (Expression)Expression.Parameter(typeof(double), "a0_b1"), "" },
            new object[] { "_private", true, (Expression)Expression.Parameter(typeof(double), "_private"), "" },
            new object?[] { "0abc", false, null, "0abc" },
            new object?[] { "+abc", false, null, "+abc" },
            new object?[] { "-abc", false, null, "-abc" },
            new object[] { "abc+def", true, (Expression)Expression.Parameter(typeof(double), "abc"), "+def" },
            new object?[] { "", false, null, "" },
        };
        [Theory]
        [MemberData(nameof(VariableTestData))]
        public void VariableTest(string input, bool expectedIsSuccess, Expression? expectedValue, string expectedRemain)
        {
            Result<Expression> result = Variable().Parse(input);

            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedValue?.ToString() ?? "null", result.Value?.ToString() ?? "null");
            Assert.Equal(expectedRemain, result.Remain);
        }

        public static readonly object?[][] LiteralTestData =
        {
            new object[] { "123abc", true, (Expression)Expression.Constant(123), "abc" },
            new object[] { "123.45abc", true, (Expression)Expression.Constant(123.45), "abc" },
            new object[] { "+123.45abc", true, (Expression)Expression.Constant(123.45), "abc" },
            new object[] { "-123.45abc", true, (Expression)Expression.Constant(-123.45), "abc" },
            new object?[] { "abc", false, null, "abc" },
        };
        [Theory]
        [MemberData(nameof(LiteralTestData))]
        public void LiteralTest(string input, bool expectedIsSuccess, Expression? expectedValue, string expectedRemain)
        {
            Result<Expression> result = Literal().Parse(input);

            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedValue?.ToString() ?? "null", result.Value?.ToString() ?? "null");
            Assert.Equal(expectedRemain, result.Remain);
        }


        public static readonly object?[][] PrimaryExprTestData =
        {
            new object[] { "123.45abc", true, (Expression)Expression.Constant(123.45), "abc" },
            new object[] { "_private", true, (Expression)Expression.Parameter(typeof(double), "_private"), "" },
            new object[] { "abc+def", true, (Expression)Expression.Parameter(typeof(double), "abc"), "+def" },
            new object?[] { "+abc", false, null, "+abc" },
            new object?[] { "-abc", false, null, "-abc" },
        };
        [Theory]
        [MemberData(nameof(PrimaryExprTestData))]
        public void PrimaryExprTest(string input, bool expectedIsSuccess, Expression? expectedValue, string expectedRemain)
        {
            Result<Expression> result = PrimaryExpr().Parse(input);

            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedValue?.ToString() ?? "null", result.Value?.ToString() ?? "null");
            Assert.Equal(expectedRemain, result.Remain);
        }

        public static readonly object?[][] UnaryExprTestData =
        {
            new object[] { "123.45abc", true, (Expression)Expression.Constant(123.45), "abc" },
            new object[] { "_private", true, (Expression)Expression.Parameter(typeof(double), "_private"), "" },
            new object[] { "abc+def", true, (Expression)Expression.Parameter(typeof(double), "abc"), "+def" },
            new object[] { "+abc", true, (Expression)Expression.Parameter(typeof(double), "abc"), "" },
            new object[] { "-abc", true, (Expression)Expression.Negate(Expression.Parameter(typeof(double), "abc")), "" },
        };
        [Theory]
        [MemberData(nameof(UnaryExprTestData))]
        public void UnaryExprTest(string input, bool expectedIsSuccess, Expression? expectedValue, string expectedRemain)
        {
            Result<Expression> result = UnaryExpr().Parse(input);

            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedValue?.ToString() ?? "null", result.Value?.ToString() ?? "null");
            Assert.Equal(expectedRemain, result.Remain);
        }
    }
}
