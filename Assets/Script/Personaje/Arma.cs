
using UnityEngine;

public class Arma : MonoBehaviour
{
    [SerializeField] private int daño;
    [SerializeField] private float velocidadAtaque = 0.5f;
    [SerializeField] private float alcance;

    [SerializeField] private Transform visualEspada;
    [SerializeField] private Transform puntoAgarre;
    [SerializeField] private Espada espada;
    [SerializeField] private Transform visualZonaImpacto;

    private Jugador jugador;
    private ZonaImpacto zonaImpacto;

    public int ObtenerDaño()
    {
        return daño;
    }

    private void Awake()
    {
        jugador = GetComponentInParent<Jugador>();

        if (espada == null)
        {
            espada = GetComponent<Espada>();
        }

        if (puntoAgarre != null)
        {
            zonaImpacto =
                puntoAgarre.GetComponentInChildren<ZonaImpacto>();
        }

        if (zonaImpacto != null)
        {
            zonaImpacto.DesactivarZona();
        }
    }

    public void Atacar()
    {
        if (espada != null)
        {
            espada.Atacar();
        }

        if (zonaImpacto != null)
        {
            zonaImpacto.ActivarZona();
        }
    }

    public void FinalizarAtaque()
    {
        if (zonaImpacto != null)
        {
            zonaImpacto.DesactivarZona();
        }
    }

private void LateUpdate()
{
    if (jugador == null)
        return;

    if (visualEspada != null)
    {
        Vector3 posicion = visualEspada.localPosition;
        posicion.x = Mathf.Abs(posicion.x) * jugador.direccion;
        visualEspada.localPosition = posicion;

        Vector3 escala = visualEspada.localScale;
        escala.x = Mathf.Abs(escala.x);
        visualEspada.localScale = escala;
    }

    if (visualZonaImpacto != null)
    {
        Vector3 posicion = visualZonaImpacto.localPosition;
        posicion.x = Mathf.Abs(posicion.x) * jugador.direccion;
        visualZonaImpacto.localPosition = posicion;
    }
}
}
