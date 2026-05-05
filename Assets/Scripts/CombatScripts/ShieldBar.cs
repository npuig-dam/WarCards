using UnityEngine;

public class ShieldBar : MonoBehaviour
{
    //Variable de la mida Y
    public int y = 20;
    //Variable del CombatUnit (enemy i player)
    public CombatUnit owner;
    //LLargada maxima de la barra
    public float Width = 30;
    //Varaible de la barra (el transform)
    public RectTransform bar;

    public void UpdateBar(int current)
    {
        //Variable de la nova mida
        float newWidth = ((float)current / owner.maxShield) * Width;
        //Actualitzar la barra
        bar.sizeDelta = new Vector2(newWidth, y);
    }
}
