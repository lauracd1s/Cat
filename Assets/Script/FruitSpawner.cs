using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [Header("Configuración")]
    public Sprite[] fruitSprites;     // Arrastra tus sprites aquí
    public float spawnInterval = 0.5f;
    public float fallSpeed = 2f;
    public float spawnWidth = 9f;     // Ancho del área de spawn

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnFruit();
        }
    }

    void SpawnFruit()
{
    Camera cam = Camera.main;
    
    // Calcular bordes reales de la cámara
    float topY = cam.orthographicSize;
    float halfWidth = cam.orthographicSize * cam.aspect;
    
    float x = Random.Range(-halfWidth, halfWidth);
    Vector3 spawnPos = new Vector3(x, topY + 0.5f, 0f); // Z en 0
    
    GameObject fruit = new GameObject("Fruit");
    fruit.transform.position = spawnPos;
    
    SpriteRenderer sr = fruit.AddComponent<SpriteRenderer>();
    sr.sprite = fruitSprites[Random.Range(0, fruitSprites.Length)];
    sr.sortingLayerName = "Default";
    sr.sortingOrder = 2;
    
    fruit.transform.localScale = Vector3.one * 0.5f;
    fruit.AddComponent<FruitFall>().speed = fallSpeed;

    fruit.AddComponent<CircleCollider2D>().isTrigger = true;
    fruit.AddComponent<FruitCollect>();

}
}