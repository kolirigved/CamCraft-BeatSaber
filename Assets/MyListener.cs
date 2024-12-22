using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class MyListener : MonoBehaviour
{
    private Thread thread;
    private UdpClient client; // UDP client
    private bool isRunning = true;  // Flag to control thread termination
    public int connectionPort = 25001;
    private IPEndPoint remoteEndPoint; // End point of the client (from which we receive data)
    [SerializeField] public float[] LeftData = new float[3];
    [SerializeField] public float[] RightData = new float[3];

    void Start()
    {
        thread = new Thread(new ThreadStart(GetData));
        thread.IsBackground = true; // Close the thread when the application is closed
        thread.Start();
    }

    void GetData()
    {
        try
        {
            client = new UdpClient(connectionPort); // Create a UDP client
            client.Client.ReceiveTimeout = 1000; // Set the client receive timeout to 1 second
            remoteEndPoint = new IPEndPoint(IPAddress.Any, connectionPort); // Any IP, same port
            Debug.Log("UDP server started, waiting for data...");

            while (isRunning)
            {
                try
                {
                    if (client.Available > 0) // Check if there is data available
                    {
                        byte[] data = client.Receive(ref remoteEndPoint); // Receive data from the client 
                        string text = Encoding.UTF8.GetString(data);
                        ParseData(text);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error in receiving data: " + e.Message);
                }

                Thread.Sleep(1); // Reduce CPU usage
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Server error: " + e.Message);
        }
    }

    void ParseData(string data)
    {
        // Input data coordinates are separated by spaces
        string[] values = data.Split(' ');
        if (values[0] == "Left")
        {
            LeftData[0] = float.Parse(values[1]);
            LeftData[1] = float.Parse(values[2]);
            LeftData[2] = float.Parse(values[3]);
            RightData[0] = float.Parse(values[5]);
            RightData[1] = float.Parse(values[6]);
            RightData[2] = float.Parse(values[7]);
        }
        else if (values[0] == "Right")
        {
            RightData[0] = float.Parse(values[1]);
            RightData[1] = float.Parse(values[2]);
            RightData[2] = float.Parse(values[3]);
            LeftData[0] = float.Parse(values[5]);
            LeftData[1] = float.Parse(values[6]);
            LeftData[2] = float.Parse(values[7]);
        }
    }

    void OnApplicationQuit()
    {
        isRunning = false;  // Stop the thread loop gracefully
        if (thread != null && thread.IsAlive)
        {
            thread.Join(); // Wait for the thread to finish before cleanup
        }

        Cleanup(); // Clean up resources
    }

    void Cleanup()
    {
        if (client != null)
        {
            client.Close();
            client = null;
        }
    }
}
