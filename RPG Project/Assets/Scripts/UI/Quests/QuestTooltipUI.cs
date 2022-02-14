using System.Collections;
using System.Collections.Generic;
using Quests;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestTooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Transform objectiveContainer;
    [FormerlySerializedAs("objectivePrefab")] [SerializeField] private GameObject objectiveCompletePrefab;
    [SerializeField] private GameObject objectiveIncompletePrefab;
    
    public void Setup(QuestStatus questStatus)
    {
        Quest quest = questStatus.GetQuest();
        
        title.text = quest.GetTitle();
        objectiveContainer.DetachChildren();

        foreach (string objective in quest.GetObjectives())
        {
            GameObject objectivePrefab = questStatus.IsObjectiveComplete(objective)
                ? objectiveCompletePrefab
                : objectiveIncompletePrefab;
            
            GameObject objectiveInstance = Instantiate(objectivePrefab, objectiveContainer);
            TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
            objectiveText.text = objective;
        }
    }
}
