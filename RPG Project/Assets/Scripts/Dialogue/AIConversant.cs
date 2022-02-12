using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Dialogue characterDialogue;
        [SerializeField] private string conversantName;

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (characterDialogue == null)
            {
                return false;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                PlayerConversant playerConversant = callingController.GetComponent<PlayerConversant>();
                
                playerConversant.Quit();
                playerConversant.StartDialogue(characterDialogue, this);
            }

            return true;
        }

        public string GetName()
        {
            return conversantName;
        }
    }
}
