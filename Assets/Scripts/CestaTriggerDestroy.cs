using UnityEngine;

public class CestaTriggerDestroy : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //GravedadBolsa bolsa = other.GetComponent<GravedadBolsa>();

        Destroy(other.gameObject);
    }
}