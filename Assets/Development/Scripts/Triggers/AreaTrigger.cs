using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    #region Properties
    public TriggerType TriggerType;

    [Header("Teleport")]
    [SerializeField] TeleportTrigger TeleportTriggerParams;

    [Space][Header("Area")]
    [SerializeField] AreaChangeTrigger AreaTriggerParams;

    Controller InteractingPlayer;

    #endregion Properties


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Skip if player is already inside trigger
        if (InteractingPlayer != null) { return; }

        // Only activates when player enters
        if (other.tag != "Player") {return;}

        InteractingPlayer = other.GetComponent<Controller>();

        switch (TriggerType)
        {
            case TriggerType.Teleport:
                UiManager.Instance.PlayBlackScreenEffect();
                StartCoroutine(TeleportCoroutine(other.GetComponent<Controller>()));
                break;
            case TriggerType.AreaChange:
                BeginAreaTransition();
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractingPlayer = null;
    }

    IEnumerator TeleportCoroutine(Controller PlayerController)
    {
        GameManager.Instance.ChangeState(GameState.Freeze);
        GameManager.Instance.ToggleCameraConfiner(false);
        PlayerController.gameObject.transform.position = TeleportTriggerParams.Destination.position;
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.ChangeState(GameState.FreeRoam);
        GameManager.Instance.SetCameraConfiner(TeleportTriggerParams.NewCameraConfiner);
        GameManager.Instance.ToggleCameraConfiner(true);
    }

    void BeginAreaTransition()
    {
        Debug.Log("Begin Area Transition");

        // Check which direction
        // A --> B
        if (!AreaTriggerParams.Flip)
        {
            StartCoroutine(AreaTriggerParams.FromArea.Container.FadeOut());
            StartCoroutine(AreaTriggerParams.ToArea.Container.FadeIn());

            CameraManger.Instance.SwitchCamera(AreaTriggerParams.ToArea.CameraName);
        }
        // A <-- B
        else
        {
            StartCoroutine(AreaTriggerParams.FromArea.Container.FadeIn());
            StartCoroutine(AreaTriggerParams.ToArea.Container.FadeOut());

            CameraManger.Instance.SwitchCamera(AreaTriggerParams.FromArea.CameraName);
        }

        // Finally, flip direction
        AreaTriggerParams.Flip = !AreaTriggerParams.Flip;
    }

    #region Internal Classes
    [System.Serializable]
    public class TeleportTrigger
    {
        public Transform Destination;
        public BoxCollider NewCameraConfiner;
    }

    [System.Serializable]
    public class AreaChangeTrigger
    {
        public bool Flip;

        public OverworldAreaInfo FromArea;
        public OverworldAreaInfo ToArea;
    }

    #endregion 
}

[System.Serializable]
public struct OverworldAreaInfo
{
    public OverworldArea Container;
    public string CameraName;
}