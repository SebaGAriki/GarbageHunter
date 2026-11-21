using UnityEngine;

public class PozoTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GravedadBolsa bolsa = other.GetComponent<GravedadBolsa>();

        if (bolsa != null && !bolsa.atrapada)
        {
            // 1. Inutilizamos el control de la bolsa
            bolsa.atrapada = true;
            bolsa.enabled = false;

            // 2. Desactivamos el collider (sirve para BoxCollider y SphereCollider)
            Collider col = bolsa.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }

            // 3. Congelamos su física para que quede quieta en el pozo sin atravesar el piso hacia el vacío
            Rigidbody rb = bolsa.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // 4. Buscamos todos los cúmulos disponibles en el nivel
            CumuloInteractivo[] cumulosRestantes = FindObjectsOfType<CumuloInteractivo>();

            if (cumulosRestantes.Length > 0)
            {
                CumuloInteractivo masCercano = null;
                float distanciaMinima = Mathf.Infinity;

                foreach (CumuloInteractivo cumulo in cumulosRestantes)
                {
                    float distancia = Vector3.Distance(bolsa.transform.position, cumulo.transform.position);
                    if (distancia < distanciaMinima)
                    {
                        distanciaMinima = distancia;
                        masCercano = cumulo;
                    }
                }

                if (masCercano != null)
                {
                    masCercano.TransformarEnBolsa();
                }
            }
            else
            {
                if (GameManager.Instancia != null)
                {
                    GameManager.Instancia.VerificarFinPorPozo();
                }
            }
        }
    }
}