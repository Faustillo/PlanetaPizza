using UnityEngine;
using UnityEngine.InputSystem;

public class IdleArma : State_Base
{
    public override void UpdateState()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            state_machine.SetState<AtaqueArma>();
        }
    }
}