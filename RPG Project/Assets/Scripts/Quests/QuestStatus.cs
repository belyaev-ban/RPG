using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    public class QuestStatus
    {
        private Quest _quest;
        private List<string> _completedObjectives = new List<string>();

        public QuestStatus(Quest newQuest)
        {
            _quest = newQuest;
        }

        public Quest GetQuest()
        {
            return _quest;
        }

        public int CountCompletedObjectives()
        {
            return _completedObjectives.Count;
        }

        public bool IsObjectiveComplete(string objective)
        {
            return _completedObjectives.Contains(objective);
        }

        public void CompleteObjective(string objective)
        {
            if (_quest.HasObjective(objective) && !_completedObjectives.Contains(objective))
            {
                _completedObjectives.Add(objective);
            }
        }
    }
}
