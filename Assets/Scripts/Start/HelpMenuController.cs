using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuController : MonoBehaviour
{
    [SerializeField] private GameObject helpPanel; // ���� �г�

    private bool isHelpVisible = false; // ���� �г��� �����ִ��� ����

    void Start()
    {
        helpPanel.SetActive(false); // ������ �� ���� �г��� ��Ȱ��ȭ
    }

    void Update()
    {
        // ESC Ű�� ������ �� ���� �г��� ���� ������ ����
        if (Input.GetKeyDown(KeyCode.Escape) && isHelpVisible)
        {
            ToggleHelpPanel();
        }
    }

    public void ShowHelp()
    {
        isHelpVisible = true;
        helpPanel.SetActive(true); // �г��� Ȱ��ȭ
    }

    public void ToggleHelpPanel()
    {
        isHelpVisible = !isHelpVisible; // ���� ���
        helpPanel.SetActive(isHelpVisible); // �г� Ȱ��ȭ/��Ȱ��ȭ
    }
}
