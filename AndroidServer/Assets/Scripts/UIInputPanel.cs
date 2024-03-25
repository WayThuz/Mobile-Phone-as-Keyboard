using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using VirtualKeyboard.Android;

public class UIInputPanel : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;
    [SerializeField] private Client client;
    [SerializeField] private Server server;
    [SerializeField] private List<TMP_Text> texts;
    [SerializeField] private TMP_Text inputDisplay;

    // Start is called before the first frame update
    void Start() {
        AssignEvents();
    }

    private void AssignEvents() {
        for(int i = 0; i < buttons.Count; i++) {
            if(buttons[i]) {
                if(server && server.enabled) {
                    var text = texts[i];
                    buttons[i].onClick.AddListener(() => {
                        server.Send(text.text);
                        inputDisplay.text = $"Input: {text.text}";
                    });
                }
            }
        }
    }
}
