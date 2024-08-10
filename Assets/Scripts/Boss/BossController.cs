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

    // BossShooting ��ũ��Ʈ�� ������ ���� �߰�
    private BossShooting bossShooting;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bossShooting = GetComponent<BossShooting>(); // BossShooting ������Ʈ�� ������

        StartCoroutine(AttackRoutine()); // ���� �ڷ�ƾ ����
    }

    void Update()
    {
        if (target != null && !isDead)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < followDistance)
            {
                if (distance > stopDistance && !isAttacking)
                {
                    Vector3 direction = (target.position - transform.position).normalized;
                    transform.position += direction * speed * Time.deltaTime;

                    if (direction.x < 0)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }

                    animator.SetFloat("Speed", speed);
                }
                else
                {
                    animator.SetFloat("Speed", 0);
                }
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            if (!isAttacking && !isDead)
            {
                StartCoroutine(Attack());
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        int attackChoice = Random.Range(0, 4); // 0~3 ������ ����, �� 4���� ����

        animator.SetBool("isAttacking", true);

        switch (attackChoice)
        {
            case 0:
                animator.Play("Attack1");
                // ����1: 360�� �߻�
                StartCoroutine(bossShooting.Shoot360());
                break;
            case 1:
                animator.Play("Attack1");
                // ����2: WavePattern ���
                StartCoroutine(bossShooting.WavePattern());
                break;
            case 2:
                animator.Play("Attack2");
                // ����3: FanPattern ���
                StartCoroutine(bossShooting.FanPattern());
                break;
            case 3:
                animator.Play("Attack2");
                // ����4: HomingPattern ���
                StartCoroutine(bossShooting.RandomSpreadPattern());
                break;
        }

        yield return new WaitForSeconds(0.6f);

        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }


    private IEnumerator Die()
    {
        isDead = true;
        animator.SetTrigger("Death");

        yield return new WaitForSeconds(deathAnimationDuration);

        Destroy(gameObject);
    }
}
