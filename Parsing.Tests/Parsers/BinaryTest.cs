using System.Linq.Expressions;
using static Parsing.Parsers.Binary;

namespace Parsing.Tests.Parsers
{
    public class BinaryTest
    {
        public static readonly object?[][] RelationalExprTestData =
        {
            new object[]
            {
                "abc == 123.4",
                true,
                Expression.Equal(Expression.Parameter(typeof(double), "abc"), Expression.Constant(123.4)),
                ""
            }
        };

        [Theory]
        [MemberData(nameof(RelationalExprTestData))]
        public void RelationalExprTest(string input, bool expectedIsSuccess, Expression? expectedValue, string expectedRemain)
        {
            Result<Expression> result = RelationalExpr().Parse(input);

            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedValue?.ToString() ?? "null", result.Value?.ToString() ?? "null");
            Assert.Equal(expectedRemain, result.Remain);
        }

        public static readonly object?[][] ExponentialExprTestData =
        {
            new object[]
            {
                "abc ** 123.4",
                true,
                Expression.Power(
                    Expression.Parameter(typeof(double), "abc"),
                    Expression.Constant(123.4)
                ),
                ""
            },
            new object[]
            {
                "abc ** 123.4 ** 5.6",
                true,
                Expression.Power(
                    Expression.Parameter(typeof(double), "abc"),
                    Expression.Power(
                        Expression.Constant(123.4),
                        Expression.Constant(5.6)
                    )
                ),
                ""
            },
            new object[] { "_private", true, (Expression)Expression.Parameter(typeof(double), "_private"), "" },
            new object[] { "-abc", true, (Expression)Expression.Negate(Expression.Parameter(typeof(double), "abc")), "" },
        };
        [Theory]
        [MemberData(nameof(ExponentialExprTestData))]
        public void ExponentialExprTest(string input, bool expectedIsSuccess, Expression? expectedValue, string expectedRemain)
        {
            Result<Expression> result = ExponentialExpr().Parse(input);

            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedValue?.ToString() ?? "null", result.Value?.ToString() ?? "null");
            Assert.Equal(expectedRemain, result.Remain);
        }
    }
}
