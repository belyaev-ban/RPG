using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG Project/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] private string[] objectives;

        public string GetTitle()
        {
            return name;
        }

        public IEnumerable<string> GetObjectives()
        {
            return objectives;
        }

        public int GetObjectiveCount()
        {
            return objectives.Length;
        }
    }
}
