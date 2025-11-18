using TMPro;
using UnityEngine;

public class ChatScreen : MonoBehaviour
{
    public TMP_InputField ChatInput;
    public TextMeshProUGUI ChatText;
    public static ChatScreen Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        //Press Enter to send
        if (ChatInput != null && ChatInput.isFocused && Input.GetKeyDown(KeyCode.Return))
            SendButtonClicked();
    }

    public void SendButtonClicked()
    {
        if (string.IsNullOrWhiteSpace(ChatInput.text))
            return;

        string msg = ChatInput.text.Trim();

        if (MainMenuScreen.IsServer)
        {
            ChatServer.Instance.Send(msg);
            ShowMessage("Server : " + msg);
        }
        else
        {
            ChatClient.Instance.Send(msg);
            ShowMessage("Client : " + msg);
        }

        ChatInput.text = "";
        ChatInput.ActivateInputField();
    }

    public void ShowMessage(string text)
    {
        ChatText.text += text + "\n";
    }

    public void OnDisconnectClicked()
    {
        Debug.Log("Disconnect clicked");

        try
        {
            //Stop the server or client safely
            if (MainMenuScreen.IsServer && ChatServer.Instance != null)
            {
                ChatServer.Instance.enabled = false;
            }
            else if (ChatClient.Instance != null)
            {
                ChatClient.Instance.enabled = false;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Error while disconnecting: " + e.Message);
        }

        //Return to main menu
        var menu = Object.FindFirstObjectByType<MainMenuScreen>(UnityEngine.FindObjectsInactive.Include);

        if (menu != null)
        {
            menu.gameObject.SetActive(true);
        }

        //Hide chat screen
        gameObject.SetActive(false);
    }
}
