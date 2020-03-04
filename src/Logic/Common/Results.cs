 public class Result
    {
        private bool IsSuccess { get; }
        public List<Error> Errors { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, List<Error> errorMessages)
        {
            IsSuccess = isSuccess;
            Errors = errorMessages;
        }

        public static Result Fail(List<Error> errors)
        {
            return new Result(false, errors);
        }
        public static Result<T> Fail<T>(List<Error> errors)
        {
            return new Result<T>(default(T), false, errors);
        }
        public static Result Ok()
        {
            return new Result(true, null);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, null);
        }


        public static Result Combine(params Result[] results)
        {
            if (results.Any(r => r.IsFailure))
            {
                return results.FirstOrDefault(r => r.IsFailure);
            }

            return Ok();
        }
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                //this should never happens!
                if (IsFailure)
                {
                    throw new InvalidOperationException();
                }
                return _value;
            }
        }

        protected internal Result(T value, bool isSuccess, List<Error> errorMessage)
            : base(isSuccess, errorMessage)
        {
            _value = value;
        }
    }

    public class Error
    {
        public int Code { get; set; }
        public string Details { get; set; }
    }
