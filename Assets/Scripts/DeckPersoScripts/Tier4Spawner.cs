using System.Collections.Generic;
using UnityEngine;

public class Tier4Spawner : MonoBehaviour
{
    [SerializeField]
    public Transform CenterTransform;

    [SerializeField]
    public AllTiers allTiers;

    [SerializeField]
    public Transform spawnTransform;
    //Distancia de separacio entre les cartes de la ma
    public float cardSpacing = 2f;

    public List<Cards> handCards = new();

    public GameManager gameManager;
    public int handSize;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();


        handSize = gameManager.deckPrefabs.Count;

        ShowDeck();
    }

    public void ShowDeck()
    {
        for (int i = 0; i < allTiers.TierVIPrefabs.Count; i++)
        {
            ShowCard(i);
        }

    }


    public void ShowCard(int num)
    {
        GameObject prefab = DrawCard(num);

        GameObject cardGO = null;
        try
        {
            cardGO = Instantiate(prefab, spawnTransform);
            cardGO.name = prefab.name;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Problema al instanciar un altre p vegada" + e.Message);
        }

        Cards card = cardGO.GetComponent<Cards>();
        DragObject2D drag = cardGO.GetComponent<DragObject2D>();

        if (card == null || drag == null)
        {
            Debug.LogError("Sus muertos no va");
            return;
        }

        cardGO.transform.localScale = new Vector3(1, 1, 0);
        card.inTheMainHand = false;

        handCards.Add(card);

        UpdateDeckLayout();

    }

    public GameObject DrawCard(int num)
    {

        GameObject prefab = allTiers.TierVIPrefabs[num];

        return prefab;
    }


    public void UpdateDeckLayout()
    {
        //Mira cuantes cartes hi ha a la baralla disponible i ho guarda
        int count = handCards.Count;
        if (count == 0) return;

        //Fem el calcul utilitzant quantes cartes hi ha per saber com s'han de distribuir
        //d'aquesta forma queden de forma natural al mig
        float totalWidth = (count - 1) * cardSpacing;
        float startX = -totalWidth / 2f;

        //For per moure cada carta on toca (recordem que nomes s'executa al modificar el nombre de cartes)
        for (int i = 0; i < count; i++)
        {
            if (handCards[i] == null) continue;

            Transform t = handCards[i].transform;
            DragObject2D drag = t.GetComponent<DragObject2D>();
            SpriteRenderer sprite = t.GetComponent<SpriteRenderer>();


            Vector3 localPos = new Vector3(startX + i * cardSpacing, 0f, 0f);
            Vector3 worldPos = CenterTransform.TransformPoint(localPos);


            if (sprite != null)
            {
                sprite.sortingOrder = i;
            }

            if (drag != null)
            {
                drag.UpdateBasePosition(worldPos);
            }

            if (drag != null && drag.isDragging) continue;

            t.SetParent(CenterTransform);
            t.localPosition = localPos;
            t.localRotation = Quaternion.identity;

        }


    }
}
