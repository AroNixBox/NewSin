using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushing : MonoBehaviour
{

    [SerializeField] private Transform push;
    [SerializeField] private Transform jump;
    [SerializeField] private Transform paddle;
    [SerializeField] private GameObject boat;
    [SerializeField] private Animator animator;
    private Rigidbody rb;
    [SerializeField] private Collider myCollider;
    private bool jumpingTarget;
    private bool hasReachedTarget;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(!hasReachedTarget)
        {
             Quaternion targetrotation = Quaternion.LookRotation(push.transform.position - transform.position);
             transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, 1* Time.deltaTime);
             transform.position += transform.forward * 1f * Time.deltaTime;
        }
        if(hasReachedTarget && !jumpingTarget)
        {
            Quaternion targetrotation = Quaternion.LookRotation(jump.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, 1 * Time.deltaTime);
            transform.position += transform.forward * 10f * Time.deltaTime;
        }

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Finish"))
        {
            hasReachedTarget = true;
            Destroy(collision.gameObject);
            animator.SetBool("jump", true);
            boat.transform.parent = null;

        }
        if (collision.CompareTag("Finish2"))
        {
            rb.isKinematic = true;
            jumpingTarget = true;
            myCollider.enabled = false;
            transform.position = paddle.position;
            transform.parent = paddle.parent;
            boat.GetComponent<Boatscript>().goDriveIntoTheSun = true;
        }

    }
}
