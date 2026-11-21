using UnityEngine;

public class CestaTrigger : MonoBehaviour
{
    [Tooltip("Tag de la bolsa que corresponde al color de este cesto (ej. BolsaVerde, BolsaAmarilla)")]
    public string tagCorrecto;

    void OnTriggerEnter(Collider other)
    {
        GravedadBolsa bolsa = other.GetComponent<GravedadBolsa>();

        // Acepta cualquier bolsa activa que no esté atrapada en un pozo
        if (bolsa != null && !bolsa.atrapada)
        {
            bool esColorCorrecto = other.CompareTag(tagCorrecto);

            // La marcamos para evitar que dispare el trigger dos veces antes de destruirse
            bolsa.atrapada = true;

            if (GameManager.Instancia != null)
            {
                GameManager.Instancia.BolsaDepositada(bolsa, esColorCorrecto);
            }

            Destroy(other.gameObject);
        }
    }
}