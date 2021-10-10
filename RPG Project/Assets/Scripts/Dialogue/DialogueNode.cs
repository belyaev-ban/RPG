using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private List<string> children = new List<string>();
        [SerializeField] private Rect editorRect = new Rect(0, 0, 200, 80);
        [SerializeField] private string text;

        public List<string> Children => children;
        public Rect EditorRect => editorRect;

        public string Text
        {
            get => text;
            set
            {
                if (value != text)
                {
                    Undo.RecordObject(this, "Dialogue node updated");
                    text = value;
                    EditorUtility.SetDirty(this); //for some reason Undo does not working correctly in that aspect
                }
            }
        }

#if UNITY_EDITOR
        private void Awake()
        {
            name = "DialogueNode_" + Guid.NewGuid().ToString();
        }

        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Dialogue node moved");
            editorRect.position = newPosition;
            EditorUtility.SetDirty(this); //for some reason Undo does not working correctly in that aspect
        }
        
        public void SetPosition(DialogueNode parentNode)
        {
            SetPosition(new Vector2(parentNode.editorRect.x + parentNode.editorRect.width + 10, parentNode.editorRect.y));
        }

        public void RemoveChild(string nodeName)
        {
            Undo.RecordObject(this, "Removed dialogue link");
            children.Remove(nodeName);
            EditorUtility.SetDirty(this); //for some reason Undo does not working correctly in that aspect
        }

        public void AddChild(string nodeName)
        {
            Undo.RecordObject(this, "Add dialogue link");
            children.Add(nodeName);
            EditorUtility.SetDirty(this); //for some reason Undo does not working correctly in that aspect
        }
#endif
    }
}