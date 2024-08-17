using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager 사용을 위한 네임스페이스
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float health = 100f; // 플레이어의 체력
    public float maxHealth = 100f;
    public Image[] HealthBar;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private bool isDead = false; // 플레이어가 죽었는지 확인하는 플래그

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateHealthUI();
    }

    void Update()
    {
        if (isDead) return; // 플레이어가 죽었으면 이동 및 애니메이션 제어를 무시합니다.

        // Input 받기
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // 애니메이션 제어
        if (movement.magnitude > 0)
        {
            animator.SetFloat("Speed", 1f); // Speed 파라미터 설정
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        // 캐릭터 방향 전환
        if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // 좌측
        }
        else if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // 우측
        }
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero; // 플레이어가 죽었으면 물리적 움직임을 차단합니다.
            return; // FixedUpdate에서 더 이상 이동하지 않도록 합니다.
        }

        // 물리적 이동
        rb.velocity = movement * speed;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return; // 플레이어가 죽었으면 피해를 무시합니다.

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
        if (isDead) return; // 이미 죽었으면 처리하지 않음

        isDead = true; // 플레이어 상태를 죽음으로 변경
        animator.SetTrigger("Die"); // Die 애니메이션 트리거

        // 애니메이션 재생 후 씬을 다시 로드합니다.
        StartCoroutine(DieAndReloadScene());
    }

    private IEnumerator DieAndReloadScene()
    {
        // Death 애니메이션 재생 시간 동안 대기
        yield return new WaitForSeconds(3f); // 애니메이션의 길이에 맞게 조정

        // 현재 씬을 다시 로드하여 Start 씬으로 돌아갑니다.
        SceneManager.LoadScene("Start"); // 씬 이름을 "Start"로 설정
    }
}
