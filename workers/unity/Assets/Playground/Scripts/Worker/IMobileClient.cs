namespace Playground
{
    public interface IMobileClient
    {
        string IpAddress { get; set; }

        ConnectionScreenController ConnectionScreenController { get; set; }

        void TryConnect();
    }
}
