using UnityEngine;
using System.Collections;

public class VoiceModule : MonoBehaviour {
    public GUIText _text;
    public HoveringText _hover;
    public float _timeToExpire;
	// Use this for initialization
	void Start () {
	
	}
	
	public void Speak(string words)
    {
        _text.enabled = true;
        _text.text = words;
        Invoke("Stop", _timeToExpire);
    }

    private void Stop()
    {
        _text.enabled = false;
    }
}
