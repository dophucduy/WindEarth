using System.Net; 
using System.Net.Sockets;
using TMPro; 
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button joinBtn;
    
    [Header("UI Elements")]
    [SerializeField] private GameObject connectionPanel; 
    [SerializeField] private TMP_InputField ipInput;   
    [SerializeField] private TMP_Text ipDisplayText;     

    private void Awake()
    {
        hostBtn.onClick.AddListener(() => {
            StartHostGame();
        });

        joinBtn.onClick.AddListener(() => {
            StartClientGame();
        });
    }

    private void StartHostGame()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        
        transport.ConnectionData.ServerListenAddress = "0.0.0.0";
        transport.ConnectionData.Address = "127.0.0.1";

        NetworkManager.Singleton.OnServerStarted += OnServerStarted;


        NetworkManager.Singleton.StartHost();

        connectionPanel.SetActive(false);

        string localIP = GetLocalIPAddress();
        ipDisplayText.gameObject.SetActive(true);
        ipDisplayText.text = "Host IP: " + localIP;

    }

    private void OnServerStarted()
    {
        Debug.Log("Server Started!");
        Debug.Log("Selected Level: " + GameData.SelectedLevel);

        NetworkManager.Singleton.OnServerStarted -= OnServerStarted;

        NetworkManager.Singleton.SceneManager.LoadScene(
            "Level" + GameData.SelectedLevel,
            LoadSceneMode.Single
        );
    }

    private void StartClientGame()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        if (!string.IsNullOrEmpty(ipInput.text))
        {
            transport.ConnectionData.Address = ipInput.text;
        }
        else
        {
            transport.ConnectionData.Address = "127.0.0.1"; 
        }

        NetworkManager.Singleton.StartClient();

        connectionPanel.SetActive(false);
    }

    private string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "Not Found (Use 127.0.0.1)";
    }
}