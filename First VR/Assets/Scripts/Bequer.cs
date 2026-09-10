using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[System.Serializable]
public class Combinacao
{
    public int tuboA;
    public int tuboB;
    public Color resultado;
}

public class Bequer : MonoBehaviour
{
    public Transform liquido;
    public float escalaCheio = 1f;
    public float anguloDescarte = 100f;

    public Color corAlvo = Color.magenta;
    public float volumeAlvo = 0.66f;
    public List<Combinacao> combinacoes = new List<Combinacao>();

    public bool mostrarMarca = true;
    public Color corMarca = new Color(0.9f, 0.9f, 0.9f, 1f);
    public Color corMarcaAtingida = new Color(0.2f, 1f, 0.45f, 1f);
    public float espessuraMarca = 0.004f;
    public float folgaMarca = 1.03f;

    private List<int> tubos = new List<int>();
    private List<Color> cores = new List<Color>();
    private float volume = 0f;
    private Material material;
    private Vector3 posicaoInicial;
    private Quaternion rotacaoInicial;
    private Material materialMarca;

    void Start()
    {
        material = liquido.GetComponent<Renderer>().material;
        posicaoInicial = transform.position;
        rotacaoInicial = transform.rotation;

        GetComponent<XRGrabInteractable>().selectExited.AddListener(x => VoltarAoLugar());

        if (mostrarMarca)
            CriarMarca();

        Esvaziar();
    }

    void CriarMarca()
    {
        MeshFilter filtroVidro = GetComponent<MeshFilter>();
        MeshFilter filtroLiquido = liquido.GetComponent<MeshFilter>();

        if (filtroVidro == null || filtroVidro.sharedMesh == null ||
            filtroLiquido == null || filtroLiquido.sharedMesh == null)
        {
            Debug.LogWarning("Bequer " + name + ": sem MeshFilter para calcular a marca de nivel");
            return;
        }

        Bounds caixaVidro = filtroVidro.sharedMesh.bounds;
        Bounds caixaLiquido = filtroLiquido.sharedMesh.bounds;

        float escalaAlvo = escalaCheio * volumeAlvo;
        float altura = liquido.localPosition.y + caixaLiquido.max.y * escalaAlvo;
        float raio = Mathf.Max(caixaVidro.extents.x, caixaVidro.extents.z) * folgaMarca;

        GameObject anel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        anel.name = "MarcaNivel";

        Collider colisorAnel = anel.GetComponent<Collider>();
        colisorAnel.enabled = false;
        Destroy(colisorAnel);

        anel.transform.SetParent(transform, false);
        anel.transform.localPosition = new Vector3(caixaVidro.center.x, altura, caixaVidro.center.z);
        anel.transform.localRotation = Quaternion.identity;
        anel.transform.localScale = new Vector3(raio * 2f, espessuraMarca * 0.5f, raio * 2f);

        Renderer renderizador = anel.GetComponent<Renderer>();
        renderizador.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderizador.receiveShadows = false;

        materialMarca = renderizador.material;
        materialMarca.EnableKeyword("_EMISSION");

        AtualizarMarca();
    }

    void AtualizarMarca()
    {
        if (materialMarca == null)
            return;

        Color cor = volume >= volumeAlvo ? corMarcaAtingida : corMarca;

        materialMarca.color = cor;
        materialMarca.SetColor("_BaseColor", cor);
        materialMarca.SetColor("_EmissionColor", cor * (volume >= volumeAlvo ? 1.2f : 0.1f));
    }

    void VoltarAoLugar()
    {
        transform.position = posicaoInicial;
        transform.rotation = rotacaoInicial;
    }

    void Update()
    {
        if (volume <= 0f)
            return;

        if (Vector3.Angle(transform.up, Vector3.up) > anguloDescarte)
            Esvaziar();
    }

    public void Receber(int idTubo, Color cor, float quantidade)
    {
        if (!tubos.Contains(idTubo))
        {
            tubos.Add(idTubo);
            cores.Add(cor);
        }

        volume = volume + quantidade;
        AtualizarVisual();
    }

    public void Esvaziar()
    {
        tubos.Clear();
        cores.Clear();
        volume = 0f;
        AtualizarVisual();
    }

    public bool EstaCorreto()
    {
        if (volume < volumeAlvo)
            return false;

        return CorParecida(CorDaMistura(), corAlvo);
    }

    public string Resumo()
    {
        string ids = "";
        for (int i = 0; i < tubos.Count; i++)
            ids = ids + tubos[i] + (i < tubos.Count - 1 ? "+" : "");

        Color mistura = CorDaMistura();
        float diferenca = Mathf.Abs(mistura.r - corAlvo.r)
                        + Mathf.Abs(mistura.g - corAlvo.g)
                        + Mathf.Abs(mistura.b - corAlvo.b);

        return name
             + " vol=" + volume.ToString("F3") + "/" + volumeAlvo.ToString("F2")
             + (volume >= volumeAlvo ? " (OK)" : " (FALTA)")
             + " tubos=[" + ids + "]"
             + " mistura=" + ColorUtility.ToHtmlStringRGB(mistura)
             + " alvo=" + ColorUtility.ToHtmlStringRGB(corAlvo)
             + " dif=" + diferenca.ToString("F3") + (diferenca < 0.15f ? " (OK)" : " (LONGE)")
             + " => correto=" + EstaCorreto();
    }

    Color CorDaMistura()
    {
        if (cores.Count == 0)
            return Color.white;

        if (cores.Count == 1)
            return cores[0];

        if (cores.Count == 2)
        {
            for (int i = 0; i < combinacoes.Count; i++)
            {
                Combinacao c = combinacoes[i];
                if (tubos.Contains(c.tuboA) && tubos.Contains(c.tuboB))
                    return c.resultado;
            }
        }

        Color media = Color.black;
        for (int i = 0; i < cores.Count; i++)
            media = media + cores[i];

        return media / cores.Count;
    }

    bool CorParecida(Color a, Color b)
    {
        float diferenca = Mathf.Abs(a.r - b.r) + Mathf.Abs(a.g - b.g) + Mathf.Abs(a.b - b.b);
        return diferenca < 0.15f;
    }

    void AtualizarVisual()
    {
        bool temLiquido = volume > 0f;
        liquido.gameObject.SetActive(temLiquido);

        Vector3 escala = liquido.localScale;
        escala.y = escalaCheio * volume;
        liquido.localScale = escala;

        if (temLiquido)
        {
            Color cor = CorDaMistura();
            material.color = cor;
            material.SetColor("_EmissionColor", cor * 0.35f);
        }

        AtualizarMarca();
    }
}
