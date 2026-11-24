using UnityEngine;

public class CumuloInteractivo : MonoBehaviour
{
    public GameObject prefabBolsa;
    [Header("Ajuste de Alturas")]
    [Tooltip("Altura fija en el mundo (eje Y) donde aparecerán los cúmulos")]
    public float alturaMundoCumulo = 0.8f;

    [Tooltip("Elevación extra al convertir este cúmulo en bolsa para que no nazca enterrada en el piso")]
    public float elevacionBolsa = 1f;

    void OnMouseDown()
    {
        if (GameManager.juegoPausado) return;

        GravedadBolsa bolsaAnterior = GravedadBolsa.bolsaActiva;

        // Si hay una bolsa activa y NO está atrapada, la convertimos en cúmulo
        if (bolsaAnterior != null && !bolsaAnterior.atrapada)
        {
            InfoBolsa info = bolsaAnterior.GetComponent<InfoBolsa>();

            if (info != null && info.prefabCumulo != null)
            {
                // Mantiene X y Z donde quedó la bolsa, pero fija Y en 0.8 del mundo
                Vector3 posCumulo = bolsaAnterior.transform.position;
                posCumulo.y = alturaMundoCumulo;
                Instantiate(info.prefabCumulo, posCumulo, Quaternion.identity, transform.parent);
            }

            Destroy(bolsaAnterior.gameObject);
        }

        // Despertamos este cúmulo
        TransformarEnBolsa();
    }

    // Este método convierte este cúmulo en bolsa sin dejar nada atrás
    public void TransformarEnBolsa()
    {
        if (prefabBolsa != null)
        {
            // Como el cúmulo está a 0.8 (al ras del piso) y el pivote de la bolsa está en su centro (~1.77),
            // sumamos elevacionBolsa para que la esfera aparezca apoyada sobre el suelo y no atravesándolo
            Vector3 posBolsa = transform.position;
            posBolsa.y += elevacionBolsa;

            Instantiate(prefabBolsa, posBolsa, Quaternion.identity, transform.parent);
        }

        Destroy(gameObject);
    }
}