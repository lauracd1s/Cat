using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    public GameObject winText;
    public float delay = 2f;

    [Header("Condiciones")]
    public bool requireBossDead = false;
    public BossController boss;

    [Header("Monedas requeridas")]
    public int requiredCoins = 0;

    private bool canWin = false;

    void Update()
    {
        if (requireBossDead && boss == null)
        {
            canWin = true;
        }
        else if (!requireBossDead)
        {
            canWin = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canWin) return;

        if (collision.CompareTag("Player"))
        {
            // 💰 Validar monedas
            if (GameManager.instance.coins < requiredCoins)
            {
                Debug.Log("No tienes suficientes frutas");
                return;
            }

            winText.SetActive(true);

            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
                player.enabled = false;

            Invoke(nameof(GoToShop), delay);
        }
    }

    void GoToShop()
    {
        SceneManager.LoadScene("Shop");
    }
}
