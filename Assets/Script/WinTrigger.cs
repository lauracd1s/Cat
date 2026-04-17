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
            canWin = true;
        else if (!requireBossDead)
            canWin = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canWin) return;
        if (!collision.CompareTag("Player")) return;

        if (GameManager.instance.coins < requiredCoins)
        {
            Debug.Log("No tienes suficientes frutas");
            return;
        }

        if (winText != null)
            winText.SetActive(true);

        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
            player.enabled = false;

        Invoke(nameof(GoToNext), delay);
    }

    void GoToNext()
    {
        string current = SceneManager.GetActiveScene().name;
        Debug.Log("Saliendo de: " + current);

        switch (current)
        {
            case "Nivel1": SceneManager.LoadScene("Nivel2"); break;
            case "Nivel2": SceneManager.LoadScene("Nivel3"); break;
            case "Nivel3": SceneManager.LoadScene("Nivel4"); break;
            case "Nivel4": SceneManager.LoadScene("TransicionNivel"); break;
            case "Nivel5": SceneManager.LoadScene("MenuPrincipal"); break;
            default:
                Debug.LogWarning("Escena no reconocida: " + current);
                break;
        }
    }
}