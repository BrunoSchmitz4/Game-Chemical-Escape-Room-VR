using UnityEngine;

public class TuboLiquido : MonoBehaviour
{
    public int idTubo = 1;
    public Color cor = Color.yellow;

    public Transform liquido;
    public float escalaCheio = 0.64f;

    private float quantidade = 1f;

    void Start()
    {
        Encher();
    }

    public bool TemLiquido()
    {
        return quantidade > 0f;
    }

    public float Retirar(float pedido)
    {
        if (pedido > quantidade)
            pedido = quantidade;

        quantidade = quantidade - pedido;
        AtualizarVisual();
        return pedido;
    }

    public void Encher()
    {
        quantidade = 1f;
        AtualizarVisual();
    }

    void AtualizarVisual()
    {
        Vector3 escala = liquido.localScale;
        escala.y = escalaCheio * quantidade;
        liquido.localScale = escala;
    }
}
