using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        public string uid;
        public string text;
        public string[] children;

        public Rect editorPosition = new Rect(0, 0, 200, 80);
    }
}