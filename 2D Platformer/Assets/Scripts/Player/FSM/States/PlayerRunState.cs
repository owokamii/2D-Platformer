using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public override void EnterState(PlayerStateMachine player)
    {
        Debug.Log("entered run state");
    }

    public override void UpdateState(PlayerStateMachine player)
    {
        Debug.Log("currently run state");

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            player.SwitchState(player.playerIdleState);
        }
    }

    public override void OnCollisionEnter2D(PlayerStateMachine player, Collision2D collision)
    {
        
    }
}
