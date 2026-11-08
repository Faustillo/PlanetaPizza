
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : Entidad
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private State_Machine stateMachine;

    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento = 5f;

    [Header("Sprint")]
    [SerializeField] private float multVelocidadSprint = 1.8f;
    [SerializeField] private float costoSprint = 10f;

    [Header("Energia")]
    [SerializeField, Range(0f, 100f)]
    private float porcentajeEnergiaInicial = 25f;

    [Header("Consumo pasivo")]
    [SerializeField] private float consumoPasivo = 1f;

    [Header("Daño e invulnerabilidad")]
    [SerializeField] private float duracionInvulnerabilidad = 1f;

    private bool estaInvulnerable;
    private float temporizadorInvulnerabilidad;

    [Header("Recarga")]
    private bool estaRecargando;

    [Header("Lentitud")]
    private readonly List<float> lentitudes = new List<float>();

    private Vector2 direccionMovimiento;
    private bool sprintSolicitado;
    private bool muerteSolicitada;

    private float temporizadorLogPasivo;
    private float temporizadorLogSprint;

    public Vector2 DireccionMovimiento => direccionMovimiento;
    public float VelocidadMovimiento => velocidadMovimiento;
    public float MultVelocidadSprint => multVelocidadSprint;
    public float CostoSprint => costoSprint;

    public bool SprintSolicitado => sprintSolicitado;
    public bool EstaRecargando => estaRecargando;
    public bool EstaInvulnerable => estaInvulnerable;

    public State_Machine StateMachine => stateMachine;

    public bool PuedeHacerSprint =>
        EstaViva &&
        !muerteSolicitada &&
        vida > 0f &&
        direccionMovimiento != Vector2.zero;

    protected override void Awake()
    {
        base.Awake();

        CambiarVida(vidaMax * porcentajeEnergiaInicial / 100f);

        Debug.Log($"[ENERGIA] Jugador inicializado: {vida:F2}/{vidaMax}");

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (stateMachine == null)
            stateMachine = GetComponent<State_Machine>();
    }

    public override void Actualizar(float dt)
    {
        if (muerteSolicitada)
            return;

        ActualizarInvulnerabilidad(dt);

        LeerMovimiento();
        LeerSprint();

        if (!estaRecargando)
        {
            float consumo = consumoPasivo * dt;

            ConsumirEnergia(consumo);

            if (muerteSolicitada)
                return;

            temporizadorLogPasivo += dt;

            if (temporizadorLogPasivo >= 1f)
            {
                Debug.Log(
                    $"[ENERGIA] Consumo pasivo: -{consumoPasivo:F2}/s | " +
                    $"Energía actual: {vida:F2}/{vidaMax}"
                );

                temporizadorLogPasivo = 0f;
            }
        }
    }

    private void ActualizarInvulnerabilidad(float dt)
    {
        if (!estaInvulnerable)
            return;

        temporizadorInvulnerabilidad -= dt;

        if (temporizadorInvulnerabilidad <= 0f)
        {
            estaInvulnerable = false;
            temporizadorInvulnerabilidad = 0f;

            Debug.Log(
                "[DAÑO] Terminó la invulnerabilidad. " +
                "El jugador puede recibir daño nuevamente."
            );
        }
    }

    private void LeerMovimiento()
    {
        if (Keyboard.current == null)
        {
            direccionMovimiento = Vector2.zero;
            return;
        }

        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed) horizontal--;
        if (Keyboard.current.dKey.isPressed) horizontal++;

        if (Keyboard.current.sKey.isPressed) vertical--;
        if (Keyboard.current.wKey.isPressed) vertical++;

        direccionMovimiento =
            new Vector2(horizontal, vertical).normalized;
    }

    private void LeerSprint()
    {
        if (Keyboard.current == null)
        {
            sprintSolicitado = false;
            return;
        }

        sprintSolicitado =
            Keyboard.current.leftShiftKey.isPressed ||
            Keyboard.current.rightShiftKey.isPressed;
    }

    public void Mover(float multiplicadorVelocidad)
    {
        if (rb == null || !EstaViva || muerteSolicitada)
            return;

        float multiplicadorLentitud = 1f;

        foreach (float factor in lentitudes)
            multiplicadorLentitud *= factor;

        Vector2 desplazamiento =
            direccionMovimiento *
            velocidadMovimiento *
            multiplicadorVelocidad *
            multiplicadorLentitud *
            Time.deltaTime;

        rb.MovePosition(rb.position + desplazamiento);
    }

    public void ConsumirEnergia(float cantidad)
    {
        if (!EstaViva || muerteSolicitada || cantidad <= 0f)
            return;

        CambiarVida(vida - cantidad);

        if (vida <= 0f)
        {
            muerteSolicitada = true;

            Debug.Log(
                "[ENERGIA] ¡Energía agotada! Entrando en estado Morir."
            );

            stateMachine.SetState<MorirState>();
        }
    }

    public void ConsumirEnergiaSprint(float dt)
    {
        if (!EstaViva || muerteSolicitada || estaRecargando)
            return;

        float consumo = costoSprint * dt;

        ConsumirEnergia(consumo);

        if (muerteSolicitada)
            return;

        temporizadorLogSprint += dt;

        if (temporizadorLogSprint >= 1f)
        {
            Debug.Log(
                $"[ENERGIA] Consumo adicional del sprint: " +
                $"-{costoSprint:F2}/s | " +
                $"Energía actual: {vida:F2}/{vidaMax}"
            );

            temporizadorLogSprint = 0f;
        }
    }

    public void RecargarEnergia(float cantidad)
    {
        if (!EstaViva || muerteSolicitada || cantidad <= 0f)
            return;

        float energiaAnterior = vida;

        CambiarVida(vida + cantidad);

        Debug.Log(
            $"[ENERGIA] Recarga: +{vida - energiaAnterior:F2} | " +
            $"Energía actual: {vida:F2}/{vidaMax}"
        );
    }

    public void SetRecargando(bool recargando)
    {
        if (muerteSolicitada)
            return;

        estaRecargando = recargando;

        Debug.Log(
            recargando
                ? "[ENERGIA] Comenzó la recarga."
                : "[ENERGIA] Terminó la recarga."
        );
    }

    public override void RecibirDanio(float cantidad)
    {
        if (!EstaViva ||
            muerteSolicitada ||
            estaInvulnerable ||
            cantidad <= 0f)
            return;

        float energiaAnterior = vida;

        // Aplicamos el daño utilizando nuestro sistema de energía.
        ConsumirEnergia(cantidad);

        float danioAplicado = energiaAnterior - vida;

        Debug.Log(
            $"[DAÑO] Jugador recibió {danioAplicado:F2} de daño | " +
            $"Energía actual: {vida:F2}/{vidaMax}"
        );

        // Si el daño provocó la muerte, no activamos invulnerabilidad.
        if (muerteSolicitada)
            return;

        // Activamos la protección temporal.
        estaInvulnerable = true;
        temporizadorInvulnerabilidad = duracionInvulnerabilidad;

        Debug.Log(
            $"[DAÑO] Invulnerabilidad activada durante " +
            $"{duracionInvulnerabilidad:F2} segundos."
        );
    }

    public void AplicarLentitud(float factor)
    {
        if (factor <= 0f || factor > 1f)
            return;

        if (!lentitudes.Contains(factor))
            lentitudes.Add(factor);
    }

    public void QuitarLentitud(float factor)
    {
        lentitudes.Remove(factor);
    }
}
