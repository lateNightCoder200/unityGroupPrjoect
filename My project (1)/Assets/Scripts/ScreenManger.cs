using UnityEngine;
using UnityEngine.SceneManagement;
public class ScreenManger : MonoBehaviour
{
     

    public void GoToSignUpPage()
    {
        SceneManager.LoadScene("SignUpScene");
    }

    public void GoToLoginPage()
    {
        SceneManager.LoadScene("LoginScene");
    }
    

    public void GoToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void Exit()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("StartScene");
    }

    public void GoToSetPatientInfoScene()
    {
        SceneManager.LoadScene("SetPatientInfo");
    }

    public void GoToHomeScene()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void GoToMyProfileScene()
    {
        SceneManager.LoadScene("MyProfileScene");
    }

    public void GoToUpdatePatientInfoScene()
    {
        SceneManager.LoadScene("UpdatePatientInfoScene");
    }

  
    public void GoToUpdateUserNameScene()
    {
        SceneManager.LoadScene("UpdateUserNameScene");
    }
    

    public void GoToDageBoekMainScene()
    {
        SceneManager.LoadScene("DageBoekMainScene");
    }

    

    public void GoToAddNoteScene()
    {
        SceneManager.LoadScene("AddNoteScene");
    }
}
