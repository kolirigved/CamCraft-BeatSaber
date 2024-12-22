using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SendToClient : MonoBehaviour
{
    // send mesg to client in udp
    public string ip = "127.0.0.1";
    public int port = 25003;

    void Start()
    {
        UdpClient client = new UdpClient(ip, port);
        byte[] data = Encoding.UTF8.GetBytes("Start");
        client.Send(data, data.Length);
        client.Close();
    }

    public void StartCamera()
    {
        UdpClient client = new UdpClient(ip, port);
        byte[] data = Encoding.UTF8.GetBytes("Start");
        client.Send(data, data.Length);
        client.Close();
    }

    void StopCamera()
    {
        UdpClient client = new UdpClient(ip, port);
        byte[] data = Encoding.UTF8.GetBytes("Stop");
        client.Send(data, data.Length);
        client.Close();
    }

    public void OnApplicationQuit(){
        UdpClient client = new UdpClient(ip, port);
        byte[] data = Encoding.UTF8.GetBytes("Quit");
        client.Send(data, data.Length);
        client.Close();
    }
    
}
