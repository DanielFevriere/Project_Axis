using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldArea : MonoBehaviour
{
    public bool BeginHidden;

    static float fadeInDuration = 0.5f;
    static float fadeOutDuration = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        if (BeginHidden)
        {
            Renderer[] children;
            children = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in children)
            {
                r.sharedMaterial.SetFloat("_Opacity", 0f);
            }
            
            gameObject.SetActive(false);
        }
    }

    public IEnumerator FadeIn()
    {
        gameObject.SetActive(true);

        float t = 0;

        Renderer[] children;
        children = GetComponentsInChildren<Renderer>();


        while (t < fadeInDuration)
        {
            float alpha = t / fadeInDuration;
            foreach (Renderer r in children)
            {
                r.sharedMaterial.SetFloat("_Opacity", alpha);
            }

            t += Time.deltaTime;
            yield return null;
        }
    }
 
    public IEnumerator FadeOut()
    {
        float t = 0;

        Renderer[] children;
        children = GetComponentsInChildren<Renderer>();


        while (t < fadeOutDuration)
        {
            float alpha = t / fadeOutDuration;
            foreach (Renderer r in children)
            {
                r.sharedMaterial.SetFloat("_Opacity", 1 - alpha);
            }

            t += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

}
