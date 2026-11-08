using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Imágenes de Obstáculos")]
    [SerializeField] private List<Sprite> obstacleSprites = new List<Sprite>();
    [SerializeField] private int poolSize = 6;

    [Header("Movimiento y Velocidad")]
    [SerializeField] private float minSpawnDelay = 2f;
    [SerializeField] private float maxSpawnDelay = 5f;
    [SerializeField] private float obstacleSpeed = 4f;

    [Header("Tamaños Aleatorios")]
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 1.5f;

    [Header("Seguimiento de Cámara")]
    [Tooltip("Si está activo, el spawner seguirá la posición horizontal de la cámara principal.")]
    [SerializeField] private bool followCameraX = true;

    private List<GameObject> objectPool = new List<GameObject>();
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = FindObjectOfType<Camera>();
        }

        InitializePool();
        StartCoroutine(SpawnRoutine());
    }

    private void Update()
    {
        // Hace que el spawner se mueva junto con la cámara en el eje X
        if (followCameraX && mainCamera != null)
        {
            transform.position = new Vector3(mainCamera.transform.position.x, transform.position.y, transform.position.z);
        }
    }

    private void InitializePool()
    {
        if (obstacleSprites == null || obstacleSprites.Count == 0)
        {
            Debug.LogError("¡La lista de imágenes está vacía! Añade sprites en el Inspector.", this);
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obstacle = new GameObject("Obstacle_Item");
            obstacle.transform.SetParent(transform);
            obstacle.AddComponent<SpriteRenderer>();
            obstacle.SetActive(false);
            objectPool.Add(obstacle);
        }
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(waitTime);
            SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        if (obstacleSprites == null || obstacleSprites.Count == 0 || mainCamera == null) return;

        foreach (GameObject obstacle in objectPool)
        {
            if (obstacle != null && !obstacle.activeSelf)
            {
                // 1. Asignar sprite aleatorio
                Sprite randomSprite = obstacleSprites[Random.Range(0, obstacleSprites.Count)];
                SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();
                if (sr != null) sr.sprite = randomSprite;

                // 2. Calcular los límites actuales de la cámara de forma dinámica
                float cameraHeight = mainCamera.orthographicSize * 2f;
                float cameraWidth = cameraHeight * mainCamera.aspect;

                float rightEdgeX = mainCamera.transform.position.x + (cameraWidth / 2f) + 3f; // Fuera a la derecha
                float minY = mainCamera.transform.position.y - (cameraHeight / 2f) + 1f;
                float maxY = mainCamera.transform.position.y + (cameraHeight / 2f) - 1f;

                // 3. Posición inicial
                float randomY = Random.Range(minY, maxY);
                obstacle.transform.position = new Vector3(rightEdgeX, randomY, 0f);

                // 4. Escala aleatoria
                float randomScale = Random.Range(minScale, maxScale);
                obstacle.transform.localScale = new Vector3(randomScale, randomScale, 1f);

                // 5. Activar y mover
                obstacle.SetActive(true);
                StartCoroutine(MoveAndCheckObstacle(obstacle));
                return;
            }
        }
    }

    private IEnumerator MoveAndCheckObstacle(GameObject obstacle)
    {
        while (obstacle != null && obstacle.activeSelf && mainCamera != null)
        {
            // Mover hacia la izquierda de forma constante
            obstacle.transform.Translate(Vector3.left * obstacleSpeed * Time.deltaTime, Space.World);

            // Calcular el límite izquierdo actual de la cámara dinámicamente
            float cameraHeight = mainCamera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * mainCamera.aspect;
            float leftEdgeX = mainCamera.transform.position.x - (cameraWidth / 2f) - 3f; // Fuera a la izquierda

            // Si el objeto supera el borde izquierdo de la cámara, se desactiva (vuelve al pool)
            if (obstacle.transform.position.x < leftEdgeX)
            {
                obstacle.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }
}