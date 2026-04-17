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
        SceneManager.LoadScene("Nivel2");
    }
}
