using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class RegisterController : MonoBehaviour
{
    [Header("Input")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;

    [Header("UI")]
    public TextMeshProUGUI errorText;
    public GameObject registerPanel;
    public GameObject loginPanel;

    public void OnClickRegister()
    {
        errorText.text = "";

        string username = usernameInput.text.Trim();
        string password = passwordInput.text;
        string confirm = confirmPasswordInput.text;

        // 1️⃣ Validate
        if (string.IsNullOrEmpty(username))
        {
            ShowError("Username is required");
            return;
        }

        if (password.Length < 6)
        {
            ShowError("Password must be at least 6 characters");
            return;
        }

        if (password != confirm)
        {
            ShowError("Passwords do not match");
            return;
        }


        //// 3️⃣ Quay lại Login
        //registerPanel.SetActive(false);
        //loginPanel.SetActive(true);

        // Call API
        StartCoroutine(RegisterCoroutine(username, password));
    }

    void ShowError(string message)
    {
        errorText.text = message;
    }
    void ShowSuccess(string message)
    {
        errorText.color = Color.green;
        errorText.text = message;
    }


    IEnumerator RegisterCoroutine(string username, string password)
    {
        string url = "https://localhost:7068/api/Authentication/register";

        RegisterRequest data = new RegisterRequest
        {
            username = username,
            password = password
        };

        string json = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        // ❌ Lỗi mạng
        if (request.result != UnityWebRequest.Result.Success)
        {
            ShowError("Cannot connect to server");
            yield break;
        }

        // ✅ Backend trả 201 = OK
        if (request.responseCode == 201)
        {
            Debug.Log("Register success");

            ShowSuccess("Register successful! Please login.");

            // delay 1.5s rồi quay lại Login
            yield return new WaitForSeconds(1.5f);

            registerPanel.SetActive(false);
            loginPanel.SetActive(true);
            yield break;
        }

        // ❌ Trường hợp khác
        ShowError("Register failed");
    }



    [System.Serializable]
    public class RegisterRequest
    {
        public string username;
        public string password;
    }

    [System.Serializable]
    public class RegisterResponse
    {
        public bool success;
        public string message;
    }
}
