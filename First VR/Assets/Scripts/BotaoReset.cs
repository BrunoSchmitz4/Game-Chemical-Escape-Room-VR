using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BotaoReset : MonoBehaviour
{
    public Bequer[] bequeres;
    public TuboLiquido[] tubos;
    public PuzzleSala1 puzzle;
    public AudioSource somReset;
    public bool diagnostico = true;

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

        if (diagnostico)
        {
            interativo.hoverEntered.AddListener(x => Debug.Log("[Reset] HOVER em " + name));
            interativo.selectEntered.AddListener(x => Debug.Log("[Reset] SELECT em " + name));

            string lista = "";
            for (int i = 0; i < bequeres.Length; i++)
                lista = lista + bequeres[i].name + " ";

            Debug.Log("[Reset] " + name + " ligado. Esvazia: " + lista + "| enche " + tubos.Length + " tubo(s)");
        }
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

        if (diagnostico)
            Debug.Log("[Reset] " + name + " EXECUTOU Reiniciar()");
    }
}
