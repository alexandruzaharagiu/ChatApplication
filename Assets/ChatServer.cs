using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ChatServer : MonoBehaviour
{
    private TcpListener server;
    private TcpClient client;

    private int port = 13000;
    private IPAddress serverIp = IPAddress.Parse("127.0.0.1");

    private bool isClientConnected = false;
    private bool isServerRunning = false;
    private Thread serverThread;

    private NetworkStream stream;
    private byte[] data;

    public static ChatServer Instance;
    
    void Awake()
    {
        Instance = this;
    }

    public void Host(string ip)
    {
        serverIp = IPAddress.Parse(ip);
        server = new TcpListener(serverIp, port);
        server.Start();
        isServerRunning = true;
        Debug.Log("SERVER :: Start");
        serverThread = new Thread(receiverThread);
        serverThread.Start();

    }

    // Update is called once per frame
    void Update()
    {

    }
    //Recieve messages
    void receiverThread()
    {
        while (isServerRunning)
        {
            if (isClientConnected == false)
            {
                client = server.AcceptTcpClient();
                Debug.Log("SERVER :: Client Connected");
                isClientConnected = true;
                stream = client.GetStream();
            }
            else
            {
                //receive the messages
                data = new byte[256];
                string msg = string.Empty;
                int bytes = stream.Read(data, 0, data.Length);
                msg = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Debug.Log("SERVER :: Message received = " + msg);
                UnityMainThreadDispatcher.Instance().Enqueue(() => ChatScreen.Instance.ShowMessage("Client: " + msg));
            }
            Thread.Sleep(100);
        }
    }

    //Send message logic

    public void Send(string msg)
    {

        byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
        stream.Write(data, 0, data.Length);

    }

    private void OnDisable()
    {
        isClientConnected = false;
        isServerRunning = false;
        stream.Close();
        server.Stop();
    }
}