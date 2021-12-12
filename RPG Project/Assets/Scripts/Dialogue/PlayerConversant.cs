using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private Dialogue currentDialogue;

        public string GetText()
        {
            if (currentDialogue is null)
            {
                return "";
            }
            
            return currentDialogue.GetRootNode().Text;
        }
    }

}