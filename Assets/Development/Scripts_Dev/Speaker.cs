using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    public string nameID;
    string displayName;
    public TMP_Text NameText;
    public TMP_Text LineText;

    private void Start()
    {
        DialogueManager.Instance.listOfSpeakers.Add(this);
        gameObject.SetActive(false);
    }
}
