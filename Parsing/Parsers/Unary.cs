using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parsing.Parsers
{
    public static class Unary
    {
        public static Parser<Expression> Variable() =>
            from name in Basic.Identifier()
            select (Expression)Expression.Parameter(typeof(double), name);

        public static Parser<Expression> Literal() =>
            from number in Basic.Number()
            select (Expression)Expression.Constant(number);

        public static Parser<Expression> PrimaryExpr() =>
            Variable()
            .OrElse(Literal())
            .OrElse(
                from _1 in Basic.CharOf('(')
                from _2 in Basic.WhiteSpaces()
                from relationalExpr in Binary.RelationalExpr()
                from _3 in Basic.WhiteSpaces()
                from _4 in Basic.CharOf(')')
                select relationalExpr
            );

        public static Parser<Expression> UnaryExpr() =>
            (from _ in Basic.CharOf('-')
             from primary in PrimaryExpr()
             select (Expression)Expression.Negate(primary))
            .OrElse(
                from _ in Basic.CharOf('+').Option()
                from primary in PrimaryExpr()
                select primary
            );
    }
}
