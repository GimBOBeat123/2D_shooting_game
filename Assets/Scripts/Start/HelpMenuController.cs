using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuController : MonoBehaviour
{
    [SerializeField] private GameObject helpPanel; // 도움말 패널

    private bool isHelpVisible = false; // 도움말 패널이 열려있는지 여부

    void Start()
    {
        helpPanel.SetActive(false); // 시작할 때 도움말 패널을 비활성화
    }

    void Update()
    {
        // ESC 키를 눌렀을 때 도움말 패널이 열려 있으면 닫음
        if (Input.GetKeyDown(KeyCode.Escape) && isHelpVisible)
        {
            ToggleHelpPanel();
        }
    }

    public void ShowHelp()
    {
        isHelpVisible = true;
        helpPanel.SetActive(true); // 패널을 활성화
    }

    public void ToggleHelpPanel()
    {
        isHelpVisible = !isHelpVisible; // 상태 토글
        helpPanel.SetActive(isHelpVisible); // 패널 활성화/비활성화
    }
}
