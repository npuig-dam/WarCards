using UnityEngine;
using static AttackCard;

public class DefenseType : Cards
{
    //Varaible de defensa, similar en funcionament amb el dmg
    //Encara queden moltes variables per aplicar
    public int deff;



    //Referencia directe al ActualDeckManager
    public ActualDeckManager actualDeckManager;
    public Player player;

    void Awake()
    {
        //Posem la variable de dany verdader (ignorar armadura) en fals
        trueDmg = false;

        //Com son prefabs les referencies als scripts de l'escena s'gafen a l'awake, 
        //es fa el mateix que en el DragObject2D
        handManager = FindObjectOfType<HandManager>();
        actualDeckManager = FindObjectOfType<ActualDeckManager>();
        player = FindObjectOfType<Player>();

    }

    //Creacio de la enum
    public CardName cardName;

    //Enum amb els noms de les cartes, servira per el switch i per editar els prefabs de forma més eficaç
    //Totes les cartes de la categoria Attack tenen el seu nom aqui
    public enum CardName
    {
        LittleShield,
        MagicShield
    }
    public override void OnPlay()
    {

        switch (cardName)
        {
            //A cada case s'hi atribueix un dmg i en alguns casos un estat
            //els estats els gestiona el CombatManager i el CombatUnit
            case CardName.LittleShield:
                deff = 3;
                //Metode per aconseguir un estat
                CombatManager.Instance.GetShield(owner, deff, 0);
                break;
            case CardName.MagicShield:
                deff = 2;
                //Metode per aconseguir un estat
                CombatManager.Instance.GetShield(owner, deff, player.cardsPlayed);
                break;
            default:
                break;
        }
    }
}
