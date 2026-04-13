using UnityEngine;

public class FruitFall : MonoBehaviour
{
    public float speed = 2f;
    private float rotationSpeed;
    private float bottomY;

    void Start()
    {
        speed = Random.Range(1.5f, 3.5f);
        rotationSpeed = Random.Range(-60f, 60f); // más suave
        bottomY = -Camera.main.orthographicSize - 1f;
        // Transparencia al sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color c = sr.color;
        c.a = 0.35f; // 0 = invisible, 1 = sólido — prueba entre 0.2 y 0.5
        sr.color = c;
    }

    void Update()
    {
        // Caída siempre hacia abajo (Space.World es el fix clave)
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        
        // Rotación solo en el eje Z (visual, no afecta movimiento)
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        if (transform.position.y < bottomY)
            Destroy(gameObject);
    }
}