using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : CombatUnit
{
    public new string name = "Player";

    //Per referenciar la barra de vida
    public HealthBar healthBar;

    public ShieldBar shieldBar;

    public PileRemainControl remainControl;

    //Per canviar el text de vida
    [SerializeField]
    public TextMeshProUGUI UIHealth;

    [SerializeField]
    public TextMeshProUGUI UIShield;

    void Awake()
    {

        //S'aplica la vida que a de tindre i l'energia
        //ja que la necessita per tirar les cartes
        currentHP = maxHP;
        currentEnergy = 8;

        Poison = false;
        Bleeding = false;
        Weak = false;
        Fragile = false;
        Strong = false;
        Regen = false;

        UpdateShield();
        remainControl.UpdateRemains();
    }

    //Funcio per aplicar el dany (l'enemic te el mateix de moment)
    public override void TakeDamage(int amount, bool trueDmg)
    {


        if (trueDmg)
        {
            currentHP -= amount;
        }
        else
        {
            if (currentShield > 0)
            {
                //Aqui es fa l'intercanvi entre el dmg i l'escut 
                //utilitzo la funcio Min (matematica) que agafa entre els dos valors 
                //el minim, ja que fa un equilibri perfecte, si el dmg es mes baix que
                //l'escut actual es restara de l'escut, i despres es restara el seu mateix valor
                //evitant aixi que quan s'acaba l'if, no hi ha dany que traslladar
                //Per altre banda si el dmg es mes gran que l'escut, es restara d'aquell i el resultant
                //passara a la resta de vida

                if (Fragile)
                {
                    while ((currentShield > 0)&&(amount > 0))
                    {
                        currentShield -= 2;
                        
                        amount -= 1;
                    }

                    if(currentShield < 0)
                    {
                        currentShield = 0;
                    }
                }
                else
                {
                    int shieldAbsorb = Mathf.Min(currentShield, amount);
                    currentShield -= shieldAbsorb;
                    amount -= shieldAbsorb;
                }


            }


            //El resultant de la operacio anterior es resta a la vida
            currentHP -= amount;


        }

        UpdateShield();
        UpdateHP();


     

        //En cas de que l'enemic es mori
        if (currentHP <= 0)
        {
            Die();
        }
   
    }

    public void UpdateHP()
    {
        //Update de la barra de vida visual
        healthBar.UpdateBar(currentHP);

        //Update del text de la vida
        UIHealth.text = currentHP.ToString();
    }

    public void UpdateShield()
    {
        shieldBar.UpdateBar(currentShield);

        UIShield.text = currentShield.ToString();

    }

    override public void TakeShield(int amount)
    {
        currentShield += amount;
        UpdateShield();

    }

    //Metode per eliminar l'enemic
    protected override void Die()
    {
        Debug.Log(name + " died");
        Destroy(gameObject);
        SceneManager.LoadScene("MenuScene");
    }
}
