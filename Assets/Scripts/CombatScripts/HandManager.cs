using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    //Rep script de DeckManager desde inspector
    public DeckManager deckManager;
    //Rep script del Player desde inspector
    public Player player;
    //Rep el transform d'un objecte buid que funciona com a "spawn" de la ma
    public Transform handTransform;

    public Transform spawnTransform;
    //Distancia de separacio entre les cartes de la ma
    public float cardSpacing = 0.1f;

 

    //Llista de cartes/prefabs que en el moment d'activar el Draw s'emplenarà de prefabs per simular la ma
    private List<Cards> handCards = new();

    public PileRemainControl remainControl;

    //Metode per iniciar el torn del jugador (handSize es un valor Hardcoded del player)
    public void StartPlayerTurn(int handSize)
    {
        //Activara el metode fins a tindre la ma completa
        while (handCards.Count < handSize)
        {
            DrawCard();
        }
    }

    //Metode per agafar les cartes (emplenar la llista fins al maxim)
    public void DrawCard()
    {
        //Escull un dels prefabs del DeckManager (activa el draw) i el guarda
        GameObject prefab = deckManager.DrawCard();
        if (prefab == null) return;

        //Asigna a la carta el HandManager, aixo permet que la carta pugui interactuar amb aquest script desde un script que ja te a dins
        //s'ha de fer principalment perque el script DragObject2D no se li pot vincular el HandManager desde l'inspector ja que el Drag esta
        //a dins d'un prefab, d'aquesta forma quan es crea una carta de li vincula automatic
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

        // Manually push references so the card doesn't have to "Find" them
        drag.handManager = this;
        drag.player = this.player;

        //Assigna el propietari de la carta i el prefab de la classe (s'assigna a ella mateixa)
        card.owner = player;
        card.cardPrefab = prefab;

        // Es guarda l'escala del prefab original per passar-lo a escala que es necessita (els prefabs estan a escala 4 per veure's millor)
        card.Scale = cardGO.transform.localScale;

        


        //Afegim la carta a la ma i actualitzem la distribucio de les cartes a la ma 
        handCards.Add(card);
        UpdateHandLayout();

        if (remainControl != null)
        {
            remainControl.UpdateRemains();
        }
    }


    //Metode per jugar una carta
    public void PlayCard(Cards card)
    {
        //Debug per comprovar que funciona
        Debug.Log("Activates card");

        //Activa el metode Play de la carta (si es d'alguna variable anira directe a la seva classe filla)
        //Borrem la carta de la ma, l'afegim a la carta a la baralla de descartades i destruim l'objecte, 
        //fent l'efecte de que s'ha mogut de lloc
        card.OnPlay();
        handCards.Remove(card);
        deckManager.DiscardCard(card);
        Destroy(card.gameObject);
  
        //Tornem a actualitzar la posicio de les cartes a la ma
        UpdateHandLayout();
    }

    //Metode per actualitzar les cartes a la ma (nomes del jugador)
    public void UpdateHandLayout()
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
            Vector3 worldPos = handTransform.TransformPoint(localPos);

            //Realment aquests IF eren essencials quan les cartes estaven solapades, ara mateix no tenen gaire funció
            if (sprite != null)
            {
                sprite.sortingOrder = i;
            }

            //If per reiniciar la posicio al deixar de arrastrar la carta
            if (drag != null)
            {
                drag.UpdateBasePosition(worldPos);
            }

            if (drag != null && drag.isDragging) continue;

            t.SetParent(handTransform);
            t.localPosition = localPos;
            t.localRotation = Quaternion.identity;
            t.localScale = handCards[i].Scale;
        }
    }
}
