namespace Parsing.Tests
{
    public class ParserTest
    {
        [Fact]
        public void ParseTest()
        {
            static Result<string> f(string input) => Result<string>.Succeed(input, "");
            Parser<string> parser = new(f);

            Result<string> result = parser.Parse("abc");

            Assert.Equal(f("abc"), result);
        }
    }
}