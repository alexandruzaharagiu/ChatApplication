using TMPro;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour
{

    public TMP_InputField IPAddressInput;
    public GameObject chatScreen;

    public static bool IsServer = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

    public void HostButtonClicked()
    {
        IsServer = true;
        Debug.Log("Host clicked");
        ChatServer.Instance.Host(IPAddressInput.text);
        chatScreen.SetActive(true);

    }

    public void JoinButtonClicked()
    {
        IsServer = false;
        ChatClient.Instance.Join(IPAddressInput.text);
        chatScreen.SetActive(true);
    }
}