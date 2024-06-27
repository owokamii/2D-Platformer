using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Attributes")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dodgeRollForce;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("References")]
    [SerializeField] private GameObject turningCam;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Hidden")]
    private CameraTurn cameraTurn;
    private PlayerLedgeGrab playerLedgeGrab;
    private bool isFacingRight = true;
    private float dodgeRollTime = 0.3f;
    private float dodgeRollCooldown = 0.4f;
   // private float attackTime = 0.3f;
    private bool canDodgeRoll = true;
    private bool isDodgeRolling = false;
    //private bool isAttacking = false;
    //private bool canAttack = true;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    private float xAxis;

    public float GetxAxis { get; private set; }
    public float GetxVelocity { get; private set; } // still need to fix this that relates to animation playing movement eventho running up against a wall
    public bool GetisFacingRight { get; private set; }
    public bool GetisDodgeRolling { get; private set; }
   /* public bool GetisAttacking { get; private set; }*/

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerLedgeGrab = GetComponent<PlayerLedgeGrab>();
        cameraTurn = turningCam.GetComponent<CameraTurn>();
    }

    private void Update()
    {
        HandleDodgeRoll();

        if (isDodgeRolling) return;

        HandleGravity();
        HandleJump();
        HandleMovement();

    }

    public bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayerMask);
        return raycastHit.collider != null;
    }

    private void HandleGravity()
    {
        if (rigidBody.velocity.y < 0)
        {
            rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidBody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        if (xAxis < 0 && isFacingRight || xAxis > 0 && !isFacingRight)
        {
            HandleFlip();
        }

        if(!playerLedgeGrab.GetisGrabbing)
        {
            xAxis = Input.GetAxisRaw("Horizontal");
            rigidBody.velocity = new Vector2(xAxis * runSpeed, rigidBody.velocity.y);
        }

        GetxVelocity = rigidBody.velocity.x;
    }

    private void HandleJump()
    {   
        if(IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            //rigidBody.velocity = Vector2.up * jumpForce; //old version of jump
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
        }

        if(Input.GetKeyUp(KeyCode.Space) && rigidBody.velocity.y > 0f) //low jump
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
    }

    private void HandleDodgeRoll()
    {
        GetisDodgeRolling = isDodgeRolling;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDodgeRoll)
        {
            StartCoroutine(DodgeRoll());
        }
    }

    /*private void HandleAttack()
    {
        GetisAttacking = isAttacking;

        if(!playerLedgeGrab.GetisGrabbing)
        {
            if (Input.GetMouseButtonDown(0) && canAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }*/

    private IEnumerator DodgeRoll()
    {
        canDodgeRoll = false;
        isDodgeRolling = true;

        rigidBody.velocity = new Vector2(transform.localScale.x * dodgeRollForce, 0);
        yield return new WaitForSeconds(dodgeRollTime);

        isDodgeRolling = false;
        yield return new WaitForSeconds(dodgeRollCooldown);
        canDodgeRoll = true;
    }

    /*private IEnumerator Attack()
    {
        canAttack = false;
        isAttacking = true;

        rigidBody.velocity = new Vector2(0, 0); 
        yield return new WaitForSeconds(attackTime); // need to be adjusted according to attack 1 - 3's total time frame

        isAttacking = false;
        canAttack = true;
    }*/

    private void HandleFlip()
    {
        isFacingRight = !isFacingRight;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        cameraTurn.CallTurn();

        /*if (isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
            cameraTurn.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
            cameraTurn.CallTurn();
        }*/
    }
}

/*
// ledge grab
// if you're hanging off a ledge:
// - you can press cancel to drop down
// - or press action to jump onto the platform
// ledge grab happens when you jump onto a platform's edge
// (considering if you drop near a ledge to perform ledge grab or not)
*/
