using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public Animation animation;
    public Animator animator;
    public Image img;
    public float duration = 1.0f;
    public GameObject gameObject;

   

    private void Awake()
    {
        gameObject.SetActive(false);
        animation = GetComponent<Animation>();
        animator = GetComponent<Animator>();
        img = GetComponent<Image>();
        StartFadeSequence();
    }
    public void StartFadeSequence()
    {
      
        Debug.Log("Começa a difuminar");
        if(SceneManager.GetActiveScene().name != "LoginScene")
        {
            gameObject.SetActive(true);
            img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
            animator.SetInteger("mode", 0);
            animator.SetInteger("mode", 2);
            StartCoroutine(FadeRoutine());

            Debug.Log("Sube el puto telon");
        }
    }

    public void StartLeaveSequence(string sceneName)
    {
        
        gameObject.SetActive(true);
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
        animator.SetInteger("mode", 0);
        animator.SetInteger("mode", 1);
        StartCoroutine(LeaveRoutine(sceneName));
    }

    private IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(duration);


        float currentTime = 0;
        UnityEngine.Color startColor = img.color;
        float startAlpha = startColor.a;
        float targetAlpha = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / duration);

            UnityEngine.Color c = img.color;
            c.a = newAlpha;
            img.color = c;

            yield return null; 
        }


        gameObject.SetActive(false);
    }

    private IEnumerator LeaveRoutine(string sceneName)
    {
        yield return new WaitForSeconds(duration);

        SceneManager.LoadScene(sceneName);
    }
}
