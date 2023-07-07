using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsing.Parsers
{
    public static class Basic
    {
        /// <summary>
        /// 特定の文字をパースするパーサを新たに作成する
        /// </summary>
        /// <param name="c">文字</param>
        /// <returns>新しいパーサ</returns>
        public static Parser<char> CharOf(char c) => new Parser<char>(input => input.StartsWith(c)
            ? Result<char>.Succeed(c, input[1..])
            : Result<char>.Fail(input)
        );

        /// <summary>
        /// 特定の条件を満たす文字をパースするパーサを新たに作成する
        /// </summary>
        /// <param name="cond">文字が満たすべき条件を記述した関数</param>
        /// <returns>新しいパーサ</returns>
        public static Parser<char> CharOf(Func<char, bool> cond) => new(input => input.Length > 0 && cond(input[0])
            ? Result<char>.Succeed(input[0], input[1..])
            : Result<char>.Fail(input)
        );

        /// <summary>
        /// 特定の文字列をパースするパーサを新たに作成する
        /// </summary>
        /// <param name="s">文字列</param>
        /// <returns>新しいパーサ</returns>
        public static Parser<string> StringOf(string s) => new(input => input.StartsWith(s)
            ? Result<string>.Succeed(s, input[s.Length..])
            : Result<string>.Fail(input)
        );

        /// <summary>
        /// 連続する空白文字列をパースするパーサを新たに作成する
        /// </summary>
        /// <remarks>このパーサは空白文字がない場合も成功を表す結果を返し、得られる値は空文字列になる</remarks>
        /// <returns>新しいパーサ</returns>
        public static Parser<string> WhiteSpaces() =>
            from spaces in CharOf(char.IsWhiteSpace).Many()
            select string.Concat(spaces);

        /// <summary>
        /// 算用数字1桁を文字としてパースするパーサを新たに作成する
        /// </summary>
        /// <returns>新しいパーサ</returns>
        public static Parser<char> Digit() => CharOf("0123456789".Contains);


        private static Parser<decimal> UnsignedInteger() =>
            from digits in Digit().AtLeastOne()
            select decimal.Parse(string.Concat(digits));

        private static Parser<decimal> UnsignedDecimal() =>
            from integerPart in Digit().Many()
            from point in CharOf('.')
            from fractionalPart in Digit().AtLeastOne()
            select decimal.Parse(string.Concat(integerPart.Append(point).Concat(fractionalPart)));

        private static Parser<decimal> UnsignedNumber() => UnsignedDecimal().OrElse(UnsignedInteger());

        /// <summary>
        /// 1つの数をパースするパーサを新たに作成する
        /// </summary>
        /// <returns>新しいパーサ</returns>
        public static Parser<decimal> Number() =>
            (from _ in CharOf('+').Option()
             from number in UnsignedNumber()
             select number)
            .OrElse(
                from negativeSign in CharOf('-')
                from number in UnsignedNumber()
                select -number
            );

        /// <summary>
        /// 識別子として使える文字のうち数字でないものをパースするパーサを新たに作成する
        /// </summary>
        /// <returns>新しいパーサ</returns>
        public static Parser<char> NonDigit() => CharOf("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_".Contains);

        /// <summary>
        /// 識別子として使える文字列をパースするパーサを新たに作成する
        /// </summary>
        /// <returns>新しいパーサ</returns>
        public static Parser<string> Identifier() =>
            from head in NonDigit()
            from tail in Digit().OrElse(NonDigit()).Many()
            select string.Concat(tail.Prepend(head));
    }
}
