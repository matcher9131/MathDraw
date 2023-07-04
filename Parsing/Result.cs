namespace Parsing
{
    public record Result<T>(bool IsSuccess, T? Value, string Remain)
    {
        public static Result<T> Succeed(T? value, string remain) => new(true, value, remain);
        public static Result<T> Fail(string remain) => new Result<T>(false, default, remain);
    }
}