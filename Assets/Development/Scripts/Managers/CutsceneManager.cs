using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    #region Global static reference
    private static CutsceneManager instance;
    public static CutsceneManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<CutsceneManager>();
                return instance;
            }
        }
    }
    #endregion

    public bool inCutscene = false;

    public int currentCutsceneIndex = 0;
    public List<PlayableDirector> cutsceneHolder;
    

    // Start is called before the first frame update
    void Start()
    {
        //Makes sure the cutscene doesn't play on start
        foreach(PlayableDirector p in cutsceneHolder)
        {
            p.Stop();
        }

        DialogueManager.Instance.OnCloseDialogue += ResumeCutscene;
        
        StartCutscene();
    }
    private void Awake()
    {
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

    public void PauseCutscene()
    {
        cutsceneHolder[currentCutsceneIndex].Pause();
    }

    public void ResumeCutscene()
    {
        GameManager.Instance.ChangeState(GameState.Cutscene);
        cutsceneHolder[currentCutsceneIndex].Resume();
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
