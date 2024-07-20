using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public Transform target; // 캐릭터의 Transform
    [SerializeField] public float speed = 2f; // 보스의 이동 속도
    [SerializeField] public float followDistance = 10f; // 보스가 캐릭터를 추격하는 최대 거리
    [SerializeField] public float stopDistance = 5f; // 보스가 캐릭터와 일정 거리 이상일 때 이동을 멈추는 거리
    [SerializeField] public float health = 100f; // 보스 몬스터의 체력
    [SerializeField] public float deathAnimationDuration = 1f; // Death 애니메이션의 지속 시간 (초)

    private Animator animator;
    private Rigidbody2D rb;
    private bool isAttacking = false; // 공격 중인지 여부
    private bool attackChoice = false; // Attack1 또는 Attack2 선택 여부
    private bool isDead = false; // 보스가 죽었는지 여부

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(AttackRoutine()); // 공격 코루틴 시작
    }

    void Update()
    {
        if (target != null && !isDead)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < followDistance)
            {
                // 거리 조건에 따라 이동 여부 결정
                if (distance > stopDistance && !isAttacking) // stopDistance 이상일 때만 이동
                {
                    // 보스 몬스터가 캐릭터를 향해 이동하도록 설정
                    Vector3 direction = (target.position - transform.position).normalized;
                    transform.position += direction * speed * Time.deltaTime;

                    // 보스 몬스터의 스프라이트가 거꾸로 돌아가지 않도록 설정
                    if (direction.x < 0)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }

                    // Speed 파라미터를 설정하여 Run 애니메이션 실행
                    animator.SetFloat("Speed", speed);
                }
                else
                {
                    // stopDistance 이하일 때는 이동하지 않고 Idle 상태로 설정
                    animator.SetFloat("Speed", 0);
                }
            }
            else
            {
                // followDistance를 벗어나면 Idle 상태로 설정
                animator.SetFloat("Speed", 0);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount; // 체력 감소
        if (health <= 0)
        {
            StartCoroutine(Die()); // Death 애니메이션 시작
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // 2초 대기

            if (!isAttacking && !isDead) // 공격 중이 아닐 때만 공격 실행
            {
                StartCoroutine(Attack());
                yield return new WaitForSeconds(1f); // 공격 애니메이션이 끝날 때까지 대기
            }
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true; // 공격 시작
        attackChoice = Random.value > 0.5f; // Attack1 또는 Attack2 선택

        animator.SetBool("isAttacking", true); // 공격 애니메이션 활성화

        if (attackChoice)
        {
            animator.Play("Attack1");
        }
        else
        {
            animator.Play("Attack2");
        }

        // 공격 애니메이션이 끝날 때까지 대기
        yield return new WaitForSeconds(0.6f); // 공격 애니메이션이 완료되는 시간을 맞추세요.

        isAttacking = false; // 공격 종료
        animator.SetBool("isAttacking", false); // 공격 애니메이션 비활성화

        // 이동 애니메이션 설정
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < followDistance)
        {
            if (distance > stopDistance)
            {
                animator.SetFloat("Speed", speed); // 이동 애니메이션 활성화
            }
            else
            {
                animator.SetFloat("Speed", 0); // stopDistance 이하일 때 Idle 상태
            }
        }
        else
        {
            animator.SetFloat("Speed", 0); // followDistance를 벗어난 경우 Idle 상태
        }
    }

    private IEnumerator Die()
    {
        isDead = true; // 보스가 죽었음을 표시
        animator.SetTrigger("Death"); // Death 애니메이션 재생

        // Death 애니메이션이 끝날 때까지 대기
        yield return new WaitForSeconds(deathAnimationDuration);

        // 보스 몬스터 제거
        Destroy(gameObject);
    }
}
