using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    [System.Serializable]
    public class QuestStatus
    {
        [SerializeField] private Quest quest;
        [SerializeField] private List<string> completedObjectives;

        public Quest GetQuest()
        {
            return quest;
        }

        public int CountCompletedObjectives()
        {
            return completedObjectives.Count;
        }

        public bool IsObjectiveComplete(string objective)
        {
            return completedObjectives.Contains(objective);
        }
    }
}
