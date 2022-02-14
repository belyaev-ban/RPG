using System.Collections;
using System.Collections.Generic;
using GameDevTV.Core.UI.Tooltips;
using Quests;
using UnityEngine;

public class QuestTooltipSpawner : TooltipSpawner
{
    public override void UpdateTooltip(GameObject tooltip)
    {
        QuestStatus questStatus = GetComponent<QuestItemUI>().GetQuestStatus();
        tooltip.GetComponent<QuestTooltipUI>().Setup(questStatus);
    }

    public override bool CanCreateTooltip()
    {
        return true;
    }
}