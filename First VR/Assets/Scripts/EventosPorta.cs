using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class EventosPorta : MonoBehaviour
{
    private bool isOpen = false;
    private HingeJoint hinge;
    public TeleportationArea teleporte;
    public XRGrabInteractable grabPorta;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
    }

    void Update()
    {
        float angle = hinge.angle;

        if (!isOpen && angle <= -40)
        {
            isOpen = true;
            teleporte.enabled = true;
            grabPorta.enabled = true;
        }
        else
        {
            if (isOpen && angle > -40)
            {
                isOpen = false;
                teleporte.enabled = false;
                grabPorta.enabled = false;
            }
        }
    }
}
