using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    public class QuestStatus
    {
        private Quest _quest;
        private List<string> _completedObjectivesReferences = new List<string>();

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
            return _completedObjectivesReferences.Count;
        }

        public bool IsObjectiveComplete(string objectiveReference)
        {
            return _completedObjectivesReferences.Contains(objectiveReference);
        }

        public void CompleteObjective(Quest.Objective objective)
        {
            if (_quest.HasObjective(objective.reference) && !_completedObjectivesReferences.Contains(objective.reference))
            {
                _completedObjectivesReferences.Add(objective.reference);
            }
        }
    }
}
