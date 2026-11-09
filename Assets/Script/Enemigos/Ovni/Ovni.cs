using UnityEngine;

/// <summary>
/// Ovni: cruza la pantalla en diagonal (State_CruzarDiagonal) con una zona
/// triangular que ralentiza (ZonaRalentizacion, objeto hijo).
/// Si choca con el jugador le quita energía, y el ovni sigue su camino.
/// </summary>
public class Ovni : Enemigo
{
    [Tooltip("Si está activo, los disparos no le hacen daño.")]
    [SerializeField] private bool invulnerable = false;

    // Cada vez que el spawner lo reactiva desde el pool, vuelve como nuevo.
    private void OnEnable()
    {
        Reiniciar();
    }

    public override void RecibirDanio(float cantidad)
    {
        if (invulnerable) return;
        base.RecibirDanio(cantidad);
    }

    // El daño por contacto lo hace Enemigo.OnTriggerEnter2D -> OnColisionJugador.
    // A diferencia del Motochoro, el Ovni NO muere al chocar: no se sobrescribe nada.
}
