using UnityEngine;

public class GravedadBolsa : MonoBehaviour
{
    public float multiplicadorGravedad = 40f;
    private Rigidbody rb;

    void Start()
    {
        Input.gyro.enabled = true;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Desactivamos la gravedad por defecto
    }

    void FixedUpdate()
    {
        // Leer la inclinación del dispositivo
        Vector3 inclinacion = Input.acceleration;

        // Mapear la inclinación del celular (X e Y) a los ejes del piso en Unity (X y Z)
        Vector3 gravedadFalsa = new Vector3(inclinacion.x, 0, inclinacion.y);

        // Aplicar la inclinación como aceleración (movimiento de la bolsa)
        rb.AddForce(gravedadFalsa * multiplicadorGravedad, ForceMode.Acceleration);

        // Añadir una fuerza artificial constante hacia abajo para asegurar que no flote
        rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
    }
}