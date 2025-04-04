using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using System.Linq;

public class SetUserNameSceneManger : MonoBehaviour
{
 
    public TMP_InputField userNameInput;
    public Button submmitButton;
    public TextMeshProUGUI errorText;

    private string apiUrl = "https://localhost:7223/api/User";

    void Start()
    {
        submmitButton.onClick.AddListener(OnClicked);
      
    }

    void OnClicked()
    {
        string email = PlayerPrefs.GetString("userEmail");

        string newUserName = userNameInput.text;

        if ( string.IsNullOrEmpty(newUserName))
        {
            errorText.text = "User name is required!";
        }

        else
        {
            Debug.Log(" good 0");
            StartCoroutine(SetUserNameRequest(email, newUserName));
        }


    }
    public IEnumerator SetUserNameRequest(string Email, string newUserName )
    {
        UserName requestData = new UserName { email = Email, newUserName = newUserName };
        string jsonData = JsonConvert.SerializeObject(requestData);
      
        Debug.Log(Email);
        Debug.Log(" good 2");
        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl, jsonData, "application/json"))
        {

            string token = PlayerPrefs.GetString("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

     
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("good 3");
                SceneManager.LoadScene("HomeScene");  
            }
            else
            {
                Debug.Log("good 4");
                errorText.text = "Error: " + request.downloadHandler.text;
            }
        }
    }
}


 
class UserName
{
    public string email;
    public string newUserName;
}