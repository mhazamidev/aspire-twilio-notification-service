namespace Notification.Application.Services;

public class OtpGenerator
{
    public string Generate()
    {
        var random = new Random();

        return random.Next(100000, 999999).ToString();
    }
}
