using UnityEngine;
using UnityEngine.UI;
using FigNet.Entangle;

public class DisplayPingUI : MonoBehaviour
{
    [SerializeField]
    private Text ping;

    // Update is called once per frame
    void Update()
    {
        ping.text = $"Ping {EN.GetPing()}";
    }
}
