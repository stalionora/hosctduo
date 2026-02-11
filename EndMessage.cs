using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class EndMessage : MonoBehaviour{
    private void Start(){
        _text = GetComponent<TextMeshProUGUI>();
        
    }
    public async void ChangeMessage(){
        _text.enabled = !_text.enabled;
        await Task.Delay(_delayTime);
        if(_text != null)
            _text.enabled = !_text.enabled;
    }

    private TextMeshProUGUI _text;
    private const int _delayTime = 1000; 
}
