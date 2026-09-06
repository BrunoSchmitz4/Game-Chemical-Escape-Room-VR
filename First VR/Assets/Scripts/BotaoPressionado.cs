using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
public class BotaoPressionado : MonoBehaviour
{
    public int numBotao;
    void Start()
    {
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(x => Pressionei());
    }
    public void Pressionei()
    {
        // aqui vai a sua logica de acordo com o puzzle 
        print("PRESS " + numBotao);
    }
}