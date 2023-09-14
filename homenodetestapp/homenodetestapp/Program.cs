using System.Net.Sockets;
using System.Text;

namespace homenodetestapp;

internal class Program
{
    const string bpqNode = "node1";
    const int fbbPort = 8011;
    const string bpqUser = "sysop";
    const string bpqPassword = "rad10";

    static async Task Main(string[] args)
    {
        Console.WriteLine("Connecting to near node...");

        CancellationTokenSource connectTimeoutSource = new(TimeSpan.FromSeconds(10));
        TcpClient tcpClient = new();

        try
        {
            await tcpClient.ConnectAsync(bpqNode, fbbPort, connectTimeoutSource.Token);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Connection failed: " + ex.Message);
            return;
        }

        NetworkStream stream = tcpClient.GetStream();
        StreamReader streamReader = new(stream);
        StreamWriter streamWriter = new(stream) { AutoFlush = true };

        await streamWriter.WriteAsync($"{bpqUser}\r{bpqPassword}\rBPQTERMTCP\r");
        streamReader.WaitToReceive("Connected to TelnetServer\r");
        await streamWriter.WriteAsync("C farapp\r");

        // from the near node
        ReceiveLineToConsole(streamReader);

        // from the far node
        ReceiveLineToConsole(streamReader);
        
        Console.WriteLine("Enter a decimal number to send that byte to the far end");
        while (true)
        {
            var input = Console.ReadLine();

            if (input == "q")
            {
                break;
            }

            if (!byte.TryParse(input, out var b))
            {
                Console.WriteLine("Invalid number");
                continue;
            }

            stream.Write(new[] { b }, 0, 1);
        }

        /*for (byte b = 0; b <= 255; b++)
        {
            Console.WriteLine(b);
            stream.Write(new[] { b }, 0, 1);
            await Task.Delay(250);
        }*/

        /*List<byte> data = new();
        for (int i = 0; i< 256; i++)
        {
            data.Add((byte)i);
        }
        var buffer = data.ToArray();
        stream.Write(buffer, 0, buffer.Length);*/

        tcpClient.Close();
    }

    private static void ReceiveLineToConsole(StreamReader streamReader)
        => Console.WriteLine($"< {streamReader.WaitToReceive("\r").Trim()}");
}