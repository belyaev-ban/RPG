using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] private QuestItemUI questPrefab;
    private QuestList _questList;

    private void Start()
    {
        _questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        _questList.QuestListUpdated += UpdateUI;
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        
        foreach (QuestStatus questStatus in _questList.GetStatuses())
        {
            QuestItemUI iuInstance = Instantiate<QuestItemUI>(questPrefab, transform);
            iuInstance.Setup(questStatus);
        }
    }
}
