
namespace Contact.Application.Exceptions;
public class ServiceException : Exception
{
    public ServiceException(string message) : base(message)
    {
    }
}