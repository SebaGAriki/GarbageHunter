using UnityEngine;

public class PuertaPlaca : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Arrastra aquí el objeto de la puerta que debe bajar y subir")]
    public Transform puerta;

    [Tooltip("Opcional: arrastra la malla de la placa si quieres que se hunda al pisarla")]
    public Transform visualPlaca;

    [Header("Configuración de la Puerta")]
    [Tooltip("Cuántas unidades baja la puerta en Y al activar la placa")]
    public float distanciaBajada = 3f;

    [Tooltip("Qué tan rápido baja y sube la puerta")]
    public float velocidadPuerta = 4f;

    [Header("Configuración de la Placa (Opcional)")]
    public float hundimientoPlaca = 0.08f;

    private Vector3 posInicialPuerta;
    private Vector3 posInicialPlaca;
    private Rigidbody rbPuerta;
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        if (puerta != null)
        {
            posInicialPuerta = puerta.position;
            rbPuerta = puerta.GetComponent<Rigidbody>();
        }

        if (visualPlaca != null)
        {
            posInicialPlaca = visualPlaca.position;
        }
    }

    void FixedUpdate()
    {
        bool estaPresionada = HayBolsaOCumuloEncima();

        // 1. Mover la puerta
        if (puerta != null)
        {
            Vector3 destinoPuerta = estaPresionada
                ? posInicialPuerta - new Vector3(0f, Mathf.Abs(distanciaBajada), 0f)
                : posInicialPuerta;

            Vector3 nuevaPos = Vector3.MoveTowards(
                puerta.position,
                destinoPuerta,
                velocidadPuerta * Time.fixedDeltaTime
            );

            if (rbPuerta != null && rbPuerta.isKinematic)
                rbPuerta.MovePosition(nuevaPos);
            else
                puerta.position = nuevaPos;
        }

        // 2. Hundir visualmente la placa
        if (visualPlaca != null)
        {
            Vector3 destinoPlaca = estaPresionada
                ? posInicialPlaca - new Vector3(0f, hundimientoPlaca, 0f)
                : posInicialPlaca;

            visualPlaca.position = Vector3.MoveTowards(
                visualPlaca.position,
                destinoPlaca,
                1.5f * Time.fixedDeltaTime
            );
        }
    }

    private bool HayBolsaOCumuloEncima()
    {
        if (boxCollider == null) return false;

        // Calcula el centro y tamaño real del BoxCollider de la placa en el mundo
        Vector3 centroMundo = transform.TransformPoint(boxCollider.center);
        Vector3 mitadTamano = Vector3.Scale(boxCollider.size, transform.lossyScale) * 0.5f;

        // Detecta TODOS los colliders dentro de la caja de la placa en este instante (incluidos objetos kinematic quietos)
        Collider[] collidersDentro = Physics.OverlapBox(
            centroMundo,
            mitadTamano,
            transform.rotation,
            Physics.AllLayers,
            QueryTriggerInteraction.Collide
        );

        for (int i = 0; i < collidersDentro.Length; i++)
        {
            Collider col = collidersDentro[i];
            if (col.gameObject == gameObject) continue;

            if (col.GetComponentInParent<GravedadBolsa>() != null ||
                col.GetComponentInParent<CumuloInteractivo>() != null)
            {
                return true;
            }
        }

        return false;
    }
}