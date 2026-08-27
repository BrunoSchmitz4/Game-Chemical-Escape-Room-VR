using UnityEngine;

public class DerramarLiquido : MonoBehaviour
{
    [SerializeField]
    private float anguloMin = 100f; // Ângulo mínimo para considerar "virado" 

    private bool inZonaDerramar = false;
    private bool estahDerramando = false;

    void Update()
    {
        if (!inZonaDerramar || estahDerramando)
            return;

        float angulo = Vector3.Angle(transform.up, Vector3.up);

        if (angulo > anguloMin)
        {
            Pour();
        }
    }

    void Pour()
    {
        estahDerramando = true;
        print("Ingrediente derramado!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZonaDerramar"))
        {
            gameObject.GetComponent<Outline>().OutlineWidth = 5f;
            inZonaDerramar = true;
            print("In ZonaDerramar!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ZonaDerramar"))
        {
            gameObject.GetComponent<Outline>().OutlineWidth = 0f;
            inZonaDerramar = false;
            print("Out ZonaDerramar!");
        }
    }
}