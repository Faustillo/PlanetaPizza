using UnityEngine;
using UnityEngine.InputSystem;

public class Moverse : State_Base
{
    private Jugador jugador;
    private Rigidbody2D rb;

    public override void EnterState()
    {
        jugador = controlledObject.GetComponent<Jugador>();
        rb = controlledObject.GetComponent<Rigidbody2D>();
    }

    public override void UpdateState()
    {
        // 1. Transición al estado Saltar si se presiona la barra espaciadora 🦘
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            state_machine.SetState<Saltar>();
            return;
        }

        // 2. Control de movimiento horizontal 🚶
        float horizontal = 0;

        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            horizontal = -1;
        else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            horizontal = 1;

        if (horizontal != 0)
        {
            // Guardamos hacia dónde mira el personaje
            jugador.direccion = horizontal > 0 ? 1 : -1;

            // Aplicamos el movimiento horizontal
            rb.linearVelocity = new Vector2(horizontal * jugador.velocidad, rb.linearVelocity.y);
        }
        else
        {
            // Frenamos el movimiento horizontal y volvemos a Idle
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            state_machine.SetState<Idle>();
        }
    }
}