using UnityEngine;

public enum Faccion { Jugador, Enemigo }

/// <summary>
/// Proyectil recto. Daña solo a la facción contraria:
///  - Faccion.Jugador -> daña Enemigos
///  - Faccion.Enemigo -> daña al Jugador
/// El collider debe ser Is Trigger.
/// </summary>
public class Proyectil : MonoBehaviour
{
    [SerializeField] private float danio = 10f;
    [SerializeField] private float velocidad = 8f;
    [SerializeField] private float vidaUtil = 4f; // segundos antes de autodestruirse

    private Faccion faccion;
    private Vector2 direccion;

    /// <summary>Se llama justo después de Instantiate.</summary>
    public void Inicializar(Vector2 pos, Vector2 dir, Faccion faccionDueno, float danioProyectil)
    {
        transform.position = pos;
        direccion = dir.normalized;
        faccion = faccionDueno;
        danio = danioProyectil;

        Destroy(gameObject, vidaUtil);
    }

    private void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Entidad entidad = other.GetComponent<Entidad>();
        if (entidad == null) return;

        bool esObjetivoValido = faccion == Faccion.Jugador
            ? entidad is Enemigo
            : entidad is Jugador;

        if (!esObjetivoValido) return;

        entidad.RecibirDanio(danio);
        Desactivar();
    }

    public void Desactivar()
    {
        Destroy(gameObject);
    }
}
