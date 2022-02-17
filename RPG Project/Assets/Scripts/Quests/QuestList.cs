using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    public class QuestList : MonoBehaviour
    {
        private List<QuestStatus> _statuses = new List<QuestStatus>();

        public event Action QuestListUpdated;

        public void OnQuestListUpdated()
        {
            QuestListUpdated?.Invoke();
        }

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return _statuses;
        }

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest))
            {
                return;
            }
            
            QuestStatus newStatus = new QuestStatus(quest);
            _statuses.Add(newStatus);
            
            OnQuestListUpdated();
        }

        public bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            QuestStatus questStatus = GetQuestStatus(quest);
            questStatus?.CompleteObjective(objective);
            
            OnQuestListUpdated();
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus questStatus in _statuses)
            {
                if (questStatus.GetQuest() == quest)
                {
                    return questStatus;
                }
            }

            return null;
        }
    }
}
