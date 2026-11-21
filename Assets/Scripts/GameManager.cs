using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    void Awake()
    {
        Instancia = this;
    }

    public void ChequearVictoria(string tagBolsa)
    {
        if (GameObject.FindGameObjectsWithTag(tagBolsa).Length <= 1)
        {
            panelVictoria.SetActive(true);
            MusicManager.instance.PonerMusicaVictoria();
        }
    }

    public void MostrarDerrota()
    {
        panelDerrota.SetActive(true);
        MusicManager.instance.PonerMusicaDerrota();

    }

    public void ReiniciarNivel()
    {
        // Recarga la escena activa actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IrAlMenu()
    {
        // Asegúrate de que el nombre coincida exactamente con tu escena de menú
        SceneManager.LoadScene("Menu");
    }
}