using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;
using TMPro;

namespace RPG.UI
{
    public class DualogueUI : MonoBehaviour
    {
        private PlayerConversant _playerConversant;
        [SerializeField] private TextMeshProUGUI AIText;
        

        // Start is called before the first frame update
        void Start()
        {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            AIText.text = _playerConversant.GetText();
        }

        // Update is called once per frame
        void Update()
        { }
    }
}
