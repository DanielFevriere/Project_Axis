using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashAbility : MonoBehaviour
{
    Controller moveScript;

    public bool dashing;
    public float dashSpeed;
    public float dashTime;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        //Dash if space was pressed this frame and grounded
        if (kb.spaceKey.wasPressedThisFrame)
        {
            //dashing = true;
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            moveScript.characterController.Move(moveScript.movement * dashSpeed * Time.fixedDeltaTime);
            yield return null;
        }
    }

}
