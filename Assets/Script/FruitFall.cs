using UnityEngine;

public class FruitFall : MonoBehaviour
{
    public float speed = 2f;
    private float rotationSpeed;
    private float bottomY;

    void Start()
    {
        rotationSpeed = Random.Range(-90f, 90f);
        bottomY = -Camera.main.orthographicSize - 1f;
    }

    void Update()
    {
        // Caída
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        // Rotación suave
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Destruir cuando sale de pantalla
        if (transform.position.y < bottomY)
            Destroy(gameObject);
    }
}