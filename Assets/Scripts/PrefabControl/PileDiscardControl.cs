using TMPro;
using UnityEngine;

public class PileDiscardControl : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI discardCount;

    [SerializeField]
    public DeckManager deckManager;
    private void Awake()
    {
        
    }

    public void UpdateDiscards()
    {
        discardCount.text = deckManager.discardPile.Count.ToString();
    }
}
