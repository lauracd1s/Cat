using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class BotonJugar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
                                         IPointerDownHandler, IPointerUpHandler
{
    [Header("Referencias")]
    public Image fondoBoton;
    public TextMeshProUGUI textoBoton;
    public RectTransform shadowRect; // un duplicate del botón detrás (sombra)

    [Header("Colores")]
    public Color colorNormal    = new Color(1f, 0.48f, 0.20f);   // naranja
    public Color colorHover     = new Color(1f, 0.60f, 0.30f);   // naranja claro
    public Color colorPresionado = new Color(0.85f, 0.35f, 0.10f); // naranja oscuro
    public Color colorSombra    = new Color(0.55f, 0.22f, 0.05f);

    private Vector2 posOriginal;
    private bool presionado = false;

    void Start()
    {
        if (fondoBoton) fondoBoton.color = colorNormal;
        if (shadowRect) shadowRect.GetComponent<Image>().color = colorSombra;
        posOriginal = GetComponent<RectTransform>().anchoredPosition;
    }

    // Hover
    public void OnPointerEnter(PointerEventData e)
    {
        if (fondoBoton) fondoBoton.color = colorHover;
        transform.localScale = Vector3.one * 1.05f;
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (fondoBoton) fondoBoton.color = colorNormal;
        transform.localScale = Vector3.one;
        if (!presionado) ResetPos();
    }

    // Click — efecto de presión bajando el botón
    public void OnPointerDown(PointerEventData e)
    {
        presionado = true;
        fondoBoton.color = colorPresionado;
        transform.localScale = Vector3.one * 0.97f;
        // Baja el botón 4px para simular presión
        GetComponent<RectTransform>().anchoredPosition = posOriginal + new Vector2(0, -4);
        if (shadowRect)
            shadowRect.GetComponent<Image>().color = new Color(0.55f, 0.22f, 0.05f, 0.3f);
    }

    public void OnPointerUp(PointerEventData e)
    {
        presionado = false;
        fondoBoton.color = colorNormal;
        transform.localScale = Vector3.one;
        ResetPos();
    }

    void ResetPos()
    {
        GetComponent<RectTransform>().anchoredPosition = posOriginal;
        if (shadowRect)
            shadowRect.GetComponent<Image>().color = colorSombra;
    }
}