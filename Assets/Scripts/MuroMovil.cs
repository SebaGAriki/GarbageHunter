using UnityEngine;

public class MuroMovil : MonoBehaviour
{
    public enum EjeMovimiento { EjeX, EjeZ }

    [Header("Dirección")]
    [Tooltip("Elige si el muro se mueve en X (izquierda/derecha) o en Z (adelante/atrás)")]
    public EjeMovimiento eje = EjeMovimiento.EjeX;

    [Header("Límites desde la posición inicial")]
    [Tooltip("Cuánto se mueve hacia -X o -Z (ej: 2 significa 2 unidades hacia el lado negativo)")]
    public float distanciaNegativa = 2f;

    [Tooltip("Cuánto se mueve hacia +X o +Z (ej: 3 significa 3 unidades hacia el lado positivo)")]
    public float distanciaPositiva = 2f;

    [Header("Ritmo")]
    [Tooltip("Qué tan rápido recorre el camino de punta a punta")]
    public float velocidad = 2f;

    [Tooltip("Desfase inicial (0 a 1) para que varios muros no empiecen sincronizados")]
    [Range(0f, 1f)]
    public float desfaseInicial = 0f;

    private Vector3 posicionInicialLocal;
    private Rigidbody rb;

    void Start()
    {
        posicionInicialLocal = transform.localPosition;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Genera un valor suave que va de 0 a 1 y vuelve a 0 constantemente
        float t = (Mathf.Sin((Time.time * velocidad) + (desfaseInicial * Mathf.PI * 2f)) + 1f) * 0.5f;

        // Interpola entre -distanciaNegativa y +distanciaPositiva
        float desplazamiento = Mathf.Lerp(-Mathf.Abs(distanciaNegativa), Mathf.Abs(distanciaPositiva), t);

        Vector3 offset = (eje == EjeMovimiento.EjeX)
            ? new Vector3(desplazamiento, 0f, 0f)
            : new Vector3(0f, 0f, desplazamiento);

        Vector3 nuevaPosLocal = posicionInicialLocal + offset;

        if (rb != null && rb.isKinematic)
        {
            Vector3 nuevaPosGlobal = transform.parent != null
                ? transform.parent.TransformPoint(nuevaPosLocal)
                : nuevaPosLocal;

            rb.MovePosition(nuevaPosGlobal);
        }
        else
        {
            transform.localPosition = nuevaPosLocal;
        }
    }

    // Dibuja una línea verde en la ventana Scene para ver el recorrido exacto sin dar Play
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 centro = Application.isPlaying ? posicionInicialLocal : transform.localPosition;

        Vector3 dir = (eje == EjeMovimiento.EjeX) ? Vector3.right : Vector3.forward;
        Vector3 puntoNeg = centro - dir * Mathf.Abs(distanciaNegativa);
        Vector3 puntoPos = centro + dir * Mathf.Abs(distanciaPositiva);

        if (transform.parent != null)
        {
            puntoNeg = transform.parent.TransformPoint(puntoNeg);
            puntoPos = transform.parent.TransformPoint(puntoPos);
        }

        Gizmos.DrawLine(puntoNeg, puntoPos);
        Gizmos.DrawSphere(puntoNeg, 0.15f);
        Gizmos.DrawSphere(puntoPos, 0.15f);
    }
}