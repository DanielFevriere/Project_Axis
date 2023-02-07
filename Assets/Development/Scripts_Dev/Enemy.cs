using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    #region Properties
    SpriteRenderer spriteRenderer;
    Material material;

    [Range(0f, 1f)] public float damageStaggerDuration = 0.5f;
    [ColorUsageAttribute(true, true)] public Color takeDamageColor = new Color(1, 0, 0);
    [SerializeField] AnimationCurve damagedBlendCurve;

    IEnumerator TakeDamageCoroutine;
    #endregion

    public void TakeDamage()
    {
        Debug.Log(name + " took damage");

        if (TakeDamageCoroutine != null)
        {
            StopCoroutine(TakeDamageCoroutine);
        }

        TakeDamageCoroutine = Coroutine_TakeDamage();
        StartCoroutine(TakeDamageCoroutine);
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Take Damage
    IEnumerator Coroutine_TakeDamage()
    {
        yield return null;

        // Set damaged color on material
        //material.SetColor("_TakeDamageColor", takeDamageColor);

        float timeElapsed = 0f;
        while (timeElapsed < damageStaggerDuration)
        {
            timeElapsed += Time.deltaTime;

            // Lerp down from damaged color
            float t = timeElapsed / damageStaggerDuration;
            float value = damagedBlendCurve.Evaluate(t);
            material.SetFloat("_DamagedBlend", value);

            yield return null;
        }
    }

    #endregion
}
