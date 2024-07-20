using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public Transform target; // ĳ������ Transform
    [SerializeField] public float speed = 2f; // ������ �̵� �ӵ�
    [SerializeField] public float followDistance = 10f; // ������ ĳ���͸� �߰��ϴ� �ִ� �Ÿ�
    [SerializeField] public float stopDistance = 5f; // ������ ĳ���Ϳ� ���� �Ÿ� �̻��� �� �̵��� ���ߴ� �Ÿ�
    [SerializeField] public float health = 100f; // ���� ������ ü��
    [SerializeField] public float deathAnimationDuration = 1f; // Death �ִϸ��̼��� ���� �ð� (��)

    private Animator animator;
    private Rigidbody2D rb;
    private bool isAttacking = false; // ���� ������ ����
    private bool attackChoice = false; // Attack1 �Ǵ� Attack2 ���� ����
    private bool isDead = false; // ������ �׾����� ����

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(AttackRoutine()); // ���� �ڷ�ƾ ����
    }

    void Update()
    {
        if (target != null && !isDead)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < followDistance)
            {
                // �Ÿ� ���ǿ� ���� �̵� ���� ����
                if (distance > stopDistance && !isAttacking) // stopDistance �̻��� ���� �̵�
                {
                    // ���� ���Ͱ� ĳ���͸� ���� �̵��ϵ��� ����
                    Vector3 direction = (target.position - transform.position).normalized;
                    transform.position += direction * speed * Time.deltaTime;

                    // ���� ������ ��������Ʈ�� �Ųٷ� ���ư��� �ʵ��� ����
                    if (direction.x < 0)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }

                    // Speed �Ķ���͸� �����Ͽ� Run �ִϸ��̼� ����
                    animator.SetFloat("Speed", speed);
                }
                else
                {
                    // stopDistance ������ ���� �̵����� �ʰ� Idle ���·� ����
                    animator.SetFloat("Speed", 0);
                }
            }
            else
            {
                // followDistance�� ����� Idle ���·� ����
                animator.SetFloat("Speed", 0);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount; // ü�� ����
        if (health <= 0)
        {
            StartCoroutine(Die()); // Death �ִϸ��̼� ����
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // 2�� ���

            if (!isAttacking && !isDead) // ���� ���� �ƴ� ���� ���� ����
            {
                StartCoroutine(Attack());
                yield return new WaitForSeconds(1f); // ���� �ִϸ��̼��� ���� ������ ���
            }
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true; // ���� ����
        attackChoice = Random.value > 0.5f; // Attack1 �Ǵ� Attack2 ����

        animator.SetBool("isAttacking", true); // ���� �ִϸ��̼� Ȱ��ȭ

        if (attackChoice)
        {
            animator.Play("Attack1");
        }
        else
        {
            animator.Play("Attack2");
        }

        // ���� �ִϸ��̼��� ���� ������ ���
        yield return new WaitForSeconds(0.6f); // ���� �ִϸ��̼��� �Ϸ�Ǵ� �ð��� ���߼���.

        isAttacking = false; // ���� ����
        animator.SetBool("isAttacking", false); // ���� �ִϸ��̼� ��Ȱ��ȭ

        // �̵� �ִϸ��̼� ����
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < followDistance)
        {
            if (distance > stopDistance)
            {
                animator.SetFloat("Speed", speed); // �̵� �ִϸ��̼� Ȱ��ȭ
            }
            else
            {
                animator.SetFloat("Speed", 0); // stopDistance ������ �� Idle ����
            }
        }
        else
        {
            animator.SetFloat("Speed", 0); // followDistance�� ��� ��� Idle ����
        }
    }

    private IEnumerator Die()
    {
        isDead = true; // ������ �׾����� ǥ��
        animator.SetTrigger("Death"); // Death �ִϸ��̼� ���

        // Death �ִϸ��̼��� ���� ������ ���
        yield return new WaitForSeconds(deathAnimationDuration);

        // ���� ���� ����
        Destroy(gameObject);
    }
}
