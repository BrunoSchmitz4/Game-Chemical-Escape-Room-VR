using UnityEngine;

public class DerramarLiquido : MonoBehaviour
{
    [SerializeField]
    private float anguloMin = 85f;

    public float velocidade = 0.5f;
    public AudioSource somDerramar;
    public bool ignorarColisaoComBequer = true;

    private TuboLiquido tubo;
    private Outline contorno;
    private Bequer bequerAtual;
    private bool estahDerramando = false;

    void Start()
    {
        tubo = GetComponent<TuboLiquido>();
        contorno = GetComponent<Outline>();

        if (ignorarColisaoComBequer)
            IgnorarColisaoComBequeres();
    }

    void IgnorarColisaoComBequeres()
    {
        Collider[] meus = GetComponentsInChildren<Collider>();
        Bequer[] bequeres = FindObjectsByType<Bequer>();

        for (int b = 0; b < bequeres.Length; b++)
        {
            Collider[] dele = bequeres[b].GetComponentsInChildren<Collider>();

            for (int i = 0; i < meus.Length; i++)
            {
                if (meus[i].isTrigger)
                    continue;

                for (int j = 0; j < dele.Length; j++)
                {
                    if (dele[j].isTrigger)
                        continue;

                    Physics.IgnoreCollision(meus[i], dele[j], true);
                }
            }
        }
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
                somDerramar.Play();
            else
                somDerramar.Stop();
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
