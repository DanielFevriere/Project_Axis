using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public string damageID;
    public float damageDealt;

    public bool onCooldown = false;
    float cooldownTime = 0.5f;
    float cooldownTimer;

    private void Start()
    {
        cooldownTimer = cooldownTime;
    }

    private void Update()
    {
        //Cooldown time between damage
        if(onCooldown)
        {
            cooldownTimer -= Time.deltaTime;
        }
        if(cooldownTimer <= 0)
        {
            cooldownTimer = cooldownTime;
            onCooldown = false;
        }
    }

    [SerializeField] LayerMask targetLayer;

    // Interaction with units and environments
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject)
        {
            return;
        }

        Debug.Log("Hitbox collided with " + other.gameObject.name);

        // Deals damage if collided with targetLayer
        if (((1 << other.gameObject.layer) & targetLayer) != 0) //If the collided objects layer is the target layer
        {
            if (other.TryGetComponent(out IDamageable entity))
            {
                if(!onCooldown)
                {
                    entity.TakeDamage(damageDealt);
                    CheckQuest();
                    onCooldown = true;
                }
            }
        }
    }

    public void CheckQuest()
    {
        //Cycles through the active quests
        for (int i = 0; i < QuestManager.Instance.activeQuests.Count; i++)
        {
            //Cycles through the conditions of active quests
            for (int j = 0; j < QuestManager.Instance.activeQuests[i].conditions.Count; j++)
            {
                //If the collectible ID is the same as the quest condition ID,
                if (damageID == QuestManager.Instance.activeQuests[i].conditions[j].ConditionID)
                {
                    //Gives progress
                    QuestManager.Instance.activeQuests[i].conditions[j].GiveProgress((int)damageDealt);
                }
            }
        }
    }

}
