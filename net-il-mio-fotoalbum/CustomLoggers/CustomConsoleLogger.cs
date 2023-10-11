using System.Diagnostics;

namespace net_il_mio_fotoalbum.CustomLoggers
{
    public class CustomConsoleLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            Debug.WriteLine("LOG: " + message);
        }
    }
}
