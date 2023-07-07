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
            select (Expression)Expression.Parameter(typeof(decimal), name);

        public static Parser<Expression> Literal() =>
            from number in Basic.Number()
            select (Expression)Expression.Constant(number);

        public static Parser<Expression> PrimaryExpr() => Variable().OrElse(Literal());

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
