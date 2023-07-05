namespace Parsing.Tests
{
    public class ParsersTest
    {
        [Theory]
        [InlineData("abc", 'a', true, 'a', "bc")]
        [InlineData("a", 'a', true, 'a', "")]
        [InlineData("abc", 'd', false, default(char), "abc")]
        [InlineData("", 'a', false, default(char), "")]
        public void CharOfTest(string input, char c, bool expectedIsSuccess, char expectedValue, string expectedRemain)
        {
            Result<char> result = Parsers.CharOf(c).Parse(input);

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
            Result<char> result = Parsers.CharOf(cond).Parse(input);

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
            Result<string> result = Parsers.StringOf(s).Parse(input);

            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedValue, result.Value);
            Assert.Equal(expectedRemain, result.Remain);
        }
    }
}
