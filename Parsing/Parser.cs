using System.ComponentModel;

namespace Parsing
{
    public class Parser<T>
    {
        private readonly Func<string, Result<T>> f;

        public Parser(Func<string, Result<T>> f)
        {
            this.f = f;
        }

        public Result<T> Parse(string input) => this.f(input);
    }
}
