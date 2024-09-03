using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    #region Properties
    SpriteRenderer spriteRenderer;
    Material material;

    //Ground Detection + Gravity
    RaycastHit GroundHit;
    public string enemyID;
    public LayerMask GroundLayer;
    [SerializeField] GameObject damagePopupPrefab;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem deathEffect;

    public GameObject hpVisual;
    public GameObject hpBar;
    public GameObject hpbBar;

    public Material hpInstance;
    public Material hpbInstance;

    Image hpRenderer;
    Image hpbRenderer;

    float bHealth = 1;
    public bool stunned;
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

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }

        hpRenderer = hpBar.GetComponent<Image>();
        hpbRenderer = hpbBar.GetComponent<Image>();

        hpInstance = Instantiate(hpRenderer.material);
        hpbInstance = Instantiate(hpbRenderer.material);

        hpRenderer.material = hpInstance;
        hpbRenderer.material = hpbInstance;

        GetComponent<Knockback>().OnKnockBackBegin.AddListener(StartStun);
        GetComponent<Knockback>().OnComplete.AddListener(EndStun);
    }

    public void TakeDamage(float DamageTaken)
    {
        GameManager.Instance.AddTP(2);

        Debug.Log(name + " took damage");

        hpVisual.SetActive(true);

        if (TakeDamageCoroutine != null)
        {
            StopCoroutine(TakeDamageCoroutine);
        }

        DamagePopup.Create(damagePopupPrefab, transform.position, (int)DamageTaken);
        hitEffect.Play();

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

    private void FixedUpdate()
    {
        //Updating health in UI via the Material
        float fHealth = (float)GetComponent<Stats>().currentStats[(int)Stat.HP] / (float)GetComponent<Stats>().currentStats[(int)Stat.MaxHP];
        hpInstance.SetFloat("_Health", fHealth);

        bHealth = Mathf.Lerp(bHealth, fHealth, 0.01f);
        hpbInstance.SetFloat("_BHealth", bHealth);
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

    public void StartStun()
    {
        stunned = true;
    }

    public void EndStun()
    {
        stunned = false;
        GetComponent<EnemyAIBrain>().CompleteAction();
    }

    #region Take Damage
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

    private void OnDestroy()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        //Cycles through the active quests
        for (int i = 0; i < QuestManager.Instance.activeQuests.Count; i++)
        {
            //Cycles through the conditions of active quests
            for (int j = 0; j < QuestManager.Instance.activeQuests[i].conditions.Count; j++)
            {
                //If the enemy ID is the same as the quest condition ID,
                if (enemyID == QuestManager.Instance.activeQuests[i].conditions[j].ConditionID)
                {
                    //Gives progress for destroying the enemy
                    QuestManager.Instance.activeQuests[i].conditions[j].GiveProgress(1);
                }
            }
        }

        //Can't remember why I did this, but it's probably some sort of bugfix
        DOTween.Sequence()
            .AppendInterval(0.1f)
            .AppendCallback(() =>
            {
                EncounterManager.Instance.UpdateEnemyCount();
            });
    }

    #endregion
}
