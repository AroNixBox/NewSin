using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Tree : MonoBehaviour, ITreeDamageable
{
    int healthAmount;
    public enum Type
    {
        Tree,
        Log,
        LogHalf,
        Stump
    }

    [SerializeField] private Type treeType;
    [SerializeField] private Transform treeLog;
    [SerializeField] private Transform treeLogHalf;
    [SerializeField] private Transform treeStump;
    [SerializeField] private UnityEvent TreeFell;

    private void Awake()
    {
        switch (treeType)
        {
            default:
            case Type.Tree: healthAmount = 30; break;
            case Type.Log: healthAmount = 50; break;
            case Type.LogHalf: healthAmount = 50; break;
            case Type.Stump: healthAmount = 50; break;
        }
    }
    public void Damage(int amount)
    {
        healthAmount -= amount;
    }
    private void FixedUpdate()
    {
        if (healthAmount < 0)
        {
            OnTreeDead();
        }
    }


    private void OnTreeDead()
    {
        // Add another function before executing! =)
        switch (treeType)
        {
            default:
            case Type.Tree:
                //spawn Log
                Vector3 treeLogOffSet =transform.right * 0.3f;
                Instantiate(treeLog, transform.position + treeLogOffSet, Quaternion.Euler(0,0,90));
                //Spawn Stump
                treeLogOffSet = transform.right * -5.3f;
                Instantiate(treeStump, transform.position + treeLogOffSet, Quaternion.Euler(0,0,0));
                break;

            case Type.Log:
                //spawn fx
                //spawn LogHalf
                float logYPositionAboveStump = -3.6f;
                treeLogOffSet = transform.right * logYPositionAboveStump;
                Instantiate(treeLogHalf, transform.position + treeLogOffSet, transform.rotation);
                //spawn LogHalf
                float logYPositionAboveFirstHalf = -0.5f;
                treeLogOffSet = transform.right * logYPositionAboveFirstHalf;
                Instantiate(treeLogHalf, transform.position + treeLogOffSet, transform.rotation);

                float logYPositionAboveSecondHalf = 2.8f;
                treeLogOffSet = transform.right * logYPositionAboveSecondHalf;
                Instantiate(treeLogHalf, transform.position + treeLogOffSet, transform.rotation);
                break;

            case Type.LogHalf:
                //SpawnFx
                break;

            case Type.Stump:
                //SpawnFX
                break;
        }
        this.gameObject.SetActive(false);
    }
    //Deal Damage to another tree
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            switch (treeType)
            {
                default:
                case Type.Log:
                    TreeFell?.Invoke();
                    return;
            }
        }
        if (collision.gameObject.TryGetComponent<ITreeDamageable>(out ITreeDamageable treeDamageable))
        {
            if (collision.relativeVelocity.magnitude > 1f)
            {
                int damageAmount = Random.Range(5, 20);
                //DamagePopUp
                treeDamageable.Damage(damageAmount);
            }
        }
    }
}
