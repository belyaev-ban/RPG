using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        public string uid;
        public string text;
        public string[] children;
    }
}