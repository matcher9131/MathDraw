namespace Parsing
{
    /// <summary>
    /// パース結果を表すクラス
    /// </summary>
    /// <typeparam name="T">結果の型</typeparam>
    /// <param name="IsSuccess">パースが成功したかどうかを表す</param>
    /// <param name="Value">パースして得られた値を表す（※パースに失敗したときは<c>default(T)</c>になる）</param>
    /// <param name="Remain">パースして残った文字列を表す</param>
    public record Result<T>(
        bool IsSuccess,
        T? Value,
        string Remain
    )
    {
        /// <summary>
        /// パース結果（成功）を作成するファクトリメソッド
        /// </summary>
        /// <param name="value">パースして得られた値</param>
        /// <param name="remain">残りの文字列</param>
        /// <returns>パース結果</returns>
        public static Result<T> Succeed(T? value, string remain) => new(true, value, remain);

        /// <summary>
        /// パース結果（失敗）を作成するファクトリメソッド
        /// </summary>
        /// <param name="remain">残りの文字列</param>
        /// <returns>パース結果</returns>
        public static Result<T> Fail(string remain) => new(false, default, remain);
    }
}