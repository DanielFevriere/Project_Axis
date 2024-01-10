using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCombatStatusPanel : MonoBehaviour
{
    #region Properties

    Stats OwnerStats;
    
    [SerializeField] Image CharacterMug;
    [SerializeField] TMP_Text NameText;
    [SerializeField] Image HPBar;
    [SerializeField] Image SPBar;

    Material HPBarMaterial;
    
    #endregion
    
    #region Monobehavior

    void Awake()
    {
        HPBarMaterial = new Material(HPBar.material);
        HPBarMaterial.SetFloat("_Fill", 1f);
        HPBar.material = HPBarMaterial;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    #endregion
    
    #region Functions

    public void SetUpNewOwner(Stats NewOwnerStats)
    {
        // If this UI is previously associated with a different character
        if (OwnerStats != null)
        {
            OwnerStats.OnTakeDamage -= OnHPUpdate;
        }
        
        OwnerStats = NewOwnerStats;
        
        // Makes HP update everytime associated owner takes damage
        OwnerStats.OnTakeDamage += OnHPUpdate;
    }
    
    void OnHPUpdate()
    {
        if (OwnerStats)
        {
            float Fill = OwnerStats.GetStat(Stat.HP) / (float)OwnerStats.GetStat(Stat.MaxHP);
            HPBarMaterial.SetFloat("_Fill", Fill);
        }
    }

    void SetHPFill()
    {
        
    }
    
    #endregion
    
   
}
