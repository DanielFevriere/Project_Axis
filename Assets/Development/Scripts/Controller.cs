 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    public GameObject aimerGraphic;
    public string characterName;
    public bool isControlling;
    public InputMaster controls;
    public CharacterController characterController;
    public List<KeyControl> slot;
    public bool attacking = false;
    public bool inBattle = false;

    //Movement vars
    public bool moving = false;
    public bool running = false;
    public bool dashing = false;
    public float curSpeed;
    public float walkSpeed;
    public float runSpeed;
    public FacingDirection direction = FacingDirection.Down;
    public Vector2 facingVector;
    [SerializeField] float groundCheckDistance = 0.08f;
    
    //Slope vars
    public float maxSlopeAngle = 45;
    public float Angle;
    public bool onSlope;
    RaycastHit SlopeHit;

    //Ground Detection
    RaycastHit GroundHit;
    public LayerMask GroundLayer;

    //Gravity vars
    public bool isGrounded;
    public bool isFalling;
    public float curGravity;
    public float setGravity;
    public float verticalVel;
    public float terminalVel;

    public Vector3 movement;

    Material material;

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
        
        // Render
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }
        
        // Auto-adds player to party if necessary
        GameManager.Instance.TryAddCharacterToPlayerParty(gameObject);
    }

    //Always called every frame
    //The reason Update() is empty is because this isn't always active
    private void Update()
    {
    }

    //Constantly called when the Gamemanager is in a certain state
    public void HandleUpdate()
    {
        //The player shouldn't update if dead
        if (GetComponent<Player>().dead)
        {
            return;
        }

        //This is the character thats being controlled
        isControlling = true;
        
        //Disables an AI action
        GetComponent<AIChasePartyLeader>().enabled = false;

        //Fetches the keyboard input system
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        //Sets the direction int in the animator controller
        anim.SetFloat("xAxis", facingVector.x);
        anim.SetFloat("yAxis", facingVector.y);
        anim.SetBool("moving", moving);
        anim.SetBool("running", running);

        //Checks to see if on slope
        CheckSlope();

        //Sets movement data
        //If the player isnt stunned they can move
        if(!GetComponent<Player>().stunned)
        {
            Movement();
        }

        // Rotate weapon
        if (playerWeapon != null)
        {
            bool inUse = false;
            for (int i = 0; i < GetComponentInChildren<SkillHolder>().skillList.Count; i++)
            {
                if(GetComponentInChildren<SkillHolder>().skillList[i].inUse)
                {
                    inUse = true;
                }
            }
            if(!inUse)
            {
                playerWeapon.UpdateWeaponRotation();
            }
        }

        //GroundCheck
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out GroundHit, groundCheckDistance, GroundLayer);

        //Slope check
        if(onSlope)
        {
            isGrounded = true;
        }

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
            curGravity = setGravity;
            verticalVel -= curGravity * Time.fixedDeltaTime;
        }

        //If your falling speed reaches terminal velocity, it cant go past it
        if (verticalVel <= terminalVel)
        {
            verticalVel = terminalVel;
        }
        
        //Automatic Sprinting in combat
        if(inBattle)
        {
            running = true;
        }
        else
        {
            //Manual sprinting out of combat
            if (kb.leftShiftKey.isPressed)
            {
                running = true;
            }
            else
            {
                running = false;
            }
        }

        //Character Swap mechanic
        if (kb.eKey.wasReleasedThisFrame)
        {
            GameManager.Instance.SwapCharacter();
        }

        //Debug key, test features like resetting atlas and quotes
        if (kb.lKey.wasReleasedThisFrame)
        {
            AtlasManager.Instance.ResetAtlas(); //Un applies the effects of all nodes.
            //SayQuote(); //Disable Quotes for now, just a test feature
        }

        //Open Menu
        OpenMenu();

        // Debug stuff
        DebugUpdate();
    }

    public void HandleFixedUpdate()
    {
        //If the player is not attacking
        if (!GetComponentInChildren<SkillHolder>().attackSkill.inUse)
        {
            attacking = false;

            Vector3 move;

            //Moves based on if the player is on a slope or not
            if (onSlope)
            {
                //Moves based on the slope vector
                move = GetSlopeMoveDirection() * Time.fixedDeltaTime;
                if (move != Vector3.zero)
                {
                    characterController.Move(move);
                }
                curGravity = 0;
            }
            else
            {
                //Moves based on the movement vector
                move = movement * Time.fixedDeltaTime;
                if (move != Vector3.zero)
                {
                    characterController.Move(move);
                }
                curGravity = setGravity;
            }
        }
        else
        {
            attacking = true;
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

        //Sets speed depending on if walking or running
        curSpeed = running ? runSpeed : walkSpeed;

        //8 directional movement
        movement = new Vector3(moveInput.x, verticalVel, moveInput.y) * (curSpeed * 10) * Time.fixedDeltaTime;

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
            moving = false;
        }
        else
        {
            moving = true;
        }

        // Override rotation based on aim if not moving or attacking
        if (!moving || attacking)
        {
            //Mouse based player rotation only in combat
            if(GameManager.Instance.CurrentState == GameState.Battle)
            {
                if (playerWeapon != null)
                {
                    // Remap 0,360 to -180,180
                    float yaw = playerWeapon.yawRotation > 180.0f ? playerWeapon.yawRotation - 360 : playerWeapon.yawRotation;
                    //Debug.Log("Current yaw: " + yaw);
                    // Up
                    if (yaw > -45.0f && yaw < 45.0f)
                    {
                        direction = FacingDirection.Up;
                        facingVector = new Vector2(0, 1);
                    }
                    // Right
                    else if (yaw >= 45.0f && yaw <= 135.0f)
                    {
                        direction = FacingDirection.Right;
                        facingVector = new Vector2(1, 0);
                    }
                    // Down
                    else if (yaw > 135.0f || yaw < -135.0f)
                    {
                        direction = FacingDirection.Down;
                        facingVector = new Vector2(0, -1);
                    }
                    // Left
                    else
                    {
                        direction = FacingDirection.Left;
                        facingVector = new Vector2(-1, 0);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Checks to see if you are on a slope
    /// </summary>
    void CheckSlope()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Vector3.down, out SlopeHit, 3f))
        {
            Angle = Vector3.Angle(Vector3.up, SlopeHit.normal);
            onSlope = Angle < maxSlopeAngle && Angle != 0;
        }
    }

    //This was just a gizmo drawn for slop detection debugging, consider deleting
    
        void OnDrawGizmosSelected()
        {
            // Draw the raycast gizmo
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.position + Vector3.down * 3f);
        }
    
    /// <summary>
    /// Vector3 generated based on the movement on a slope
    /// </summary>
    Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(movement, SlopeHit.normal);
    }

    //Plays a dialogue for test purposes
    public void SayQuote()
    {
        StartCoroutine(DialogueManager.Instance.ShowConversation(convo));
    }

    //Returns the facting direction
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
        return new Vector2(0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Reactivate the character controller and deactivate the Rigidbody
            characterController.enabled = true;
            rb.velocity = Vector3.zero;
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

    /// <summary>
    /// Opens Menu
    /// </summary>
    void OpenMenu()
    {
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        if (kb.escapeKey.wasPressedThisFrame)
        {
            UiManager.Instance.Hud.ToggleVisibility();
            UiManager.Instance.Menu.ToggleVisibility();
            //UiManager.Instance.SwitchToTab("Character");
        }
    }


    #region Debug Stuff
    void DebugUpdate()
    {
        Keyboard kb = InputSystem.GetDevice<Keyboard>();

        if (kb.tKey.wasPressedThisFrame)
        {
            UiManager.Instance.Debug.ToggleVisibility();
        }
    }
    #endregion
}
