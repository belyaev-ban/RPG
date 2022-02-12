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
        [SerializeField] private GameObject choicePrefab;
        [SerializeField] private Transform choiceRoot;
        [SerializeField] private GameObject AIResponseRoot;
        [SerializeField] private Button quitButton;
        [SerializeField] private TextMeshProUGUI conversantName;
        

        // Start is called before the first frame update
        void Start()
        {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            _playerConversant.ConversationUpdated += UpdateUI;
            
            nextButton.onClick.AddListener(() => _playerConversant.Next());
            quitButton.onClick.AddListener(() => _playerConversant.Quit());
            
            UpdateUI();
        }
        
        private void UpdateUI()
        {
            gameObject.SetActive(_playerConversant.IsActive());
            if (!_playerConversant.IsActive())
            {
                return;
            }

            conversantName.text = _playerConversant.GetCurrentConversantName();
            
            bool choosing = _playerConversant.IsChoosing();
            AIResponseRoot.SetActive(!choosing);
            choiceRoot.gameObject.SetActive(choosing);
            
            if (choosing)
            {
                BuildChoiceList();
            }
            else
            {
                AIText.text = _playerConversant.GetText();
                nextButton.gameObject.SetActive(_playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            // clear old choices
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }

            // populate new choices
            foreach (DialogueNode node in _playerConversant.GetChoices())
            {
                GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                choiceInstance.GetComponentInChildren<TextMeshProUGUI>().text = node.Text;

                Button button = choiceInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => _playerConversant.SelectChoice(node));
            }
        }
    }
}
