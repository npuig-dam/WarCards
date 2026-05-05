using TMPro;
using UnityEngine;

public class CardInfoUIManager : MonoBehaviour
{
 
    //Referencia a ell mateix
    public static CardInfoUIManager Instance;

    //Referencies dels textos de la UI de l'escena
    // en aquest cas tots estan dins d'un requadre
    public TextMeshProUGUI titleUI;
    public TextMeshProUGUI infoUI1;
    public TextMeshProUGUI infoUI2;
    public TextMeshProUGUI infoUI3;

    //Referencia al canvas de l'escena (no el general, sino el Requadre)
    public GameObject canvas;

    private void Awake()
    {
        //Aquest If s'utilitza perque quan comenci la partida no veiem el requadre de text
        // aixo es fa perque aquest requadre no es pot iniciar directament per culpa dels prefabs
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Funcio per actualitzar el titol del requadre

    public void UpdateCardTitle(string title)
    {
        if (titleUI != null) titleUI.text = title;
    }

    //Funcio per actualitzar la informacio1 del requadre
    public void UpdateCardInfo1(string info)
    {
        if (infoUI1 != null) infoUI1.text = info;
    }

    //Funcio per actualitzar la informacio2 del requadre
    public void UpdateCardInfo2(string info)
    {
        if (infoUI2 != null) infoUI2.text = info;
    }

    //Funcio per actualitzar la informacio3 del requadre
    public void UpdateCardInfo3(string info)
    {
        if (infoUI3 != null) infoUI3.text = info;
    }

    //Funcio per activar o desactivar visualment el canvas del requadre
    public void ToggleInfo(bool state)
    {
        if (canvas != null) canvas.SetActive(state);
    }
}