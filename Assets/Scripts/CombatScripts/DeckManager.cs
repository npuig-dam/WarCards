using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    //Llista amb els prefabs de la baralla inicial.
    public List<GameObject> startingDeckPrefabs;

    //Llista amb les cartes que encara queden a la baralla
    public List<GameObject> remainingDeck;

    //Llista amb la baralla de descartades
    public List<GameObject> discardPile = new();

    //Referencia al manager dels torns
    public TurnManager turnManager = new();

    //Referencia al gameManager
    public GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        //S'agafa la baralla real del gameManager que l'extreu del DB
        startingDeckPrefabs = new List<GameObject>(gameManager.deckPrefabs);

        //Al començar la baralla s'emplena amb els prefabs de la inicial
        remainingDeck = new List<GameObject>(startingDeckPrefabs);

        //Es bareja la baralla
        Shuffle(remainingDeck);
    }



    //Funcio per agafar una carta de la baralla (retorna un objecte Card)
    public GameObject DrawCard()
    {
        //En cas de que s'hagi d'agafar una carta i no en quedin a la disponible,
        //es fa un refill desde descartes
        if (remainingDeck.Count == 0)
            RefillFromDiscard();

     
        //Tria una carta aleatoria de les disponibles a la baralla
        int index = Random.Range(0, remainingDeck.Count);

        //Escull la carta aleatoria i la guarda
        GameObject prefab = remainingDeck[index];

        //Elimina aquesta carta escollida de la baralla
        remainingDeck.RemoveAt(index);

        //Retorna el prefab de la carta
        return prefab;
    }

  

    //Funcio per descartar la carta seleccionada
    public void DiscardCard(Cards card)
    {
        //La forma de "descartar" es guardar aquest prefab dins de la llista de descartes
        //If per veure si es el torn del player (d'aquesta forma la baralla de descartes nomes hi ha cartes del player)
        if (turnManager.IsPlayerTurn())
        {
            discardPile.Add(card.cardPrefab);
            startingDeckPrefabs.Remove(card.cardPrefab);
        }

    }

    //Funcio que s'activa per reemplenar la baralla principal
    private void RefillFromDiscard()
    {
        //S'agafa tota la llista de descartes i s'aplica a la baralla principal
        remainingDeck = new List<GameObject>(discardPile);

        //Es reinicia la baralla de descartes
        discardPile.Clear();

        //Es torna a barrejar la baralla
        Shuffle(remainingDeck);
    }

    //Funcio per fer la barreja de cartes
    private void Shuffle(List<GameObject> list)
    {
        //Es va moguent cada carta a una posicio aleatoria de la llista (es fan dos canvis a la vegada, es podrien fer molts si es volgues)
        for (int i = 0; i < list.Count; i++)
        {
            //Es una forma de randomitzar molt simple
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }
}
