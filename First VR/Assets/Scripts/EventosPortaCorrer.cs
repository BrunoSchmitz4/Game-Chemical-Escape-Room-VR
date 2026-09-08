using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class EventosPortaCorrer : MonoBehaviour
{
    private bool isOpen = false;
    private ConfigurableJoint joint;

    public TeleportationArea teleporte;
    public Outline outlinePorta;

    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
    }

    float GetJointLinearX()
    {
        Vector3 worldAnchor = joint.transform.TransformPoint(joint.anchor);

        Vector3 connectedAnchor = joint.connectedAnchor;

        Vector3 delta = worldAnchor - connectedAnchor;

        Vector3 axisX = joint.transform.TransformDirection(Vector3.right);

        float displacementX = Vector3.Dot(delta, axisX);

        return displacementX;
    }

    void Update()
    {
        if (isOpen)
            return;

        float abertura = Mathf.Abs(GetJointLinearX());

        if (abertura >= 0.6)
        {
            isOpen = true;
            teleporte.enabled = true;
            outlinePorta.OutlineWidth = 0f;
        }
    }
}
