using UnityEngine;

public class CumuloInteractivo : MonoBehaviour
{
    // Aquí arrastrarás el Prefab de la ESFERA de este mismo color
    public GameObject prefabBolsa;

    void OnMouseDown()
    {
        // 1. Buscamos la bolsa que estabas controlando hasta ahora
        GravedadBolsa bolsaAnterior = GravedadBolsa.bolsaActiva;

        if (bolsaAnterior != null)
        {
            InfoBolsa info = bolsaAnterior.GetComponent<InfoBolsa>();

            // Creamos el cúmulo quieto en el lugar exacto donde quedó la bolsa anterior
            if (info != null && info.prefabCumulo != null)
            {
                Instantiate(info.prefabCumulo, bolsaAnterior.transform.position, Quaternion.identity, transform.parent);
            }

            // Destruimos la bolsa anterior
            Destroy(bolsaAnterior.gameObject);
        }

        // 2. Creamos la nueva bolsa móvil en el lugar de este cúmulo que tocaste
        if (prefabBolsa != null)
        {
            Instantiate(prefabBolsa, transform.position, Quaternion.identity, transform.parent);
        }

        // 3. Destruimos este cúmulo porque ya se transformó en bolsa
        Destroy(gameObject);
    }
}
