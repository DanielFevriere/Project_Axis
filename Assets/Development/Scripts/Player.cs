using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public string playerID;
    Material material;
    public bool dead;

    private void Start()
    {
    }

    #region Take Damage
    public void TakeDamage(float DamageTaken)
    {
        Debug.Log(name + " took damage");

        if (TakeDamageCoroutine != null)
        {
            StopCoroutine(TakeDamageCoroutine);
        }

        // Take damage
        TakeDamageCoroutine = Coroutine_TakeDamage();
        StartCoroutine(TakeDamageCoroutine);
        GetComponent<Stats>().ModifyStat(Stat.HP, (int)-DamageTaken);

        //Checks if dead
        if (GetComponent<Stats>().currentStats[(int)Stat.HP] <= 0)
        {
            dead = true;
            if(gameObject == GameManager.Instance.partyLeader)
            {
                GameManager.Instance.SwapCharacter();
            }
            //Destroy(gameObject);
        }
    }

    [Range(0f, 1f)] public float damageStaggerDuration = 0.1f;
    [ColorUsageAttribute(true, true)] public Color takeDamageColor = new Color(1, 0, 0);
    [SerializeField] AnimationCurve damagedBlendCurve;
    IEnumerator TakeDamageCoroutine;

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
