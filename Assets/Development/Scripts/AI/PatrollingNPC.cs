using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PatrollingNPC : MonoBehaviour
{
    NavMeshAgent agent;
    public Animator anim;
    public bool talking;
    public Transform target;
    public bool inTalkingDistance = false;
    public Conversation convo;
    public float walkSpeed;
    public Transform currentTarget;
    public list<Transform> targetList;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        StartPatrolling();
    }

    // Update is called once per frame
    void Update()
    {
        //Sets npc directional animations
        anim.SetFloat("xAxis", agent.velocity.x);
        anim.SetFloat("yAxis", agent.velocity.y);
        anim.SetBool("Walking", !talking);

        //If the NPC reaches the next target/waypoint, Switch Target
        if(Physics.CheckSphere(currentTarget.position, 1f)
            {
                NextTarget();
            }

        if(!talking)
        {
            //Automatically faces player
            Vector3 targetDirection = target.position - transform.position;
            Quaternion desiredRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 150 * Time.deltaTime);

            agent.SetDestination(target.position);
            agent.isStopped = false;
        }

        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        if (inTalkingDistance && GameManager.Instance.CurrentState == GameState.FreeRoam && kb.fKey.wasPressedThisFrame)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            talking = true;
            //Changes the games state to dialogue
            GameManager.Instance.ChangeState(GameState.Dialogue);

            StartCoroutine(DialogueManager.Instance.ShowConversation(convo));

            DialogueManager.Instance.OnCloseDialogue += StartPatrolling;
        }
    }

    void StartPatrolling()
    {
        talking = false;
        DialogueManager.Instance.OnCloseDialogue -= StartPatrolling;

    }

    //Switches target
    void NextTarget()
    {
        for(int i = 0;i < targetList.Count;i++)
        {
            if(currentTarget == targetList[i])
            {
                if(i != targetList[i-1])
                {
                    currentTarget = targetList[i+1];
                }
                else
                {
                    currentTarget = targetList[0];
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.partyLeader)
        {
            inTalkingDistance = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == GameManager.Instance.partyLeader)
        {
            inTalkingDistance = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        inTalkingDistance = false;
    }
}
