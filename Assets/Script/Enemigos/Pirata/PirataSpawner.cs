using System.Collections;

using System.Collections.Generic;

using UnityEngine;



/// <summary>

/// Lanza piratas de forma continua, con espera aleatoria entre cada uno.

/// Siempre entran por la derecha: State_OndaSeno elige la altura y el recorrido.

/// Usa pool: los piratas se apagan al salir o morir y se reutilizan.

/// </summary>

public class PirataSpawner : MonoBehaviour

{

    [Header("Piratas")]

    [SerializeField] private GameObject pirataPrefab;

    [SerializeField] private int tamanoPool = 4; // máximo de piratas a la vez



    [Header("Frecuencia (segundos entre apariciones)")]

    [SerializeField] private float esperaMin = 2f;

    [SerializeField] private float esperaMax = 6f;



    [SerializeField] private bool iniciarAutomaticamente = true;



    private readonly List<GameObject> pool = new List<GameObject>();

    private Coroutine rutina;



    private void Start()

    {

        for (int i = 0; i < tamanoPool; i++)

        {

            GameObject pirata = Instantiate(pirataPrefab, transform);

            pirata.SetActive(false);

            pool.Add(pirata);

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

        foreach (GameObject pirata in pool)

        {

            if (!pirata.activeSelf)

            {

                pirata.SetActive(true); // el estado elige altura y onda solo

                return;

            }

        }

    }

}