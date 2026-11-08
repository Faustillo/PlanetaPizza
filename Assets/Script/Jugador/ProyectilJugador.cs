
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProyectilJugador : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float velocidad = 12f;
    [SerializeField] private float vidaUtil = 3f;
    [SerializeField] private float danio = 10f;

    private Rigidbody2D rb;
    private Vector2 direccion;
    private float temporizador;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        temporizador = vidaUtil;
    }

    private void Update()
    {
        temporizador -= Time.deltaTime;

        if (temporizador <= 0f)
            Desactivar();
    }

    private void FixedUpdate()
    {
        if (direccion == Vector2.zero)
            return;

        Vector2 desplazamiento =
            direccion * velocidad * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + desplazamiento);
    }

    public void Inicializar(Vector2 posicion, Vector2 direccionDisparo)
    {
        transform.position = posicion;
        direccion = direccionDisparo.normalized;
        temporizador = vidaUtil;
    }

    public void Desactivar()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;

        Enemigo enemigo = other.GetComponentInParent<Enemigo>();

        if (enemigo != null)
        {
            enemigo.RecibirDanio(danio);

            Debug.Log(
                $"[DISPARO] Impacto contra {enemigo.name} | Daño: {danio}"
            );
        }

        Desactivar();
    }
}
