using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PuzzleBotoes : MonoBehaviour
{
    public int botaoCerto = 2;
    public int vezesNecessarias = 3;

    public Outline outlinePorta;
    public XRGrabInteractable grabPorta;
    public AudioSource somVitoria;

    private int contador = 0;
    private bool resolvido = false;

    void Start()
    {
        outlinePorta.OutlineWidth = 0f;
        grabPorta.enabled = false;
    }

    public void Pressionar(int numBotao)
    {
        if (resolvido)
            return;

        if (numBotao == botaoCerto)
            contador = contador + 1;
        else
            contador = 0;

        if (contador >= vezesNecessarias)
            Resolver();
    }

    void Resolver()
    {
        resolvido = true;
        outlinePorta.OutlineWidth = 5f;
        grabPorta.enabled = true;
        somVitoria.Play();
    }
}
