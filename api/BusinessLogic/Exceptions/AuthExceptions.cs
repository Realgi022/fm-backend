
namespace BusinessLogic.Exceptions
{

    //400
    public class InvalidSyntaxException : Exception
    {
        public InvalidSyntaxException()
            : base("The server could not understand the request due to invalid syntax.")
        {
        }
    }
    //401 
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException()
            : base("Invalid email or password.")
        {
        }
    }
    //409 
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException()
            : base("Email already exists")
        {
        }
    }
}
