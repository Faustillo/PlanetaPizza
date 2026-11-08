using System;
using UnityEngine;

/// <summary>
/// Clase base de Jugador y Enemigo.
/// Comparten vida, velocidad, movimiento, animación y muerte.
/// En el Jugador, "vida" es la energía de la nave.
/// </summary>
public abstract class Entidad : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float vidaMax = 100f;
    [SerializeField] protected float velocidad = 5f;

    [Header("Referencias")]
    [SerializeField] protected Animator animador;

    protected float vida;
    protected Vector2 direccion;
    protected bool estaViva = true;

    // Eventos para UI, GameManager, Spawner, etc.
    public event Action<float, float> OnVidaCambiada; // (vidaActual, vidaMax)
    public event Action<Entidad> OnMuerte;

    public Vector2 Posicion => transform.position;
    public bool EstaViva => estaViva;
    public float Vida => vida;
    public float VidaMax => vidaMax;

    protected virtual void Awake()
    {
        vida = vidaMax;
        if (animador == null)
            animador = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!estaViva) return;
        Actualizar(Time.deltaTime);
    }

    /// <summary>
    /// Lógica por frame que NO depende del estado (ej: consumo pasivo de energía).
    /// El comportamiento (perseguir, zigzag, etc.) va en los States, no acá.
    /// </summary>
    public virtual void Actualizar(float dt) { }

    /// <summary>Mueve la entidad según 'direccion' y 'velocidad'.</summary>
    public virtual void Mover(float dt)
    {
        transform.Translate(direccion.normalized * velocidad * dt, Space.World);
    }

    /// <summary>Resta vida. Si llega a 0 llama a Morir().</summary>
    public virtual void RecibirDanio(float cantidad)
    {
        if (!estaViva || cantidad <= 0f) return;

        CambiarVida(vida - cantidad);

        if (vida <= 0f)
            Morir();
    }

    /// <summary>Marca la entidad como muerta y avisa por evento.</summary>
    public virtual void Morir()
    {
        if (!estaViva) return;

        estaViva = false;
        // Con pooling, State_Machine reinicia en OnEnable (ver State_Machine.cs).
        OnMuerte?.Invoke(this);
        // Los hijos pueden sobrescribir para animación, drops, etc.
        // y deben llamar a base.Morir().
        gameObject.SetActive(false);
    }

    /// <summary>Deja la entidad como nueva (para reutilizarla desde un pool).</summary>
    public virtual void Reiniciar()
    {
        estaViva = true;
        CambiarVida(vidaMax);
    }

    public void ReproducirAnimacion(string nombre)
    {
        if (animador != null)
            animador.Play(nombre);
    }

    /// <summary>Único punto donde se modifica 'vida' (la limita y notifica).</summary>
    protected void CambiarVida(float nuevaVida)
    {
        vida = Mathf.Clamp(nuevaVida, 0f, vidaMax);
        OnVidaCambiada?.Invoke(vida, vidaMax);
    }
}