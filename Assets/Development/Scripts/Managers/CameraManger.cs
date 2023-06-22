using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManger : MonoBehaviour
{
    #region Singleton
    private static CameraManger instance;

    public static CameraManger Instance
    {
        get
        {
            if(instance != null)
            {
                return instance;
            }
            else
            {
                instance = FindObjectOfType<CameraManger>();
                return instance;
            }
        }
    }
    #endregion

    public Animator CameraAnimator;

    public void SwitchCamera(string CameraName)
    {
        if (CameraAnimator == null)
        {
            Debug.LogError("State Driven Camera unassigned!");
            return;
        }

        CameraAnimator.Play(CameraName);
    }
}
