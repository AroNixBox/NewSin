using UnityEngine;

public class PickableObject : MonoBehaviour
{
    private Rigidbody rb;
    private Transform objectGrabPointTransform;
    private Collider thisCollider;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        thisCollider = rb.GetComponent<Collider>();
    }
    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        rb.useGravity = false;
        thisCollider.enabled = false;
    }
    public void Drop()
    {
        this.objectGrabPointTransform = null;
        rb.useGravity = true;
        this.thisCollider.enabled = true;
    }
    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            rb.MovePosition(newPosition);
        }
    }
}
