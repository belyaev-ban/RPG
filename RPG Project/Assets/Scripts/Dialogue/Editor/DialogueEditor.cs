using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private Dialogue _selectedDialogue = null;
        
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
            
            
            
            foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
            {
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
            }
        }
    }
}