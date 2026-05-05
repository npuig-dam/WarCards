using UnityEngine;

public class HealthBar : MonoBehaviour
{
    //Script per la gestio de la barra de vida

    //Variable de la mida Y
    public int y = 20;
    //Variable del CombatUnit (enemy i player)
    public CombatUnit owner;
    //LLargada maxima de la barra
    public float Width = 100;
    //Varaible de la barra (el transform)
    public RectTransform bar;


    private void Awake()
    {
   
    }


    //Update de la barra de vida amb referencia a la vida actual del CombatUnit
    public void UpdateBar(int current)
    {
        //Variable de la nova mida
        float newWidth = ((float)current / owner.maxHP) * Width;
        //Actualitzar la barra
        bar.sizeDelta = new Vector2(newWidth, y);
    }
}
