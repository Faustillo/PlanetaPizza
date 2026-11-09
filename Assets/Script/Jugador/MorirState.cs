using UnityEngine;

public class MorirState : State_Base
{
    private Jugador jugador;

    protected override void Awake()
    {
        base.Awake();
        jugador = controlledObject.GetComponent<Jugador>();
    }

    public override void EnterState()
    {
        Debug.Log("Jugador: Morir");

        jugador.Morir();
    }

    public override void UpdateState()
    {
    }

    public override void ExitState(string nextState)
    {
    }
}