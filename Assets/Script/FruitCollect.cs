using UnityEngine;

public class FruitCollect : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.AddCoins(value);
            Destroy(gameObject);
        }
    }
}
