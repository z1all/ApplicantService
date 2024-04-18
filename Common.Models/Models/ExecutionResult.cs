using System.Collections.Immutable;

namespace Common.Models.Models
{
    public class ExecutionResult
    {
        private ImmutableDictionary<string, List<string>> _errors = ImmutableDictionary<string, List<string>>.Empty;
        public ImmutableDictionary<string, List<string>> Errors
        {
            get { return _errors; }
            init
            {
                _errors = value;
                IsSuccess = false;
            }
        }
        public bool IsSuccess { get; init; }

        public ExecutionResult() { }

        public ExecutionResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public ExecutionResult(string keyError, params string[] error)
        {
            IsSuccess = false;
            _errors = _errors.Add(keyError, error.ToList());
        }
    }

    public class ExecutionResult<TSuccessResult> : ExecutionResult /*where TSuccessResult : class*/
    {
        public TSuccessResult? _result;

        public TSuccessResult? Result
        {
            get { return _result; }
            init
            {
                _result = value;
                IsSuccess = true;
            }
        }

        public ExecutionResult() { }
        public ExecutionResult(TSuccessResult result) { Result = result; }

        public ExecutionResult(string keyError, string error) : base(keyError, error) { }
    }
}
