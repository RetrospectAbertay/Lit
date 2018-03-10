using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour {

    enum MenuState
    {
        MAIN,
        OPTIONS,
        LEVELSELECT
    }

    public float SelectionSpacing;
    private Text[] SelectionText;
    private Image[] SelectionIndicator;
    private float curSelection;
    private MenuState curState;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
