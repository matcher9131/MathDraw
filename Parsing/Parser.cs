using System.ComponentModel;

namespace Parsing
{
    /// <summary>
    /// パーサを表すクラス
    /// </summary>
    /// <typeparam name="T">パースして得られる値の型</typeparam>
    public class Parser<T>
    {
        private readonly Func<string, Result<T>> f;

        /// <summary>
        /// 入力文字列からパース結果を得る関数を指定して新しいインスタンスを初期化する
        /// </summary>
        /// <param name="f"></param>
        public Parser(Func<string, Result<T>> f)
        {
            this.f = f;
        }

        /// <summary>
        /// パースを行う
        /// </summary>
        /// <param name="input">入力文字列</param>
        /// <returns>パース結果</returns>
        public Result<T> Parse(string input) => this.f(input);

        /// <summary>
        /// パースして得られる値を変換する関数を指定して新しいパーサを作成する
        /// </summary>
        /// <typeparam name="TResult">変換先の型</typeparam>
        /// <param name="selector">パースして得られる値を変換する関数</param>
        /// <returns>新しいパーサ</returns>
        public Parser<TResult> Select<TResult>(Func<T?, TResult> selector) => new(input =>
        {
            var (isSuccess, value, remain) = this.Parse(input);
            return isSuccess ? Result<TResult>.Succeed(selector(value), remain) : Result<TResult>.Fail(remain);
        });

        /// <summary>
        /// パースして得られる値を変換する関数を2つ指定して、連続してパースを行うパーサを新たに作成する
        /// </summary>
        /// <typeparam name="T1">1回目のパースで得られる値の型</typeparam>
        /// <typeparam name="TResult">2回目のパースで得られる値の型</typeparam>
        /// <param name="selector1">1回目のパーサを作成する関数</param>
        /// <param name="resultSelector">2回目のパースで得られる値を変換する関数</param>
        /// <returns>新しいパーサ</returns>
        public Parser<TResult> SelectMany<T1, TResult>(
            Func<T?, Parser<T1>> selector1,
            Func<T?, T1?, TResult> resultSelector
        ) => new(input =>
        {
            var (isSuccess1, value1, remain1) = this.Parse(input);
            if (isSuccess1)
            {
                var (isSuccess2, value2, remain2) = selector1(value1).Parse(remain1);
                if (isSuccess2)
                {
                    return Result<TResult>.Succeed(resultSelector(value1, value2), remain2);
                }
            }
            return Result<TResult>.Fail(input);
        });

        /// <summary>
        /// このインスタンスによるパースが成功すればその結果を返し、失敗すれば2つ目のパーサによるパース結果を返すようなパーサを新たに作成する
        /// </summary>
        /// <param name="other">2つ目のパーサ</param>
        /// <returns>新しいパーサ</returns>
        public Parser<T> OrElse(Parser<T> other) => new(input =>
        {
            var result = this.Parse(input);
            return result.IsSuccess ? result : other.Parse(input);
        });

        /// <summary>
        /// このインスタンスによるパースが成功すればその結果を返し、失敗しても成功を表す結果を返すようなパーサを新たに作成する
        /// </summary>
        /// <returns>新しいパーサ</returns>
        public Parser<T> Option() => new(input =>
        {
            var result = this.Parse(input);
            return result.IsSuccess ? result : Result<T>.Succeed(default, input);
        });

        /// <summary>
        /// このインスタンスによるパースを失敗するまで連続して行い、その結果を返すようなパーサを新たに作成する
        /// </summary>
        /// <remarks>この新たなパーサは1回目のパースに失敗しても成功を表す結果を返す</remarks>
        /// <returns>新しいパーサ</returns>
        public Parser<IEnumerable<T?>> Many() => new(input =>
        {
            List<T?> values = new();
            string prevRemain = input;
            while (true)
            {
                var (isSuccess, value, remain) = this.Parse(prevRemain);
                if (!isSuccess || remain == prevRemain) break;
                values.Add(value);
                prevRemain = remain;
            }
            return Result<IEnumerable<T?>>.Succeed(values, prevRemain);
        });

        /// <summary>
        /// このインスタンスによるパースを失敗するまで連続して行い、その結果を返すようなパーサを新たに作成する
        /// </summary>
        /// <remarks>この新たなパーサは1回目のパースに失敗した場合は失敗を表す結果を返す</remarks>
        /// <returns>新しいパーサ</returns>
        public Parser<IEnumerable<T?>> AtLeastOne() =>
            from value in this
            from values in this.Many()
            select values.Prepend(value);
    }
}
