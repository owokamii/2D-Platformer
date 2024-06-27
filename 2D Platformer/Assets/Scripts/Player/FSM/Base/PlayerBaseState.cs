using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateMachine player);

    public abstract void UpdateState(PlayerStateMachine player);

    public abstract void OnCollisionEnter2D(PlayerStateMachine player, Collision2D collision);
}
