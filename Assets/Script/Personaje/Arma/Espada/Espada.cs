
using UnityEngine;

public class Espada : MonoBehaviour
{
    [SerializeField] private Transform puntoAgarre;

    [Header("Movimiento del ataque")]
    [SerializeField] private float duracionAtaque = 0.35f;
    [SerializeField] private float anguloAtaque = 100f;
    [SerializeField] private float anguloPreparacion = 15f;

    private Jugador jugador;
    private Arma arma;
    private Quaternion rotacionInicial;

    private bool atacando;
    private float tiempoAtaque;

    private void Awake()
    {
        jugador = GetComponentInParent<Jugador>();
        arma = GetComponent<Arma>();

        if (puntoAgarre != null)
        {
            rotacionInicial = puntoAgarre.localRotation;
        }
    }

    private void Update()
    {
        if (!atacando || puntoAgarre == null)
            return;

        tiempoAtaque += Time.deltaTime;

        float progreso = Mathf.Clamp01(
            tiempoAtaque / duracionAtaque
        );

        float direccion = jugador != null
            ? jugador.direccion
            : 1f;

        float angulo;

        if (progreso < 0.15f)
        {
            // Preparación breve
            float t = progreso / 0.15f;
            t = t * t * (3f - 2f * t);

            angulo = Mathf.Lerp(
                0f,
                -anguloPreparacion,
                t
            );
        }
        else if (progreso < 0.65f)
        {
            // Corte rápido
            float t = (progreso - 0.15f) / 0.50f;
            t = t * t * (3f - 2f * t);

            angulo = Mathf.Lerp(
                -anguloPreparacion,
                anguloAtaque,
                t
            );
        }
        else
        {
            // Recuperación
            float t = (progreso - 0.65f) / 0.35f;
            t = t * t * (3f - 2f * t);

            angulo = Mathf.Lerp(
                anguloAtaque,
                0f,
                t
            );
        }

        puntoAgarre.localRotation =
            rotacionInicial *
            Quaternion.Euler(0f, 0f, -angulo * direccion);

        if (progreso >= 1f)
        {
            atacando = false;
            puntoAgarre.localRotation = rotacionInicial;

            if (arma != null)
            {
                arma.FinalizarAtaque();
            }
        }
    }

    public void Atacar()
    {
        if (atacando || puntoAgarre == null)
            return;

        atacando = true;
        tiempoAtaque = 0f;
    }
}
