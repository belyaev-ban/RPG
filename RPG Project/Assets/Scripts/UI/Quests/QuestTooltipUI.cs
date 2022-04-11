using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Quests;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestTooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Transform objectiveContainer;
    [SerializeField] private TextMeshProUGUI rewardsContainer;
    [FormerlySerializedAs("objectivePrefab")] [SerializeField] private GameObject objectiveCompletePrefab;
    [SerializeField] private GameObject objectiveIncompletePrefab;

    public void Setup(QuestStatus questStatus)
    {
        Quest quest = questStatus.GetQuest();
        
        title.text = quest.GetTitle();
        objectiveContainer.DetachChildren();

        foreach (Quest.Objective objective in quest.GetObjectives())
        {
            GameObject objectivePrefab = questStatus.IsObjectiveComplete(objective.reference)
                ? objectiveCompletePrefab
                : objectiveIncompletePrefab;
            
            GameObject objectiveInstance = Instantiate(objectivePrefab, objectiveContainer);
            TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
            objectiveText.text = objective.description;
        }

        List<string> rewards = new List<string>();
        foreach (Quest.Reward reward in quest.GetRewards())
        {
            if (reward.amount > 1)
            {
                rewards.Add($"{reward.item.GetDisplayName()} x{reward.amount}");
            }
            else
            {
                rewards.Add($"{reward.item.GetDisplayName()}");
            }
        }

        rewardsContainer.text = String.Join(", ", rewards);
    }
}
