using System.Collections;
using System.Collections.Generic;
using Quests;
using TMPro;
using UnityEngine;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI progress;

    private QuestStatus _questStatus;
    
    public void Setup(QuestStatus questStatus)
    {
        Quest quest = questStatus.GetQuest();
        
        title.text = quest.GetTitle();
        progress.text = questStatus.CountCompletedObjectives() + "/" + quest.GetObjectiveCount();

        _questStatus = questStatus;
    }

    public QuestStatus GetQuestStatus()
    {
        return _questStatus;
    }
}
