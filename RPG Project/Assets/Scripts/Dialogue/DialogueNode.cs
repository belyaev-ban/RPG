using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        public string uid = Guid.NewGuid().ToString();
        public string text;
        public List<string> children = new List<string>();

        public Rect editorRect = new Rect(0, 0, 200, 80);

        public DialogueNode()
        {
        }

        public DialogueNode(DialogueNode parentNode)
        {
            editorRect.x = parentNode.editorRect.x + parentNode.editorRect.width + 10;
            editorRect.y = parentNode.editorRect.y;
        }
    }
}