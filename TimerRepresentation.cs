using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimerRepresentation: MonoBehaviour {
    //[RequireComponent(typeof(TextMeshProUGUI))]
    
    public void OnUpdateNumber() {
        ++_seconds;
       _text.text = $"{_seconds / 60:D2}:{_seconds % 60:D2}";
    }
    private void Start(){
        _text = GetComponent<TextMeshProUGUI>();
    }
    
    private TMP_Text _text;
    int _seconds = 0;
}