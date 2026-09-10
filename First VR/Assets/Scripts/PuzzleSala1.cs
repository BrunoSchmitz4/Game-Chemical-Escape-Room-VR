using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PuzzleSala1 : MonoBehaviour
{
    public Bequer[] sequencia;

    public Outline outlinePorta;
    public EventosPorta eventosPorta;
    public XRGrabInteractable grabPorta;
    public AudioSource somVitoria;
    public bool diagnostico = true;

    private bool[] jaCorreto;
    private int indice = 0;
    private bool resolvido = false;

    void Start()
    {
        jaCorreto = new bool[sequencia.Length];
        outlinePorta.OutlineWidth = 0f;
        grabPorta.enabled = false;
        eventosPorta.enabled = false;
    }

    void Update()
    {
        if (resolvido)
            return;

        for (int i = 0; i < sequencia.Length; i++)
        {
            bool correto = sequencia[i].EstaCorreto();

            if (diagnostico && correto != jaCorreto[i])
                Debug.Log("[Puzzle1] mudou -> " + sequencia[i].Resumo() + " | indice=" + indice);

            if (correto && !jaCorreto[i])
                Registrar(i);

            jaCorreto[i] = correto;
        }
    }

    public void Reiniciar()
    {
        if (resolvido)
            return;

        indice = 0;

        for (int i = 0; i < jaCorreto.Length; i++)
            jaCorreto[i] = false;
    }

    void Registrar(int i)
    {
        if (diagnostico)
            Debug.Log("[Puzzle1] Registrar(" + i + "=" + sequencia[i].name + ") esperado=" + indice);

        if (i == indice)
        {
            indice = indice + 1;
        }
        else if (i == 0)
        {
            indice = 1;
        }
        else
        {
            indice = 0;

            if (diagnostico)
                Debug.LogWarning("[Puzzle1] FORA DE ORDEM: " + sequencia[i].name
                    + " ficou correto, mas o esperado era " + sequencia[indice].name
                    + ". A sequencia zerou. Os bequeres que JA estao corretos nao disparam de novo "
                    + "- e preciso ESVAZIAR e refazer para destravar.");
        }

        if (indice >= sequencia.Length)
            Resolver();
    }

    void Resolver()
    {
        resolvido = true;
        outlinePorta.OutlineWidth = 5f;
        grabPorta.enabled = true;
        eventosPorta.enabled = true;
        somVitoria.Play();

        if (diagnostico)
            Debug.Log("[Puzzle1] RESOLVIDO -> grab do Trinco1 habilitado, Outline aceso");
    }
}
