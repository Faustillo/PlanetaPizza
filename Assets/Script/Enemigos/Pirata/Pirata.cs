using UnityEngine;

/// <summary>
/// Pirata: cruza de derecha a izquierda en onda senoidal (State_OndaSeno)
/// y dispara hacia adelante (izquierda) con una frecuencia aleatoria.
/// Solo dispara mientras está dentro de la pantalla.
/// </summary>
public class Pirata : Enemigo
{
    [Header("Disparo")]
    [SerializeField] private Proyectil proyectilPrefab;
    [SerializeField] private Transform puntoDisparo; // hijo ubicado en el frente de la nave
    [SerializeField] private float danioProyectil = 10f;
    [SerializeField] private float intervaloMin = 1f;
    [SerializeField] private float intervaloMax = 3f;

    private float temporizadorDisparo;
    private Camera cam;

    protected override void Awake()
    {
        base.Awake();
        cam = Camera.main;
    }

    // Cada vez que el spawner lo reactiva, vuelve como nuevo.
    private void OnEnable()
    {
        Reiniciar();
        ReiniciarTemporizador();
    }

    /// <summary>Lógica independiente del estado: el disparo.</summary>
    public override void Actualizar(float dt)
    {
        if (!EstaEnPantalla()) return;

        temporizadorDisparo -= dt;
        if (temporizadorDisparo <= 0f)
        {
            Disparar();
            ReiniciarTemporizador();
        }
    }

    private void Disparar()
    {
        Vector2 origen = puntoDisparo != null ? (Vector2)puntoDisparo.position : (Vector2)transform.position;

        Proyectil p = Instantiate(proyectilPrefab);
        p.Inicializar(origen, Vector2.left, Faccion.Enemigo, danioProyectil);
    }

    private void ReiniciarTemporizador()
    {
        temporizadorDisparo = Random.Range(intervaloMin, intervaloMax);
    }

    private bool EstaEnPantalla()
    {
        if (cam == null) cam = Camera.main;

        Vector3 v = cam.WorldToViewportPoint(transform.position);
        return v.x > 0f && v.x < 1f && v.y > 0f && v.y < 1f;
    }
}
