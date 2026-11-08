
using UnityEngine;
using System.Collections.Generic;

public class ZonaImpacto : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Arma arma;

    private HashSet<GameObject> enemigosGolpeados =
        new HashSet<GameObject>();

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        arma = GetComponentInParent<Arma>();

        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
    }

    public void ActivarZona()
    {
        enemigosGolpeados.Clear();

        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }
    }

    public void DesactivarZona()
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("enemy"))
        {
            return;
        }

        GameObject objetivo = other.attachedRigidbody != null
            ? other.attachedRigidbody.gameObject
            : other.gameObject;

        if (enemigosGolpeados.Contains(objetivo))
        {
            return;
        }

        enemigosGolpeados.Add(objetivo);

        IDamageable danable = objetivo.GetComponentInParent<IDamageable>();

        if (danable == null)
        {
            Debug.LogWarning(
                "El enemigo " + objetivo.name +
                " no tiene un componente IDamageable."
            );
            return;
        }

        if (arma == null)
        {
            Debug.LogWarning(
                "No se encontró el componente Arma."
            );
            return;
        }

        int dañoAplicado = arma.ObtenerDaño();
        danable.RecibirDaño(dañoAplicado);
    }
}
