using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager ����� ���� ���ӽ����̽�
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float health = 100f; // �÷��̾��� ü��
    public float maxHealth = 100f;
    public Image[] HealthBar;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private bool isDead = false; // �÷��̾ �׾����� Ȯ���ϴ� �÷���

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateHealthUI();
    }

    void Update()
    {
        if (isDead) return; // �÷��̾ �׾����� �̵� �� �ִϸ��̼� ��� �����մϴ�.

        // Input �ޱ�
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // �ִϸ��̼� ����
        if (movement.magnitude > 0)
        {
            animator.SetFloat("Speed", 1f); // Speed �Ķ���� ����
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        // ĳ���� ���� ��ȯ
        if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // ����
        }
        else if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // ����
        }
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = Vector2.zero; // �÷��̾ �׾����� ������ �������� �����մϴ�.
            return; // FixedUpdate���� �� �̻� �̵����� �ʵ��� �մϴ�.
        }

        // ������ �̵�
        rb.velocity = movement * speed;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return; // �÷��̾ �׾����� ���ظ� �����մϴ�.

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
        if (isDead) return; // �̹� �׾����� ó������ ����

        isDead = true; // �÷��̾� ���¸� �������� ����
        animator.SetTrigger("Die"); // Die �ִϸ��̼� Ʈ����

        // �ִϸ��̼� ��� �� ���� �ٽ� �ε��մϴ�.
        StartCoroutine(DieAndReloadScene());
    }

    private IEnumerator DieAndReloadScene()
    {
        // Death �ִϸ��̼� ��� �ð� ���� ���
        yield return new WaitForSeconds(3f); // �ִϸ��̼��� ���̿� �°� ����

        // ���� ���� �ٽ� �ε��Ͽ� Start ������ ���ư��ϴ�.
        SceneManager.LoadScene("Start"); // �� �̸��� "Start"�� ����
    }
}
