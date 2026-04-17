using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida")]
    public int health = 2;

    [Header("Drop de Fruta")]
    public GameObject fruitPrefab;
    public int minDrop = 1;
    public int maxDrop = 3;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        DropFruits();
        Destroy(gameObject);
    }

    void DropFruits()
    {
        if (fruitPrefab == null) return;

        int amount = Random.Range(minDrop, maxDrop + 1);

        for (int i = 0; i < amount; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(0.3f, 0.8f),
                0
            );

            GameObject fruit = Instantiate(
                fruitPrefab,
                transform.position + offset,
                Quaternion.identity
            );

            // 🔥 EFECTO PRO (opcional)
            Rigidbody2D rb = fruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(new Vector2(
                    Random.Range(-2f, 2f),
                    Random.Range(2f, 4f)
                ), ForceMode2D.Impulse);
            }
        }
    }
}
