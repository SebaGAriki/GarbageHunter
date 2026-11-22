using UnityEngine;
using UnityEngine.UI;

public class ControlVolumen : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelVolumen;
    public Slider sliderVolumen;

    [Header("Opcional: Pausar nivel al abrir")]
    [Tooltip("Márcalo en la escena del Nivel si quieres que la pelota se pause mientras ajustas el volumen")]
    public bool pausarJuegoAlAbrir = false;

    private const string CLAVE_VOLUMEN = "VolumenGeneral";

    void Start()
    {
        // Carga el volumen guardado anteriormente (si no existe, empieza al 100% = 1f)
        float volumenGuardado = PlayerPrefs.GetFloat(CLAVE_VOLUMEN, 1f);
        AudioListener.volume = volumenGuardado;

        if (sliderVolumen != null)
        {
            sliderVolumen.minValue = 0f;
            sliderVolumen.maxValue = 1f;
            sliderVolumen.value = volumenGuardado;

            // Conecta automáticamente el movimiento del Slider con el cambio de volumen
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }

        if (panelVolumen != null)
        {
            panelVolumen.SetActive(false);
        }
    }

    public void AbrirMenuVolumen()
    {
        if (panelVolumen != null)
            panelVolumen.SetActive(true);

        if (pausarJuegoAlAbrir)
            Time.timeScale = 0f;
    }

    public void CerrarMenuVolumen()
    {
        if (panelVolumen != null)
            panelVolumen.SetActive(false);

        if (pausarJuegoAlAbrir)
            Time.timeScale = 1f;
    }

    public void CambiarVolumen(float nuevoValor)
    {
        AudioListener.volume = nuevoValor;
        PlayerPrefs.SetFloat(CLAVE_VOLUMEN, nuevoValor);
        PlayerPrefs.Save();
    }
}