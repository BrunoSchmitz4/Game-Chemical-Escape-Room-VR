using UnityEngine;

public class DerramarLiquido : MonoBehaviour
{
    [SerializeField]
    private float anguloMin = 85f;

    public float velocidade = 0.5f;
    public AudioSource somDerramar;

    private TuboLiquido tubo;
    private Outline contorno;
    private Bequer bequerAtual;
    private bool estahDerramando = false;

    void Start()
    {
        tubo = GetComponent<TuboLiquido>();
        contorno = GetComponent<Outline>();
    }

    void Update()
    {
        bool deveDerramar = PodeDerramar();

        if (deveDerramar)
            Derramar();

        if (deveDerramar != estahDerramando)
        {
            estahDerramando = deveDerramar;

            if (estahDerramando)
            {
                somDerramar.Play();
                print("Derramando tubo " + tubo.idTubo);
            }
            else
            {
                somDerramar.Stop();
            }
        }
    }

    bool PodeDerramar()
    {
        if (bequerAtual == null || !tubo.TemLiquido())
            return false;

        return Vector3.Angle(transform.up, Vector3.up) > anguloMin;
    }

    void Derramar()
    {
        float quantidade = tubo.Retirar(velocidade * Time.deltaTime);
        bequerAtual.Receber(tubo.idTubo, tubo.cor, quantidade / 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZonaDerramar"))
        {
            contorno.OutlineWidth = 5f;
            bequerAtual = other.GetComponentInParent<Bequer>();
            print("Tubo " + tubo.idTubo + " entrou em " + bequerAtual.name +
                  " | incline mais de " + anguloMin + " graus para derramar");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ZonaDerramar"))
        {
            contorno.OutlineWidth = 0f;
            bequerAtual = null;
        }
    }
}
