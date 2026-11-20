using UnityEngine;

public class GravedadBolsa : MonoBehaviour
{
    // Esta variable guarda automáticamente cuál es la bolsa activa en el nivel
    public static GravedadBolsa bolsaActiva;

    public float multiplicadorGravedad = 40f;
    private Rigidbody rb;

    void OnEnable()
    {
        bolsaActiva = this; // Al aparecer, se registra como la bolsa que controlas
    }

    void OnDisable()
    {
        if (bolsaActiva == this) bolsaActiva = null;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        Vector3 inclinacion = Input.acceleration;
        Vector3 gravedadFalsa = new Vector3(inclinacion.x, 0, inclinacion.y);
        rb.AddForce(gravedadFalsa * multiplicadorGravedad, ForceMode.Acceleration);
        rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
    }
}