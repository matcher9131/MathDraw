using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsing
{
    public static class Parsers
    {
        /// <summary>
        /// 特定の文字をパースするパーサを新たに作成する
        /// </summary>
        /// <param name="c">文字</param>
        /// <returns>新しいパーサ</returns>
        public static Parser<char> CharOf(char c) => new(input => input.StartsWith(c)
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
    }
}
