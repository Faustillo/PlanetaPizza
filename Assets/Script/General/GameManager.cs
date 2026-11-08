using UnityEngine;
using UnityEngine.SceneManagement;

public enum EstadoJuego { Jugando, Victoria, Derrota }

/// <summary>
/// Controla el estado de la partida. Versión mínima: victoria y derrota.
/// Luego se le puede agregar el contador de tiempo (tiempoRestante, AgregarTiempo).
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Pantallas (Panels del Canvas, desactivados al inicio)")]
    [SerializeField] private GameObject panelVictoria;
    [SerializeField] private GameObject panelDerrota; // opcional por ahora

    [Header("Spawners a detener al terminar")]
    [SerializeField] private OvniSpawner ovniSpawner;
    [SerializeField] private PirataSpawner pirataSpawner;

    [Header("Escenas")]
    [SerializeField] private string escenaMenu = "Menu";

    public EstadoJuego Estado { get; private set; } = EstadoJuego.Jugando;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f; // por si se llegó acá desde una pausa
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
