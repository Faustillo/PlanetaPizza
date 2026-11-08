using UnityEngine;
using UnityEngine.InputSystem;

public class Saltar : State_Base
{
    private Jugador jugador;
    private Rigidbody2D rb;

    public override void EnterState()
    {
        // Mensaje de prueba para verificar si la máquina de estados activa este estado 🧪
        Debug.Log("¡Entró al estado Saltar!");

        jugador = controlledObject.GetComponent<Jugador>();
        rb = controlledObject.GetComponent<Rigidbody2D>();

        // Aplicamos el impulso vertical hacia arriba 🦘
        if (jugador != null && rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jugador.fuerzaDeSalto);
        }
    }

    public override void UpdateState()
    {
        // Control de movimiento horizontal en el aire 🌬️
        float horizontal = 0;

        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            horizontal = -1;
        else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            horizontal = 1;

        if (rb != null && jugador != null)
        {
            if (horizontal != 0)
            {
                jugador.direccion = horizontal > 0 ? 1 : -1;
            }

            rb.linearVelocity = new Vector2(horizontal * jugador.velocidad, rb.linearVelocity.y);
        }
    }

    // Regreso a Idle al hacer contacto con el suelo 🧱
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Suelo"))
        {
            state_machine.SetState<Idle>();
        }
    }
}