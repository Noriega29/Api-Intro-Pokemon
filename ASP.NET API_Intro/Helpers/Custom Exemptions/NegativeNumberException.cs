namespace ASP.NET_API_Intro.Exceptions
{
    public class NegativeNumberException : Exception
    {
        public NegativeNumberException() : base ("El número no puede ser negativo.")
        {

        }
    }
}
