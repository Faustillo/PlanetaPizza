
using UnityEngine;
using UnityEngine.InputSystem;

public class ControladorDisparo : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private ProyectilJugador prefabProyectil;

    [Header("Configuración")]
    [SerializeField] private float cooldownDisparo = 0.35f;
    [SerializeField] private float costoDisparo = 3f;

    private Jugador jugador;
    private float temporizadorDisparo;

    private void Awake()
    {
        jugador = GetComponentInParent<Jugador>();
    }

    private void Update()
    {
        if (jugador == null || !jugador.EstaViva)
            return;

        temporizadorDisparo -= Time.deltaTime;

        LeerDisparo();
    }

    private void LeerDisparo()
    {
        if (Mouse.current == null)
            return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Disparar();
    }

    private void Disparar()
    {
        if (temporizadorDisparo > 0f)
            return;

        if (jugador.EstaRecargando)
            return;

        if (jugador.Vida < costoDisparo)
            return;

        if (puntoDisparo == null || prefabProyectil == null)
            return;

        Vector2 direccion = puntoDisparo.right;

        ProyectilJugador proyectil = Instantiate(
            prefabProyectil,
            puntoDisparo.position,
            puntoDisparo.rotation
        );

        proyectil.Inicializar(
            puntoDisparo.position,
            direccion
        );

        jugador.ConsumirEnergia(costoDisparo);

        temporizadorDisparo = cooldownDisparo;

        Debug.Log(
            $"[DISPARO] Proyectil creado | " +
            $"Costo: {costoDisparo} | " +
            $"Energía restante: {jugador.Vida:F2}/{jugador.VidaMax}"
        );
    }
}
