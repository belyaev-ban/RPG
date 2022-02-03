using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private Dialogue testDialogue;
        private Dialogue _currentDialogue;
        private DialogueNode _currentNode = null;
        private bool _isChoosing = false;

        public event Action ConversationUpdated;
        
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2);
            StartDialogue(testDialogue);
        }

        public void StartDialogue(Dialogue newDialogue)
        {
            _currentDialogue = newDialogue;
            _currentNode = _currentDialogue.GetRootNode();
            
            OnConversationUpdated();
        }

        public string GetText()
        {
            if (_currentNode is null)
            {
                return "";
            }
            
            return _currentNode.Text;
        }

        public bool IsChoosing()
        {
            return _isChoosing;
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return _currentDialogue.GetPlayerChildren(_currentNode);
        }

        public void Next()
        {
            int numPlayerResponses = _currentDialogue.GetPlayerChildren(_currentNode).Count();
            if (numPlayerResponses > 0)
            {
                _isChoosing = true;
                
                OnConversationUpdated();
                return;
            }
            
            DialogueNode[] children = _currentDialogue.GetAIChildren(_currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Length);
            _currentNode = children[randomIndex];
            
            OnConversationUpdated();
        }

        public bool HasNext()
        {
            return _currentDialogue.GetAllChildren(_currentNode).Any();
        }

        public bool IsActive()
        {
            return _currentDialogue != null;
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            _currentNode = chosenNode;
            _isChoosing = false;
            Next();
        }

        protected virtual void OnConversationUpdated()
        {
            ConversationUpdated?.Invoke();
        }

        public void Quit()
        {
            _currentDialogue = null;
            _currentNode = null;
            _isChoosing = false;
            
            OnConversationUpdated();
        }
    }

}