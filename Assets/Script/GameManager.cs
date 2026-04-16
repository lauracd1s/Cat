using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int coins = 0;

    public int lives = 3;
    public GameObject[] lifeIcons;

    public GameObject deathText; // 👈 NUEVO
    void Start()
{
    lifeIcons = GameObject.FindGameObjectsWithTag("LifeIcon");
}

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 🔥 CLAVE
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoseLife()
{
    if (lives > 0)
    {
        lives--;

        if (lives < lifeIcons.Length)
        {
            lifeIcons[lives].SetActive(false);
        }
    }

    if (lives <= 0)
{
    if (deathText == null)
    {
        FindUI(); // 👈 intenta buscar otra vez
    }

    if (deathText != null)
    {
        deathText.SetActive(true);
    }
    else
    {
        Debug.LogError("❌ No hay DeathText en esta escena");
    }

    Invoke("RestartLevel", 2f);
}
}

    void RestartLevel()
    {
        SceneManager.LoadScene("Nivel1");
    }
    void OnEnable()
{
    SceneManager.sceneLoaded += OnSceneLoaded;
}

void OnDisable()
{
    SceneManager.sceneLoaded -= OnSceneLoaded;
}

void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    Invoke("FindUI", 0.1f);

}
void FindUI()
{
    lifeIcons = GameObject.FindGameObjectsWithTag("LifeIcon");

    GameObject dt = GameObject.FindGameObjectWithTag("DeathText");

    if (dt != null)
    {
        deathText = dt;
        deathText.SetActive(false);
        Debug.Log("✅ DeathText encontrado");
    }
    else
    {
       // Debug.LogWarning("⚠️ DeathText NO encontrado en esta escena");
    }
}

public void AddCoins(int amount)
{
    coins += amount;
    Debug.Log("Monedas: " + coins);
}

}