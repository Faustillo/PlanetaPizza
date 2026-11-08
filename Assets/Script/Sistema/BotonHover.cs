using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Agranda un poco el botón cuando el mouse pasa por encima.
/// Ponerlo en cada botón del menú.
/// </summary>
public class BotonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float escalaHover = 1.1f; // 1.1 = 10% más grande
    [SerializeField] private float suavizado = 12f;

    private Vector3 escalaOriginal;
    private Vector3 escalaObjetivo;

    private void Awake()
    {
        escalaOriginal = transform.localScale;
        escalaObjetivo = escalaOriginal;
    }

    private void Update()
    {
        // unscaledDeltaTime: funciona aunque el juego esté pausado (timeScale = 0)
        transform.localScale = Vector3.Lerp(
            transform.localScale, escalaObjetivo, suavizado * Time.unscaledDeltaTime);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        escalaObjetivo = escalaOriginal * escalaHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        escalaObjetivo = escalaOriginal;
    }

    private void OnDisable()
    {
        // Si el botón se oculta con el mouse encima, vuelve a su tamaño normal.
        transform.localScale = escalaOriginal;
        escalaObjetivo = escalaOriginal;
    }
}
