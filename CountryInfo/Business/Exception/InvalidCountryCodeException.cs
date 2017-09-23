namespace Business.Exception
{
    public class InvalidCountryCodeException : System.Exception
    {
        public InvalidCountryCodeException()
            : base("'Country code' is invalid. Please enter a valid 2 or 3 letter ISO code. Example 'BR'. Only alphabets are allowed.")
        {
        }
    }
}
