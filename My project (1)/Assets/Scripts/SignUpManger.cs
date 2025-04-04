using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using System.Linq;
using UnityEditor.PackageManager.Requests;

public class RegisterManger : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button signUpButton;
    public TextMeshProUGUI errorText;

    private string apiUrl = "https://localhost:7223/account/register";

    void Start()
    {
        passwordInput.contentType = TMP_InputField.ContentType.Password;
        signUpButton.onClick.AddListener(OnLoginClicked);
    }

   
   
    void OnLoginClicked()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            errorText.text = "Email and Password are required!";
        }

        else if (password.Length < 10)
        {
            errorText.text = "Password must be at least 10 characters long";
        }

        else if (!password.Any(char.IsLower))
        {
            errorText.text = "Password must contain at least one lowercase letter";
        }

        else if (!password.Any(char.IsUpper))
        {
            errorText.text = "Password must contain at least one uppercase letter";
        }

        else if (!password.Any(char.IsDigit))
        {
            errorText.text = "Password must contain at least one digit";
        }

        else if (!password.Any(c => !char.IsLetterOrDigit(c)))
        {
            errorText.text = "Password must contain at least one special character";
        }

        else
        {
            StartCoroutine(RegisterationRequest(email, password));
        }

         
    }

    IEnumerator RegisterationRequest(string email, string password)
    {
      
        var loginData = new { email = email, password = password };
        string jsonData = JsonConvert.SerializeObject(loginData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl , jsonData , "application/json"))
        {
          
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {

                LoginResponse response = JsonConvert.DeserializeObject<LoginResponse>(request.downloadHandler.text);


                PlayerPrefs.SetString("registerationMassage", "Registratie succesvol! Log in om je registratie te voltooien en toegang te krijgen tot je account!");
                SceneManager.LoadScene("LoginScene");

            }
            else
            {
                errorText.text = "Error: you email is alread exist!";
            }
        }
    }

  
}