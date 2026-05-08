using TMPro;
using UnityEngine;

public class TextControl : MonoBehaviour
{
    public TextMeshProUGUI endText;
    public GameManager gameManager;
    public bool prova;


    public void Awake()
    {
 
    }

    public void Start()
    {
        gameManager = GetComponent<GameManager>();

        if (GameManager.instance != null)
        {
            bool win = GameManager.instance.YouWon;
            Debug.Log("jodeeer " + win);

            if (win)
            {
                endText.text = "YOU WON!";
            }
            else
            {
                endText.text = "YOU LOST!";
            }
        }
    }
}
