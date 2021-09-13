using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private List<DialogueNode> nodes = new List<DialogueNode>();
        public DialogueNode rootNode = new DialogueNode();

        private void Awake()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                nodes.Add(rootNode);
            }
#endif
        }


        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }
    }
}
