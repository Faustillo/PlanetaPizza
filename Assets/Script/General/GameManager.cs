using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EstadoJuego { Jugando, Victoria, Derrota }

/// <summary>
/// Controla el estado de la partida: tiempo, victoria y derrota.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Tiempo")]
    [SerializeField] private float tiempoInicial = 60f;
    [SerializeField] private TextMeshProUGUI textoTiempo;
    [SerializeField] private float segundosAlerta = 10f; // el texto se pone rojo por debajo de esto
    [SerializeField] private Color colorNormal = Color.white;
    [SerializeField] private Color colorAlerta = Color.red;

    [Header("Pantallas (Panels del Canvas, desactivados al inicio)")]
    [SerializeField] private GameObject panelVictoria;
    [SerializeField] private GameObject panelDerrota;
    [Tooltip("Opcional: texto dentro del panel de derrota donde se muestra el motivo.")]
    [SerializeField] private TextMeshProUGUI textoMotivoDerrota;

    [Header("Spawners a detener al terminar")]
    [SerializeField] private OvniSpawner ovniSpawner;
    [SerializeField] private PirataSpawner pirataSpawner;

    [Header("Escenas")]
    [SerializeField] private string escenaMenu = "Menu";

    private float tiempoRestante;

    public EstadoJuego Estado { get; private set; } = EstadoJuego.Jugando;
    public float TiempoRestante => tiempoRestante;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f; // por si se llegó acá desde una pausa
        tiempoRestante = tiempoInicial;
        ActualizarTextoTiempo();
    }

    private void Update()
    {
        if (Estado != EstadoJuego.Jugando) return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            ActualizarTextoTiempo();
            Derrota("tiempo");
            return;
        }

        ActualizarTextoTiempo();
    }

    /// <summary>Suma (o resta, si es negativo) segundos al reloj.</summary>
    public void AgregarTiempo(float segundos)
    {
        if (Estado != EstadoJuego.Jugando) return;

        tiempoRestante = Mathf.Max(0f, tiempoRestante + segundos);
    }

    private void ActualizarTextoTiempo()
    {
        if (textoTiempo == null) return;

        int total = Mathf.CeilToInt(tiempoRestante);
        textoTiempo.text = $"{total / 60:00}:{total % 60:00}";
        textoTiempo.color = tiempoRestante <= segundosAlerta ? colorAlerta : colorNormal;
    }

    public void Victoria()
    {
        if (Estado != EstadoJuego.Jugando) return; // ya terminó la partida

        Estado = EstadoJuego.Victoria;
        Terminar(panelVictoria);
    }

    public void Derrota(string motivo = "")
    {
        if (Estado != EstadoJuego.Jugando) return;

        Estado = EstadoJuego.Derrota;
        Debug.Log("Derrota: " + motivo);

        if (textoMotivoDerrota != null)
        {
            textoMotivoDerrota.text = motivo == "tiempo"
                ? "¡Se acabó el tiempo!\nLa pizza se enfrió."
                : "¡Te quedaste sin energía!";
        }

        Terminar(panelDerrota);
    }

    private void Terminar(GameObject panel)
    {
        if (ovniSpawner != null) ovniSpawner.Detener();
        if (pirataSpawner != null) pirataSpawner.Detener();

        if (panel != null) panel.SetActive(true);

        Time.timeScale = 0f; // congela el juego detrás de la pantalla
    }

    // --- Botones de las pantallas (OnClick) ---

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaMenu);
    }
}
