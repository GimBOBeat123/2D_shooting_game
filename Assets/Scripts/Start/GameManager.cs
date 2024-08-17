using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgmAudioSource; // 배경 음악을 재생할 AudioSource
    [SerializeField] private AudioClip bgmClip; // 배경 음악 클립

    void Start()
    {
        // 배경 음악이 설정되어 있으면 재생
        if (bgmAudioSource != null && bgmClip != null)
        {
            bgmAudioSource.clip = bgmClip;
            bgmAudioSource.loop = true; // 음악 반복 재생 설정
            bgmAudioSource.Play(); // 음악 재생 시작
        }
    }
}
