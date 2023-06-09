using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour, ITreeDamageable
{
    public static PlayerStats Instance { get; private set; }
    private float healthAmount = 100f;
    private float hungerAmount = 0;
    private float maxHunger = 100f;
    private Animator animator;
    private ThirdPersonController controller;
    [SerializeField] private UiStats uiStats;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        controller = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
        hungerAmount = 0;
    }
    public void Eat(float amount)
    {
        hungerAmount -= amount;
    }
    public void Damage(int amount)
    {
        healthAmount -= amount;
    }
    public void Update()
    {
        if (healthAmount <= 0)
        {
            animator.SetBool("Die", true);
            controller.jumpForce = 0f;
            controller.movementForce = 0f;
            controller.maxSpeed = 0f;
            uiStats.HealthSlider.value = 0f;
            StartCoroutine(LoadLostScene());
            return;
        }
        if (hungerAmount < maxHunger)
        {
            hungerAmount += Time.deltaTime * .5f;
        }
        if(hungerAmount > 70 && healthAmount > 0)
        {
            healthAmount -= Time.deltaTime * 0.2f * hungerAmount * 0.2f;
        }
        if(hungerAmount < 30 && healthAmount < 100)
        {
            healthAmount += Time.deltaTime * 0.5f;
        }
        uiStats.HealthSlider.value = healthAmount;
        uiStats.HungerSlider.value = hungerAmount;
    }
    private IEnumerator LoadLostScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Lost");
    }
}
