namespace Sanaap.Service.Contracts
{
    /// <summary>
    /// One time password >> OTP
    /// </summary>
    public interface IOtpNumberGenerator
    {
        int GetOtpNumber();
    }
}
