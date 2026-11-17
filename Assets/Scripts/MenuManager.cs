using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void IniciarJuego()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void SalirJuego()
    {
        Debug.Log("Cerrando la aplicación...");
        Application.Quit();
    }
}
