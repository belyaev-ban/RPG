using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace Quests
{
    public class QuestList : MonoBehaviour, IPredicateEvaluator
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

        public bool HasQuest(string questName)
        {
            return HasQuest(Quest.GetByName(questName));
        }

        public void CompleteObjective(Quest quest, Quest.Objective objective)
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
        
        private QuestStatus GetQuestStatus(string questName)
        {
            return GetQuestStatus(Quest.GetByName(questName));
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            switch (predicate)
            {
                case "HasQuest": return HasQuest(parameters[0]);
                case "Completed": return GetQuestStatus(parameters[0]).IsComplete();
            }

            return null;
        }
    }
}
