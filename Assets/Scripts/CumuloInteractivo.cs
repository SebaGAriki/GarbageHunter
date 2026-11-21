using UnityEngine;

public class CumuloInteractivo : MonoBehaviour
{
    public GameObject prefabBolsa;

    void OnMouseDown()
    {
        GravedadBolsa bolsaAnterior = GravedadBolsa.bolsaActiva;

        // Si hay una bolsa activa y NO está atrapada, la convertimos en cúmulo
        if (bolsaAnterior != null && !bolsaAnterior.atrapada)
        {
            InfoBolsa info = bolsaAnterior.GetComponent<InfoBolsa>();

            if (info != null && info.prefabCumulo != null)
            {
                Instantiate(info.prefabCumulo, bolsaAnterior.transform.position, Quaternion.identity, transform.parent);
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
            Instantiate(prefabBolsa, transform.position, Quaternion.identity, transform.parent);
        }

        Destroy(gameObject);
    }
}