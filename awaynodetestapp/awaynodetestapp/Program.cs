using System.Net;
using System.Net.Sockets;

namespace awaynodetestapp;

internal class Program
{
    static async Task Main(string[] args)
    {
        var tcpListener = new TcpListener(IPAddress.Loopback, 63001);
        tcpListener.Start();

        while (true)
        {
            Console.WriteLine("Listening...");
            using var client = await tcpListener.AcceptTcpClientAsync();
            Console.WriteLine("Accepted client");
            using var stream = client.GetStream();
            using var writer = new StreamWriter(stream) { AutoFlush = true };

            while (true)
            {
                int c;
                try
                {
                    c = stream.ReadByte();
                }
                catch (IOException)
                {
                    Console.WriteLine("Client disconnected");
                    break;
                }

                if (c == -1)
                {
                    Console.WriteLine("Client disconnected");
                    break;
                }

                var formatted = c.ToString("X").ToLower();
                if (formatted.Length == 1)
                {
                    formatted = "0" + formatted;
                }

                string output = $"0x{formatted}  {c:000}  {GetPrintable(c)}";

                Console.WriteLine(output);
                writer.Write(output);
            }
        }
    }

    private static string GetPrintable(int c) => c switch
    {
        8 => "BS",
        10 => "LF",
        13 => "CR",
        >= 0x20 and <= 0x7e => ((char)c).ToString(),
        _ => "",
    };
}
