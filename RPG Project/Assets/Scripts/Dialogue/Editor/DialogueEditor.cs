using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private Dialogue _selectedDialogue = null;
        private GUIStyle _nodeStyle;
        private DialogueNode _draggedNode = null;
        private Vector2 _dragOffset;
        
        [MenuItem("Window/Dialogue/Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<DialogueEditor>();
            window.titleContent = new GUIContent("Dialogue Editor");
            window.Show();
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (obj is null)
            {
                return false;
            }
            ShowWindow();
            
            return true;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            _nodeStyle = new GUIStyle();
            _nodeStyle.normal.background = Texture2D.grayTexture;
            _nodeStyle.padding = new RectOffset(5, 5, 10, 10);
        }


        private void OnSelectionChanged()
        {
            var obj = Selection.activeObject as Dialogue;
            if (obj != null)
            {
                _selectedDialogue = obj;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (_selectedDialogue is null)
            {
                EditorGUILayout.LabelField("No dialogue selected");
                return;
            }

            ProcessEvents();
            foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
            {
                DrawNode(node);
            }
            foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
            {
                DrawConnections(node);
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            float bezierMultiplier = 0.8f;
            
            foreach (DialogueNode childNode in _selectedDialogue.GetAllChildren(node))
            {
                Vector3 startPosition, endPosition, startTangent, endTangent;
                startPosition = new Vector3(node.editorRect.xMax, node.editorRect.center.y);
                endPosition = new Vector3(childNode.editorRect.xMin, childNode.editorRect.center.y);

                startTangent = new Vector3(startPosition.x - (startPosition.x - endPosition.x) * bezierMultiplier,
                    startPosition.y);
                endTangent = new Vector3(endPosition.x - (endPosition.x - startPosition.x) * bezierMultiplier,
                    endPosition.y);

                Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, Color.white, null, 4f);
            }
        }


        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && _draggedNode is null)
            {
                _draggedNode = GetNodeAtPoint(Event.current.mousePosition);
                if (_draggedNode != null)
                {
                    _dragOffset = Event.current.mousePosition - _draggedNode.editorRect.position;
                }
            } else if (Event.current.type == EventType.MouseDrag && _draggedNode != null)
            {
                
                Undo.RecordObject(_selectedDialogue, "Dialogue node moved");
                _draggedNode.editorRect.position = Event.current.mousePosition - _dragOffset;
                
                GUI.changed = true;
            } else if (Event.current.type == EventType.MouseUp && _draggedNode != null)
            {
                _draggedNode = null;
            }
        }


        private void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.editorRect, _nodeStyle);
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Node: ");
            string bufText = EditorGUILayout.TextField(node.text);
            string bufUid = EditorGUILayout.TextField(node.uid);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_selectedDialogue, "Dialogue node updated");
                node.text = bufText;
                node.uid = bufUid;
            }

            GUILayout.EndArea();
        }
        
        
        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode result = null;
            foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
            {
                if (node.editorRect.Contains(point))
                {
                    result = node;
                }
            }

            return result;
        }
    }
}