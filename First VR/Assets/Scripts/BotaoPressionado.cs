using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BotaoPressionado : MonoBehaviour
{
    public int numBotao;
    public PuzzleBotoes puzzle;

    void Start()
    {
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(x => Pressionei());
    }

    public void Pressionei()
    {
        print("PRESS " + numBotao);
        puzzle.Pressionar(numBotao);
    }
}
