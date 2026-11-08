
using UnityEngine;

public class AtaqueArma : State_Base
{
    private Arma arma;
    private float tiempoAtaque;

    public override void EnterState()
    {
        tiempoAtaque = 0f;

        arma = controlledObject.GetComponent<Arma>();

        if (arma != null)
        {
            arma.Atacar();
        }
    }

    public override void UpdateState()
    {
        tiempoAtaque += Time.deltaTime;

        if (tiempoAtaque >= 0.5f)
        {
            state_machine.SetState<IdleArma>();
        }
    }
}
