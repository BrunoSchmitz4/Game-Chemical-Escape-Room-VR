using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

public class ShowMessageNaArea : MonoBehaviour
{
    public Transform playerCamera;
    public GameObject messageObject;
    public LocomotionMediator locomotionSystem;

    public XRRayInteractor leftRay;
    public XRRayInteractor rightRay;

    public float maxDistance = 3f;

    public GameObject prefabBolhas;
    public AudioSource somVitoria;
    public float distanciaTela = 1.5f;
    public float larguraTela = 1.2f;
    public float duracaoAnimacao = 0.6f;
    public int quantidadeBolhas = 80;
    public bool inverterTela = false;

    private bool triggered = false;

    void Update()
    {
        if (triggered) return;

        Ray ray = new Ray(playerCamera.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                triggered = true;
                Comemorar(hit.point);
            }
        }
    }

    void Comemorar(Vector3 posicaoJogador)
    {
        Camera camera = playerCamera.GetComponentInChildren<Camera>();

        RectTransform retangulo = messageObject.GetComponent<RectTransform>();
        RectTransform texto = retangulo.GetChild(0).GetComponent<RectTransform>();
        float escalaFinal = larguraTela / texto.sizeDelta.x;

        PrepararTela(camera, retangulo);
        StartCoroutine(AnimarEntradaTela(retangulo, escalaFinal));

        DispararBolhas(posicaoJogador);

        if (somVitoria != null)
            somVitoria.Play();

        Time.timeScale = 0;
        locomotionSystem.enabled = false;

        leftRay.enabled = false;
        rightRay.enabled = false;
    }

    void PrepararTela(Camera camera, RectTransform retangulo)
    {
        messageObject.SetActive(true);

        Canvas tela = messageObject.GetComponent<Canvas>();
        tela.renderMode = RenderMode.WorldSpace;
        tela.worldCamera = camera;

        retangulo.SetParent(camera.transform, false);
        retangulo.localPosition = new Vector3(0, 0, distanciaTela);
        retangulo.localRotation = inverterTela ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        retangulo.localScale = Vector3.zero;
    }

    IEnumerator AnimarEntradaTela(RectTransform retangulo, float escalaFinal)
    {
        float tempo = 0f;

        while (tempo < duracaoAnimacao)
        {
            tempo += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(tempo / duracaoAnimacao);
            retangulo.localScale = Vector3.one * escalaFinal * t;
            yield return null;
        }

        retangulo.localScale = Vector3.one * escalaFinal;
    }

    void DispararBolhas(Vector3 posicao)
    {
        Debug.Log("[Vitoria] DispararBolhas em " + posicao + " | prefabBolhas = "
            + (prefabBolhas != null ? prefabBolhas.name : "NULO"));

        if (prefabBolhas == null)
            return;

        Vector3 posicaoBolhas = posicao + Vector3.up * 1.2f;
        GameObject bolhas = Instantiate(prefabBolhas, posicaoBolhas, Quaternion.identity);
        ParticleSystem sistema = bolhas.GetComponent<ParticleSystem>();

        Debug.Log("[Vitoria] instanciado " + bolhas.name + " em " + posicaoBolhas
            + " | ParticleSystem = " + (sistema != null ? "encontrado" : "NAO ENCONTRADO"));

        if (sistema == null)
            return;

        ParticleSystem.MainModule principal = sistema.main;
        principal.useUnscaledTime = true;
        principal.loop = false;
        principal.startColor = new ParticleSystem.MinMaxGradient(Color.magenta);
        principal.startSize = 0.8f;

        ParticleSystem.EmissionModule emissao = sistema.emission;
        emissao.rateOverTime = 0;

        ParticleSystem.ShapeModule formato = sistema.shape;
        formato.shapeType = ParticleSystemShapeType.Sphere;
        formato.radius = 1.5f;

        ParticleSystemRenderer renderizador = bolhas.GetComponent<ParticleSystemRenderer>();
        Debug.Log("[Vitoria] renderer.enabled = " + (renderizador != null ? renderizador.enabled.ToString() : "SEM RENDERER")
            + " | material = " + (renderizador != null && renderizador.sharedMaterial != null ? renderizador.sharedMaterial.name : "NENHUM"));

        sistema.Play();
        sistema.Emit(quantidadeBolhas);

        Debug.Log("[Vitoria] particleCount apos Emit = " + sistema.particleCount
            + " | isPlaying = " + sistema.isPlaying);

        Destroy(bolhas, 10f);
    }
}
