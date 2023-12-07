namespace Application.Interfaces
{
    public interface ICurrentUserService
    {
        string Username { get; }
        string HostServerName { get; }
        string OriginRequest { get; }
        string RoleName { get; }
        List<KeyValuePair<string, string>> Claims { get; }
    }
}