using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PuzzleSala1 : MonoBehaviour
{
    public Bequer[] sequencia;

    public Outline outlinePorta;
    public EventosPorta eventosPorta;
    public XRGrabInteractable grabPorta;
    public AudioSource somVitoria;

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

            if (correto && !jaCorreto[i])
                Registrar(i);

            jaCorreto[i] = correto;
        }
    }

    void Registrar(int i)
    {
        if (i == indice)
            indice = indice + 1;
        else if (i == 0)
            indice = 1;
        else
            indice = 0;

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
    }
}
