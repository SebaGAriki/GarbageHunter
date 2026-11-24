using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SonidoAlPresionar : MonoBehaviour
{
    void Start()
    {
        // Conecta automáticamente el clic del botón con el MusicManager activo
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (MusicManager.instance != null)
                MusicManager.instance.SonidoBoton();
        });
    }
}
