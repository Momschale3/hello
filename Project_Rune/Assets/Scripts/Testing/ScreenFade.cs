using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    //===================================================================================================================================//

    [Tooltip("Fade into scene on entering it")]
    public bool fadeOnStart = true;
    [Tooltip("duration of the fading process")]
    public float fadeDuration = 2;
    [Tooltip("color of the fading screen")]
    public Color fadeColor;
    Renderer rend;

    bool isDone = true;

    public delegate void OnFadingEnded(bool fadeOut); // bool checks fading type
    public static event OnFadingEnded onFadingEnded;

    //===================================================================================================================================//

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (fadeOnStart) FadeIn();

        PlayerHealth.onPlayerDeath += FadeOut;
        PlayerHealth.onPlayerRevival += FadeIn;
    }

    //===================================================================================================================================//

    public void FadeIn()
    {
        Fade(1, 0, false);
        Debug.LogWarning("FADE IN!");
    }

    //===================================================================================================================================//

    public void FadeOut()
    {
        Debug.LogWarning("FADE OUT!");
        Fade(0, 1, true);
    }

    //===================================================================================================================================//

    void Fade(float alphaIn, float alphaOut, bool fadeOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut, fadeOut));
    }

    //===================================================================================================================================//

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut, bool fadeOut)
    {
        while (!isDone)
        {
            yield return null;
        }
        isDone = false;
        float timer = 0;
        while (timer <= fadeDuration) 
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            
            rend.material.SetColor("_BaseColor", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = fadeColor;
        newColor2.a =  alphaOut;
        rend.material.SetColor("_BaseColor", newColor2);
        isDone = true;
        onFadingEnded?.Invoke(fadeOut);
    }

    //===================================================================================================================================//

}
