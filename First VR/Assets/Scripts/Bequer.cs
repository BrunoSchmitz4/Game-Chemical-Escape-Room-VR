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
    public float anguloDescarte = 120f;

    public Color corAlvo = Color.magenta;
    public float volumeAlvo = 0.66f;
    public List<Combinacao> combinacoes = new List<Combinacao>();

    private List<int> tubos = new List<int>();
    private List<Color> cores = new List<Color>();
    private float volume = 0f;
    private Material material;
    private Vector3 posicaoInicial;
    private Quaternion rotacaoInicial;

    void Start()
    {
        material = liquido.GetComponent<Renderer>().material;
        posicaoInicial = transform.position;
        rotacaoInicial = transform.rotation;

        GetComponent<XRGrabInteractable>().selectExited.AddListener(x => VoltarAoLugar());

        Esvaziar();
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
    }
}
