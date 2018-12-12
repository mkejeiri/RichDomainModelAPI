using System;
using Logic.Common;

namespace Logic.Customers
{
    public class CustomerName : ValueObject<CustomerName>
    {

        //make it immutable
        public string Value { get; }
        private CustomerName(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static Result<CustomerName> Create(string customerName)
        {
            customerName = (customerName ?? string.Empty).Trim();
            if (customerName.Length == 0 )
            {
                return Result.Fail<CustomerName>("customer name should not be empty");                  
            }

            if (customerName.Length >100 )
            {
                return Result.Fail<CustomerName>("customer name is too long"); 
            }

            return Result.Ok(new CustomerName(customerName)); 
        }

        protected override bool EqualsCore(CustomerName other)
        {
            return Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        protected override int GetHashCodeCore()
        {
           return Value.GetHashCode() ;
        }

        public static implicit operator string (CustomerName customerName)
        {
            return customerName.Value;
        }

        public static explicit operator CustomerName(string name)
        {
            return Create(name).Value;
        }
    }
}
