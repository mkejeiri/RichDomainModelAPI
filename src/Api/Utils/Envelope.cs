using System;

namespace Api.Utils
{
    //This a wrapper on top of the response: so we could have a std response (error/data)
    //less confusion about how to process the responses from the server
    public class Envelope<T>
    {
        public T Result;
        public string ErrorMessage { get; }
        public DateTime TimeGenerated { get; }

        protected internal Envelope(T result, string errorMessage)
        {
            Result = result;
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
            TimeGenerated = DateTime.UtcNow;
        }
    }

    public class Envelope : Envelope<string>
    {
        protected Envelope(string errorMessage) : base(null, errorMessage){}

        public static Envelope<T> Ok<T>(T result)
        {
            return new Envelope<T>(result,null);
        }
        public static Envelope Ok()
        {
            return new Envelope(null);
        }

        public static Envelope Error(string errorMessage)
        {
            return new Envelope(errorMessage);
        }
    }
}
