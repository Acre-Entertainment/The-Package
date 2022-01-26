using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LookingDir
{
    Right,
    Left
}

public abstract class State<FSM>
{
    public abstract void Enter(FSM fsm);

    public abstract void Update(FSM fsm);

    public abstract void FixedUpdate(FSM fsm);

    public virtual void OnTriggerEnter2D(Collider2D other, FSM fsm) {}

    public virtual void OnTriggerExit2D(Collider2D other, FSM fsm) {}

    public virtual void OnTriggerStay2D(Collider2D other, FSM fsm) {}

    public virtual void OnCollisionEnter2D(Collision2D collision, FSM fsm) {}

    public virtual void Exit(FSM fsm) {}

    public virtual void DrawGizmos(FSM fsm) {}
}