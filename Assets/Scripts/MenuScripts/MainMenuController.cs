using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameManager gameManager;
    public TransitionManager transManager;
    public void GoToPlayScene()
    {
        transManager.StartLeaveSequence("BattleSceneAI");
    }

    public void GoToOptionsScene()
    {
        transManager.StartLeaveSequence("OptionsMenu");
    }

    public void GoToStatsScene()
    {
        transManager.StartLeaveSequence("PlayerStats");
    }

    public void GoToDeckScene()
    {
        transManager.StartLeaveSequence("DeckPersonalizer");
    }
    public void GoBackToMenu()
    {
        transManager.StartLeaveSequence("MenuScene");
    }

    public void GoBackToLogin()
    {
        transManager.StartLeaveSequence("LoginScene");
    }

    public void ExitTheGame()
    {

        Application.Quit();
    }

}
