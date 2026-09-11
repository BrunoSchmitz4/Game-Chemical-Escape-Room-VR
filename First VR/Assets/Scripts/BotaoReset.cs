using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BotaoReset : MonoBehaviour
{
    public Bequer[] bequeres;
    public TuboLiquido[] tubos;
    public PuzzleSala1 puzzle;
    public AudioSource somReset;

    void Start()
    {
        if (bequeres == null || bequeres.Length == 0)
            bequeres = FindObjectsByType<Bequer>();

        if (tubos == null || tubos.Length == 0)
            tubos = FindObjectsByType<TuboLiquido>();

        if (puzzle == null)
            puzzle = FindAnyObjectByType<PuzzleSala1>();

        XRSimpleInteractable interativo = GetComponentInChildren<XRSimpleInteractable>();

        if (interativo == null)
        {
            Debug.LogError("[Reset] " + name + ": NAO achei XRSimpleInteractable neste objeto nem nos filhos");
            return;
        }

        interativo.selectEntered.AddListener(x => Reiniciar());
    }

    public void Reiniciar()
    {
        for (int i = 0; i < bequeres.Length; i++)
            bequeres[i].Esvaziar();

        for (int i = 0; i < tubos.Length; i++)
            tubos[i].Encher();

        if (puzzle != null)
            puzzle.Reiniciar();

        if (somReset != null)
            somReset.Play();
    }
}
