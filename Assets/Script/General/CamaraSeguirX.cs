using UnityEngine;

/// <summary>
/// La cámara sigue al objetivo (el Jugador) solo en el eje X.
/// Y y Z quedan fijos. Opcionalmente limita el recorrido para no mostrar
/// espacio fuera del nivel.
/// Ponerlo en la Main Camera.
/// </summary>
public class CamaraSeguirX : MonoBehaviour
{
    [Tooltip("Si queda vacío, busca el objeto con tag Player.")]
    [SerializeField] private Transform objetivo;

    [Tooltip("Desfase en X respecto al jugador. Positivo = la cámara va más adelante.")]
    [SerializeField] private float desfaseX = 0f;

    [Tooltip("Tiempo de suavizado. 0 = sigue pegado al jugador.")]
    [SerializeField] private float suavizado = 0.15f;

    [Header("Límites del nivel (posición de la cámara)")]
    [SerializeField] private bool usarLimites = false;
    [SerializeField] private float xMin = 0f;
    [SerializeField] private float xMax = 50f;

    private float velocidadActual;

    private void Start()
    {
        if (objetivo == null)
        {
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");
            if (jugador != null) objetivo = jugador.transform;
        }
    }

    // LateUpdate: la cámara se mueve después de que el jugador ya se movió (evita temblores).
    private void LateUpdate()
    {
        if (objetivo == null) return;

        float xDeseada = objetivo.position.x + desfaseX;

        if (usarLimites)
            xDeseada = Mathf.Clamp(xDeseada, xMin, xMax);

        float x = suavizado > 0f
            ? Mathf.SmoothDamp(transform.position.x, xDeseada, ref velocidadActual, suavizado)
            : xDeseada;

        // Solo cambia X: Y y Z se mantienen.
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
