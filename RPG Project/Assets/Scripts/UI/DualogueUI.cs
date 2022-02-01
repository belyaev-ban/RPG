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
            bool choosing = _playerConversant.IsChoosing();
            AIResponseRoot.SetActive(!choosing);
            choiceRoot.gameObject.SetActive(choosing);
            
            if (choosing)
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
                }
            }
            else
            {
                AIText.text = _playerConversant.GetText();
                nextButton.gameObject.SetActive(_playerConversant.HasNext());
            }
        }
    }
}
