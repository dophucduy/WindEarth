using System.Collections;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LoginController : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text errorText;

    [Header("API")]
    //public string loginUrl = "https://localhost:7068/api/Authentication/login";
    private string loginUrl = "https://winearthserver.onrender.com/api/Authentication/login";


    public void OnLoginClicked()
    {
        errorText.text = "";

        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            errorText.text = "Please enter username and password";
            return;
        }

        StartCoroutine(LoginRequest(username, password));
    }

    IEnumerator LoginRequest(string username, string password)
    {
        LoginRequestBody body = new LoginRequestBody
        {
            username = username,
            password = password
        };

        string json = JsonUtility.ToJson(body);

        using (UnityWebRequest request = new UnityWebRequest(loginUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            Debug.Log("Result: " + request.result);
            Debug.Log("Error: " + request.error);
            Debug.Log("Response: " + request.downloadHandler.text);

            if (request.result != UnityWebRequest.Result.Success)
            {
                errorText.text = request.error;
            }
            else
            {
                HandleLoginSuccess(request.downloadHandler.text);
            }
        }
    }

    void HandleLoginSuccess(string json)
    {
        // Tạm thời coi là login thành công
        Debug.Log("Login success!");

        // Load scene tiếp theo
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    [System.Serializable]
    class LoginRequestBody
    {
        public string username;
        public string password;
    }
    
}
