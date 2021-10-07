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
        private Vector2 _scrollPosition = new Vector2();
        
        [NonSerialized] private DialogueNode _draggedNode = null;
        [NonSerialized] private GUIStyle _nodeStyle;
        [NonSerialized] private Vector2 _dragOffset;
        [NonSerialized] private DialogueNode _creatingNode = null;
        [NonSerialized] private DialogueNode _deletingNode = null;
        [NonSerialized] private DialogueNode _linkingNode = null;

        [NonSerialized] private bool _draggingCanvas = false;
        [NonSerialized] private Vector2 _draggingCanvasOffset;
        private const float CanvasTextureSize = 50f;

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

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            Rect canvas = GUILayoutUtility.GetRect(_selectedDialogue.windowSize.x, _selectedDialogue.windowSize.y);
            Texture2D backgroundTexture = Resources.Load("background") as Texture2D;
            Rect texCoords = new Rect(0, 0, _selectedDialogue.windowSize.x / CanvasTextureSize, _selectedDialogue.windowSize.y / CanvasTextureSize);
            GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, texCoords);
            
            foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
            {
                DrawConnections(node);
            }
            foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
            {
                DrawNode(node);
            }
            
            EditorGUILayout.EndScrollView();
            
            ProcessNodesEvents();
        }

        private void DrawConnections(DialogueNode node)
        {
            float bezierMultiplier = 0.8f;
            
            foreach (DialogueNode childNode in _selectedDialogue.GetAllChildren(node))
            {
                Vector3 startPosition, endPosition, startTangent, endTangent;
                startPosition = new Vector3(node.EditorRect.xMax, node.EditorRect.center.y);
                endPosition = new Vector3(childNode.EditorRect.xMin, childNode.EditorRect.center.y);

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
                    _dragOffset = Event.current.mousePosition - _draggedNode.EditorRect.position;
                    Selection.activeObject = _draggedNode;
                }
                else
                {
                    _draggingCanvas = true;
                    _draggingCanvasOffset = Event.current.mousePosition + _scrollPosition;
                    Selection.activeObject = _selectedDialogue;
                }
            } else if (Event.current.type == EventType.MouseDrag && _draggedNode != null)
            {
                _draggedNode.SetPosition(Event.current.mousePosition - _dragOffset);

                GUI.changed = true;
            } else if (Event.current.type == EventType.MouseUp && _draggedNode != null)
            {
                _draggedNode = null;
            } else if (Event.current.type == EventType.MouseDrag && _draggingCanvas)
            {
                _scrollPosition = _draggingCanvasOffset - Event.current.mousePosition;
                
                GUI.changed = true;
            } else if (Event.current.type == EventType.MouseUp && _draggingCanvas)
            {
                _draggingCanvas = false;
            }
        }
        
        private void ProcessNodesEvents()
        {
            if (_creatingNode != null)
            {
                _selectedDialogue.CreateNode(_creatingNode);
                _creatingNode = null;
            }

            if (_deletingNode != null)
            {
                _selectedDialogue.DeleteNode(_deletingNode);
                _deletingNode = null;
            }
        }
        
        private void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.EditorRect, _nodeStyle);
            
            node.Text = EditorGUILayout.TextField(node.Text);

            DrawNodeButtons(node);

            GUILayout.EndArea();
        }

        private void DrawNodeButtons(DialogueNode node)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("x"))
            {
                _deletingNode = node;
            }
            
            if (_linkingNode is null)
            {
                if (GUILayout.Button("link"))
                {
                    _linkingNode = node;
                }
            }
            else if (_linkingNode == node)
            {
                if (GUILayout.Button("cancel"))
                {
                    _linkingNode = null;
                }
            } else if (_linkingNode.Children.Contains(node.name))
            {
                if (GUILayout.Button("unlink"))
                {
                    _linkingNode.RemoveChild(node.name);
                    _linkingNode = null;
                }
            } else
            {
                if (GUILayout.Button("child"))
                {
                    _linkingNode.AddChild(node.name);
                    _linkingNode = null;
                }
            }
            
            if (GUILayout.Button("+"))
            {
                _creatingNode = node;
            }
            GUILayout.EndHorizontal();
        }


        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode result = null;
            foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
            {
                if (node.EditorRect.Contains(point + _scrollPosition))
                {
                    result = node;
                }
            }

            return result;
        }
    }
}