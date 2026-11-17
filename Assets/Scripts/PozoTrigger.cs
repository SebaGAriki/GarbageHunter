using UnityEngine;

public class PozoTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BolsaVerde"))
        {
            Debug.Log("¡Derrota! La bolsa cayó en el pozo.");
            if (GameManager.Instancia != null)
            {
                GameManager.Instancia.MostrarDerrota();
            }
        }
    }
}