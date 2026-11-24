using UnityEngine;

[System.Serializable]
public class LineaDialogo
{
    public string nombrePersonaje = "Opip";
    public Sprite retratoPersonaje; // Permite cambiar la expresión en cada cuadro

    [TextArea(3, 5)]
    public string textoEspanol;

    [TextArea(3, 5)]
    public string textoIngles;

    // Devuelve el texto según el idioma guardado en el dispositivo (por defecto Español)
    public string ObtenerTexto()
    {
        string idioma = PlayerPrefs.GetString("Idioma", "ES");
        if (idioma == "EN" && !string.IsNullOrEmpty(textoIngles))
        {
            return textoIngles;
        }
        return textoEspanol;
    }
}

[CreateAssetMenu(fileName = "NuevoDialogo", menuName = "GarbageHunter/Secuencia de Dialogo")]
public class SecuenciaDialogo : ScriptableObject
{
    public LineaDialogo[] lineas;
}