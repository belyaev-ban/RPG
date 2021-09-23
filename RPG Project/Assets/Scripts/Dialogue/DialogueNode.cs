using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        public string uid;
        public string text;
        public string[] children;

        [FormerlySerializedAs("editorPosition")] public Rect editorRect = new Rect(0, 0, 200, 80);
    }
}