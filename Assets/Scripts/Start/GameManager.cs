using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgmAudioSource; // ��� ������ ����� AudioSource
    [SerializeField] private AudioClip bgmClip; // ��� ���� Ŭ��

    void Start()
    {
        // ��� ������ �����Ǿ� ������ ���
        if (bgmAudioSource != null && bgmClip != null)
        {
            bgmAudioSource.clip = bgmClip;
            bgmAudioSource.loop = true; // ���� �ݺ� ��� ����
            bgmAudioSource.Play(); // ���� ��� ����
        }
    }
}
