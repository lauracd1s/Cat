using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    public GameObject winText;
    public float delay = 3f; // ⏱ tiempo antes de cambiar escena

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            winText.SetActive(true);

            // opcional: detener movimiento del jugador
            collision.GetComponent<PlayerMovement>().enabled = false;

            // opcional: pausar físicas (pero OJO con Invoke)
            Time.timeScale = 1f;

            Invoke("GoToMenu", delay);
        }
    }

    void GoToMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}