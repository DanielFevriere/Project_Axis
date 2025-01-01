using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public string playerID;
    [SerializeField] GameObject damagePopupPrefab;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem healEffect;

    Material material;
    SpriteRenderer spriteRenderer;
    public bool stunned;
    public bool dead;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }

        GetComponent<Knockback>().OnKnockBackBegin.AddListener(StartStun);
        GetComponent<Knockback>().OnComplete.AddListener(EndStun);
    }

    private void Update()
    {
        Animator anim = GetComponentInChildren<Animator>();
        anim.SetFloat("weapon", GameManager.Instance.CurrentState == GameState.Battle ? 1 : 0);
        anim.SetBool("stunned", stunned);
    }

    #region Take Damage
    public void TakeDamage(float DamageTaken)
    {
        Debug.Log(name + " took damage");

        if(gameObject == GameManager.Instance.partyLeader)
        {
            UiManager.Instance.PlayRedFlashEffect();
        }
        DamagePopup.Create(damagePopupPrefab, transform.position, (int)DamageTaken);
        hitEffect.Play();


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

    public void StartStun()
    {
        stunned = true;
    }

    public void EndStun()
    {
        stunned = false;
    }

    [Range(0f, 1f)] public float damageStaggerDuration = 0.1f;
    [ColorUsageAttribute(true, true)] public Color takeDamageColor = new Color(1, 0, 0);

    [SerializeField] AnimationCurve damagedBlendCurve;
    IEnumerator TakeDamageCoroutine;

    IEnumerator Coroutine_TakeDamage()
    {
        yield return null;

        // Set damaged color on material
        material.SetColor("_TakeDamageColor", takeDamageColor);

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

    #region Heal Damage
    public void HealDamage(float damageHealed)
    {
        Debug.Log(name + " healed damage");

        Instantiate(healEffect, transform.position, Quaternion.identity);

        if (HealDamageCoroutine != null)
        {
            StopCoroutine(HealDamageCoroutine);
        }

        //Heal damage
        HealDamageCoroutine = Coroutine_HealDamage();
        StartCoroutine(HealDamageCoroutine);
        GetComponent<Stats>().ModifyStat(Stat.HP, (int)damageHealed);
    }
    [ColorUsageAttribute(true, true)] public Color healDamageColor = new Color(0, 1, 0);

    [SerializeField] AnimationCurve healBlendCurve;
    IEnumerator HealDamageCoroutine;

    IEnumerator Coroutine_HealDamage()
    {
        yield return null;

        // Set damaged color on material
        material.SetColor("_HealDamageColor", healDamageColor);

        float timeElapsed = 0f;
        while (timeElapsed < damageStaggerDuration)
        {
            timeElapsed += Time.deltaTime;

            // Lerp down from damaged color
            float t = timeElapsed / damageStaggerDuration;
            float value = damagedBlendCurve.Evaluate(t);
            material.SetFloat("_HealedBlend", value);

            yield return null;
        }
    }
    #endregion


}
