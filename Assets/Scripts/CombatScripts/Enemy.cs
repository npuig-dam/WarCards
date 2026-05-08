using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : CombatUnit
{
    //Aquesta classe hereda de CombatUnit

    //Per referenciar la barra de vida
    public HealthBar healthBar;

    //Referencia a la barra d'escut
    public ShieldBar shieldBar;

    //Referencia el TMP de vida
    [SerializeField]
    public TextMeshProUGUI UIHealth;

    //Referencia el TMP d'escut
    [SerializeField]
    public TextMeshProUGUI UIShield;

    //Audios
    public AudioSource audioClipControl;
    public AudioClip dmg;
    public AudioClip shield;

    //Variable del nom (de moment no fa res pero mes endevant tindra una importancia)
    public string name = "Enemy";

    void Awake()
    {
        //S'aplica la vida que a de tindre
        currentHP = maxHP;

        //Tots els atributs d'estats comencen desactivats
        Poison = false;
        Bleeding = false;
        Weak = false;
        Fragile = false;
        Strong = false;
        Regen = false;

        //Actualizar l'escut
        UpdateShield();
    }

    //Crea una llista de Prefabs posada desde l'inspector
    public List<GameObject> deckPrefabs;


    //Funcio per escollir una carta de forma aleatoria
    public GameObject DrawRandomCard()
    {
        //De forma aleatoria s'escull una carta de la llista de prefabs, s'instancia i despres s'elimina
        // recordem que totes les accions dels enemics van per temps (coroutina)
        int index = Random.Range(0, deckPrefabs.Count);
        GameObject prefab = deckPrefabs[index];

        return prefab;
    }

    //Funcio per aplicar el dany (el player te el mateix de moment)
    public override void TakeDamage(int amount, bool trueDmg)
    {

        if (trueDmg)
        {
            audioClipControl.PlayOneShot(dmg);
            //Si el trueDmg esta activat rep el dmg ignorant escut
            currentHP -= amount;
        }
        else
        {
            if (currentShield > 0)
            {
                audioClipControl.PlayOneShot(shield);

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
                        Debug.Log(currentShield);
                        amount -= 1;
                        Debug.Log(amount);
                    }
                    if (currentShield < 0)
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

            if (amount > 0) audioClipControl.PlayOneShot(dmg);

            //El resultant de la operacio anterior es resta a la vida
            currentHP -= amount;


        }

        //Update de la barra de vida despres de fer
        //els intercanvis
        UpdateHP();

        //Update de l'escut
        UpdateShield();


        //En cas de que l'enemic es mori
        if (currentHP <= 0)
        {
            gameManager.PlayerWon(true);
            Debug.Log("guanyat " + gameManager.YouWon);
            Die();
        }

    }

    //Funcio per actualitzar la barra de vida
    public void UpdateHP()
    {
        //Update de la barra de vida visual
        healthBar.UpdateBar(currentHP);

        //Update del text de la vida
        UIHealth.text = currentHP.ToString();
    }

    //Funcio per actualitzar la barra d'escut
    public void UpdateShield()
    {
        //Update de la barra d'escut visual
        shieldBar.UpdateBar(currentShield);

        //Update del text de l'escut
        UIShield.text = currentShield.ToString();

    }

    //Funcio per veure l'escut resultant 
    public override void TakeShield(int amount, int s)
    {
        //Es fa la resta i despres l'actualitzacio
        currentShield += amount;
        UpdateShield();

    }

    //Metode per eliminar l'enemic
    public override void Die()
    {
        Debug.Log(name+" a mort");
    
        Debug.Log("enemic " + gameManager.YouWon);
        SceneManager.LoadScene("EndScene");
    }
}

