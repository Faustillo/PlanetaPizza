
using UnityEngine;

public class SprintState : State_Base
{
    private Jugador jugador;

    protected override void Awake()
    {
        base.Awake();
        jugador = controlledObject.GetComponent<Jugador>();
    }

    public override void EnterState()
    {
        Debug.Log("Jugador: Sprint");
    }

    public override void UpdateState()
    {
        if (!jugador.PuedeHacerSprint || !jugador.SprintSolicitado)
        {
            VolverAlEstadoCorrespondiente();
            return;
        }

        // Movimiento con velocidad aumentada.
        jugador.Mover(jugador.MultVelocidadSprint);

        // Consumo adicional de energia por sprint.
        jugador.ConsumirEnergiaSprint(Time.deltaTime);
    }

    private void VolverAlEstadoCorrespondiente()
    {
        if (jugador.DireccionMovimiento != Vector2.zero)
            state_machine.SetState<MovimientoState>();
        else
            state_machine.SetState<IdleState>();
    }
}
