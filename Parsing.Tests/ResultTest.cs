namespace Parsing.Tests
{
    public class ResultTest
    {
        [Fact]
        public void SucceedTest()
        {
            Result<int> result = Result<int>.Succeed(42, "foo");

            Assert.True(result.IsSuccess);
            Assert.Equal(42, result.Value);
            Assert.Equal("foo", result.Remain);
        }

        [Fact]
        public void FailTest()
        {
            Result<int> result = Result<int>.Fail("foo");

            Assert.False(result.IsSuccess);
            Assert.Equal(default, result.Value);
            Assert.Equal("foo", result.Remain);
        }
    }
}
