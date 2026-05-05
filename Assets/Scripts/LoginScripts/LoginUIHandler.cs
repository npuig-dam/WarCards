using TMPro; 
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginUIHandler : MonoBehaviour
{
    
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TransitionManager transManager;
    private bool loginSuccess;

    public async void OnLoginClick()
    {
      
        string user = usernameInput.text;
        string pass = passwordInput.text;

        Debug.Log($"Provant el user {user}");


        loginSuccess  = await GameManager.instance.TryLogin(user, pass);

        if (loginSuccess)
        {
            Debug.Log("Intenta el canvi d'escena");
            transManager.StartLeaveSequence("MenuScene");
       
        }
        else
        {
            //per veure l'error
            Debug.Log("no arriba a temps "+loginSuccess);
        }
    }

    public void OnRegisterClick()
    {
        SceneManager.LoadScene("RegisterScene");
    }
}