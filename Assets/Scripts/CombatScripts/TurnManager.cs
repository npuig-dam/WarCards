using System.Collections;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    //Referencia al Handmanager desde l'inspector
    public HandManager playerHand;

    //Referencia al enemyAI desde l'inspector
    public EnemyAI enemyAI;

    public EnergyBar energyBar;

    //Estadistiques de la ma del jugador i la regeneracio d'energia (podrien estar a la classe player pero no s'utilitza desde alla o sigui que o deixo aqui)
    public int playerHandSize = 5;
    public int energyRegen = 3;

    public GameObject canvasPoison;
    public GameObject canvasBleeding;
    public GameObject canvasStrong;
    public GameObject canvasWeak;
    public GameObject canvasFragile;
    public GameObject canvasRegen;

    public TextMeshProUGUI PoisonCount;
    public TextMeshProUGUI BleedCount;
    public TextMeshProUGUI StrongCount;
    public TextMeshProUGUI RegenCount;
    public TextMeshProUGUI WeakCount;
    public TextMeshProUGUI FragileCount;

    //Referencia al objecte player
    public Player player;

    //Bool per saber si es el torn del player
    public bool playerTurn = true;

    void Start()
    {
        //Comença el torn del jugador quan inicia l'escena
        StartPlayerTurn();

    }

    //Metode pel torn del jugador 
    private void StartPlayerTurn()
    {
        player.cardsPlayed = 0;
        //Es posa en true el torn i s'emplena la ma amb les cartes necessaries
        playerTurn = true;
        playerHand.StartPlayerTurn(playerHandSize);

        //Es fa el reemplenament d'energia (com que el maxim es 6 nomes calen aquestes alternatives)
        // en cas de tindre el maxim no es fa res (a l'iniciar la partida comença amb el maxim)
        if(player.currentEnergy + energyRegen < player.maxEnergy)
        {
            player.currentEnergy += energyRegen;
            Debug.Log("RegeneraEnergia");
        }
        else if (player.currentEnergy + energyRegen >= 8)
        {
            player.currentEnergy = 8;
        }

        energyBar.UpdateEnergy(player.currentEnergy);
      
      
        
    }

    //Metode per finalitzar el torn del jugador
    public void EndPlayerTurn()
    {
        if (player.Poison)
        {
            player.currentHP -= 2;
            player.UpdateHP();
           
        }

        if ((player.Bleeding) && (player.currentShield <= 0))
        {
            player.currentHP -= player.BleedingCharges;
            player.UpdateHP();
        }

        //Al començar la ronda del player es resten les cargues en cas de que l'estat estigui activar
        if (player.Poison) 
        { 
            player.PoisonCharges -= 1;
            PoisonCount.text = player.PoisonCharges.ToString();
        }
        if (player.Weak)
        {
            player.WeakTurns -= 1;
            WeakCount.text = player.WeakTurns.ToString();
        }
        if (player.Strong)
        {
            player.StrongTurns -= 1;
            StrongCount.text = player.StrongTurns.ToString();
        }
        if (player.Regen)
        {
            player.RegenCharges -= 1;
            player.currentHP += 2;
            RegenCount.text = player.RegenCharges.ToString();
        }
        if (player.Fragile)
        {
            player.FragileTurns -= 1;
            FragileCount.text = player.FragileTurns.ToString();
        }


        //En cas de que les cargues arribin a 0 passen a estar en fals
        if (player.PoisonCharges == 0)
        {
            player.Poison = false;
            canvasPoison.SetActive(false);
            PoisonCount.text = "";
        }

        if (player.WeakTurns == 0)
        {
            player.Weak = false;
            canvasWeak.SetActive(false);
            WeakCount.text = "";
        }
        if (player.StrongTurns == 0)
        {
            player.Strong = false;
            canvasStrong.SetActive(false);
            StrongCount.text = "";
        }
        if (player.RegenCharges == 0)
        {
            player.Regen = false;
            canvasRegen.SetActive(false);
            RegenCount.text = "";
        }
        if (player.FragileTurns == 0)
        {
            player.Fragile = false;
            canvasFragile.SetActive(false);
            FragileCount.text = "";
        }
        if (player.BleedingCharges == 0)
        {
            player.Bleeding = false;
            canvasBleeding.SetActive(false);
            BleedCount.text = "";
        }

        //El torn en fals i comença automaticament la corrutina de l'enemic
        playerTurn = false;
        StartCoroutine(EnemyTurn());
    }

    //Fa el torn de l'enemic i després s'activa un altre cop el torn del jugador
    private IEnumerator EnemyTurn()
    {
        yield return enemyAI.PlayEnemyTurn();
        StartPlayerTurn();
    }

    //Metode per veure si es el torn del jugador
    public bool IsPlayerTurn() => playerTurn;
}
