using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackPlayer : AIAction
{
    NavMeshAgent agent;
    EnemyAIBrain brain;

    public GameObject attackBox;
    public GameObject warningBox;

    public bool onCooldown = false;
    public float attackWindupTime;
    public float attackTime;
    public float attackCoolDown;

    private void Start()
    {
        //Gets components
        brain = GetComponent<EnemyAIBrain>();
        agent = GetComponent<NavMeshAgent>();
    }

    public override void PerformAction()
    {
        if(onCooldown)
        {
            brain.CompleteAction();
            return;
        }    

        DOTween.Sequence()
        //Turns on warning box
        .AppendCallback(() =>
        {
            agent.isStopped = true;
            transform.rotation = transform.rotation;
            warningBox.SetActive(true);
        })
        .AppendInterval(attackWindupTime)
        //Turns on Attack box, turns off warning box
        .AppendCallback(() =>
        {
            warningBox.SetActive(false);
            attackBox.SetActive(true);
            onCooldown = true;
        })
        //Turns off Attack box, goes on cooldown
        .AppendInterval(attackTime)
        .AppendCallback(() =>
        {
            attackBox.SetActive(false);
            onCooldown = true;
            brain.CompleteAction();
        })
        .AppendInterval(attackCoolDown)
        //Goes off cooldown
        .AppendCallback(() =>
        {
            onCooldown = false;
        });
    }
}
