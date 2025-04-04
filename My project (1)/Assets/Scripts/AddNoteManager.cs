using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using Newtonsoft.Json;
using TMPro;


public class AddNoteManager : MonoBehaviour
{

    public TMP_InputField TitleInput;
    public TMP_InputField NoteInput;
    public Button submitButton;
    public TextMeshProUGUI errorText;

    void Start()
    {
        if (PlayerPrefs.HasKey("authToken"))
        {

            Debug.Log("User is logged in");
        }
        else
        {
            Debug.Log("User is not logged in.");

            SceneManager.LoadScene("LoginScene");
        }


        submitButton.onClick.AddListener(OnSubmitClicked);
    }
    void OnSubmitClicked()
    {
        string email = PlayerPrefs.GetString("userEmail");
       

        string apiUrl = "https://localhost:7223/AddNote";
        string noteInput = NoteInput.text;
        string titleInput = TitleInput.text;




        if (string.IsNullOrEmpty(noteInput) || string.IsNullOrEmpty(titleInput))
        {
            errorText.text = "Title and Note are required!";
            return;
        }

        else
        {
            StartCoroutine(AddNoteRequest(email, noteInput , titleInput, apiUrl));
        }
    }
    IEnumerator AddNoteRequest(string email, string noteInput , string titleInput, string apiUrl)
    {

        NoteDTO NoteData = new NoteDTO() { 
            userEmail = email,
            Name = titleInput,
            Note = noteInput,
        };

        string jsonData = JsonConvert.SerializeObject(NoteData);


        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl, jsonData, "application/json"))
        {
            string token = PlayerPrefs.GetString("authToken", "");
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {

                LoginResponse response = JsonConvert.DeserializeObject<LoginResponse>(request.downloadHandler.text);

                
 
                SceneManager.LoadScene("DageBoekMainScene");
 
            }
            else
            {
                errorText.text = "Title is already exist!";
            }
        }
    }
}
public class NoteDTO
{

    public string userEmail { get; set; }
    public string Name { get; set; }
    public string Note { get; set; }
}