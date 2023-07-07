using static Parsing.Parsers.Basic;

namespace Parsing.Tests.Parsers
{
    public class BasicTest
    {
        [Theory]
        [InlineData("abc", 'a', true, 'a', "bc")]
        [InlineData("a", 'a', true, 'a', "")]
        [InlineData("abc", 'd', false, default(char), "abc")]
        [InlineData("", 'a', false, default(char), "")]
        public void CharOfTest(string input, char c, bool expectedIsSuccess, char expectedValue, string expectedRemain)
        {
            Result<char> result = CharOf(c).Parse(input);

            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedValue, result.Value);
            Assert.Equal(expectedRemain, result.Remain);
        }

        public static readonly object[][] CharOfTest2Data =
        {
            new object[] { "abc", (char c) => c == 'a' || c == 'b', true, 'a', "bc" },
            new object[] { "bcd", (char c) => c == 'a' || c == 'b', true, 'b', "cd" },
            new object[] { "cde", (char c) => c == 'a' || c == 'b', false, default(char), "cde" },
            new object[] { "", (char c) => c == 'a' || c == 'b', false, default(char), "" }
        };
        [Theory]
        [MemberData(nameof(CharOfTest2Data))]
        public void CharOfTest2(string input, Func<char, bool> cond, bool expectedIsSuccess, char expectedValue, string expectedRemain)
        {
            Result<char> result = CharOf(cond).Parse(input);

            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedValue, result.Value);
            Assert.Equal(expectedRemain, result.Remain);
        }

        [Theory]
        [InlineData("abc", "ab", true, "ab", "c")]
        [InlineData("abc", "abc", true, "abc", "")]
        [InlineData("abc", "abcd", false, default(string), "abc")]
        [InlineData("", "abcd", false, default(string), "")]
        public void StringOfTest(string input, string s, bool expectedIsSuccess, string expectedValue, string expectedRemain)
        {
            Result<string> result = StringOf(s).Parse(input);

            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedValue, result.Value);
            Assert.Equal(expectedRemain, result.Remain);
        }

        [Theory]
        [InlineData(" ", true, " ", "")]
        [InlineData(" a", true, " ", "a")]
        [InlineData("\r\na", true, "\r\n", "a")]
        [InlineData("\ta", true, "\t", "a")]
        [InlineData("a", true, "", "a")]
        [InlineData("", true, "", "")]
        public void WhiteSpacesTest(string input, bool expectedIsSuccess, string expectedValue, string expectedRemain)
        {
            Result<string> result = WhiteSpaces().Parse(input);

            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedValue, result.Value);
            Assert.Equal(expectedRemain, result.Remain);
        }


        public static readonly object[][] DigitTestData = "0123456789".Select(c => new object[] { c.ToString(), true, c, "" }).ToArray();
        [Theory]
        [MemberData(nameof(DigitTestData))]
        [InlineData("0.", true, '0', ".")]
        [InlineData("a0", false, default(char), "a0")]
        [InlineData("", false, default(char), "")]
        public void DigitTest(string input, bool expectedIsSuccess, char expectedValue, string expectedRemain)
        {
            Result<char> result = Digit().Parse(input);

            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedValue, result.Value);
            Assert.Equal(expectedRemain, result.Remain);
        }

        public static readonly object[][] NumberTestData =
        {
            new object[] { "123abc", true, 123m, "abc" },
            new object[] { "3.1415", true, 3.1415m, "" },
            new object[] { ".1020", true, 0.1020m, "" },
            new object[] { "123.a", true, 123m, ".a" },
            new object[] { "a123", false, default(decimal), "a123" },
            new object[] { "", false, default(decimal), "" },
            new object[] { "+123abc", true, 123m, "abc" },
            new object[] { "+3.1415", true, 3.1415m, "" },
            new object[] { "+.1020", true, 0.1020m, "" },
            new object[] { "+123.a", true, 123m, ".a" },
            new object[] { "+a123", false, default(decimal), "+a123" },
            new object[] { "+", false, default(decimal), "+" },
            new object[] { "-123abc", true, -123m, "abc" },
            new object[] { "-3.1415", true, -3.1415m, "" },
            new object[] { "-.1020", true, -0.1020m, "" },
            new object[] { "-123.a", true, -123m, ".a" },
            new object[] { "-a123", false, default(decimal), "-a123" },
            new object[] { "-", false, default(decimal), "-" },
        };
        [Theory]
        [MemberData(nameof(NumberTestData))]
        public void NumberTest(string input, bool expectedIsSuccess, decimal expectedValue, string expectedRemain)
        {
            Result<decimal> result = Number().Parse(input);

            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedValue, result.Value);
            Assert.Equal(expectedRemain, result.Remain);
        }

        [Theory]
        [InlineData("abc", true, "abc", "")]
        [InlineData("abc(", true, "abc", "(")]
        [InlineData("0abc", false, default(string), "0abc")]
        [InlineData("_a", true, "_a", "")]
        [InlineData("a123", true, "a123", "")]
        [InlineData("+", false, default(string), "+")]
        [InlineData("", false, default(string), "")]
        [InlineData("foo_bar", true, "foo_bar", "")]
        [InlineData("FooBar", true, "FooBar", "")]
        public void IdentifierTest(string input, bool expectedIsSuccess, string expectedValue, string expectedRemain)
        {
            Result<string> result = Identifier().Parse(input);

            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedValue, result.Value);
            Assert.Equal(expectedRemain, result.Remain);
        }
    }
}
