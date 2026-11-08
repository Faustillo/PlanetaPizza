using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Reproduce un sonido al pasar el mouse por el botón y otro al hacer clic.
/// Se puede usar junto con BotonHover en el mismo botón.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class BotonSonido : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioClip sonidoHover;
    [SerializeField] private AudioClip sonidoClick;
    [SerializeField, Range(0f, 1f)] private float volumen = 1f;

    private AudioSource fuente;

    private void Awake()
    {
        fuente = GetComponent<AudioSource>();
        fuente.playOnAwake = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Reproducir(sonidoHover);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Reproducir(sonidoClick);
    }

    private void Reproducir(AudioClip clip)
    {
        if (clip != null)
            fuente.PlayOneShot(clip, volumen);
    }
}
