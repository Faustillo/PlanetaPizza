using UnityEngine;

/// <summary>
/// Base de todos los enemigos. Hace daño al Jugador por contacto.
/// El movimiento/comportamiento vive en los States (State_Machine), no acá.
/// </summary>
public abstract class Enemigo : Entidad
{
    [Header("Enemigo")]
    [SerializeField] protected float danioContacto = 15f;

    [Tooltip("Tiempo que se suma al reloj al matarlo (aleatorio entre X e Y).")]
    [SerializeField] protected Vector2 rangoTiempoExtra = new Vector2(1f, 5f);

    /// <summary>Se calcula al morir. El GameManager lo lee desde OnMuerte.</summary>
    public float TiempoExtra { get; private set; }

    // El collider del cuerpo debe ser Is Trigger.
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!estaViva || !other.CompareTag("Player")) return;

        Jugador jugador = other.GetComponent<Jugador>();
        if (jugador != null)
            OnColisionJugador(jugador);
    }

    /// <summary>Qué pasa al chocar con el jugador. Los hijos pueden sobrescribirlo.</summary>
    public virtual void OnColisionJugador(Jugador jugador)
    {
        jugador.RecibirDanio(danioContacto);
    }

    public override void Morir()
    {
        if (!estaViva) return;

        // Se calcula antes de avisar para que los suscriptores a OnMuerte ya lo tengan.
        TiempoExtra = Random.Range(rangoTiempoExtra.x, rangoTiempoExtra.y);
        base.Morir();
    }
}
