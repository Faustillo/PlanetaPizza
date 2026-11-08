using System.Collections;
using UnityEngine;

public class State_Machine : MonoBehaviour
{
    [SerializeField] private State_Base currentState;
    private State_Base nextState;
    private Coroutine stateMachineCoroutine;

    void Start()
    {
        if (currentState != null)
        {
            // Iniciamos la máquina de estados
            stateMachineCoroutine = StartCoroutine(StateMachineLoop());
        }
    }

    /// <summary>
    /// El corazón de la máquina de estados. 
    /// Corre en paralelo y no bloquea el Update general del juego.
    /// </summary>
    private IEnumerator StateMachineLoop()
    {
        while (true)
        {
            if (currentState == null)
            {
                yield return null;
                continue;
            }

            currentState.EnterState();

            while (nextState == null)
            {
                currentState.UpdateState();
                // Esto obliga a Unity a saltar al siguiente frame 
                // ignorando el TimeScale para la lógica de la corrutina
                yield return null;
            }

            currentState = nextState;
            nextState = null;
        }
    }

    public void SetState(State_Base newState)
    {
        if (newState != null && newState != currentState)
        {
            nextState = newState;
        }
    }

    public void SetState<T>() where T : State_Base
    {
        var newState = GetComponentInChildren<T>();
        if (newState != null && newState != currentState)
        {
            nextState = newState;
        }
    }

    public State_Base GetCurrentState() => currentState;

    // Limpieza por seguridad
    private void OnDisable()
    {
        if (stateMachineCoroutine != null) StopCoroutine(stateMachineCoroutine);
    }
}