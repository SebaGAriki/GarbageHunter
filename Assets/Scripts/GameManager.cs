using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para controlar las imágenes de las estrellas
using TMPro; // Necesario para controlar TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    [Header("HUD en Pantalla")]
    public TextMeshProUGUI textoContadorBolsas;
    public TextMeshProUGUI textoTemporizador;


    [Header("Configuración de Tiempo (Estrella 3)")]
    public float tiempoLimiteSegundos = 30f; // Ajustable por nivel en el Inspector
    private float tiempoTranscurrido = 0f;
    private bool nivelTerminado = false;

    [Header("UI de Estrellas en Panel Victoria")]
    public Image imagenEstrella1; // Misión 1: Superar el nivel
    public Image imagenEstrella2; // Misión 2: Todos los cestos correctos
    public Image imagenEstrella3; // Misión 3: Tiempo límite

    [Tooltip("Opcional: Arrastra aquí tus sprites de estrella vacía y llena")]
    public Sprite spriteEstrellaVacia;
    public Sprite spriteEstrellaLlena;

    [Header("Estadísticas (Solo lectura)")]
    public int totalBolsasNivel = 0;
    public int bolsasDepositadas = 0;
    public int aciertosDeColor = 0;

    void Awake()
    {
        Instancia = this;
    }

    void Start()
    {
        int bolsasIniciales = FindObjectsOfType<GravedadBolsa>().Length;
        int cumulosIniciales = FindObjectsOfType<CumuloInteractivo>().Length;
        totalBolsasNivel = bolsasIniciales + cumulosIniciales;

        ActualizarHUD();
    }

    void Update()
    {
        // Cuenta el tiempo mientras el nivel esté en curso
        if (!nivelTerminado)
        {
            tiempoTranscurrido += Time.deltaTime;
            ActualizarHUD();
        }
    }
    private void ActualizarHUD()
    {
        // 1. Contador de bolsas correctas / total
        if (textoContadorBolsas != null)
        {
            textoContadorBolsas.text = aciertosDeColor + " / " + totalBolsasNivel;
        }

        // 2. Temporizador decreciente que se detiene en 0
        if (textoTemporizador != null)
        {
            float tiempoRestante = Mathf.Max(0f, tiempoLimiteSegundos - tiempoTranscurrido);
            int segundosEnteros = Mathf.CeilToInt(tiempoRestante);
            textoTemporizador.text = segundosEnteros + "s";
        }
    }

    public void BolsaDepositada(GravedadBolsa bolsaDepositada, bool esColorCorrecto)
    {
        bolsasDepositadas++;
        if (esColorCorrecto)
        {
            aciertosDeColor++;
        }

        GravedadBolsa[] todasLasBolsas = FindObjectsOfType<GravedadBolsa>();
        int bolsasJugablesRestantes = 0;

        foreach (GravedadBolsa b in todasLasBolsas)
        {
            if (b != bolsaDepositada && !b.atrapada && b.enabled)
            {
                bolsasJugablesRestantes++;
            }
        }

        CumuloInteractivo[] cumulosRestantes = FindObjectsOfType<CumuloInteractivo>();

        if (bolsasJugablesRestantes == 0 && cumulosRestantes.Length == 0)
        {
            MostrarVictoria();
        }
        else if (bolsasJugablesRestantes == 0 && cumulosRestantes.Length > 0)
        {
            CumuloInteractivo masCercano = null;
            float distanciaMinima = Mathf.Infinity;

            foreach (CumuloInteractivo cumulo in cumulosRestantes)
            {
                float distancia = Vector3.Distance(bolsaDepositada.transform.position, cumulo.transform.position);
                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    masCercano = cumulo;
                }
            }

            if (masCercano != null)
            {
                masCercano.TransformarEnBolsa();
            }
        }
    }

    public void VerificarFinPorPozo()
    {
        if (bolsasDepositadas > 0)
        {
            MostrarVictoria();
        }
        else
        {
            MostrarDerrota();
        }
    }

    public void MostrarVictoria()
    {
        if (nivelTerminado) return;
        nivelTerminado = true;

        // Evaluamos las 3 condiciones de forma independiente
        bool ganoEstrella1 = true; // Siempre se gana al superar el nivel
        bool ganoEstrella2 = (aciertosDeColor == totalBolsasNivel); // Ningún error ni bolsa perdida
        bool ganoEstrella3 = (tiempoTranscurrido <= tiempoLimiteSegundos);

        // Rellenamos o dejamos vacía cada estrella según su misión
        ActualizarVisualEstrella(imagenEstrella1, ganoEstrella1);
        ActualizarVisualEstrella(imagenEstrella2, ganoEstrella2);
        ActualizarVisualEstrella(imagenEstrella3, ganoEstrella3);

        panelVictoria.SetActive(true);
    }

    private void ActualizarVisualEstrella(Image imgEstrella, bool conseguida)
    {
        if (imgEstrella == null) return;

        // Si asignaste ambos sprites en el Inspector, intercambia las imágenes
        if (spriteEstrellaLlena != null && spriteEstrellaVacia != null)
        {
            imgEstrella.sprite = conseguida ? spriteEstrellaLlena : spriteEstrellaVacia;
            imgEstrella.color = Color.white;
        }
        else
        {
            // Respaldo automático para prototipo si aún no tienes los 2 sprites dibujados
            imgEstrella.color = conseguida ? Color.yellow : new Color(0.25f, 0.25f, 0.25f, 0.85f);
        }
    }

    public void MostrarDerrota()
    {
        if (nivelTerminado) return;
        nivelTerminado = true;
        panelDerrota.SetActive(true);
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IrAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void CargarSiguienteNivel()
    {
        int siguienteNivelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (siguienteNivelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(siguienteNivelIndex);
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
    }
}