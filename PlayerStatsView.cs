using UnityEditor;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(UnityEngine.UI.Image))]
class PlayerStatsView : MonoBehaviour {
    [SerializeField]
    private TMP_Text _healthPoints;
    [SerializeField]
    private TMP_Text _resources;
    [SerializeField]
    private TMP_Text _name;

    public void OnUpdateStats(Player newStats){
        Debug.Log("Updating stats");
        _healthPoints.text = newStats._healthPoints.ToString();
        _resources.text = newStats._resources.ToString();
    }

}