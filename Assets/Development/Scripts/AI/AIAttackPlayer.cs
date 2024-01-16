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
    public bool windingUpAttack;
    public bool attacking;

    public float attackWindupTime;
    public float attackWindupTimer;

    public float attackTime;
    public float attackTimer;

    public float attackCoolDownTime;
    public float attackCoolDownTimer;

    private void Start()
    {
        //Gets components
        brain = GetComponent<EnemyAIBrain>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(onCooldown)
        {
            attackCoolDownTimer -= Time.deltaTime;
        }

        if(attackCoolDownTimer <= 0)
        {
            attackCoolDownTimer = attackCoolDownTime;
            onCooldown = false;
        }

        if(windingUpAttack)
        {
            attackWindupTimer -= Time.deltaTime;
        }    

        if(attackWindupTimer <= 0)
        {
            attackWindupTimer = attackWindupTime;
            windingUpAttack = false;

            Attack();
        }

        if(attacking)
        {
            attackTimer -= Time.deltaTime;
        }

        if(attackTimer <= 0)
        {
            attackBox.SetActive(false);
            attackTimer = attackTime;
            attacking = false;
            onCooldown = true;
            brain.CompleteAction();
        }
    }

    public override void PerformAction()
    {
        WindupAttack();
    }

    public void WindupAttack()
    {
        windingUpAttack = true;
        warningBox.SetActive(true);
    }

    public void Attack()
    {
        attacking = true;
        warningBox.SetActive(false);
        attackBox.SetActive(true);
    }
}
