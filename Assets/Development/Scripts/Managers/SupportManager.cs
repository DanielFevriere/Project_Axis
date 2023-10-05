using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportManager : MonoBehaviour
{
    #region Global static reference
    private static SupportManager instance;
    public static SupportManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<SupportManager>();
                return instance;
            }
        }
    }
    #endregion

    public List<SupportCharacter> supportCharacters;
}
