using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class FrameReceiver : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;  // UI RawImage to display the received frame
    [SerializeField] private int port = 5005;   // Port to receive the frames

    private UdpClient udpClient;
    private IPEndPoint endPoint;
    private Texture2D receivedTexture;

    // Thread-safe queue to store incoming frames
    private Queue<byte[]> frameQueue = new Queue<byte[]>();
    private CancellationTokenSource cancellationTokenSource;

    void Start()
    {
        // Initialize the Texture2D for displaying frames
        receivedTexture = new Texture2D(2, 2);

        // Set up the UDP client to receive data
        udpClient = new UdpClient(port);
        endPoint = new IPEndPoint(IPAddress.Any, port);
        udpClient.Client.ReceiveTimeout = 1000;

        // Start a cancellation token for the receive task
        cancellationTokenSource = new CancellationTokenSource();

        // Start the asynchronous UDP receive task
        Task.Run(() => ReceiveFrameAsync(cancellationTokenSource.Token));
    }

    void Update()
    {
        // Check if there are frames in the queue
        if (frameQueue.Count > 0)
        {
            byte[] frameData = frameQueue.Dequeue(); // Dequeue the next frame

            // Update the texture with the frame data on the main thread
            UpdateTexture(frameData);
        }
    }

    async Task ReceiveFrameAsync(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                // Receive data asynchronously
                UdpReceiveResult result = await udpClient.ReceiveAsync();

                byte[] frameData = result.Buffer;

                // Add the received frame data to the queue
                lock (frameQueue)
                {
                    frameQueue.Enqueue(frameData);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error receiving frame: {e.Message}");
        }
        finally
        {
            // Clean up the UDP client
            udpClient.Close();
        }
    }

    // This method will be called to update the texture once the frame data is received
    void UpdateTexture(byte[] frameData)
    {
        // Load the received image data into the texture
        receivedTexture.LoadImage(frameData);

        // Apply the texture to the RawImage
        rawImage.texture = receivedTexture;
    }

    void OnApplicationQuit()
    {
        cancellationTokenSource?.Cancel();
        // Clean up UDP client and cancel the receive task on quit
        if (udpClient != null)
        {
            udpClient.Close();
        }

        // Cancel the receiving task
        
    }
}
