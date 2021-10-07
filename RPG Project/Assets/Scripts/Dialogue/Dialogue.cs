using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        public Vector2 windowSize = new Vector2(4000, 4000);
        
        [SerializeField] private List<DialogueNode> nodes = new List<DialogueNode>();
        
        private Dictionary<string, DialogueNode> _nodeLookup = new Dictionary<string, DialogueNode>();

        private void Awake()
        {
            //otherwise wont work in exported game
            OnValidate();
        }


        private void OnValidate()
        {
            if (nodes.Count == 0)
            {
                CreateNode(null);
            }
            
            _nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes())
            {
                _nodeLookup[node.name] = node;
            }
        }


        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (string childName in parentNode.Children)
            {
                if (_nodeLookup.ContainsKey(childName))
                {
                    yield return _nodeLookup[childName];
                }
            }
        }

#if UNITY_EDITOR
        public void CreateNode(DialogueNode parentNode)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            Undo.RegisterCreatedObjectUndo(newNode, "Created DialogueNode");
            if (parentNode != null)
            {
                parentNode.AddChild(newNode.name);
                newNode.SetPosition(parentNode);
            }
            Undo.RecordObject(this, "Added DialogueNode");
            nodes.Add(newNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToBeDeleted)
        {
            Undo.RecordObject(this, "Deleted DialogueNode");
            nodes.Remove(nodeToBeDeleted);
            OnValidate();
            CleanDanglingChildren(nodeToBeDeleted);
            Undo.DestroyObjectImmediate(nodeToBeDeleted);
        }

        private void CleanDanglingChildren(DialogueNode nodeToBeDeleted)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToBeDeleted.name);
            }
        }
#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode dialogueNode in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(dialogueNode) == "")
                    {
                        AssetDatabase.AddObjectToAsset(dialogueNode, this);
                    }
                }    
            }
#endif
        }

        public void OnAfterDeserialize()
        {
            
        }
    }
}
