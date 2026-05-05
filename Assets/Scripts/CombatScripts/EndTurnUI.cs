using UnityEngine;
using UnityEngine.UI;

public class EndTurnUI : MonoBehaviour
{
    //Es relaciona amb el canvas desde l'inspector
    public TurnManager turnManager;
    public Button endTurnButton;

    void Start()
    {
        //Un Onclick del boto de Acabar torn
        endTurnButton.onClick.AddListener(OnEndTurnButtonClicked);
    }

    //Metode per fer el canvi de torn
    void OnEndTurnButtonClicked()
    {
        //Comprova si es el torn del player
        if (turnManager.IsPlayerTurn())
        {
            //Acaba el torn del jugador i desactiva el boto
            turnManager.EndPlayerTurn();
            endTurnButton.interactable = false;
        }
    }

    void Update()
    {
        //Va comprovant si ja es el torn del jugador i quan ho es, torna a activar el boto
        //Encara no esta preparat per cancelar accions durant el torn de l'enemic
        if (turnManager.IsPlayerTurn())
            endTurnButton.interactable = true;
    }
}
