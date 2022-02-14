using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    public class QuestList : MonoBehaviour
    {
        [SerializeField] private QuestStatus[] statuses;

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }
    }
}
