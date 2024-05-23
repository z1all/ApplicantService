using System.Collections.Immutable;

namespace Common.Models.Models
{
    public class ExecutionResult
    {
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

        private bool _isSuccess = false;
        public bool IsSuccess 
        {
            get { return _isSuccess; } 
            set 
            {
                _isSuccess = value;
                if (IsSuccess) StatusCode = StatusCodeExecutionResult.Ok;
            }
        }

        public StatusCodeExecutionResult StatusCode { get; protected set; }

        public ExecutionResult() { }

        public ExecutionResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public ExecutionResult(StatusCodeExecutionResult statusCode, string keyError, params string[] error)
        {
            IsSuccess = false;
            _errors = _errors.Add(keyError, error.ToList());
            StatusCode = statusCode;
        }

        public ExecutionResult(StatusCodeExecutionResult statusCode, ImmutableDictionary<string, List<string>> errors)
        {
            IsSuccess = false;
            _errors = errors;
            StatusCode = statusCode;
        }
    }

    public class ExecutionResult<TSuccessResult> : ExecutionResult /*where TSuccessResult : class*/
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
        public ExecutionResult(TSuccessResult result) { Result = result; }

        public ExecutionResult(StatusCodeExecutionResult statusCode, string keyError, params string[] error) : base(statusCode, keyError, error) { }

        public ExecutionResult(StatusCodeExecutionResult statusCode, ImmutableDictionary<string, List<string>> errors) : base(statusCode, errors) { }
    }
}
