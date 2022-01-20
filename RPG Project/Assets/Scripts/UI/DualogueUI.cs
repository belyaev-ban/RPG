using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DualogueUI : MonoBehaviour
    {
        private PlayerConversant _playerConversant;
        [SerializeField] private TextMeshProUGUI AIText;
        [SerializeField] private Button nextButton;
        

        // Start is called before the first frame update
        void Start()
        {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            nextButton.onClick.AddListener(Next);
            
            UpdateUI();
        }

        private void Next()
        {
            _playerConversant.Next();
            UpdateUI();
        }

        private void UpdateUI()
        {
            AIText.text = _playerConversant.GetText();
            nextButton.gameObject.SetActive(_playerConversant.HasNext());
        }
    }
}
