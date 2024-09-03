using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamGaugePanel : MonoBehaviour
{
    #region Properties
    [SerializeField] Image TPBar;

    Material TpMaterial;

    #endregion

    #region Monobehavior

    void Awake()
    {
        TpMaterial = new Material(TPBar.material);
        TpMaterial.SetFloat("_Fill", 1f);
        TPBar.material = TpMaterial;

        // Makes TP update
        GameManager.Instance.OnTPUpdate += OnTPUpdate;

        OnTPUpdate();
    }
    #endregion

    #region Functions

    public void SetUpNewOwner()
    {

    }

    void OnTPUpdate()
    {
        float Fill = (float)GameManager.Instance.currentTP / (float)GameManager.Instance.maxTP;
        TpMaterial.SetFloat("_Fill", Fill);
    }

    void SetHPFill()
    {

    }

    #endregion


}

