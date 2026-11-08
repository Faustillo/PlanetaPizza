using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TEMPORAL: solo para que el proyecto compile mientras el programador A
/// no sube el Jugador real. BORRAR este archivo cuando llegue el definitivo.
/// Tiene los métodos que usan Sol, ZonaRalentizacion y Enemigo.
/// </summary>
public class Jugador : Entidad
{
    private readonly List<float> lentitudes = new List<float>();

    public void RecargarEnergia(float cantidad)
    {
        CambiarVida(vida + cantidad);
    }

    public void SetRecargando(bool recargando) { }

    public void AplicarLentitud(float factor)
    {
        lentitudes.Add(factor);
    }

    public void QuitarLentitud(float factor)
    {
        lentitudes.Remove(factor);
    }
}
