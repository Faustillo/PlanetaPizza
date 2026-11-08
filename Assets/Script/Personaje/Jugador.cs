using UnityEngine;

public class Jugador : MonoBehaviour, IDamageable
{
    [SerializeField] public int vida;
    [SerializeField] public int velocidad;
    [SerializeField] public int daño;

    public float fuerzaDeSalto = 7f; // Ajusta este valor en el Inspector si salta mucho o poco
    public int direccion = 1;

    public void RecibirDaño(int daño)
    {
        vida -= daño;

        Debug.Log("El jugador recibió " + daño + " de daño. Vida restante: " + vida);

        if (vida <= 0)
        {
            Morir();
        }
    }
    private void Morir()
    {
        Debug.Log("El jugador murió");
    }
}
