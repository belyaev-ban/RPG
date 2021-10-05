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

        public Dialogue()
        { }

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

        public void CreateNode(DialogueNode parentNode)
        {
            DialogueNode newNode = new DialogueNode(parentNode);
            parentNode.children.Add(newNode.uid);
            nodes.Add(newNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToBeDeleted)
        {
            nodes.Remove(nodeToBeDeleted);
            OnValidate();
            CleanDanglingChildren(nodeToBeDeleted);
        }

        private void CleanDanglingChildren(DialogueNode nodeToBeDeleted)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.children.Remove(nodeToBeDeleted.uid);
            }
        }
    }
}
