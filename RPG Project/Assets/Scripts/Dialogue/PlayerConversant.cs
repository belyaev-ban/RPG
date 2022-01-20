using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private Dialogue currentDialogue;
        private DialogueNode _currentNode = null;

        private void Awake()
        {
            _currentNode = currentDialogue.GetRootNode();
        }

        public string GetText()
        {
            if (_currentNode is null)
            {
                return "";
            }
            
            return _currentNode.Text;
        }

        public void Next()
        {
            DialogueNode[] children = currentDialogue.GetAllChildren(_currentNode).ToArray();
            int randomIndex = Random.Range(0, children.Length);
            _currentNode = children[randomIndex];
        }

        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(_currentNode).Any();
        }
    }

}