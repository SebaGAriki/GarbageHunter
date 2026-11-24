using UnityEngine;

public class PuertaPlaca : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Arrastra aquí el objeto de la puerta que debe bajar y subir")]
    public Transform puerta;

    [Tooltip("Opcional: arrastra la malla de la placa si quieres que se hunda al pisarla")]
    public Transform visualPlaca;

    [Header("Configuración de la Puerta")]
    [Tooltip("Cuántas unidades baja la puerta en Y local al activar la placa")]
    public float distanciaBajada = 3f;

    [Tooltip("Qué tan rápido baja y sube la puerta")]
    public float velocidadPuerta = 4f;

    [Header("Configuración de la Placa (Opcional)")]
    public float hundimientoPlaca = 0.08f;

    private Vector3 posInicialPuertaLocal;
    private Vector3 posInicialPlacaLocal;
    private Rigidbody rbPuerta;
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        if (puerta != null)
        {
            posInicialPuertaLocal = puerta.localPosition;
            rbPuerta = puerta.GetComponent<Rigidbody>();
        }

        if (visualPlaca != null)
        {
            posInicialPlacaLocal = visualPlaca.localPosition;
        }
    }

    void FixedUpdate()
    {
        bool estaPresionada = HayBolsaOCumuloEncima();

        // 1. Mover la puerta en espacio local para que gire junto con el nivel
        if (puerta != null)
        {
            Vector3 destinoPuertaLocal = estaPresionada
                ? posInicialPuertaLocal - new Vector3(0f, Mathf.Abs(distanciaBajada), 0f)
                : posInicialPuertaLocal;

            Vector3 nuevaPosLocal = Vector3.MoveTowards(
                puerta.localPosition,
                destinoPuertaLocal,
                velocidadPuerta * Time.fixedDeltaTime
            );

            if (rbPuerta != null && rbPuerta.isKinematic)
            {
                Vector3 nuevaPosGlobal = puerta.parent != null
                    ? puerta.parent.TransformPoint(nuevaPosLocal)
                    : nuevaPosLocal;

                rbPuerta.MovePosition(nuevaPosGlobal);
            }
            else
            {
                puerta.localPosition = nuevaPosLocal;
            }
        }

        // 2. Hundir visualmente la placa en espacio local
        if (visualPlaca != null)
        {
            Vector3 destinoPlacaLocal = estaPresionada
                ? posInicialPlacaLocal - new Vector3(0f, hundimientoPlaca, 0f)
                : posInicialPlacaLocal;

            visualPlaca.localPosition = Vector3.MoveTowards(
                visualPlaca.localPosition,
                destinoPlacaLocal,
                1.5f * Time.fixedDeltaTime
            );
        }
    }

    private bool HayBolsaOCumuloEncima()
    {
        if (boxCollider == null) return false;

        Vector3 centroMundo = transform.TransformPoint(boxCollider.center);
        Vector3 mitadTamano = Vector3.Scale(boxCollider.size, transform.lossyScale) * 0.5f;

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

            bool esBolsa = col.GetComponentInParent<GravedadBolsa>() != null ||
                           col.GetComponentInChildren<GravedadBolsa>() != null;

            bool esCumulo = col.GetComponentInParent<CumuloInteractivo>() != null ||
                            col.GetComponentInChildren<CumuloInteractivo>() != null;

            if (esBolsa || esCumulo)
            {
                return true;
            }
        }

        return false;
    }
}