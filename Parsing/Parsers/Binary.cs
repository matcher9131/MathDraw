using System.Linq.Expressions;

namespace Parsing.Parsers
{
    internal record FollowingExprResultType(string Operator, Expression AdditiveExpr, FollowingExprResultType? FollowingRelationalExpr);

    public static class Binary
    {
        private static Func<Expression, Expression, BinaryExpression> ExpressionFactorySelector(string op) => op switch
            {
                "==" => Expression.Equal,
                "!=" => Expression.NotEqual,
                "<" => Expression.LessThan,
                ">" => Expression.GreaterThan,
                "<=" => Expression.LessThanOrEqual,
                ">=" => Expression.GreaterThanOrEqual,
                "+" => Expression.Add,
                "-" => Expression.Subtract,
                _ => throw new InvalidDataException()
            };

        #region RelationalExpr
        private static Parser<string> RelationalOperator() =>
            Basic.StringOf("==")
            .OrElse(Basic.StringOf("!="))
            .OrElse(Basic.StringOf(">"))
            .OrElse(Basic.StringOf("<"))
            .OrElse(Basic.StringOf(">="))
            .OrElse(Basic.StringOf("<="));

        private static Parser<FollowingExprResultType> FollowingRelationalExpr() =>
            (from _1 in Basic.WhiteSpaces()
             from op in RelationalOperator()
             from _2 in Basic.WhiteSpaces()
             from additiveExpr in AdditiveExpr()
             from _3 in Basic.WhiteSpaces()
             from followingRelationalExpr in FollowingRelationalExpr()
             select new FollowingExprResultType(op, additiveExpr, followingRelationalExpr)
             ).Option();

        private static Expression CreateRelationalExprResult(Expression left, FollowingExprResultType? followingRelationalExpr)
        {
            if (followingRelationalExpr == null)
            {
                return left;
            }
            else
            {
                return CreateRelationalExprResult(
                    ExpressionFactorySelector(followingRelationalExpr.Operator)(left, followingRelationalExpr.AdditiveExpr),
                    followingRelationalExpr.FollowingRelationalExpr
                );
            }
        }

        public static Parser<Expression> RelationalExpr() =>
            from additiveExpr in AdditiveExpr()
            from _1 in Basic.WhiteSpaces()
            from followingRelationalExpr in FollowingRelationalExpr()
            select CreateRelationalExprResult(additiveExpr, followingRelationalExpr);
        #endregion

        #region AdditiveExpr
        private static Parser<char> AdditiveOperator() =>
            Basic.CharOf('+')
            .OrElse(Basic.CharOf('-'));

        private static Parser<FollowingExprResultType> FollowingAdditiveExpr() =>
            (from _1 in Basic.WhiteSpaces()
             from op in AdditiveOperator()
             from _2 in Basic.WhiteSpaces()
             from multiplicativeExpr in MultiplicativeExpr()
             from _3 in Basic.WhiteSpaces()
             from followingAdditiveExpr in FollowingAdditiveExpr()
             select new FollowingExprResultType(op.ToString(), multiplicativeExpr, followingAdditiveExpr)
             ).Option();

        public static Parser<Expression> AdditiveExpr() =>
            from multiplicativeExpr in MultiplicativeExpr()
            from _1 in Basic.WhiteSpaces()
            from followingAdditiveExpr in FollowingAdditiveExpr()
            select CreateRelationalExprResult(multiplicativeExpr, followingAdditiveExpr);
        #endregion

        #region MultiplicativeExpr
        private static Parser<char> MultiplicativeOperator() =>
            Basic.CharOf('*')
            .OrElse(Basic.CharOf('/'))
            .OrElse(Basic.CharOf('%'));

        private static Parser<FollowingExprResultType> FollowingMultiplicativeExpr() =>
            (from _1 in Basic.WhiteSpaces()
             from op in MultiplicativeOperator()
             from _2 in Basic.WhiteSpaces()
             from expotentialExpr in ExponentialExpr()
             from _3 in Basic.WhiteSpaces()
             from followingMultiplicativeExpr in FollowingMultiplicativeExpr()
             select new FollowingExprResultType(op.ToString(), expotentialExpr, followingMultiplicativeExpr)
             ).Option();

        public static Parser<Expression> MultiplicativeExpr() =>
            from exponentialExpr in ExponentialExpr()
            from _1 in Basic.WhiteSpaces()
            from followingMultiplicativeExpr in FollowingMultiplicativeExpr()
            select CreateRelationalExprResult(exponentialExpr, followingMultiplicativeExpr);
        #endregion

        public static Parser<Expression> ExponentialExpr() =>
            (from unaryExpr in Unary.UnaryExpr()
             from _1 in Basic.WhiteSpaces()
             from op in Basic.StringOf("**")
             from _2 in Basic.WhiteSpaces()
             from exponentialExpr in ExponentialExpr()
             select (Expression)Expression.Power(unaryExpr, exponentialExpr)
            ).OrElse(Unary.UnaryExpr());
    }
}
