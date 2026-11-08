using System.Collections;
using UnityEngine;

public class State_Machine : MonoBehaviour
{
    // Mismo nombre de campo que antes: no se pierde la asignación en los prefabs.
    [SerializeField] private State_Base currentState;

    private State_Base initialState;
    private State_Base nextState;
    private Coroutine stateMachineCoroutine;

    private void Awake()
    {
        // Guardamos el estado inicial para poder reiniciar (pooling / respawn).
        initialState = currentState;
    }

    // OnEnable en vez de Start: se ejecuta cada vez que el objeto se reactiva.
    private void OnEnable()
    {
        currentState = initialState;
        nextState = null;

        if (currentState != null)
            stateMachineCoroutine = StartCoroutine(StateMachineLoop());
    }

    private void OnDisable()
    {
        if (stateMachineCoroutine != null)
        {
            StopCoroutine(stateMachineCoroutine);
            stateMachineCoroutine = null;
        }
    }

    private IEnumerator StateMachineLoop()
    {
        while (true)
        {
            currentState.EnterState();

            while (nextState == null)
            {
                currentState.UpdateState();
                yield return null;
            }

            // Antes no se llamaba nunca: ahora el estado puede limpiarse al salir.
            currentState.ExitState(nextState.GetType().Name);

            currentState = nextState;
            nextState = null;
        }
    }

    public void SetState(State_Base newState)
    {
        if (newState != null && newState != currentState)
            nextState = newState;
    }

    public void SetState<T>() where T : State_Base
    {
        var newState = GetComponentInChildren<T>();
        if (newState != null && newState != currentState)
            nextState = newState;
    }

    public State_Base GetCurrentState() => currentState;
}
