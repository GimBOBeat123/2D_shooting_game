using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // �Ѿ� �ӵ�
    [SerializeField] public float damage = 10f; // �Ѿ��� ������
    private Vector3 shootDirection; // �Ѿ� �߻� ����

    void Update()
    {
        // �Ѿ��� �߻� �������� �̵���Ŵ
        transform.Translate(shootDirection * speed * Time.deltaTime, Space.World);
    }

    public void Initialize(Vector3 direction, float bulletSpeed)
    {
        shootDirection = direction;
        speed = bulletSpeed; // �߻� �ӵ��� �Ѿ��� �ӵ��� ����
    }

    private void Start()
    {
        // �Ѿ� �ʱ�ȭ
        Destroy(gameObject, 2f); // ���� �ð�(��: 2��) �Ŀ� �ڵ����� ����
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ��ü�� ���̾� �Ǵ� �±װ� "Boss" �Ǵ� "Wall"�̸� �Ѿ��� ����
        if (other.CompareTag("Wall") || other.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            if (other.CompareTag("Boss") || other.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                // ������ �Ѿ˰� �浹���� ��, BossController�� �����ͼ� �������� ����
                BossController boss = other.GetComponent<BossController>();
                if (boss != null)
                {
                    boss.TakeDamage(damage); // ������ ����
                }
            }

            Destroy(gameObject); // �Ѿ� ����
        }
    }

    public void SetDamageMultiplier(float multiplier)
    {
        damage *= multiplier; // ������ ���� ����
    }
}
