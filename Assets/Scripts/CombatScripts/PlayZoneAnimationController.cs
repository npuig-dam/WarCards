using System.Collections;
using UnityEngine;

public class PlayZoneAnimationController : MonoBehaviour
{
    //Referencia del animator de la zona de joc
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    //Metode per activar la primera animacio, amb un petit delay perque es pugui completar l'animacio
    //s'activa per cada carta jugada
    public void Activate()
    {
        anim.SetBool("isPlayed", true);
        StartCoroutine(ReturnToFalse(0.1f, "isPlayed"));
  
    }


    //Metode per desactivar l'animacio
    public IEnumerator ReturnToFalse(float time, string type)
    {
        yield return new WaitForSeconds(time); 
        anim.SetBool(type, false);
    }

    //Metode per activar l'animacio del torn de l'enemic
    public void EnemyPlays()
    {
        anim.SetBool("isEnemyTurn", true);
        StartCoroutine(ReturnToFalse(0.6f, "isEnemyTurn"));

    }
    
}
