using UnityEngine;

public class AttackCard : Cards
{
    //Variable del dmg
    public int damage;

    //Referencia directe al ActualDeckManager
    public ActualDeckManager actualDeckManager;

    void Awake()
    {
        //Posem la variable de dany verdader (ignorar armadura) en fals
        trueDmg = false;

        //Com son prefabs les referencies als scripts de l'escena s'gafen a l'awake, 
        //es fa el mateix que en el DragObject2D
        handManager = FindObjectOfType<HandManager>();
        actualDeckManager = FindObjectOfType<ActualDeckManager>();

    }

    //Creacio de la enum
    public CardName cardName;

    //Enum amb els noms de les cartes, servira per el switch i per editar els prefabs de forma més eficaç
    //Totes les cartes de la categoria Attack tenen el seu nom aqui
    public enum CardName
    {
        BasicBleed,
        QuickSwap,
        Shove,
        DirectThrust,
        HeavyBlunt,
        Hurting,
        VenomDagger,
        HeavyAxe
    }

    //Script que s'executa quan es juga la carta al centre
    public override void OnPlay()
    {

        //El switch rep el nom (enum) per fer que cada carta tingui en efecte diferent
        // cada prefab te una enum diferent seleccionada, aixi que sempre nomes s'executara
        //un sol case del switch al jugar una carta
        switch (cardName)
        {
            //A cada case s'hi atribueix un dmg i en alguns casos un estat
            //els estats els gestiona el CombatManager i el CombatUnit
            case CardName.BasicBleed:
                damage = 2;
                //Metode per aconseguir un estat
                CombatManager.Instance.GetStatus(owner, CombatUnit.Status.Bleeding);
                break;
            case CardName.QuickSwap:
                damage = 1;
                //Aqui s'activa la funcio per afagar una carta mes de la baralla
                handManager.DrawCard();

                break;
            case CardName.Shove:
                damage = 1;
                CombatManager.Instance.GetStatus(owner, CombatUnit.Status.Fragile);
                break;
            case CardName.DirectThrust:
                damage = 5;
                break;
            case CardName.HeavyBlunt:
                damage = 3;
                trueDmg = true;
                break;
            case CardName.Hurting:
                //Aplicacio d'un estat repetides vegades
                CombatManager.Instance.GetStatus(owner, CombatUnit.Status.Bleeding);
                CombatManager.Instance.GetStatus(owner, CombatUnit.Status.Bleeding);
                CombatManager.Instance.GetStatus(owner, CombatUnit.Status.Bleeding);
                break;
            case CardName.VenomDagger:
                damage = 2;
                CombatManager.Instance.GetStatus(owner, CombatUnit.Status.Poison);
                CombatManager.Instance.GetStatus(owner, CombatUnit.Status.Poison);
                break;
            case CardName.HeavyAxe:
                damage = 9;
                break;
            default:
                //En cas de que el script no trobi el nom
                Debug.Log("Enum no existeix");
                break;
        }
        //S'envia el DMG, l'owner i si el dany es verdader o no
        //al combat manager, els estats no es gestionen desde aquest metode
        CombatManager.Instance.DealDamage(owner, damage, trueDmg);
    }



}



