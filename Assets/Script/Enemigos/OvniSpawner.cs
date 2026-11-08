using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lanza ovnis de forma continua, cada cierto tiempo aleatorio.
/// Reutiliza los mismos ovnis (pool): State_CruzarDiagonal se desactiva al
/// llegar y este spawner lo vuelve a activar cuando toca.
/// </summary>
public class OvniSpawner : MonoBehaviour
{
    [Header("Ovnis")]
    [SerializeField] private GameObject ovniPrefab;
    [SerializeField] private int tamanoPool = 3; // máximo de ovnis a la vez

    [Header("Frecuencia (segundos entre apariciones)")]
    [SerializeField] private float esperaMin = 3f;
    [SerializeField] private float esperaMax = 8f;

    [SerializeField] private bool iniciarAutomaticamente = true;

    private readonly List<GameObject> pool = new List<GameObject>();
    private Coroutine rutina;

    private void Start()
    {
        // Creamos el pool una vez, todos apagados.
        for (int i = 0; i < tamanoPool; i++)
        {
            GameObject ovni = Instantiate(ovniPrefab, transform);
            ovni.SetActive(false);
            pool.Add(ovni);
        }

        if (iniciarAutomaticamente)
            Iniciar();
    }

    public void Iniciar()
    {
        if (rutina == null)
            rutina = StartCoroutine(BucleSpawn());
    }

    /// <summary>Llamar desde el GameManager en Victoria/Derrota.</summary>
    public void Detener()
    {
        if (rutina != null)
        {
            StopCoroutine(rutina);
            rutina = null;
        }
    }

    private IEnumerator BucleSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(esperaMin, esperaMax));
            Spawnear();
        }
    }

    private void Spawnear()
    {
        foreach (GameObject ovni in pool)
        {
            if (!ovni.activeSelf)
            {
                // Al activarse, State_Machine arranca y el estado
                // elige esquina de salida y destino solo.
                ovni.SetActive(true);
                return;
            }
        }
        // Todos en pantalla: se salta esta aparición.
    }
}