using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override void EnterState(PlayerStateMachine player)
    {
        Debug.Log("entered jump state");
    }

    public override void UpdateState(PlayerStateMachine player)
    {
        Debug.Log("currently jump state");

        // if grounded - exit state
    }

    public override void OnCollisionEnter2D(PlayerStateMachine player, Collision2D collision)
    {

    }
}
