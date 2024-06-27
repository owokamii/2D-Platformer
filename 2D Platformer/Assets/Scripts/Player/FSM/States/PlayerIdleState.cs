using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateMachine player)
    {
        Debug.Log("entered idle state");
    }

    public override void UpdateState(PlayerStateMachine player)
    {
        Debug.Log("currently idle state");

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            player.SwitchState(player.playerRunState);
        }
        if (Input.GetKeyDown(KeyCode.Space)) // if not grounded
        {
            player.SwitchState(player.playerJumpState);
        }

    }

    public override void OnCollisionEnter2D(PlayerStateMachine player, Collision2D collision)
    {
        
    }
}
