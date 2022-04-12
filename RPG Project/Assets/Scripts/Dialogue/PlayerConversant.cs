using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using UnityEngine;

namespace Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private string playerName;
        
        private Dialogue _currentDialogue;
        private DialogueNode _currentNode = null;
        private AIConversant _currentConversant;
        private bool _isChoosing = false;
        
        public event Action ConversationUpdated;

        public void StartDialogue(Dialogue newDialogue, AIConversant newConversant)
        {
            _currentDialogue = newDialogue;
            _currentNode = _currentDialogue.GetRootNode();
            _currentConversant = newConversant;
            TriggerEnterAction();
            
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
            return FilterOnCondition(_currentDialogue.GetPlayerChildren(_currentNode));
        }

        public void Next()
        {
            int numPlayerResponses = FilterOnCondition(_currentDialogue.GetPlayerChildren(_currentNode)).Count();
            if (numPlayerResponses > 0)
            {
                _isChoosing = true;
                TriggerExitAction();
                
                OnConversationUpdated();
                return;
            }
            
            DialogueNode[] children = FilterOnCondition(_currentDialogue.GetAIChildren(_currentNode)).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Length);
            TriggerExitAction();
            _currentNode = children[randomIndex];
            TriggerEnterAction();
            
            OnConversationUpdated();
        }

        public bool HasNext()
        {
            return FilterOnCondition(_currentDialogue.GetAllChildren(_currentNode)).Any();
        }

        public bool IsActive()
        {
            return _currentDialogue != null;
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            _currentNode = chosenNode;
            TriggerEnterAction();
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
            TriggerExitAction();
            _currentNode = null;
            _currentConversant = null;
            _isChoosing = false;
            
            OnConversationUpdated();
        }

        private void TriggerEnterAction()
        {
            if (_currentNode == null)
            {
                return;
            }
            
            TriggerAction(_currentNode.GetOnEnterAction());
        }

        private void TriggerExitAction()
        {
            if (_currentNode == null)
            {
                return;
            }

            TriggerAction(_currentNode.GetOnExitAction());
        }

        private void TriggerAction(string action)
        {
            if (action == "") return;

            foreach (DialogueTrigger dialogueTrigger in _currentConversant.GetComponents<DialogueTrigger>())
            {
                dialogueTrigger.Trigger(action);
            }
        }

        public string GetCurrentConversantName()
        {
            if (_isChoosing)
            {
                return playerName;
            }

            string aiName = _currentConversant.GetName();

            return aiName == "" ? _currentConversant.name : aiName;
        }

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach (DialogueNode node in inputNode)
            {
                if (node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }
    }

}