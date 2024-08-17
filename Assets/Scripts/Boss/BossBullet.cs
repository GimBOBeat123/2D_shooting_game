using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float damage = 10f; // 총알이 주는 피해량

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject); // 총알 제거
        }

        // 총알이 플레이어와 충돌했는지 확인
        if (other.CompareTag("Player"))
        {
            // 플레이어 오브젝트에서 PlayerController 컴포넌트를 가져옴
            PlayerController player = other.GetComponent<PlayerController>();

            // 플레이어가 있으면 피해를 줌
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // 총알이 플레이어와 충돌 후 제거
            Destroy(gameObject);
        }
    }
}
