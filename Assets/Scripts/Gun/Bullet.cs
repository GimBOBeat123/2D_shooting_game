using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // 총알 속도
    [SerializeField] public float damage = 10f; // 총알의 데미지
    private Vector3 shootDirection; // 총알 발사 방향

    void Update()
    {
        // 총알을 발사 방향으로 이동시킴
        transform.Translate(shootDirection * speed * Time.deltaTime, Space.World);
    }

    public void Initialize(Vector3 direction, float bulletSpeed)
    {
        shootDirection = direction;
        speed = bulletSpeed; // 발사 속도를 총알의 속도로 설정
    }

    private void Start()
    {
        // 총알 초기화
        Destroy(gameObject, 2f); // 일정 시간(예: 2초) 후에 자동으로 제거
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 객체의 레이어 또는 태그가 "Boss" 또는 "Wall"이면 총알을 제거
        if (other.CompareTag("Wall") || other.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            if (other.CompareTag("Boss") || other.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                // 보스가 총알과 충돌했을 때, BossController를 가져와서 데미지를 적용
                BossController boss = other.GetComponent<BossController>();
                if (boss != null)
                {
                    boss.TakeDamage(damage); // 데미지 적용
                }
            }

            Destroy(gameObject); // 총알 제거
        }
    }

    public void SetDamageMultiplier(float multiplier)
    {
        damage *= multiplier; // 데미지 배율 적용
    }
}
