using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Menú principal. Los botones llaman a Jugar() y Salir() desde el evento OnClick.
/// </summary>
public class MenuPrincipal : MonoBehaviour
{
    [Tooltip("Nombre exacto de la escena del nivel (debe estar en Build Profiles).")]
    [SerializeField] private string escenaNivel = "Prueba";

    [Tooltip("Segundos de espera antes de actuar, para que alcance a sonar el clic.")]
    [SerializeField] private float retardo = 0.25f;

    private bool ocupado;

    public void Jugar()
    {
        if (ocupado) return;
        StartCoroutine(JugarConRetardo());
    }

    public void Salir()
    {
        if (ocupado) return;
        StartCoroutine(SalirConRetardo());
    }

    private IEnumerator JugarConRetardo()
    {
        ocupado = true;
        yield return new WaitForSecondsRealtime(retardo);

        Time.timeScale = 1f; // por si se llegó acá desde una pausa
        SceneManager.LoadScene(escenaNivel);
    }

    private IEnumerator SalirConRetardo()
    {
        ocupado = true;
        yield return new WaitForSecondsRealtime(retardo);

        Debug.Log("Salir: en el editor no cierra nada, solo funciona en el juego compilado.");
        Application.Quit();
        ocupado = false; // en el editor sigue en el menú
    }
}
