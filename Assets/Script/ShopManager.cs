using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public void BuyDamage()
    {
        GameManager.instance.BuyDamage();
    }

    public void BuySpeed()
    {
        GameManager.instance.BuySpeed();
    }

    public void BuyLife()
    {
        GameManager.instance.BuyLife();
    }

    public void NextLevel()
    {
        int next = GameManager.instance.currentLevel + 1;

        if (next > GameManager.instance.totalLevels)
        {
            // Ya pasó el último nivel — ir a pantalla de victoria
            SceneManager.LoadScene("MenuPrincipal"); // o "WinScreen"
        }
        else
        {
            SceneManager.LoadScene("Nivel" + next);
        }
    }
}
