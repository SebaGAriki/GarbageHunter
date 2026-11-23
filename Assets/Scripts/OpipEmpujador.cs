using UnityEngine;

public class OpipEmpujador : MonoBehaviour
{
    [Header("Posicionamiento respecto a la bolsa")]
    [Tooltip("Qué tan pegado o separado de la bolsa estará Opip")]
    public float distanciaDetras = 1.2f;
    [Tooltip("Ajusta la altura para que los pies de Opip queden al ras del piso")]
    public float ajusteAltura = -0.2f;

    [Header("Suavizado de Movimiento")]
    public float velocidadPosicion = 12f;
    public float velocidadRotacion = 15f;
    [Tooltip("Velocidad mínima de la bolsa para que Opip cambie de lado")]
    public float umbralMovimiento = 0.15f;

    private Vector3 direccionEmpuje = Vector3.forward;
    private GravedadBolsa ultimaBolsaControlada;

    void LateUpdate()
    {
        GravedadBolsa bolsa = GravedadBolsa.bolsaActiva;

        // Si no hay bolsa activa o cayó en un pozo, Opip se queda quieto
        if (bolsa == null || bolsa.atrapada) return;

        Rigidbody rb = bolsa.GetComponent<Rigidbody>();
        if (rb == null) return;

        // 1. Calculamos la velocidad horizontal de la bolsa (ignorando saltos en Y)
        Vector3 velocidadHorizontal = rb.linearVelocity;
        velocidadHorizontal.y = 0f;

        // Solo actualizamos la dirección si la bolsa se está moviendo lo suficiente
        if (velocidadHorizontal.magnitude > umbralMovimiento)
        {
            direccionEmpuje = velocidadHorizontal.normalized;
        }

        // 2. Calculamos la posición exacta detrás de la bolsa (sentido opuesto al movimiento)
        Vector3 posicionObjetivo = bolsa.transform.position - (direccionEmpuje * distanciaDetras);
        posicionObjetivo.y = bolsa.transform.position.y + ajusteAltura;

        // 3. Calculamos la rotación para que Opip siempre mire hacia la bolsa
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionEmpuje, Vector3.up);

        // Si acabas de cambiar de cúmulo, Opip aparece directo en la nueva bolsa sin cruzar todo el mapa volando
        if (ultimaBolsaControlada != bolsa)
        {
            ultimaBolsaControlada = bolsa;
            transform.position = posicionObjetivo;
            transform.rotation = rotacionObjetivo;
        }
        else
        {
            // Orbita y gira suavemente alrededor de la bolsa cuando cambias de dirección
            transform.position = Vector3.Lerp(transform.position, posicionObjetivo, Time.deltaTime * velocidadPosicion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadRotacion);
        }
    }
}