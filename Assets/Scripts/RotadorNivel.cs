using UnityEngine;

public class RotadorNivel : MonoBehaviour
{
    public float velocidadSuavizado = 10f; // Controla qué tan rápida es la animación
    private float anguloObjetivoY = 0f;

    void Start()
    {
        // Guarda la rotación inicial en Y por si el nivel no empieza en 0
        anguloObjetivoY = transform.eulerAngles.y;
    }

    void Update()
    {
        // Calcula la rotación objetivo manteniendo X y Z intactos
        Quaternion rotacionObjetivo = Quaternion.Euler(transform.eulerAngles.x, anguloObjetivoY, transform.eulerAngles.z);

        // Interpola suavemente hacia los 90 grados
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadSuavizado);
    }

    public void RotarIzquierda()
    {
        anguloObjetivoY -= 90f; // Cambia a -= 90f si gira hacia el lado contrario al que deseas
    }

    public void RotarDerecha()
    {
        anguloObjetivoY += 90f; // Cambia a += 90f si gira hacia el lado contrario al que deseas
    }
}