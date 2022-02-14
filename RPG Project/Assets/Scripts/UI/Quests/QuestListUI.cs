using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] private QuestItemUI questPrefab;

    void Start()
    {
        transform.DetachChildren();

        QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        
        foreach (QuestStatus questStatus in questList.GetStatuses())
        {
            QuestItemUI iuInstance = Instantiate<QuestItemUI>(questPrefab, transform);
            iuInstance.Setup(questStatus);
        }
    }
}
