using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SistemaDialogos : MonoBehaviour, IPointerClickHandler
{
    [Header("Contenido del Nivel (Dejar vacío si el nivel no tiene intro)")]
    public SecuenciaDialogo dialogoInicial;

    [Header("Referencias UI")]
    public GameObject panelDialogo;
    public Image imagenPersonaje;
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoDialogo;

    [Header("Configuración de Animación")]
    [Tooltip("Segundos entre cada letra (menor = más rápido)")]
    public float velocidadLetra = 0.03f;

    private int indiceActual = 0;
    private bool estaEscribiendo = false;
    private Coroutine corrutinaEscritura;

    void Start()
    {
        // Si el nivel tiene un diálogo asignado y al menos 1 línea, inicia la cinemática
        if (dialogoInicial != null && dialogoInicial.lineas != null && dialogoInicial.lineas.Length > 0)
        {
            IniciarDialogo();
        }
        else if (panelDialogo != null)
        {
            panelDialogo.SetActive(false);
        }
    }

    public void IniciarDialogo()
    {
        indiceActual = 0;
        panelDialogo.SetActive(true);

        // Paraliza el movimiento de la bolsa, el cronómetro y bloquea toques a los cúmulos
        GameManager.juegoPausado = true;
        GameManager.enDialogo = true;
        Time.timeScale = 0f;

        MostrarLineaActual();
    }

    // Se ejecuta automáticamente al tocar cualquier parte del PanelDialogo en pantalla
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameManager.enDialogo) return;

        if (estaEscribiendo)
        {
            // Primer toque mientras escribe: salta la animación y muestra el texto completo
            if (corrutinaEscritura != null) StopCoroutine(corrutinaEscritura);
            textoDialogo.text = dialogoInicial.lineas[indiceActual].ObtenerTexto();
            estaEscribiendo = false;
        }
        else
        {
            // Segundo toque (texto ya completo): avanza al siguiente cuadro
            indiceActual++;

            if (indiceActual < dialogoInicial.lineas.Length)
            {
                MostrarLineaActual();
            }
            else
            {
                FinalizarDialogo();
            }
        }
    }

    private void MostrarLineaActual()
    {
        LineaDialogo linea = dialogoInicial.lineas[indiceActual];

        if (imagenPersonaje != null && linea.retratoPersonaje != null)
        {
            imagenPersonaje.sprite = linea.retratoPersonaje;
        }

        if (textoNombre != null)
        {
            textoNombre.text = linea.nombrePersonaje;
        }

        if (corrutinaEscritura != null) StopCoroutine(corrutinaEscritura);
        corrutinaEscritura = StartCoroutine(EscribirTexto(linea.ObtenerTexto()));
    }

    private IEnumerator EscribirTexto(string fraseCompleta)
    {
        estaEscribiendo = true;
        textoDialogo.text = "";

        foreach (char letra in fraseCompleta)
        {
            textoDialogo.text += letra;
            yield return new WaitForSecondsRealtime(velocidadLetra);
        }

        estaEscribiendo = false;
    }

    private void FinalizarDialogo()
    {
        panelDialogo.SetActive(false);

        // Devuelve el control del juego y reanuda el tiempo
        GameManager.enDialogo = false;
        GameManager.juegoPausado = false;
        Time.timeScale = 1f;
    }
}
