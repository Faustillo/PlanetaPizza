
using UnityEngine;

public class IdleState : State_Base
{
    private Jugador jugador;

    protected override void Awake()
    {
        base.Awake();
        jugador = controlledObject.GetComponent<Jugador>();
    }

    public override void EnterState()
    {
        Debug.Log("Jugador: Idle");
    }

    public override void UpdateState()
    {
        if (jugador.DireccionMovimiento == Vector2.zero)
            return;

        if (jugador.SprintSolicitado && jugador.PuedeHacerSprint)
            state_machine.SetState<SprintState>();
        else
            state_machine.SetState<MovimientoState>();
    }
}
