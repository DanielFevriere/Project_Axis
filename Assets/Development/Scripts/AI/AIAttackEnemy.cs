using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackEnemy : AIAction
{
    public GameObject target;
    public SkillHolder abilityHolder;
    NavMeshAgent agent;
    AllyAIBrain brain;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        brain = gameObject.GetComponent<AllyAIBrain>();
    }

    //Attacks Target
    public override void PerformAction()
    {
        //Grabs a random nearby enemy to become the target
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            target = enemies[Random.Range(0, enemies.Length)];

            agent.SetDestination(target.transform.position);
            abilityHolder.ActivateSkill(abilityHolder.attackSkill);


        //Automatically faces enemy
        Vector3 targetDirection = target.transform.position - transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 150 * Time.deltaTime);

        DOTween.Sequence()
            .AppendInterval(abilityHolder.attackSkill.skillTime)
            .AppendCallback(() =>
            {
                if(brain.enabled)
                {
                    brain.CompleteAction();
                }
            });
    }
}
