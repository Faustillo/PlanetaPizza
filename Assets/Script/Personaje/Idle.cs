using UnityEngine;
using UnityEngine.InputSystem;

public class Idle : State_Base
{
    public override void UpdateState()
    {
        // 1. Transición al estado Saltar al presionar la barra espaciadora 🦘
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            state_machine.SetState<Saltar>();
            return;
        }

        // 2. Transición al estado Moverse si se presiona una tecla de dirección 🚶
        float horizontal = 0;

        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            horizontal = -1;
        else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            horizontal = 1;

        if (horizontal != 0)
        {
            state_machine.SetState<Moverse>();
        }
    }
}