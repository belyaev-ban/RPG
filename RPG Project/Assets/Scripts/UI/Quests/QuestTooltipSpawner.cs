using System.Collections;
using System.Collections.Generic;
using GameDevTV.Core.UI.Tooltips;
using Quests;
using UnityEngine;

public class QuestTooltipSpawner : TooltipSpawner
{
    public override void UpdateTooltip(GameObject tooltip)
    {
        Quest quest = GetComponent<QuestItemUI>().GetQuest();
        tooltip.GetComponent<QuestTooltipUI>().Setup(quest);
    }

    public override bool CanCreateTooltip()
    {
        return true;
    }
}