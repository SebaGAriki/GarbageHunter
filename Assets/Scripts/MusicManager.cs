using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Componentes")]
    public AudioSource audioSource;

    [Header("Pistas de Música")]
    public AudioClip musicaMenu;
    public AudioClip musicaNivel;
    public AudioClip musicaVictoria;
    public AudioClip musicaDerrota;

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
        // Reemplaza "Menu" y "Nivel1" con los nombres exactos de tus escenas
        switch (scene.name)
        {
            case "Menu":
                CambiarMusica(musicaMenu, true);
                break;
            case "Nivel1":
                CambiarMusica(musicaNivel, true);
                break;
            case "Nivel2":
                CambiarMusica(musicaNivel, true);
                break;
            case "Nivel3":
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

    // Métodos directos para tus Canvas
    public void PonerMusicaVictoria()
    {
        CambiarMusica(musicaVictoria, false);
    }

    public void PonerMusicaDerrota()
    {
        CambiarMusica(musicaDerrota, false);
    }
}