using System.Collections.Immutable;

namespace Common.Models
{
    public class ExecutionResult
    {
        public bool IsSuccess { get; protected init; }
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

    public class ExecutionResult<TSuccessResult> : ExecutionResult where TSuccessResult : class
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

        public ExecutionResult(string keyError, string error) : base(keyError, error) { }
    }
}
