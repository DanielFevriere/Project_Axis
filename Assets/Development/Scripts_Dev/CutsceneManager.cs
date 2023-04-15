using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public bool inCutscene = false;

    public int currentCutsceneIndex = 0;
    public List<PlayableDirector> cutsceneHolder;

    public static CutsceneManager Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        //Makes sure the cutscene doesn't play on start
        foreach(PlayableDirector p in cutsceneHolder)
        {
            p.Stop();
        }

        DialogueManager.Instance.OnCloseDialogue += () =>
        {
            if (GameManager.Instance.CurrentState == GameState.Cutscene)
            {
                EndCutscene();
            }
        };
        
        //StartCutscene();
    }
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCutscene()
    {
        GameManager.Instance.ChangeState(GameState.Cutscene);
        cutsceneHolder[currentCutsceneIndex].Play();
    }

    void NextCutscene()
    {
        currentCutsceneIndex++;
        if(cutsceneHolder[currentCutsceneIndex] != null)
        {
            cutsceneHolder[currentCutsceneIndex].Play();
        }
    }    

    void EndCutscene()
    {
        GameManager.Instance.ChangeState(GameState.FreeRoam);
        currentCutsceneIndex = 0;
    }
}
