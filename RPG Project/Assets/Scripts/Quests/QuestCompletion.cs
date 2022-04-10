using UnityEngine;

namespace Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] private Quest quest;
        [SerializeField] private Quest.Objective questObjective;

        public void CompleteObjective()
        {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();

            questList.CompleteObjective(quest, questObjective);
        }
    }
}