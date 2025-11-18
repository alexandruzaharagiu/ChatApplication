using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ChatClient : MonoBehaviour
{
    private TcpClient client;
    private string serverIp = "127.0.0.1";
    private int port = 13000;
    private Thread clientThread;
    private NetworkStream stream;
    public static ChatClient Instance = null;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Join(string ip)
    {
        Debug.Log("CLIENT :: Connecting to server");
        client = new TcpClient(serverIp, port);
        stream = client.GetStream();
        clientThread = new Thread(receiverThread);
        clientThread.Start();

    }

    //IEnumerator sendMessages()
    //{
    //    yield return new WaitForSeconds(1f);
    //    sendMessage("Hello from Client");
    //    yield return new WaitForSeconds(1f);
    //    sendMessage("This is after 1s delay");
    //}

    //Reciveing messages 
    void receiverThread()
    {
        while (client.Connected)
        {

            //receive the messages

            byte[] data = new byte[1024];
            string msg = string.Empty;
            int bytes = stream.Read(data, 0, data.Length);
            msg = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Debug.Log("SERVER :: Message received = " + msg);
            UnityMainThreadDispatcher.Instance().Enqueue(() => ChatScreen.Instance.ShowMessage("Server: " + msg));
            Thread.Sleep(100);
        }
    }


    //Send Messages
    public void Send(string msg)
    {
        byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
        stream.Write(data, 0, data.Length);
    }

    private void OnDisable()
    {
        if (stream != null)
        {
            stream.Close();
            stream = null;
        }

        if (client != null)
        {
            client.Close();
            client = null;
        }

        if (clientThread != null && clientThread.IsAlive)
        {
            clientThread.Abort();
            clientThread = null;
        }
    }

}