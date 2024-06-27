using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator animator;

    /*
    [Header("Animation States")]
    //[SerializeField] private AnimationStates animationStates;

    /*public enum AnimationStates
    {
        playerIdle,
        playerWalk,
        playerSprint,
        playerJump
    }*/
    
    const string playerIdle = "KnightIdle";
    const string playerRun = "KnightRun";
    const string playerJump = "KnightJump";
    const string playerFall = "KnightFall";
    const string playerLand = "KnightLand";
    const string playerHanging = "KnightHanging";
    const string playerDodgeRolling = "KnightRoll";
    const string playerAttack1 = "KnightAttack2";

    [Header("Hidden")]
    private PlayerMovement playerMovement;
    private PlayerLedgeGrab playerLedgeGrab;
    private PlayerAttack playerAttack;
    private string currentState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerLedgeGrab = GetComponent<PlayerLedgeGrab>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        HandleAnimation();
    }

    private void HandleAnimation()
    {
        if (playerMovement.GetisDodgeRolling) //if youre dodge rolling
        {
            ChangeAnimationState(playerDodgeRolling);
        }
        else // else if youre not dodge rolling
        {
            if(playerAttack.GetisAttacking) //if you're attacking
            {
                ChangeAnimationState(playerAttack1);
            }
            else //if youre not attacking
            {
                if (playerMovement.IsGrounded()) //if youre on ground
                {
                    if (playerMovement.GetxVelocity != 0) //if movement is detected
                    {
                        ChangeAnimationState(playerRun);
                    }
                    else //if movement is not detected
                    {
                        ChangeAnimationState(playerIdle);
                    }
                }
                else //if youre not on ground
                {
                    if (playerLedgeGrab.GetisGrabbing) //if player is grabbing
                    {
                        ChangeAnimationState(playerHanging);
                    }
                    else if (rigidBody.velocity.y > 0) //if player is not grabbing and is going up
                    {
                        ChangeAnimationState(playerJump);
                    }
                    else //if player is not grabbing and is going down
                    {
                        ChangeAnimationState(playerFall);

                        if (playerMovement.IsGrounded())
                        {
                            ChangeAnimationState(playerLand); // doesnt really play ..., it looks fine without this too
                        }

                    }
                }
            }
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);
        currentState = newState;
    }
}
