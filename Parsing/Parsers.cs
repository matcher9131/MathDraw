using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsing
{
    public static class Parsers
    {
        public static Parser<char> CharOf(char c) => new(input => input.StartsWith(c)
            ? Result<char>.Succeed(c, input[1..])
            : Result<char>.Fail(input)
        );

        public static Parser<string> StringOf(string s) => new(input => input.StartsWith(s)
            ? Result<string>.Succeed(s, input[s.Length..])
            : Result<string>.Fail(input)
        );
    }
}
