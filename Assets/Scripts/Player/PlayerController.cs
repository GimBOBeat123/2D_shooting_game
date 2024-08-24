using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float health = 100f;
    public float maxHealth = 100f;

    public Image[] HealthBar;
    public Image healCooldownImage; // 체력 회복 스킬 쿨타임 UI
    public Image speedBoostCooldownImage; // 이동 속도 증가 스킬 쿨타임 UI

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private bool isDead = false;

    private bool isHealReady = true;
    private bool isSpeedBoostReady = true;

    public float healAmount = 20f;
    public float healCooldown = 10f;
    public float speedBoostMultiplier = 2f;
    public float speedBoostDuration = 5f;
    public float speedBoostCooldown = 15f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateHealthUI();
        // 초기 색상 설정
        SetCooldownImageAlpha(healCooldownImage, 1f);
        SetCooldownImageAlpha(speedBoostCooldownImage, 1f);
    }

    private void Update()
    {
        if (isDead) return;

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        if (movement.magnitude > 0)
        {
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Q) && isHealReady)
        {
            StartCoroutine(ActivateHeal());
        }

        if (Input.GetKeyDown(KeyCode.E) && isSpeedBoostReady)
        {
            StartCoroutine(ActivateSpeedBoost());
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        rb.velocity = movement * speed;
    }

    private IEnumerator ActivateHeal()
    {
        isHealReady = false;
        HealPlayer();
        yield return StartCoroutine(CooldownRoutine(healCooldown, healCooldownImage));
        isHealReady = true;
    }

    private void HealPlayer()
    {
        health = Mathf.Min(maxHealth, health + healAmount);
        UpdateHealthUI();
    }

    private IEnumerator ActivateSpeedBoost()
    {
        isSpeedBoostReady = false;
        float originalSpeed = speed;
        speed *= speedBoostMultiplier;

        yield return new WaitForSeconds(speedBoostDuration);

        speed = originalSpeed;
        yield return StartCoroutine(CooldownRoutine(speedBoostCooldown, speedBoostCooldownImage));
        isSpeedBoostReady = true;
    }

    private IEnumerator CooldownRoutine(float cooldownTime, Image cooldownImage)
    {
        float elapsed = 0f;
        SetCooldownImageAlpha(cooldownImage, 0.5f); // 쿨타임 동안 반투명

        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime;
            cooldownImage.fillAmount = Mathf.Clamp01(1 - (elapsed / cooldownTime));
            yield return null;
        }

        cooldownImage.fillAmount = 1f;
        SetCooldownImageAlpha(cooldownImage, 1f); // 쿨타임 종료 후 완전히 보이게
    }

    private void SetCooldownImageAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            UpdateHealthUI();
        }
    }

    public void UpdateHealthUI()
    {
        float fillAmount = Mathf.Clamp01(health / maxHealth);
        foreach (var healthBar in HealthBar)
        {
            healthBar.fillAmount = fillAmount;
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Die");
        StartCoroutine(DieAndReloadScene());
    }

    private IEnumerator DieAndReloadScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Start");
    }
}
