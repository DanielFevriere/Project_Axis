using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTab : MonoBehaviour
{
    public string tabName;
    public GameObject tabContainer;
    public bool isVisible = false;

    public void ToggleVisibility()
    {
        isVisible = !isVisible;
        tabContainer.SetActive(isVisible);
    }
}