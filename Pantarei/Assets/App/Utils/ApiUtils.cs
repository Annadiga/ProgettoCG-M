public static class ApiUtils
{
    private static string host = "192.168.137.1";
    private static int port = 3000;

    private static string path_endpoint = "path";
    private static string QR_endpoint = "qr";
    public static string BuildBaseUri()
    {
        return $"http://{host}:{port}";
    }

    public static string BuildUri(string endpoint)
    {
        return endpoint.StartsWith("/") ? $"{BuildBaseUri()}{endpoint}" : $"{BuildBaseUri()}/{endpoint}";
    }

    public static string BuildPathUri(string path)
    {
        return $"{BuildUri(path_endpoint)}?path={path}";
    }

    public static string BuildQrUri(string id)
    {
        return $"{BuildUri(QR_endpoint)}?id={id}";
    }
}
