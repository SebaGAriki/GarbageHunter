using UnityEngine;

public class CestaTrigger : MonoBehaviour
{
    public string tagCorrecto;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagCorrecto))
        {
            Debug.Log("¡Bolsa depositada correctamente!");

            if (GameManager.Instancia != null)
            {
                GameManager.Instancia.ChequearVictoria(tagCorrecto);
            }

            Destroy(other.gameObject);
        }
    }
}