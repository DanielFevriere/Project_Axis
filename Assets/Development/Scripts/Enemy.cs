using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    #region Properties
    SpriteRenderer spriteRenderer;
    Material material;

    //Ground Detection + Gravity
    RaycastHit GroundHit;
    public LayerMask GroundLayer;
    public bool isGrounded;
    public bool isFalling;
    public float gravity;
    public float verticalVel;
    public float terminalVel;



    [Range(0f, 1f)] public float damageStaggerDuration = 0.5f;
    [ColorUsageAttribute(true, true)] public Color takeDamageColor = new Color(1, 0, 0);
    [SerializeField] AnimationCurve damagedBlendCurve;

    IEnumerator TakeDamageCoroutine;
    #endregion

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
            Destroy(gameObject);
        }
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
        //GroundCheck
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out GroundHit, 0.1f, GroundLayer);

        //If you are on the ground, velocity 0, and you arent falling
        if (isGrounded)
        {
            verticalVel = 0;
            isFalling = false; //makes sure falling is false
        }
        //Otherwise, fall
        else
        {
            isFalling = true;
            verticalVel -= gravity * Time.deltaTime;
        }

        //If your falling speed reaches terminal velocity, it cant go past it
        if (verticalVel <= terminalVel)
        {
            verticalVel = terminalVel;
        }
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

    private void OnDestroy()
    {
        DOTween.Sequence()
            .AppendInterval(0.1f)
            .AppendCallback(() =>
            {
                EncounterManager.Instance.UpdateEnemyCount();
            });
    }

    #endregion
}
