using UnityEngine;

/// <summary>
/// Estado del Ovni: aparece fuera de una esquina aleatoria de la cámara
/// y cruza en diagonal hasta la esquina opuesta.
/// </summary>
public class State_CruzarDiagonal : State_Base
{
    [SerializeField] private float velocidad = 4f;
    [Tooltip("Qué tan afuera de la pantalla aparece (en coordenadas de viewport).")]
    [SerializeField] private float margen = 0.1f;

    private Camera cam;
    private Vector3 destino;

    // Esquinas en viewport. El índice opuesto a i es (3 - i):
    // 0 <-> 3 (abajo-izq <-> arriba-der), 1 <-> 2 (abajo-der <-> arriba-izq)
    private static readonly Vector2[] esquinas =
    {
        new Vector2(0f, 0f), // 0: abajo-izquierda
        new Vector2(1f, 0f), // 1: abajo-derecha
        new Vector2(0f, 1f), // 2: arriba-izquierda
        new Vector2(1f, 1f), // 3: arriba-derecha
    };

    protected override void Awake()
    {
        base.Awake();
        cam = Camera.main;
    }

    public override void EnterState()
    {
        if (cam == null) cam = Camera.main;

        int origen = Random.Range(0, 4);
        int opuesta = 3 - origen;

        controlledObject.transform.position = EsquinaAMundo(origen);
        destino = EsquinaAMundo(opuesta);
    }

    public override void UpdateState()
    {
        Transform t = controlledObject.transform;

        t.position = Vector3.MoveTowards(t.position, destino, velocidad * Time.deltaTime);

        if (Vector3.Distance(t.position, destino) < 0.05f)
        {
            // Salió de la cámara: se desactiva (el Spawner puede volver a lanzarlo).
            controlledObject.SetActive(false);
        }
    }

    /// <summary>Convierte una esquina (viewport) a posición de mundo, un poco afuera de la pantalla.</summary>
    private Vector3 EsquinaAMundo(int indice)
    {
        Vector2 e = esquinas[indice];

        // Empuja el punto hacia afuera: 0 -> -margen, 1 -> 1 + margen
        float x = e.x == 0f ? -margen : 1f + margen;
        float y = e.y == 0f ? -margen : 1f + margen;

        Vector3 mundo = cam.ViewportToWorldPoint(new Vector3(x, y, 0f));
        mundo.z = 0f; // juego 2D
        return mundo;
    }
}