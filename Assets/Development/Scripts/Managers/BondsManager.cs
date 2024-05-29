using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondsManager : MonoBehaviour
{
    #region Global static reference
    private static BondsManager instance;
    public static BondsManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<BondsManager>();
                return instance;
            }
        }
    }
    #endregion

    public List<BondsCharacter> bondsCharacters;
}
