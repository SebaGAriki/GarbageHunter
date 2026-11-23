using UnityEngine;

public class TapaCesto : MonoBehaviour
{
    [Header("Configuración de Color")]
    [Tooltip("Tag exacto de la bolsa que abre esta tapa (ej. BolsaGris)")]
    public string tagCorrecto = "BolsaGris";

    [Header("Radio y Punto de Detección")]
    [Tooltip("Arrastra aquí el objeto Trigger del cesto para centrar el radio en la boca del tacho")]
    public Transform puntoDeteccion;
    [Tooltip("Ajuste fino de posición respecto al punto de detección")]
    public Vector3 desplazamientoCentro = Vector3.zero;
    [Tooltip("Tamaño del radio de apertura")]
    public float radioDeteccion = 5f;

    [Header("Ángulos de la Tapa (Eje X Local)")]
    public float anguloAbierto = 0f;
    public float anguloCerrado = 115f;
    public float velocidadTapa = 8f;

    void Update()
    {
        float anguloObjetivo = anguloCerrado;
        Vector3 centro = ObtenerCentroDeteccion();

        // Detecta todos los colliders dentro de la esfera
        Collider[] objetosEnRadio = Physics.OverlapSphere(centro, radioDeteccion);

        foreach (Collider col in objetosEnRadio)
        {
            if (col.CompareTag(tagCorrecto))
            {
                anguloObjetivo = anguloAbierto;
                break;
            }
        }

        // Rota la tapa suavemente en espacio local
        Quaternion rotacionObjetivo = Quaternion.Euler(anguloObjetivo, 0f, 0f);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotacionObjetivo, Time.deltaTime * velocidadTapa);
    }

    private Vector3 ObtenerCentroDeteccion()
    {
        // Usa el puntoDeteccion (o el padre CestoBasura) para que el área no se mueva al girar la tapa
        Transform referencia = puntoDeteccion != null ? puntoDeteccion : (transform.parent != null ? transform.parent : transform);
        return referencia.TransformPoint(desplazamientoCentro);
    }

    // Dibuja el radio en la ventana Scene al seleccionar la Tapa
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(ObtenerCentroDeteccion(), radioDeteccion);
    }
}