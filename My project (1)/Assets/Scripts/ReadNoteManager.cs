using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEditor.PackageManager.Requests;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class ReadNoteManager : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI NoteText;
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


        string noteName = PlayerPrefs.GetString("noteName");
        string email = PlayerPrefs.GetString("userEmail");

        //string noteName = "ali";
        //string email = "abdallah@gmail.com";

        string apiUrl = "https://localhost:7223/GetNote";
        StartCoroutine(FetchNote(apiUrl , noteName , email));
    }

    public IEnumerator FetchNote(string apiUrl, string name , string email)
    {

        sendNote NoteData = new sendNote()
        {
           name = name,
           email = email
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
                
                NoteDTO  note = JsonConvert.DeserializeObject<NoteDTO>(request.downloadHandler.text);
                Title.text = note.Name;
                NoteText.text = note.Note;


            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }


}


class sendNote {
    public string name;
    public string email;
}