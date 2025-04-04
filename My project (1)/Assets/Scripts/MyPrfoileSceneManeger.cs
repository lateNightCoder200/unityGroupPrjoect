using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;


public class MyPrfoileSceneMnager : MonoBehaviour
{
    //public TMP_InputField nameInput;
    //public TMP_InputField maxLengthInput;
    //public TMP_InputField maxHeightInput;

    public Button editInfoButton;
    public Button setUserNameButton;


    public TextMeshProUGUI userNameText;
    public TextMeshProUGUI firstNameText;
    public TextMeshProUGUI lastNameText;
    public TextMeshProUGUI cityText;
    public TextMeshProUGUI hospitalText;
    public TextMeshProUGUI birthDateText;


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

        string email = PlayerPrefs.GetString("userEmail", "");

        string getUrl = "https://localhost:7223/GetUserInfo?userEmail=" + email;
        StartCoroutine(FetchPatientInfo(getUrl));
    }

    public    IEnumerator  FetchPatientInfo(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            string token = PlayerPrefs.GetString("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {

                Patient patient = JsonConvert.DeserializeObject<Patient>(request.downloadHandler.text);
                UpdateUI(patient);
            }
            else
            {
                //SceneManager.LoadScene("SetPatientInfo");
            }
        }
    }

    private void UpdateUI(Patient patient)
    {
        if (patient != null)
        {
            userNameText.text = patient.userName;
            firstNameText.text = patient.firstName;
            lastNameText.text = patient.lastName;
            cityText.text = patient.city;
            hospitalText.text = patient.hospital;
            birthDateText.text = patient.birthDate;
        }
    }

}

 

class Patient
{
    public string userName;
    public string firstName;
    public string lastName;
    public string birthDate;
    public string city;
    public string hospital;
}