using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class EventosPorta : MonoBehaviour
{
    private bool isOpen = false;
    private HingeJoint hinge;
    public TeleportationArea teleporte;
    public Outline outlinePorta;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
    }

    void Update()
    {
        if (isOpen)
            return;

        float angle = hinge.angle;

        if (angle <= -40)
        {
            isOpen = true;
            teleporte.enabled = true;
            outlinePorta.OutlineWidth = 0f;
        }
    }
}
