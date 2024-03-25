using System.Collections.Immutable;

namespace UserService.Core.Application.Models
{
    public class ExecutionResult
    {
        public bool IsSuccess { get; protected set; }
        private ImmutableDictionary<string, List<string>> _errors = ImmutableDictionary<string, List<string>>.Empty;
        public ImmutableDictionary<string, List<string>> Errors
        {
            get { return _errors; }
            set 
            { 
                _errors = value;
                IsSuccess = false;
            }
        }

        public ExecutionResult() { }

        public ExecutionResult(string keyError, string error)
        {
            IsSuccess = false;
            _errors = _errors.Add(keyError, [error]);
        }
    }

    public class ExecutionResult<TSuccessResult> : ExecutionResult where TSuccessResult : class
    {
        public TSuccessResult? _result;

        public TSuccessResult? Result
        {
            get { return _result; }
            set
            {
                _result = value;
                IsSuccess = true;
            }
        }

        public ExecutionResult() { }

        public ExecutionResult(string keyError, string error) : base(keyError, error) { }
    }
}
