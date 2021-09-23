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
        
        private Dictionary<string, DialogueNode> _nodeLookup = new Dictionary<string, DialogueNode>();

        private void Awake()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                nodes.Add(new DialogueNode());
            }
#endif
            
            //otherwise wont work in exported game
            OnValidate();
        }


        private void OnValidate()
        {
            _nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes())
            {
                _nodeLookup[node.uid] = node;
            }
        }


        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (string childUid in parentNode.children)
            {
                if (_nodeLookup.ContainsKey(childUid))
                {
                    yield return _nodeLookup[childUid];
                }
            }
        }
    }
}
