using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] private Quest[] tempQuests;
    [SerializeField] private QuestItemUI questPrefab;

    void Start()
    {
        transform.DetachChildren();
        
        foreach (Quest tempQuest in tempQuests)
        {
            QuestItemUI iuInstance = Instantiate<QuestItemUI>(questPrefab, transform);
            iuInstance.Setup(tempQuest);
        }
    }
}
