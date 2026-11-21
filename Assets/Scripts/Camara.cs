using UnityEngine;

public class CamaraVisualTilt : MonoBehaviour
{
    public float anguloMaximoX = 5f; // Mantén este valor bajo para que el efecto sea sutil
    public float anguloMaximoY = 5f; // Mantén este valor bajo para que el efecto sea sutil
    public float velocidadSuavizado = 8f;

    void Update()
    {
        // Leer la inclinación del celular
        Vector3 aceleracion = Input.acceleration;

        // La aceleración en Y (adelante/atrás) rota el eje X.
        // La aceleración en X (izquierda/derecha) rota el eje Z (negativo para coincidir con la pantalla).
        float targetRotX = -aceleracion.y * anguloMaximoY;
        float targetRotZ = aceleracion.x * anguloMaximoX;

        // Crear la rotación objetivo manteniendo el eje Y en 0
        Quaternion rotacionObjetivo = Quaternion.Euler(targetRotX, 0, targetRotZ);

        // Interpolar suavemente entre la rotación actual y la objetivo
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadSuavizado);
    }
}
