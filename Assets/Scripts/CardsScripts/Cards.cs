using UnityEngine;

public class Cards : MonoBehaviour
{
    //Variables generals de totes les cartes
    //ID de la carta
    public int cardId;
    //Tier de la carta, afectara a limitacions
    public int cardTier;
    //Variable que s'aplica per Garantir afectes sense debuffs
    public bool trueDmg;
    //Cost de la carta en energia
    public int Cost;
    //Totes les cartes quan s'instancien estan fora de la ma principal
    //D'aquesta forma es pot determinar quines cartes son jugables i quines no
    public bool inTheMainHand = false;

    //Strings d'informacio per el requadre de text de les cartes
    public string information1;
    public string information2;
    public string information3;

    //Referencia al prefab propi
    public GameObject cardPrefab;

    //L'escala real de les cartes (ja que no estan a 1 1)
    public Vector3 Scale;

    //Variable de l'owner actual
    public CombatUnit owner;

    //Referencia al deckManager
    public HandManager handManager;

    //Metodes principals, a cada classe es canvien
    public virtual void OnDraw() { }
    public virtual void OnPlay() { }
    public virtual void OnDiscard() { }
}
