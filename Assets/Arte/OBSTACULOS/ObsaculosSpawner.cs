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

    private List<GameObject> objectPool = new List<GameObject>();
    private float spawnXRight = 10f;
    private float leftLimitX = -10f;
    private float minY = -3f;
    private float maxY = 3f;

    private void Start()
    {
        CalculateCameraBounds();
        InitializePool();
        StartCoroutine(SpawnRoutine());
    }

    private void CalculateCameraBounds()
    {
        if (Camera.main != null)
        {
            Vector3 rightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
            Vector3 leftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            Vector3 bottomEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            Vector3 topEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));

            spawnXRight = rightEdge.x + 3f;   // Aparecen fuera a la derecha
            leftLimitX = leftEdge.x - 3f;     // Se limpian fuera a la izquierda
            minY = bottomEdge.y + 1f;
            maxY = topEdge.y - 1f;
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

            // Añadimos el componente visual
            obstacle.AddComponent<SpriteRenderer>();

            // Desactivamos el objeto inicialmente
            obstacle.SetActive(false);
            objectPool.Add(obstacle);
        }
    }

    private IEnumerator SpawnRoutine()
    {
        // Pequeña pausa de seguridad para que la cámara calcule bien sus límites al arrancar
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
        if (obstacleSprites == null || obstacleSprites.Count == 0) return;

        foreach (GameObject obstacle in objectPool)
        {
            if (obstacle != null && !obstacle.activeSelf)
            {
                // 1. Asignar sprite aleatorio
                Sprite randomSprite = obstacleSprites[Random.Range(0, obstacleSprites.Count)];
                SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();
                if (sr != null) sr.sprite = randomSprite;

                // 2. Posición inicial (fuera de la pantalla a la derecha)
                float randomY = Random.Range(minY, maxY);
                obstacle.transform.position = new Vector3(spawnXRight, randomY, 0f);

                // 3. Escala aleatoria
                float randomScale = Random.Range(minScale, maxScale);
                obstacle.transform.localScale = new Vector3(randomScale, randomScale, 1f);

                // 4. Activar objeto y arrancar su movimiento independiente
                obstacle.SetActive(true);
                StartCoroutine(MoveObstacle(obstacle));
                return;
            }
        }
    }

    private IEnumerator MoveObstacle(GameObject obstacle)
    {
        // Mueve el obstáculo hacia la izquierda en línea recta hasta que salga de la cámara
        while (obstacle != null && obstacle.activeSelf)
        {
            obstacle.transform.Translate(Vector3.left * obstacleSpeed * Time.deltaTime, Space.World);

            // Si cruza el límite izquierdo, se desactiva (vuelve al pool)
            if (obstacle.transform.position.x < leftLimitX)
            {
                obstacle.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }
}