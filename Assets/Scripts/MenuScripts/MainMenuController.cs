using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameManager gameManager;
    public TransitionManager transManager;
    public AudioSource audioController;
    public AudioClip buttonClick;
    public void GoToPlayScene()
    {
        StartCoroutine(ActiveClickSound());
        transManager.StartLeaveSequence("BattleSceneAI");
    }

    public void GoToOptionsScene()
    {
        StartCoroutine(ActiveClickSound());
        transManager.StartLeaveSequence("OptionsMenu");
    }

    public void GoToStatsScene()
    {
        StartCoroutine(ActiveClickSound());
        transManager.StartLeaveSequence("TutoScene");
    }

    public void GoToDeckScene()
    {
        StartCoroutine(ActiveClickSound());
        transManager.StartLeaveSequence("DeckPersonalizer");
    }
    public void GoBackToMenu()
    {
        StartCoroutine(ActiveClickSound());
        transManager.StartLeaveSequence("MenuScene");
    }

    public void GoBackToLogin()
    {
        StartCoroutine(ActiveClickSound());
        transManager.StartLeaveSequence("LoginScene");
    }

    public void ExitTheGame()
    {
        StartCoroutine(ActiveClickSound());
        Application.Quit();
    }

    public IEnumerator ActiveClickSound()
    {
        audioController.PlayOneShot(buttonClick);

        yield return 2f;
    }

}
