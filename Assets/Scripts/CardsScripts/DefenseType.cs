using UnityEngine;

public class DefenseType : Cards
{
    //Varaible de defensa, similar en funcionament amb el dmg
    //Encara queden moltes variables per aplicar
    public int deff;

    public override void OnPlay()
    {
        //Activa la variable per aplicar escut del CombatManager
        CombatManager.Instance.GetShield(owner, deff);
    }

}
