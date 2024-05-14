using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Global static reference
    private static DialogueManager instance;
    public static DialogueManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<DialogueManager>();
                return instance;
            }
        }
    }
    #endregion

    [SerializeField] GameObject choiceBox;
    [SerializeField] TMP_Text dialogueName;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] int lettersPerSecond;

    [SerializeField] TMP_Text promptText;
    [SerializeField] TMP_Text choice1Text;
    [SerializeField] TMP_Text choice2Text;

    public int dialogueChoice;

    public List<Speaker> listOfSpeakers;

    public event Action OnShowDialogue;
    public event Action OnCloseDialogue;

    public Conversation shownConversation;
    int currentDialogue = 0;
    bool choosing = false;
    bool isTyping = false;


    private void Awake()
    {
        listOfSpeakers = new List<Speaker>();
    }

    public void HandleUpdate()
    {
        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();
        
        //If the dialogue key is pressed and its not already typing out a dialogue
        if (kb.fKey.wasReleasedThisFrame && !isTyping && !choosing)
        {
            currentDialogue++;
            if(currentDialogue < shownConversation.Dialogues.Count)
            {
                StartCoroutine(TypeLine(shownConversation.Dialogues[currentDialogue]));
            }
            else
            {
                isTyping = false;
                currentDialogue = 0;
                for (int i = 0; i < listOfSpeakers.Count; i++)
                {
                    listOfSpeakers[i].gameObject.SetActive(false);
                }
                OnCloseDialogue.Invoke();
            }
        }
    }

    public void StartConvo(Conversation convo)
    {
        StartCoroutine(ShowConversation(convo));
    }

    /// <summary>
    /// Prompts the player
    /// </summary>
    /// <param name="choice"></param>
    public void StartPrompt(string prompt, string choice1, string choice2)
    {
        choosing = true;
        choiceBox.SetActive(true);
        OnShowDialogue.Invoke();

        promptText.text = prompt;
        choice1Text.text = choice1;
        choice2Text.text = choice2;
    }

    public void ChoosePrompt(int choice)
    {
        choosing = false;
        choiceBox.SetActive(false);
        dialogueChoice = choice;
        OnCloseDialogue.Invoke();
    }

    public IEnumerator ShowConversation(Conversation convo)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialogue.Invoke();
        shownConversation = convo;

        dialogueName.text = convo.Dialogues[0].DisplayName;
        StartCoroutine(TypeLine(convo.Dialogues[0]));
    }

    public IEnumerator TypeLine(Dialogue dialogue)
    {
        isTyping = true;
        AudioManager.Instance.PlaySound("beep");

        for (int i = 0; i < listOfSpeakers.Count; i++)
        {
            if (listOfSpeakers[i].nameID == dialogue.NameID)
            {
                listOfSpeakers[i].gameObject.SetActive(true);
                dialogueName = listOfSpeakers[i].NameText;
                dialogueText = listOfSpeakers[i].LineText;
            }
            else
            {
                listOfSpeakers[i].gameObject.SetActive(false);
            }
        }

        dialogueName.text = dialogue.DisplayName;
        dialogueText.text = "";
        foreach(var letter in dialogue.Line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }

    public void HideDialogue()
    {

    }
}
