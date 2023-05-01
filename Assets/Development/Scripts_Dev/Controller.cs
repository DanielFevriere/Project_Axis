using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] Animator anim;
    public string characterName;
    public bool isControlling;
    public InputMaster controls;
    public CharacterController characterController;
    public List<KeyControl> slot;
    public bool attacking = false;

    //Movement vars
    public bool running = false;
    public bool dashing = false;
    public float curSpeed;
    public FacingDirection direction = FacingDirection.Down;
    public Vector2 facingVector;

    //Slope vars
    public float maxSlopeAngle = 45;
    public bool onSlope;
    RaycastHit SlopeHit;

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

    [SerializeField] Conversation convo;

    public PlayerWeapon playerWeapon;

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

        playerWeapon = GetComponentInChildren<PlayerWeapon>();
    }

    //Always called every frame
    private void Update()
    {
    
    }

    //Constantly called when the Gamemanager is in a certain state
    public void HandleUpdate()
    {
        //This is the character thats being controlled
        isControlling = true;

        GetComponent<AIFollow>().enabled = false;

        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        //Sets the direction int in the animator controller
        anim.SetFloat("xAxis", facingVector.x);
        anim.SetFloat("yAxis", facingVector.y);
        anim.SetBool("walking", running);

        //Sets anims for attacking
        anim.SetBool("attacking", attacking);

        //Sets movement data
        Movement();
        
        //Checks to see if on slope
        CheckSlope();

        //If the player is not attacking
        if(!GetComponentInChildren<AbilityHolder>().attackAbility.inUse)
        {
            attacking = false;

            //Moves based on if the player is on a slope or not
            if (onSlope)
            {
                //mMoves based on the slope vector
                characterController.Move(GetSlopeMoveDirection() * Time.fixedDeltaTime);
            }
            else
            {
                //Moves based on the movement vector
                characterController.Move(movement * Time.fixedDeltaTime);
            }
        }
        else
        {
            attacking = true;
        }


        // Rotate weapon
        if (playerWeapon != null)
        {
            playerWeapon.UpdateWeaponRotation();
        }

        //GroundCheck
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out GroundHit, 0.1f, GroundLayer);

        //If you are on the ground, velocity 0, and you arent falling
        if (isGrounded)
        {
            verticalVel = 0;
            isFalling = false; //makes sure falling is false
        }
        //Otherwise, fall
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


        
        //Character Swap mechanic
        if (kb.eKey.wasReleasedThisFrame)
        {
            Debug.Log("Swapped");
            GameManager.Instance.SwapCharacter();
        }

        //Quote test function
        if(kb.lKey.wasReleasedThisFrame)
        {
            CutsceneManager.Instance.StartCutscene();
            //SayQuote();
        }
    }

    /// <summary>
    /// Checks movement input data
    /// </summary>
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
        //(0,1)
        if (moveInput == new Vector2(0, 1))
        {
            direction = FacingDirection.Up;
            facingVector = new Vector2(0, 1);
        }
        //(0,-1)
        else if (moveInput == new Vector2(0, -1))
        {
            direction = FacingDirection.Down;
            facingVector = new Vector2(0, -1);
        }
        //(-1,0)
        else if (moveInput == new Vector2(-1, 0))
        {
            direction = FacingDirection.Left;
            facingVector = new Vector2(-1, 0);
        }
        //(1,0)
        else if (moveInput == new Vector2(1, 0))
        {
            direction = FacingDirection.Right;
            facingVector = new Vector2(1, 0);
        }
        //Diagonals
        //(1,1)
        else if (moveInput.x > 0 && moveInput.y > 0)
        {
            direction = FacingDirection.UpRight;
            facingVector = new Vector2(1, 1);
        }
        //(1, -1)
        else if (moveInput.x > 0 && moveInput.y < 0)
        {
            direction = FacingDirection.DownRight;
            facingVector = new Vector2(1, -1);
        }
        //(-1, 1)
        else if (moveInput.x < 0 && moveInput.y > 0)
        {
            direction = FacingDirection.UpLeft;
            facingVector = new Vector2(-1, 1);

        }
        //(-1, -1)
        else if (moveInput.x < 0 && moveInput.y < 0)
        {
            direction = FacingDirection.DownLeft;
            facingVector = new Vector2(-1, -1);
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

    /// <summary>
    /// Checks to see if you are on a slope
    /// </summary>
    void CheckSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out SlopeHit, 0.75f))
        {
            float Angle = Vector3.Angle(Vector3.up, SlopeHit.normal);
            onSlope = Angle < maxSlopeAngle && Angle != 0;
        }
    }

    /// <summary>
    /// Vector3 generated based on the movement on a slope
    /// </summary>
    Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(movement, SlopeHit.normal);
    }

    public void SayQuote()
    {
        StartCoroutine(DialogueManager.Instance.ShowConversation(convo));
    }
    public Vector2 ReturnDir()
    {
        switch (direction)
        {
            case FacingDirection.Up:
                return new Vector2(0, 1);
            case FacingDirection.Down:
                return new Vector2(0, -1);
            case FacingDirection.Left:
                return new Vector2(-1, 0);
            case FacingDirection.Right:
                return new Vector2(1, 0);
            case FacingDirection.UpRight:
                return new Vector2(1, 1);
            case FacingDirection.UpLeft:
                return new Vector2(1, -1);
            case FacingDirection.DownRight:
                return new Vector2(-1, 1);
            case FacingDirection.DownLeft:
                return new Vector2(-1, -1);
        }
        return new Vector2(0,0);
    }
}

public enum FacingDirection
{
    Left,
    Right,
    Up,
    Down,
    UpRight,
    DownRight,
    UpLeft,
    DownLeft
}
