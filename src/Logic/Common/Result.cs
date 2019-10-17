using System;

namespace Logic.Common
{
    public class Result
    {
        private bool IsSuccess { get; }
        public string Error { get; }
        public bool IsFailure => !IsSuccess;
        
        protected Result(bool isSuccess, string errorMessage)
        {
            if ((isSuccess && errorMessage != string.Empty) || (!isSuccess && errorMessage == string.Empty))
            {
                throw new InvalidOperationException();
            }
            IsSuccess = isSuccess;
            Error = errorMessage;
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }
        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default(T), false, message); 
        }
        public static Result Ok()
        {
            return new Result(true, string.Empty); 
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value,true, string.Empty);
        }

        public static Result Combine(params Result[] results)
        {
            foreach (Result result in results)
            {
                if (result.IsFailure)
                {
                    return result;
                }
            }
            return Ok();
        }
    }

    public class Result<T> : Result 
    {
        private readonly T _value;


        public T Value {
            get {
                if (IsFailure)
                {
                    throw new InvalidOperationException();
                }
                return _value;
            }
        }

        protected internal Result(T value, bool isSuccess, string errorMessage)
            : base(isSuccess, errorMessage )
        {
            _value = value;
        }
    }
}
