using TMPro;
using UnityEngine;

public class PileRemainControl : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI remainCount;

    [SerializeField]
    public DeckManager deckManager;
    void Start()
    {
        
    }
    public void UpdateRemains()
    {
        remainCount.text = deckManager.remainingDeck.Count.ToString();
    }
}
