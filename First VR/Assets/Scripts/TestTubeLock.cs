using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TestTubeLock : MonoBehaviour
{
    [SerializeField]
    private Transform rackTopLimit; // ponto acima do rack 
    private XRGrabInteractable grab;
    private Rigidbody rb;

    [SerializeField]
    private Transform socketPoint;

    private bool isLockedInRack = true;

    void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        LockInRack();
    }

    void Update()
    {
        if (isLockedInRack)
        {
            CheckIfReleased();
        }
    }

    void LockInRack()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation
                       | RigidbodyConstraints.FreezePositionX
                       | RigidbodyConstraints.FreezePositionZ;

        isLockedInRack = true;
    }

    void UnlockFromRack()
    {
        rb.constraints = RigidbodyConstraints.None;
        isLockedInRack = false;
    }
    void CheckIfReleased() 
    { 
        if (transform.position.y > rackTopLimit.position.y) 
        { 
            UnlockFromRack(); 
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TubeSocket") && !isLockedInRack)
        {
            SnapToSocket();
        }
    }

    void SnapToSocket()
    {
        transform.position = socketPoint.position;
        transform.rotation = socketPoint.rotation;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        GetComponent<TuboLiquido>().Encher();

        LockInRack();
    }
}
