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
public class NotesManager : MonoBehaviour
{
    public GameObject prefab;
    public Transform gridContainer;


    private string getUrl = "";

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

        string email = PlayerPrefs.GetString("userEmail");

        getUrl = "https://localhost:7223/GetNotes/" + email;
        StartCoroutine(FetchNotes(getUrl));
    }

    public IEnumerator FetchNotes(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            string token = PlayerPrefs.GetString("authToken", "");
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    List<NoteDTO> notes = JsonConvert.DeserializeObject<List<NoteDTO>>(request.downloadHandler.text);
                    DisplayNotes(notes);
                }
                catch (JsonReaderException e)
                {

                    DisplayNotes(new List<NoteDTO>());
                }
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }

    public void DisplayNotes(List<NoteDTO> notes)
    {
        foreach (var note in notes)
        {
            GameObject newPanel = Instantiate(prefab, gridContainer);


            TMP_Text nameText = newPanel.transform.Find("NoteTitle").GetComponent<TMP_Text>();
            nameText.text = note.Name;

            UnityEngine.UI.Button goButton = newPanel.transform.Find("Read").GetComponent<UnityEngine.UI.Button>();
            goButton.onClick.AddListener(() => goToReadNote(note.Name));
 

        }
    }

    private void goToReadNote(string name )
    {
        PlayerPrefs.SetString("noteName" , name);
        PlayerPrefs.Save();
        SceneManager.LoadScene("ReadNoteScene");
    }

}

 