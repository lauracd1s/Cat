using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Vidas")]
    public int lives = 3;
    public GameObject[] lifeIcons;
    public GameObject deathText;

    [Header("Monedas")]
    public int coins = 0;

    [Header("PowerUps")]
    public int damageLevel = 1;
    public int speedLevel = 1;
    public int maxLivesLevel = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        FindUI();
    }

    // 💥 PERDER VIDA
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
                FindUI();

            if (deathText != null)
                deathText.SetActive(true);

            Invoke(nameof(RestartLevel), 2f);
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene("Nivel1");
        lives = 3;
        coins = 0;
    }

    // 💰 MONEDAS
    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log("Monedas: " + coins);
    }

    // 🛒 TIENDA
    public void BuyDamage()
    {
        if (coins >= 5)
        {
            coins -= 5;
            damageLevel++;
        }
    }

    public void BuySpeed()
    {
        if (coins >= 5)
        {
            coins -= 5;
            speedLevel++;
        }
    }

    public void BuyLife()
    {
        if (coins >= 5)
        {
            coins -= 5;
            maxLivesLevel++;
            lives++;
        }
    }

    // 🔄 UI ENTRE ESCENAS
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
        Invoke(nameof(FindUI), 0.1f);
    }

    void FindUI()
    {
        lifeIcons = GameObject.FindGameObjectsWithTag("LifeIcon");

        GameObject dt = GameObject.FindGameObjectWithTag("DeathText");

        if (dt != null)
        {
            deathText = dt;
            deathText.SetActive(false);
        }
    }
}
