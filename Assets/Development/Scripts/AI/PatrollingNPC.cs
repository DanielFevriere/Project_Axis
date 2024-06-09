using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PatrollingNPC : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Quest killQuest;
    public Animator anim;
    public bool talking;
    public bool inTalkingDistance = false;
    public Conversation convo;
    public float walkSpeed;
    public Transform currentTarget;
    public List<Transform> targetList;


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
        anim.SetFloat("yAxis", agent.velocity.z);
        anim.SetBool("Walking", !talking);


        if (!talking)
        {
            Patrolling();

        }



        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        //If you are in talking distance,
        if (inTalkingDistance && GameManager.Instance.CurrentState == GameState.FreeRoam && kb.fKey.wasPressedThisFrame)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            talking = true;

            //Changes the games state to dialogue
            GameManager.Instance.ChangeState(GameState.Dialogue);
            
            DialogueManager.Instance.OnCloseDialogue += QuestCheck;

            
            StartCoroutine(DialogueManager.Instance.ShowConversation(convo));


            DialogueManager.Instance.OnCloseDialogue += StartPatrolling;
        }
    }

    public void QuestCheck()
    {
        //If it contains the quest, Check if the quest is complete
        if (QuestManager.Instance.activeQuests.Contains(killQuest))
        {
            if (QuestManager.Instance.CheckIfQuestComplete(killQuest))
            {
                QuestManager.Instance.FinishQuest(killQuest);
                BondsManager.Instance.UnlockBond(BondsManager.Instance.bondsCharacters[0], 1);
                BondsManager.Instance.UnlockBond(BondsManager.Instance.bondsCharacters[0], 2);
                BondsManager.Instance.UnlockBond(BondsManager.Instance.bondsCharacters[0], 3);
            }
        }
        //If not, accept the quest
        else
        {
            QuestManager.Instance.AcceptQuest(killQuest);
        }
        DialogueManager.Instance.OnCloseDialogue -= QuestCheck;
    }

    void StartPatrolling()
    {
        talking = false;
        DialogueManager.Instance.OnCloseDialogue -= StartPatrolling;
    }

    void Patrolling()
    {
        //Automatically faces player
        Vector3 targetDirection = currentTarget.position - transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 150 * Time.deltaTime);

        agent.SetDestination(currentTarget.position);
        agent.isStopped = false;

        float distanceToTarget = Vector3.Distance(currentTarget.position, transform.position);
        if(distanceToTarget <= 3)
        {
            NextTarget();
        }
    }

    //Switches target
    void NextTarget()
    {
        //Return if theres no targets lol
        if(targetList.Count == 0)
        {
            return;
        }

        //Goes through the list of targets
        for(int i = 0;i < targetList.Count;i++)
        {
            //If the current target is the target at the lists index,
            if(currentTarget == targetList[i])
            {
                //If the current target is not the last target in the list
                if(currentTarget != targetList[targetList.Count - 1])
                {
                    //Make the current target the next target in the list
                    currentTarget = targetList[i+1];
                    break;
                }
                //If it is
                else
                {
                    //Make the current target the first target in the list (so it rotates)
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
