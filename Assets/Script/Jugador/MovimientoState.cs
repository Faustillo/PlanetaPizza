using UnityEngine;

public class MovimientoState : State_Base
{
    private Jugador jugador;

    protected override void Awake()
    {
        base.Awake();
        jugador = controlledObject.GetComponent<Jugador>();
    }

    public override void EnterState()
    {
        Debug.Log("Jugador: Movimiento");
    }

    public override void UpdateState()
    {
        if (jugador.DireccionMovimiento == Vector2.zero)
        {
            jugador.Detener(); // con física hay que frenar la velocidad explícitamente
            state_machine.SetState<IdleState>();
            return;
        }

        if (jugador.SprintSolicitado && jugador.PuedeHacerSprint)
        {
            state_machine.SetState<SprintState>();
            return;
        }

        jugador.Mover(1f);
    }
}
