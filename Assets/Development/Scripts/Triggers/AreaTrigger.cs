using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    #region Properties
    public TriggerType TriggerType;

    [SerializeField] TeleportTrigger TeleportTriggerParams;


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
        // Only activates when player enters
        if (other.tag != "Player") {return;}

        switch (TriggerType)
        {
            case TriggerType.Teleport:
                StartCoroutine(TeleportCoroutine(other.GetComponent<Controller>()));
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    IEnumerator TeleportCoroutine(Controller PlayerController)
    {
        GameManager.Instance.ChangeState(GameState.Freeze);
        GameManager.Instance.ToggleCameraConfiner(false);
        PlayerController.gameObject.transform.position = TeleportTriggerParams.Destination.position;
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.ChangeState(GameState.FreeRoam);
        GameManager.Instance.SetCameraConfiner(TeleportTriggerParams.NewCameraConfiner);
        GameManager.Instance.ToggleCameraConfiner(true);
    }


    #region Internal Classes
    [System.Serializable]
    public class TeleportTrigger
    {
        public Transform Destination;
        public BoxCollider NewCameraConfiner;
    }

    #endregion 
}
