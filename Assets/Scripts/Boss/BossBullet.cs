using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float damage = 10f; // �Ѿ��� �ִ� ���ط�

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject); // �Ѿ� ����
        }

        // �Ѿ��� �÷��̾�� �浹�ߴ��� Ȯ��
        if (other.CompareTag("Player"))
        {
            // �÷��̾� ������Ʈ���� PlayerController ������Ʈ�� ������
            PlayerController player = other.GetComponent<PlayerController>();

            // �÷��̾ ������ ���ظ� ��
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // �Ѿ��� �÷��̾�� �浹 �� ����
            Destroy(gameObject);
        }
    }
}
