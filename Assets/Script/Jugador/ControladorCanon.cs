
using UnityEngine;
using UnityEngine.InputSystem;

public class ControladorCanon : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera camara;

    private void Awake()
    {
        if (camara == null)
            camara = Camera.main;
    }

    private void Update()
    {
        ApuntarAlMouse();
    }

    private void ApuntarAlMouse()
    {
        if (Mouse.current == null || camara == null)
            return;

        Vector2 posicionMouse = Mouse.current.position.ReadValue();

        Ray rayo = camara.ScreenPointToRay(posicionMouse);

        Plane planoJugador = new Plane(
            Vector3.forward,
            transform.position
        );

        if (!planoJugador.Raycast(rayo, out float distancia))
            return;

        Vector3 posicionObjetivo = rayo.GetPoint(distancia);

        Vector2 direccion =
            posicionObjetivo - transform.position;

        if (direccion.sqrMagnitude < 0.001f)
            return;

        float angulo = Mathf.Atan2(
            direccion.y,
            direccion.x
        ) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angulo);
    }
}
