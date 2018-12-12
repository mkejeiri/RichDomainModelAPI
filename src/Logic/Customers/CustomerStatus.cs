using System;
using Logic.Common;

namespace Logic.Customers
{
    public class CustomerStatus : ValueObject<CustomerStatus>
    {
        public static readonly CustomerStatus Regular = new CustomerStatus(CustomerStatusType.Regular, ExpirationDate.Infinite);

        public CustomerStatusType Type { get; }

        private readonly DateTime? _expirationDate;
        public ExpirationDate ExpirationDate => (ExpirationDate)_expirationDate;

        public bool IsAdvanced => Type == CustomerStatusType.Advanced && !ExpirationDate.IsExpired;

        private CustomerStatus()
        {
        }

        private CustomerStatus(CustomerStatusType type, ExpirationDate expirationDate)
            : this()
        {
            Type = type;
            _expirationDate = expirationDate ?? throw new ArgumentNullException(nameof(expirationDate));
        }

        public decimal GetDiscount() => IsAdvanced ? 0.25m : 0m;

        public CustomerStatus Promote()
        {
            return new CustomerStatus(CustomerStatusType.Advanced, (ExpirationDate)DateTime.UtcNow.AddYears(1));
        }

        protected override bool EqualsCore(CustomerStatus other)
        {
            return Type == other.Type && ExpirationDate == other.ExpirationDate;
        }

        protected override int GetHashCodeCore()
        {
            return Type.GetHashCode() ^ ExpirationDate.GetHashCode();
        }
    }


    public enum CustomerStatusType
    {
        Regular = 1,
        Advanced = 2
    }
}

//using System;

//namespace Logic.Entities
//{

//    public class CustomerStatus:ValueObject<CustomerStatus>
//    {

//        //it is not the responsability of the movie class to define the discount, it's up to CustomerStatus
//        public decimal getDiscount() => IsAdvanced ? 0.25m : 0m;
//        //[JsonConverter(typeof(StringEnumConverter))]
//        public CustomerStatusType Status { get; }
//        public static readonly CustomerStatus Regular = new CustomerStatus(CustomerStatusType.Regular, ExpirationDate.Infinite);
//        private CustomerStatus(CustomerStatusType type, ExpirationDate expirationDate): this()
//        {
//            Status = type;
//            _expirationDate = expirationDate;
//        }

//        private CustomerStatus(){}

//        public bool IsAdvanced => Status== CustomerStatusType.Advanced && !ExpirationDate.IsExpired;

//        //public virtual DateTime? StatusExpirationDate { get; set; }
//        private readonly DateTime? _expirationDate; //{ get; set; }
//        public ExpirationDate ExpirationDate
//        {
//            get => (ExpirationDate)_expirationDate;
//            //set => _expirationDate = value;
//        }

//        protected override bool EqualsCore(CustomerStatus other)
//        {
//            return Status == other.Status && ExpirationDate == other.ExpirationDate;
//        }

//        protected override int GetHashCodeCore()
//        {
//            //combine two hashes together
//            return Status.GetHashCode() ^ ExpirationDate.GetHashCode();
//        }

//        public CustomerStatus Promote()
//        {
//            return new CustomerStatus(CustomerStatusType.Advanced, (ExpirationDate)(DateTime.UtcNow.AddYears(1)));
//        }


//    }


//    public enum CustomerStatusType
//    {
//        Regular = 1,
//        Advanced = 2
//    }
//}
