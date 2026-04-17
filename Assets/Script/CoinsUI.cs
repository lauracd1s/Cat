using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Update()
    {
        if (GameManager.instance != null && text != null)
        {
            text.text = GameManager.instance.coins.ToString();
        }
    }
}