using UnityEngine;

/// <summary>
/// Zona triangular hija del Ovni. Mientras el Jugador esté dentro, se mueve más lento.
/// Requiere en este mismo objeto un PolygonCollider2D con Is Trigger activado.
/// </summary>
[RequireComponent(typeof(PolygonCollider2D))]
public class ZonaRalentizacion : MonoBehaviour
{
    [Tooltip("Multiplicador de velocidad del jugador dentro de la zona. 0.5 = mitad de velocidad.")]
    [SerializeField, Range(0.1f, 1f)] private float factorLentitud = 0.5f;

    private Jugador jugadorDentro;

    private void Reset()
    {
        // Al agregar el script, deja el collider como trigger automáticamente.
        GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || jugadorDentro != null) return;

        jugadorDentro = other.GetComponent<Jugador>();
        if (jugadorDentro != null)
            jugadorDentro.AplicarLentitud(factorLentitud);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        LiberarJugador();
    }

    // Si el Ovni se desactiva (llegó a la otra esquina) con el jugador adentro,
    // Unity NO llama a OnTriggerExit2D, así que lo liberamos acá.
    private void OnDisable()
    {
        LiberarJugador();
    }

    private void LiberarJugador()
    {
        if (jugadorDentro == null) return;

        jugadorDentro.QuitarLentitud(factorLentitud);
        jugadorDentro = null;
    }
}
