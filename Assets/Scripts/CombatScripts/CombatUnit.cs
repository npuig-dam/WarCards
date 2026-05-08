using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class CombatUnit : MonoBehaviour
{
    public GameManager gameManager;

    //Variables uniques de vida i escut,
    // tant les variables com les fixes
    //podrien ser constants, pero o deixo 
    // obert per si vull modificar la vida d'algun 
    // dels fills d'aquesta classe
    public int maxHP = 100;
    public int currentHP;
    public int currentShield;
    public int maxShield = 30;

    //Variables d'energia (nomes les utilitza el player de moment)
    public int currentEnergy;
    public int maxEnergy = 8;

    //Estats i variables d'aquests, son els TMP i els icones
    // dels estats que poden rebre els dos, cadascun tindra una 
    // refrencia diferent
    public bool Poison;
    public int PoisonCharges;
    public bool Bleeding;
    public int BleedingCharges;
    public bool Weak;
    public int WeakTurns;
    public bool Fragile;
    public int FragileTurns;
    public bool Strong;
    public int StrongTurns;
    public bool Regen;
    public int RegenCharges;

    //Enum dels estats
    public enum Status
    {
        Poison,
        Bleeding,
        Weak,
        Fragile,
        Strong,
        Regen,
    }

    protected virtual void Awake()
    {
        //Atribueix la vida actual amb la maxima
        currentHP = maxHP;
    }

    //Funcio de l'intercanvi de dmg default
    public virtual void TakeDamage(int amount, bool trueDmg)
    {
        //Revisa si hi ha escut disponible
        if (currentShield > 0)
        {
            //Aplica la resta d'escut que pot haver-hi
            int shieldAbsorb = Mathf.Min(currentShield, amount);
            currentShield -= shieldAbsorb;
            amount -= shieldAbsorb;
        }

        //Resta la vida despres de l'aplicacio de l'escut
        currentHP -= amount;

        //De moment al morir el programa explota, ja que no tinc una pantalla final, de moment
        if (currentHP <= 0)
            Die();
    }


    //Metode per l'atribucio d'escut default
    public virtual void TakeShield(int amount, int s)
    {
        currentShield += amount;
        
    }

    //Funcio editable per a cada unitat
    public abstract void Die();
    


}
