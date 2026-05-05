using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RegisterUIHandler : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField passwordCInput;
    public TMP_InputField betaCodeInput;
    public TextMeshProUGUI errorType;
    public TransitionManager transManager;

    private string trueBetaCode = "Campalans4321";

    private bool registerSuccess;
    private bool usernameAva;

    public async void OnSubmitClick()
    {
        Debug.Log("clica per registrar");
        errorType.text = "";

        string username = usernameInput.text;
        string password = passwordInput.text;
        string passwordC = passwordCInput.text;
        string betaCode = betaCodeInput.text;

        if(betaCode == trueBetaCode)
        {
            Debug.Log("provant nom "+username);
            usernameAva = await GameManager.instance.TryNewUser(username);
            
            if (usernameAva)
            {
                if (password == passwordC)
                {
                    GameManager.instance.CreateNewUser(username, password);

                    SceneManager.LoadScene("LoginScene");
                }
                else
                {
                    errorType.text = "Passwords don't match";
                }
            }
            else
            {
                errorType.text = "This username already exists !";
            }
        }
        else
        {
            errorType.text = "Wrong Beta Code !";
        }

    }



}
