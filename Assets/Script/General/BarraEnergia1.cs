using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Barra de energía del Jugador. Se actualiza sola escuchando el evento
/// Entidad.OnVidaCambiada, así que no hace falta llamarla desde ningún lado.
/// Ponerla en un objeto UI y asignar la Image de relleno (Image Type = Filled).
/// </summary>
public class BarraEnergia1 : MonoBehaviour
{
    [Tooltip("Imagen que se vacía. Debe tener Image Type = Filled.")]
    [SerializeField] private Image relleno;

    [Tooltip("Si queda vacío, busca el objeto con tag Player.")]
    [SerializeField] private Jugador jugador;

    [Header("Aspecto")]
    [SerializeField] private Color colorLleno = new Color(0.3f, 0.9f, 0.3f);
    [SerializeField] private Color colorVacio = new Color(0.95f, 0.2f, 0.2f);
    [Tooltip("Qué tan rápido se mueve la barra hacia el valor real (0 = instantáneo).")]
    [SerializeField] private float velocidadSuavizado = 4f;

    private float objetivo;

    private void Start()
    {
        if (jugador == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null) jugador = obj.GetComponent<Jugador>();
        }

        if (jugador == null || relleno == null)
        {
            Debug.LogWarning("BarraEnergia: falta asignar el Jugador o la imagen de relleno.");
            enabled = false;
            return;
        }

        // El Jugador ya fijó su energía inicial en Awake (antes de que nos suscribamos),
        // así que leemos el valor actual a mano.
        objetivo = jugador.Vida / jugador.VidaMax;
        relleno.fillAmount = objetivo;
        ActualizarColor();

        jugador.OnVidaCambiada += AlCambiarVida;
    }

    private void OnDestroy()
    {
        if (jugador != null)
            jugador.OnVidaCambiada -= AlCambiarVida;
    }

    private void AlCambiarVida(float vidaActual, float vidaMax)
    {
        objetivo = vidaMax > 0f ? vidaActual / vidaMax : 0f;
    }

    private void Update()
    {
        // unscaledDeltaTime: la barra se anima aunque el juego esté en pausa.
        relleno.fillAmount = velocidadSuavizado > 0f
            ? Mathf.MoveTowards(relleno.fillAmount, objetivo, velocidadSuavizado * Time.unscaledDeltaTime)
            : objetivo;

        ActualizarColor();
    }

    private void ActualizarColor()
    {
        relleno.color = Color.Lerp(colorVacio, colorLleno, relleno.fillAmount);
    }
}
