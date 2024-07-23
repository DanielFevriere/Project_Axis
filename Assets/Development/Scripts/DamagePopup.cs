using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    #region Global static reference
    private static DamagePopup instance;
    public static DamagePopup Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<DamagePopup>();
                return instance;
            }
        }
    }
    #endregion
    
    public TMP_Text damageText;

    private void Awake()
    {
        damageText = GetComponent<TMP_Text>();
    }

    public void Setup(int damageAmount)
    {
        damageText.SetText(damageAmount.ToString());
        Destroy(GetComponentInParent<Billboard>().gameObject, 1f);
    }
    
    public static DamagePopup Create(GameObject damagePopupPrefab, Vector3 position, int damageAmount)
    {
        Transform damagePopupTransform = Instantiate(damagePopupPrefab, position, Quaternion.identity).transform;
        DamagePopup damagePopup = damagePopupTransform.GetComponentInChildren<DamagePopup>();
        damagePopup.Setup(damageAmount);

        return damagePopup;
    }

}
