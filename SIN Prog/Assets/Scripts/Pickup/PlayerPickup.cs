using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerPickup : MonoBehaviour
{
    [field: SerializeField] VoidEventChannel eventChannel { get; set; }
    public Action onRecipePickupAction;
    private ThirdPersonActionMap actionMap;
    private PickableObject pickableObject;
    [SerializeField] private Transform interactRaycastAnker;
    [SerializeField] private Transform objectGrabPointTransform;
    private float interactionDistance = 2f;
    [SerializeField] LayerMask interactionMask;
    [SerializeField] private  ToolController toolController;

    public UnityEvent OnAxeHit;
    private Animator animator;

    //UI + Colorchange for Interaction
    [SerializeField] private GameObject pickUPUI;
    private RaycastHit raycastHit;
    private void Awake()
    {
        toolController = GetComponent<ToolController>();
        pickableObject = GetComponent<PickableObject>();
        actionMap = new ThirdPersonActionMap();
        pickUPUI.SetActive(false);
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        actionMap.OnFoot.Interact.performed += DoInteract;
        actionMap.OnFoot.Hit.performed += ToolHit;
        actionMap.OnFoot.Equip.performed += ToolSelect;
        actionMap.OnFoot.Enable();
    }
    private void OnDisable()
    {
        actionMap.OnFoot.Interact.performed -= DoInteract;
        actionMap.OnFoot.Hit.performed -= ToolHit;
        actionMap.OnFoot.Equip.performed -= ToolSelect;
        actionMap.OnFoot.Disable();
    }
    public void ToolSelect(InputAction.CallbackContext obj)
    {
        //Add Limitation if and return!!!
        //Cant switch tool while toolhit!
        toolController.SwitchTool();
    }
    public void ToolHit(InputAction.CallbackContext obj)
    {
        if (pickableObject != null)
        {
            pickableObject.Drop();
            pickableObject = null;
        }
        //Check for Equipped weapon is Axe, if yes, and Tree in Range, Hit tree once
        else if (toolController.equippedToolGO != null && toolController.equippedToolGO.tag == "Axe" &&
        Physics.Raycast(interactRaycastAnker.position, interactRaycastAnker.forward, out raycastHit, interactionDistance))
        {
            if (raycastHit.transform.TryGetComponent<ITreeDamageable>(out ITreeDamageable treeDamageable))
            {
                OnAxeHit?.Invoke();
                StartCoroutine(AxeHit(treeDamageable));
            }
        }
    }
    public IEnumerator AxeHit(ITreeDamageable treeDamageable)
    {
        animator.SetTrigger("AxeHit");
        yield return new WaitForSeconds(0.5f);
        int damageAmount = UnityEngine.Random.Range(10, 30);
        //Add DamagePopupUI here if wanted
        treeDamageable.Damage(damageAmount);
        Debug.Log(damageAmount.ToString());
    }

    public void DoInteract(InputAction.CallbackContext obj)
    {
        //If I have one Object picked up
        if (pickableObject != null)
        {
            pickableObject.Drop();
            pickableObject = null;
        }
        //Nothing picked up, shoot Ray
        else if (Physics.Raycast(interactRaycastAnker.position, interactRaycastAnker.forward, out raycastHit, interactionDistance))
        {
            //If Ray hits Object with Script PickableObject
            if (pickableObject == null && raycastHit.transform.TryGetComponent<PickableObject>(out pickableObject))
            {
                //Pick it up
                animator.SetTrigger("PickUp");
                pickableObject.Grab(objectGrabPointTransform);
            }
            //If Ray hits Object with Script ItemObject...
            else if (raycastHit.transform.TryGetComponent<ItemObject>(out ItemObject item))
            {
                animator.SetTrigger("PickUp");
                item.OnHandlePickupItem();
                //check if has ingredients after picked up item
                eventChannel.RaiseEvent();
            }
            else if (raycastHit.transform.TryGetComponent<ItemRecipe>(out ItemRecipe itemRecipe))
            {
                animator.SetTrigger("PickUp");
                itemRecipe.OnHandlePickupRecipe();
                //check if already has ingredients
                //If have many Recipes and collected many objects before, Bug: Ui wouldnt disappear..
                pickUPUI.SetActive(false);
                raycastHit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
                eventChannel.RaiseEvent();
            }
        }
    }

    private void FixedUpdate()
    {
        if (raycastHit.collider != null)
        {
            pickUPUI.SetActive(false);
            raycastHit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
        }
        //Iff Errors are caused, put this above first if..
        if (pickableObject != null)
        {
            return;
        }
        if (Physics.Raycast(interactRaycastAnker.position, interactRaycastAnker.forward, out raycastHit, interactionDistance, interactionMask))
        {
            pickUPUI.SetActive(true);
            raycastHit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
        }
    }
}
