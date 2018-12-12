using System;
using Logic.Common;

namespace Logic.Customers
{
    public class ExpirationDate : ValueObject<ExpirationDate>
    {
        public DateTime? Value { get;}
        public bool IsExpired => (Value != Infinite && Value < DateTime.UtcNow);
        public readonly static ExpirationDate Infinite = new ExpirationDate(null);
        private ExpirationDate(DateTime? value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static  Result<ExpirationDate> Create(DateTime expirationDate)
        {
            //An expiration date can potentially be in the past
            //if (expirationDate< DateTime.UtcNow)    return Result.Fail<ExpirationDate>("The expiration date cannot be in the past");
            return Result.Ok(new ExpirationDate(expirationDate));
        }


        protected override bool EqualsCore(ExpirationDate other)
        {
            return (Value==other.Value) ;
        }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode();
        }

        public static implicit operator DateTime?(ExpirationDate expirationDate)
        {
            return expirationDate.Value;
        }
        
        public static explicit operator ExpirationDate (DateTime? expirationDate)
        {
            if (expirationDate.HasValue)
            {
                return Create(expirationDate.Value).Value;
            }
            return Infinite;
        }

    }
}
