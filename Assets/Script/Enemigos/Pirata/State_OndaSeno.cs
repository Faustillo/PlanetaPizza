using UnityEngine;

/// <summary>
/// Estado del Pirata: aparece fuera de la pantalla por la derecha, a una altura
/// aleatoria, y avanza hacia la izquierda subiendo y bajando en onda senoidal
/// hasta salir por el lado izquierdo.
/// </summary>
public class State_OndaSeno : State_Base
{
    [Header("Avance")]
    [SerializeField] private float velocidadX = 3f;
    [SerializeField] private float margen = 0.1f; // fuera de pantalla (viewport)

    [Header("Onda (se sortea en cada aparición)")]
    [SerializeField] private Vector2 rangoAmplitud = new Vector2(1f, 2.5f);   // unidades de mundo
    [SerializeField] private Vector2 rangoFrecuencia = new Vector2(1f, 3f);   // radianes por segundo

    private Camera cam;
    private float xFin;
    private float yBase;
    private float amplitud;
    private float frecuencia;
    private float tiempo;

    public override void EnterState()
    {
        if (cam == null) cam = Camera.main;

        // Límites de la pantalla en mundo
        Vector3 abajoIzq = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector3 arribaDer = cam.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        float xInicio = cam.ViewportToWorldPoint(new Vector3(1f + margen, 0f, 0f)).x;
        xFin = cam.ViewportToWorldPoint(new Vector3(-margen, 0f, 0f)).x;

        // Sorteo de la onda; la amplitud se limita para que la onda entre en pantalla
        float alto = arribaDer.y - abajoIzq.y;
        amplitud = Mathf.Min(Random.Range(rangoAmplitud.x, rangoAmplitud.y), alto / 2f - 0.1f);
        frecuencia = Random.Range(rangoFrecuencia.x, rangoFrecuencia.y);

        // Altura central aleatoria, dejando lugar para la amplitud arriba y abajo
        yBase = Random.Range(abajoIzq.y + amplitud, arribaDer.y - amplitud);

        tiempo = 0f;
        controlledObject.transform.position = new Vector3(xInicio, yBase, 0f);
    }

    public override void UpdateState()
    {
        tiempo += Time.deltaTime;

        Transform t = controlledObject.transform;
        float x = t.position.x - velocidadX * Time.deltaTime;
        float y = yBase + amplitud * Mathf.Sin(tiempo * frecuencia);

        t.position = new Vector3(x, y, 0f);

        if (x <= xFin)
            controlledObject.SetActive(false); // salió por la izquierda: vuelve al pool
    }
}