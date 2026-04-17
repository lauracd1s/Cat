using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Update()
    {
        text.text = "🍎 " + GameManager.instance.coins;
    }
}
