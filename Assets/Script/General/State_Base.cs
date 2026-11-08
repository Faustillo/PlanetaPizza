using UnityEngine;

public class State_Base : MonoBehaviour
{
    // Cambiamos el nombre a state_machine para que coincida con tus otros scripts
    protected State_Machine state_machine;
    protected GameObject controlledObject;

    protected virtual void Awake()
    {
        // Buscamos la referencia con el nombre corregido
        state_machine = GetComponentInParent<State_Machine>();

        if (state_machine != null)
        {
            controlledObject = state_machine.gameObject;
        }
    }

    public virtual void EnterState() { }
    public virtual void UpdateState() { }

    // Dejamos el ExitState sin parámetros para evitar el error CS0115 anterior
    public virtual void ExitState(string nextState) { }
}
