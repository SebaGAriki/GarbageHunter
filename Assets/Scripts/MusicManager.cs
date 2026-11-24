using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Componentes")]
    public AudioSource audioSource;      // El que ya tienes para la música
    public AudioSource sfxSource;        // Nuevo AudioSource solo para efectos (SFX)

    [Header("Pistas de Música")]
    public AudioClip musicaMenu;
    public AudioClip musicaNivel;
    public AudioClip musicaVictoria;
    public AudioClip musicaDerrota;

    [Header("Efectos de Sonido (UI)")]
    public AudioClip clipBoton;

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
            return;
        }
    }

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
        // Mantén aquí los nombres exactos de tus escenas tal como los tienes ahora
        switch (scene.name)
        {
            case "Menu":
                CambiarMusica(musicaMenu, true);
                break;
            case "Nivel1":
                CambiarMusica(musicaNivel, true);
                break;
        }
    }

    public void CambiarMusica(AudioClip nuevaPista, bool enBucle)
    {
        if (nuevaPista == null) return;
        if (audioSource.clip == nuevaPista && audioSource.isPlaying) return;

        audioSource.Stop();
        audioSource.clip = nuevaPista;
        audioSource.loop = enBucle;
        audioSource.Play();
    }

    public void PonerMusicaVictoria()
    {
        CambiarMusica(musicaVictoria, false);
    }

    public void PonerMusicaDerrota()
    {
        CambiarMusica(musicaDerrota, false);
    }

    // Método para llamar desde cualquier botón del juego
    public void SonidoBoton()
    {
        if (sfxSource != null && clipBoton != null)
        {
            sfxSource.PlayOneShot(clipBoton);
        }
    }
}