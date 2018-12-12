namespace Api.Customers
{
    public class UpdateCustomerDto
    {
        //[Required]
        //[MaxLength(100, ErrorMessage = "Name is too long")]
        public virtual string Name { get; set; }       
    }
}
