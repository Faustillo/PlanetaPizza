using UnityEngine;

/// <summary>
/// Fondo infinito que se desplaza lentamente. Va como hijo de la cámara,
/// así siempre cubre la pantalla aunque la cámara se mueva.
/// Usa dos copias del sprite una al lado de la otra y las reposiciona en bucle.
/// El sprite debe ser "tileable" (el borde izquierdo encaja con el derecho).
/// </summary>
public class FondoScroll : MonoBehaviour
{
    [Tooltip("Sprite del fondo (hijo de este objeto). Se duplica solo.")]
    [SerializeField] private SpriteRenderer spriteFondo;

    [Tooltip("Unidades por segundo. Negativo = hacia la izquierda, positivo = hacia la derecha.")]
    [SerializeField] private float velocidad = -0.5f;

    private Transform a;
    private Transform b;
    private float ancho;

    private void Start()
    {
        ancho = spriteFondo.bounds.size.x;

        a = spriteFondo.transform;
        b = Instantiate(spriteFondo.gameObject, transform).transform;

        // A cubre la pantalla; B queda justo a su lado.
        a.localPosition = new Vector3(0f, a.localPosition.y, a.localPosition.z);
        b.localPosition = new Vector3(ancho, a.localPosition.y, a.localPosition.z);
    }

    private void Update()
    {
        Mover(a);
        Mover(b);
    }

    private void Mover(Transform t)
    {
        Vector3 p = t.localPosition;
        p.x += velocidad * Time.deltaTime;

        // Cuando una copia sale por completo, salta al otro extremo.
        if (p.x <= -ancho) p.x += 2f * ancho;
        else if (p.x >= ancho) p.x -= 2f * ancho;

        t.localPosition = p;
    }
}