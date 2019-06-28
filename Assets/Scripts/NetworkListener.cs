using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using System.Threading;

public class NetworkListener
{
    public static event Action<string> onServerMessage;

    private bool isRunning = true;
    private readonly TcpClient client;
    private readonly NetworkStream stream;

    public NetworkListener()
    {
        Debug.Log("[NETWORK] Opening Connection");
        try
        {
            client = new TcpClient("localhost", 8999);
            stream = client.GetStream();

            new Thread(new ThreadStart(() =>
            {
                try
                {
                    while (isRunning)
                    {
                        var message = ReadSync();
                        onServerMessage(message);
                    }
                }
                catch (System.IO.IOException)
                {
                    Debug.Log("[NETWORK] Read Interupted");
                }
            })).Start();
        }
        catch (Exception ex)
        {
            Debug.LogError("[NETWORK] " + ex);
        }
    }

    private string ReadSync()
    {
        var bytesToRead = new byte[client.ReceiveBufferSize];
        var bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);
        return Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
    }

    public void WriteSync(string message)
    {
        var bytesToSend = Encoding.ASCII.GetBytes(message);
        stream.Write(bytesToSend, 0, bytesToSend.Length);
    }

    public void Close()
    {
        Debug.Log("[NETWORK] Closing Connection");
        isRunning = false;
        client?.Close();
    }
}
