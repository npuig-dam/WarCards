using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ActualDeckManager : MonoBehaviour
{
    public Transform CenterTransform;

    public Transform spawnTransform;
    //Distancia de separacio entre les cartes de la ma
    public float cardSpacing = 2f;

    public List<Cards> handCards = new();

    public GameManager gameManager;
    public int handSize;

    public int cardCount;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

    
       handSize = gameManager.deckPrefabs.Count;

        ShowDeck();
    }

    public void ShowDeck()
    {
        for (int i = 0; i < handSize; i++)
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

        card.Scale = cardGO.transform.localScale;

        handCards.Add(card);
        card.inTheMainHand = true;
        UpdateDeckLayout();

    }

    public GameObject DrawCard(int num)
    {

        GameObject prefab = gameManager.deckPrefabs[num];
    
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
            t.localScale = new Vector3(3, 3, 0);
        }

    }

    public GameObject AddNewCard(Cards card)
    {
        cardCount = gameManager.startingDeckPrefabs.Count;

        if (cardCount == 12)
        {
            Debug.Log("Limit de cartes positiu");
            return null;
        }
        else
        {
            GameObject prefab = card.gameObject;
            int id = card.cardId;
            
            handSize += 1;
            gameManager.AddNewCardReal(id);
    
            return prefab;
        }
    }

    public void RemoveACard(Cards card)
    {
        cardCount = gameManager.startingDeckPrefabs.Count;

        if (cardCount == 8)
        {
            Debug.Log("Limit de cartes negatiu");
        }
        else
        {
            GameObject prefab = card.gameObject;
            int id = card.cardId;

            handCards.Remove(card);
            gameManager.RemoveACard(id);
            Destroy(card.gameObject);
            UpdateDeckLayout();
        }
    }
}
