using UnityEngine;

public class PickableObject : MonoBehaviour
{
    private Rigidbody rb;
    private Transform objectGrabPointTransform;
    private BoxCollider boxCollider;
    private CapsuleCollider capsuleCollider;
    private Vector3 scaleChange;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = rb.GetComponent<BoxCollider>();
        capsuleCollider = rb.GetComponent<CapsuleCollider>();
        scaleChange = new Vector3(-2f, -2f, -2f);
    }
    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        rb.useGravity = false;


        if(boxCollider)
            boxCollider.enabled = false;

        if(capsuleCollider)
            capsuleCollider.enabled = true;

        transform.localScale += new Vector3(-.8f, -.8f, -.8f);
    }
    public void Drop()
    {
        this.objectGrabPointTransform = null;
        rb.useGravity = true;

        if(boxCollider)
            boxCollider.enabled = true;

        if (capsuleCollider)
            capsuleCollider.enabled = false;

        transform.localScale += new Vector3(.8f, .8f, .8f);
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
