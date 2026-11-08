using UnityEngine;

/// <summary>
/// Planeta destino. Si el Jugador entra en su zona, se gana la partida.
/// Requiere un collider con Is Trigger activado.
/// </summary>
public class Planeta : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        OnLlegadaJugador();
    }

    public void OnLlegadaJugador()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.Victoria();
    }
}
