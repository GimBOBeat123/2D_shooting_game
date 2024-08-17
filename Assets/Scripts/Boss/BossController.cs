using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위한 네임스페이스
using UnityEngine.UI;
using TMPro;

public class BossController : MonoBehaviour
{
    public Transform target; // 캐릭터의 Transform
    [SerializeField] public float speed = 2f; // 보스의 이동 속도
    [SerializeField] public float followDistance = 10f; // 보스가 캐릭터를 추격하는 최대 거리
    [SerializeField] public float stopDistance = 5f; // 보스가 캐릭터와 일정 거리 이상일 때 이동을 멈추는 거리
    [SerializeField] public float deathAnimationDuration = 1f; // Death 애니메이션의 지속 시간 (초)

    [SerializeField] public float health = 100f; // 보스 몬스터의 체력
    [SerializeField] public float maxHealth = 100f; // 보스 몬스터의 최대 체력
    [SerializeField] public Image healthBar; // 체력 바 이미지
    [SerializeField] public TextMeshProUGUI heatlhText; // 체력 숫자 표시

    private Animator animator;
    private Rigidbody2D rb;
    private bool isAttacking = false; // 공격 중인지 여부
    private bool attackChoice = false; // Attack1 또는 Attack2 선택 여부
    private bool isDead = false; // 보스가 죽었는지 여부

    // BossShooting 스크립트를 참조할 변수 추가
    private BossShooting bossShooting;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        animator = GetComponent<Animator>();
        bossShooting = GetComponent<BossShooting>(); // BossShooting 컴포넌트를 가져옴

        StartCoroutine(AttackRoutine()); // 공격 코루틴 시작
        UpdateHealthBar();
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
        UpdateHealthBar(); // 체력 감소 Ui 업데이트

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

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            // 체력바의 Fill Amount를 체력에 맞게 업데이트
            healthBar.fillAmount = Mathf.Clamp01(health / maxHealth);
        }

        if (heatlhText != null)
        {
            // 체력 텍스트를 현재 체력에 맞게 업데이트
            heatlhText.text = $"{Mathf.CeilToInt(health)} / {Mathf.CeilToInt(maxHealth)}";
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        int attackChoice = Random.Range(0, 4); // 0~3 범위로 변경, 총 4가지 패턴

        animator.SetBool("isAttacking", true);

        switch (attackChoice)
        {
            case 0:
                animator.Play("Attack1");
                // 패턴1: 360도 발사
                StartCoroutine(bossShooting.Shoot360());
                break;
            case 1:
                animator.Play("Attack1");
                // 패턴2: WavePattern 사용
                StartCoroutine(bossShooting.WavePattern());
                break;
            case 2:
                animator.Play("Attack2");
                // 패턴3: FanPattern 사용
                StartCoroutine(bossShooting.FanPattern());
                break;
            case 3:
                animator.Play("Attack2");
                // 패턴4: RandomSpreadPattern 사용
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

        yield return new WaitForSeconds(3f);

        // Start 씬으로 전환
        SceneManager.LoadScene("Start"); // 씬 이름을 "Start"로 설정
    }
}
