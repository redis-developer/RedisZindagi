using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable

namespace Zindagi.SeedWork
{
    public interface IResult
    {
        ResultStatus Status { get; }
        IEnumerable<string> Errors { get; }
        List<ValidationError> ValidationErrors { get; }
        Type ValueType { get; }
        object GetValue();
    }

    [DebuggerDisplay("({Status}) {Value}", Name = "Action Result")]
    public class Result<T> : IResult
    {
        public Result(T value)
        {
            Value = value;
            if (Value != null) ValueType = Value.GetType();
        }

        private Result(ResultStatus status) => Status = status;

        public T Value { get; }

        public bool IsSuccess => Status == ResultStatus.Ok;
        public bool IsFailed => !IsSuccess;

        public string SuccessMessage { get; private set; } = string.Empty;

        public Type ValueType { get; private set; }
        public ResultStatus Status { get; private set; } = ResultStatus.Ok;
        public IEnumerable<string> Errors { get; private set; } = new List<string>();
        public List<ValidationError> ValidationErrors { get; private set; } = new();

        public object GetValue() => Value;

        public static implicit operator T(Result<T> result) => result.Value;
        public static implicit operator Result<T>(T value) => Success(value);

        public void ClearValueType() => ValueType = null;

        public PagedResult<T> ToPagedResult(PagedInfo pagedInfo)
        {
            var pagedResult = new PagedResult<T>(pagedInfo, Value)
            {
                Status = Status,
                SuccessMessage = SuccessMessage,
                Errors = Errors,
                ValidationErrors = ValidationErrors
            };

            return pagedResult;
        }

        public static Result<T> Success(T value) => new(value);

        public static Result<T> Success(T value, string successMessage) => new(value) { SuccessMessage = successMessage };

        public static Result<T> Error(params string[] errorMessages) => new(ResultStatus.Error) { Errors = errorMessages };

        public static Result<T> Invalid(List<ValidationError> validationErrors) => new(ResultStatus.Invalid) { ValidationErrors = validationErrors };

        public static Result<T> NotFound() => new(ResultStatus.NotFound);

        public static Result<T> Forbidden() => new(ResultStatus.Forbidden);
    }
}
