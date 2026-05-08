using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //Referencia l'objecte deckManager (script)
    public DeckManager deckManager;
    //Referencia al GameObject enemy, que sempre esta a l'escena
    public Enemy enemy;
    //Transform del gameObject que s'utilitzara per instanciar
    public Transform enemyPlayZone;

    public GameManager gameManager;

    [SerializeField]
    public PlayZoneAnimationController animController;

    public GameObject canvasPoison;
    public GameObject canvasBleeding;
    public GameObject canvasStrong;
    public GameObject canvasWeak;
    public GameObject canvasFragile;
    public GameObject canvasRegen;

    public TextMeshProUGUI EnemyPoisonCount;
    public TextMeshProUGUI EnemyBleedCount;
    public TextMeshProUGUI EnemyStrongCount;
    public TextMeshProUGUI EnemyRegenCount;
    public TextMeshProUGUI EnemyWeakCount;
    public TextMeshProUGUI EnemyFragileCount;

    //Nombre de cartes i el deay (temps) entre aquestes
    public int cardsPerTurn = 3;
    public float delay = 0.5f; 

    //Metode per activar el torn de la IA
    public IEnumerator PlayEnemyTurn()
    {
        
        //Aplica el dany de l'estat Poison
        if (enemy.Poison)
        {
            enemy.currentHP -= 2;
            enemy.UpdateHP();
 
        }

        //Aplica el dany de l'estat Bleeding, nomes s'aplica si el target no te escut
        if ((enemy.Bleeding) && (enemy.currentShield <= 0))
        {
            enemy.currentHP -= enemy.BleedingCharges;
            enemy.UpdateHP();
        }

        //Gestio dels estats pel torn de l'enemic
        //Detecta si l'estat esta activat, en cas d'estar-ho resta cargues
        //El bleeding no decreix
        if (enemy.Poison)
        {
            enemy.PoisonCharges -= 1;
            EnemyPoisonCount.text = enemy.PoisonCharges.ToString();
        }
        if (enemy.Weak)
        {
            enemy.WeakTurns -= 1;
            EnemyWeakCount.text = enemy.WeakTurns.ToString();
        }
        if (enemy.Strong)
        {
            enemy.StrongTurns -= 1;
            EnemyStrongCount.text = enemy.StrongTurns.ToString();
        }
        if (enemy.Regen)
        {
            enemy.RegenCharges -= 1;
            enemy.currentHP += 2;
            EnemyRegenCount.text = enemy.RegenCharges.ToString();
        }
        if (enemy.Fragile)
        {
            enemy.FragileTurns -= 1;
            EnemyFragileCount.text = enemy.FragileTurns.ToString();
        }

        //En cas de quedar-se sense cargues desactiva l'estat i el canvas
        if (enemy.PoisonCharges == 0)
        {
            enemy.Poison = false;
            canvasPoison.SetActive(false);
            EnemyPoisonCount.text = "";
        }

        if (enemy.WeakTurns == 0) 
        { 
            enemy.Weak = false; 
            canvasWeak.SetActive(false);
            EnemyWeakCount.text = "";
        }
        if (enemy.StrongTurns == 0) 
        { 
            enemy.Strong = false; 
            canvasStrong.SetActive(false);
            EnemyStrongCount.text = "";
        }
        if (enemy.RegenCharges == 0) 
        { 
            enemy.Regen = false; 
            canvasRegen.SetActive(false);
            EnemyRegenCount.text = "";
        }
        if (enemy.FragileTurns == 0) 
        { 
            enemy.Fragile = false; 
            canvasFragile.SetActive(false);
            EnemyFragileCount.text = "";
        }
        if(enemy.BleedingCharges == 0)
        {
            enemy.Bleeding = false;
            canvasBleeding.SetActive(false);
            EnemyBleedCount.text = "";
        }

        if(enemy.currentHP <= 0)
        {
            gameManager.PlayerWon(true);
            enemy.Die();
        }

        //Metode que s'activara per agafar les cartes necessaries i jugarles (en aquest cas 3)
        for (int i = 0; i < cardsPerTurn; i++)
        {
            //L'enemic agafa una carta de la seva baralla
            GameObject prefab = enemy.DrawRandomCard();
            if (prefab == null) yield break;

            //L'instancia automacticament
            GameObject cardGO = Instantiate(prefab, enemyPlayZone);
            Cards card = cardGO.GetComponent<Cards>();

            //Atribueix el seu owner l'enemic
            card.owner = enemy;
            card.cardPrefab = prefab;

            animController.EnemyPlays();
            //Es mostra la carta davant del player just a la zona del pilar
            cardGO.transform.localPosition = Vector3.zero;
            cardGO.transform.localRotation = Quaternion.identity;
            cardGO.transform.localScale = prefab.transform.localScale;

            //La juga i la descarta
            card.OnPlay();
            deckManager.DiscardCard(card);

          

            //Acaba el timer i torna a començar
            yield return new WaitForSeconds(delay);

            //Destrueix el clon del prefab
            Destroy(cardGO);
        }


    }



}
