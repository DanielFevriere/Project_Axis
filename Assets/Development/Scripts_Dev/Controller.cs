using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{

    Rigidbody rb;
    //[SerializeField] Animator anim;
    public InputMaster controls;
    public CharacterController characterController;
    public List<KeyControl> slot;

    //Movement vars
    public bool running = false;
    public float curSpeed;
    public int direction = 1;
    public int jumpPower;

    //Ground Detection
    RaycastHit GroundHit;
    public LayerMask GroundLayer;

    //Gravity vars
    public bool isGrounded;
    public bool isFalling;
    public float gravity;
    public float verticalVel;
    public float terminalVel;

    public Vector3 movement;


    // Start is called before the first frame update
    void Awake()
    {

        //Fetching character controller
        characterController = GetComponent<CharacterController>();

        //Four directional movement stuff
        slot = new List<KeyControl>(4);

        //Getting rigidbody
        rb = GetComponent<Rigidbody>();

        //Sets controls to a new input master
        controls = new InputMaster();

        //You must enable an action before it is used
        controls.FindAction("Movement").Enable();
    }

    // Update is called once per frame
    void Update()
    {

        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();
        
        //Sets the direction int in the animator controller
        //anim.SetInteger("Direction", direction);

        //Movement
        Movement();

        //Moves based on the movement vector
        characterController.Move(movement * Time.deltaTime);

        //If you don't need the character grounded then get rid of this part.
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out GroundHit, 0.1f, GroundLayer);

        //If you are on the ground, velocity 0, and you arent falling
        if (isGrounded)
        {
            //verticalVel = -gravity * Time.deltaTime;
            verticalVel = 0;
            isFalling = false; //makes sure falling is false
        }
        else
        {
            isFalling = true;
            verticalVel -= gravity * Time.deltaTime;
        }

        //If your falling speed reaches terminal velocity, it cant go past it
        if (verticalVel <= terminalVel)
        {
            verticalVel = terminalVel;
        }

        //Change verticle velocity if space was pressed this frame and grounded
        if (kb.spaceKey.isPressed && isGrounded)
        {
            verticalVel = jumpPower;
            isGrounded = false;
        }
    }

    public void Movement()
    {
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        //Gets movement input data and turns it into a vector2 to apply towards the characters movement animation
        Vector2 moveInput = controls.Player.Movement.ReadValue<Vector2>();

        //8 directional movement
        movement = new Vector3(moveInput.x, verticalVel, moveInput.y) * (curSpeed * 100) * Time.deltaTime;

        //Makes sure the animation variables match the movement inputs
        //anim.SetFloat("xAxis", moveInput.x);
        //anim.SetFloat("yAxis", moveInput.y);
        //anim.SetBool("Running", running);

        //Uses input data to determine direction.
        /*
         * Up = (0, 1)
         * Down = (0, -1)
         * Left = (-1, 0)
         * Right =  (1, 0)
         * Up + Right = (1, 1)
         * Down + Right = (1, -1)
         * Up + Left = (-1, 1)
         * Down + Left = (-1, -1)
        */
        
        if (moveInput == new Vector2(0, 1))
        {
            direction = 3;
        }
        else if (moveInput == new Vector2(0, -1))
        {
            direction = 1;
        }
        else if (moveInput == new Vector2(-1, 0))
        {
            direction = 4;
        }
        else if (moveInput == new Vector2(1, 0))
        {
            direction = 2;
        }

        //If there is no move input, you are not running, otherwise, you are
        if (moveInput == new Vector2(0, 0))
        {
            running = false;
        }
        else
        {
            running = true;
        }
        
    }
}
