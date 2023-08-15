using System.IO;
using System.IO.Compression;


namespace PurpleExplorer.Helpers;
public static class EncodingHelper
{
    public static string Decode(byte[] bytes, string encoding)
    {
        if (encoding == Gzip.Encoding)
        {
            try
            {
                bytes = Gzip.Decompress(bytes);
            }
            catch { }
        }

        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}

public static class Gzip
{
    public const string Encoding = "gzip";
    public static byte[] Decompress(byte[] bytes)
    {
        using (var outStream = new MemoryStream())
        {
            using (var inStream = new MemoryStream(bytes))
            using (var gzStream = new GZipStream(inStream, CompressionMode.Decompress))
            {
                gzStream.CopyTo(outStream);
            }

            return outStream.ToArray();
        }
    }
}
