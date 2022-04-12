using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG Project/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] private List<Objective> objectives = new List<Objective>();
        [SerializeField] private List<Reward> rewards = new List<Reward>();

        [System.Serializable]
        public class Reward
        {
            [Min(1)] 
            public int amount;
            public InventoryItem item;
        }
        
        [System.Serializable]
        public class Objective
        {
            public string reference;
            public string description;
        }
        
        public string GetTitle()
        {
            return name;
        }

        public IEnumerable<Objective> GetObjectives()
        {
            return objectives;
        }

        public int GetObjectiveCount()
        {
            return objectives.Count;
        }

        public bool HasObjective(string objectiveReference)
        {
            foreach (Objective objective in objectives)
            {
                if (objectiveReference == objective.reference)
                {
                    return true;
                }
            }

            return false;
        }


        public IEnumerable<Reward> GetRewards()
        {
            return rewards;
        }

        public static Quest GetByName(string questName)
        {
            foreach (Quest quest in Resources.LoadAll<Quest>(""))
            {
                if (quest.name == questName)
                {
                    return quest;
                }
            }

            return null;
        }
    }
}
