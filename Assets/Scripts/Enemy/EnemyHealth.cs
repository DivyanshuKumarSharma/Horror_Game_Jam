using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    public float currentHealth;

    [Header("Health Bar Settings")]
    [SerializeField] private GameObject healthBarPrefab;
    private Transform healthBarTransform;
    private Vector3 initialHealthBarScale;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        InitializeHealthBar();
        UpdateHealthBar();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }

    private void InitializeHealthBar()
    {
        if (healthBarPrefab != null)
        {
            GameObject healthBarInstance = Instantiate(healthBarPrefab, transform);
            healthBarTransform = healthBarInstance.transform;
            healthBarTransform.localPosition = new Vector3(0, 1.5f, 0); // Adjust as needed
            initialHealthBarScale = healthBarTransform.localScale;
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Enemy takes damage: " + damage);
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarTransform != null)
        {
            float healthPercentage = currentHealth / maxHealth;
            Vector3 newScale = initialHealthBarScale;
            newScale.x *= healthPercentage;
            healthBarTransform.localScale = newScale;
        }
    }

    private void Die()
    {
        // Handle enemy death here (e.g., play animation, destroy game object)
        if (animator != null)
        {
            animator.SetTrigger("Dead");
        }
        Destroy(gameObject, 1f); // Adjust delay as needed
    }
}
