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

public class SetPatientnfoSceneManger : MonoBehaviour
{

    public TMP_InputField firstNameInput;
    public TMP_InputField lastNameInput;
    public TMP_InputField cityInput;
    public TMP_InputField birthDateInput;
    public TMP_InputField hospitalInput;
    public TMP_InputField TreatmentPlan;
    public TMP_InputField TteatmentDate;
    public TMP_InputField doctorName;

    public Button submmitButton;
    public TextMeshProUGUI errorText;

    private string apiUrl = "https://localhost:7223/PatientInfo";

    void Start()
    {
        submmitButton.onClick.AddListener(OnClicked);
    }

    void OnClicked()
    {
        string email = PlayerPrefs.GetString("userEmail");

        string firstName = firstNameInput.text;
        string lastName = lastNameInput.text;
        string birthDate = birthDateInput.text;
        string city = cityInput.text;
        string hospital = hospitalInput.text;
        string DoctorName = doctorName.text;
        string treatmentPlan =  TreatmentPlan.text.Trim().ToUpper();
        string treatmentDate = TteatmentDate.text;


        if(treatmentPlan == "A")
            Debug.Log("a");
        if (treatmentPlan == "B")
            Debug.Log("B");
        if (string.IsNullOrEmpty(lastName))
        {
            errorText.text = "Last name is required!";
        }

        else if (string.IsNullOrEmpty(birthDate)){ 
            errorText.text = "Birth date is required!";
        }

        else if (string.IsNullOrEmpty(city)){ 
            errorText.text = "Cityis is required!";
        }
        else if (string.IsNullOrEmpty(hospital)){ 
            errorText.text = "Hospital is required!";
        }
        else if (string.IsNullOrEmpty(treatmentPlan)){ 
            errorText.text = "Treatment Plan is required!";
        }
        else if (string.IsNullOrEmpty(treatmentDate)){ 
            errorText.text = "Treatment Date is required!";
        }
        else if (string.IsNullOrEmpty(DoctorName)){ 
            errorText.text = "Doctor Name is required!";
        }
        else if (treatmentPlan != "A" && treatmentPlan != "B")
        {
            errorText.text = "Treatment Plan should be A or B";
        }
        else
        {

            StartCoroutine(SetPatientInfoRequest(email, firstName, lastName, birthDate, city, hospital, treatmentDate, treatmentPlan, DoctorName));

        }
    }

    IEnumerator SetPatientInfoRequest(string Email, string firstName, string lastName, string birthDate, string city, string hospital, string treatmentDate , string treatmentPlan , string DoctorName)
    {

       
        PatientInfo requestData = new PatientInfo 
        {
            email = Email,
            firstName = firstName,
            lastName = lastName,
            birthDate = birthDate,
            city = city,
            hospital = hospital,
            treatmentDate = treatmentDate,
            treatmentPlan = treatmentPlan,
            DoctorName = DoctorName
            
        };

       

        string jsonData = JsonConvert.SerializeObject(requestData);
   

        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl, jsonData, "application/json"))
        {

            string token = PlayerPrefs.GetString("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }
 

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                SceneManager.LoadScene("SetUserName");
                
            }
            else
            {
                errorText.text = "Error: " + request.downloadHandler.text;

                
            }
        }
    }


}
 
class PatientInfo {

    public string email;
    public string firstName;
    public string lastName;
    public string birthDate;
    public string city;
    public string hospital;
    public string DoctorName;
    public string treatmentDate;
    public string treatmentPlan;
};