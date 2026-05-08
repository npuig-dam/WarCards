using TMPro;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    //Referencia al ell mateix
    public static CombatManager Instance;

    //Es posen els dos objectes desde l'inspector (ja que son objectes de l'escena), sempre
    //hi haura un player i un enemic respectivament
    public Player player;
    public Enemy enemys;

    //Referencia a tots els elements de efectes de la partida
    //Tant del player com a l'enemy
    public GameObject EnemycanvasPoison;
    public GameObject EnemycanvasBleeding;
    public GameObject EnemycanvasStrong;
    public GameObject EnemycanvasWeak;
    public GameObject EnemycanvasFragile;
    public GameObject EnemycanvasRegen;

    public GameObject PlayercanvasPoison;
    public GameObject PlayercanvasBleeding;
    public GameObject PlayercanvasStrong;
    public GameObject PlayercanvasWeak;
    public GameObject PlayercanvasFragile;
    public GameObject PlayercanvasRegen;

   
    //Referencia a tots els textos dels efectes de la partida
    //Tant del player com a l'enemy
    public TextMeshProUGUI PlayerPoisonCount;
    public TextMeshProUGUI PlayerBleedCount;
    public TextMeshProUGUI PlayerStrongCount;
    public TextMeshProUGUI PlayerRegenCount;
    public TextMeshProUGUI PlayerWeakCount;
    public TextMeshProUGUI PlayerFragileCount;

    public TextMeshProUGUI EnemyPoisonCount;
    public TextMeshProUGUI EnemyBleedCount;
    public TextMeshProUGUI EnemyStrongCount;
    public TextMeshProUGUI EnemyRegenCount;
    public TextMeshProUGUI EnemyWeakCount;
    public TextMeshProUGUI EnemyFragileCount;


    void Awake()
    {
        //Tots els textos comencen en 0/null 
        Instance = this;
        PlayerPoisonCount.text = "";
        PlayerBleedCount.text = "";
        PlayerStrongCount.text = "";
        PlayerRegenCount.text = "";
        PlayerWeakCount.text = "";
        PlayerFragileCount.text = "";
        EnemyPoisonCount.text = "";
        EnemyBleedCount.text = "";
        EnemyStrongCount.text = "";
        EnemyRegenCount.text = "";
        EnemyWeakCount.text = "";
        EnemyFragileCount.text = "";
    }

    //Funcio per activar el DMG
    public void DealDamage(CombatUnit source, int amount, bool trueDmg )
    {
        //Es determina qui ataca a qui
        CombatUnit target = GetOpponent(source);
      
            //Es fa la interaccio del dmg
            target.TakeDamage(amount, trueDmg);
            //Debug per comprovar que funciona amb tot correcte
            Debug.Log(source.name + " dealt " + amount + " damage to " + target.name);
        
    }

    //Funcio per activar el SHIELD
    public void GetShield(CombatUnit source, int amount, int MS)
    {
        //Debug per comprovar que funciona 
        Debug.Log(source.name + " now has " + amount + " shield");

        //Aqui el target (a qui s'aplica) es el mateix que tira la carta
        CombatUnit target = source;

        //Es fa l'aplicacio de l'escut
        target.TakeShield(amount, MS);
    }

    //Funcio per aplicar els estats dels personatges
    public void GetStatus(CombatUnit source, CombatUnit.Status type)
    {
        //Es determina l'enemic ja que hi ha estats que s'apliquen a un mateix i al enemic
        //aquesta llista s'ampliara amb tots els estats

        CombatUnit enemy = GetOpponent(source);

        //Selecciona quin estat s'activa i les cargues que te
        switch (type) 
        {
            case CombatUnit.Status.Poison:

                enemy.Poison = true;
                enemy.PoisonCharges += 2;

                if(source == player)
                {
                    EnemycanvasPoison.SetActive(true);
                    EnemyPoisonCount.text = enemy.PoisonCharges.ToString();
                }
                else
                {
                    PlayercanvasPoison.SetActive(true);
                    PlayerPoisonCount.text = player.PoisonCharges.ToString();
                }
                    break;
            case CombatUnit.Status.Bleeding:
                enemy.Bleeding = true;
                enemy.BleedingCharges += 1;

                if (source == player)
                {
                    EnemycanvasBleeding.SetActive(true);
                    EnemyBleedCount.text = enemy.BleedingCharges.ToString();
                }
                else
                {
                    PlayercanvasBleeding.SetActive(true);
                    PlayerBleedCount.text = player.BleedingCharges.ToString();
                }
                break;
            case CombatUnit.Status.Weak:
                enemy.Weak = true;
                enemy.WeakTurns += 1;

                if (source == player)
                {
                    EnemycanvasWeak.SetActive(true);
                    EnemyWeakCount.text = enemy.WeakTurns.ToString();
                }
                else
                {
                    PlayercanvasWeak.SetActive(true);
                    PlayerWeakCount.text = player.WeakTurns.ToString();
                }
                break;
            case CombatUnit.Status.Fragile:
                enemy.Fragile = true;
                enemy.FragileTurns += 1;

                if (source == player)
                {
                    EnemycanvasFragile.SetActive(true);
                    EnemyFragileCount.text = enemy.FragileTurns.ToString();
                }
                else
                {
                    PlayercanvasFragile.SetActive(true);
                    PlayerFragileCount.text = player.FragileTurns.ToString();
                }
                break;
            case CombatUnit.Status.Strong:
                source.Strong = true;
                source.StrongTurns += 1;

                if (source == enemys)
                {
                    EnemycanvasStrong.SetActive(true);
                    EnemyStrongCount.text = enemy.StrongTurns.ToString();
                }
                else
                {
                    PlayercanvasStrong.SetActive(true);
                    PlayerStrongCount.text = player.StrongTurns.ToString();
                }
                break;
            case CombatUnit.Status.Regen:
                source.Regen = true;
                source.RegenCharges += 1;

                if (source == enemys)
                {
                    EnemycanvasRegen.SetActive(true);
                    EnemyRegenCount.text = enemy.RegenCharges.ToString();
                }
                else
                {
                    PlayercanvasRegen.SetActive(true);
                    PlayerRegenCount.text = player.RegenCharges.ToString();
                }
                break;
            default:
                Debug.Log("No existeix l'enum");
                break;     
        }

        //De forma resumida el Switch mirara quin efecte a d'aplicar, canviara els valors que faci falta, 
        //s'activaran els TMP i les icones de forma respectiva
    }

    //Funcio per determinar qui es el target en cada cas
    private CombatUnit GetOpponent(CombatUnit source)
    {
        //Simplement es mira qui envia la carta i es tria l'altre com a receptor
        if (source == player)
        {
            Debug.Log("Player works");
            return enemys; 
        }
        if (source == enemys)
        {
            Debug.Log("Enemy works");
            return player;
        }

        //En cas de que cap dels dos funcioni (el programa obliga a posar aquesta opcio o no funciona per x motiu)
        return null;
    }
}
