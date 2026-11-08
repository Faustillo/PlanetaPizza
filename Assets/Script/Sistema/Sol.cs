using UnityEngine;

/// <summary>
/// Sol: objeto con dos colliders.
///  1) Collider del cuerpo (IsTrigger = false): el sol físico.
///  2) Collider de la zona de recarga (IsTrigger = true): más grande que el cuerpo.
/// Mientras el Jugador esté dentro de la zona, se le recarga energía.
/// </summary>
public class Sol : MonoBehaviour
{
    [Header("Recarga")]
    [SerializeField] private float tasaRecarga = 20f; // energía por segundo

    private Jugador jugadorEnZona; // null si no hay nadie dentro

    public bool JugadorEnRango()
    {
        return jugadorEnZona != null;
    }

    // Solo el collider marcado como Trigger (la zona) dispara estos eventos.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        jugadorEnZona = other.GetComponent<Jugador>();
        if (jugadorEnZona != null)
            jugadorEnZona.SetRecargando(true); // bloquea disparo mientras carga
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (jugadorEnZona == null || !other.CompareTag("Player")) return;

        Recargar(jugadorEnZona, Time.deltaTime);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (jugadorEnZona != null)
            jugadorEnZona.SetRecargando(false);
        jugadorEnZona = null;
    }

    public void Recargar(Jugador jugador, float dt)
    {
        jugador.RecargarEnergia(tasaRecarga * dt);
    }
}
