using UnityEngine;

public class EnergyBar : MonoBehaviour
{

    //Variable del CombatUnit (enemy i player)
    public CombatUnit owner;
    //Varaible de la barra (el transform)
    public Animator anim;

    private void Awake()
    {

    }


    //Update de la barra de vida amb referencia a la vida actual del CombatUnit
    public void UpdateEnergy(int currentEnergy)
    {
        anim.SetInteger("EnergyNow", currentEnergy);
    }
}
