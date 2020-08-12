namespace DataLayer
{
    internal static class ConnectionStrings
    {
        public static string OnlineConnString { get; } =
            "server=sql6.freemysqlhosting.net;user id=sql6114920;password=I6IYmqJPs9;database=sql6114920"
            ;

        public static string LocalConnString { get; } =
            "server=localhost;user id=root;password=1234;persistsecurityinfo=True;database=sql6114920";
    }
}