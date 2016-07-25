using UnityEngine;
using System.Collections;

public struct MousePos{
    public int x;
    public int y;
    public MousePos(int x, int y){ this.x = x; this.y = y; }
}

/// <summary>
/// Mouse script. This script will hold the information of the mouse position on the screen.
/// there might be need to convert cordinate system.
/// 
/// This should be a singleton
/// 
/// currently not checking for anything but left click event
/// </summary>
public class MouseScript : MonoBehaviour {
	public static MouseScript instance;
	void Awake(){
		if( instance == null ){
			instance = this;
		}
	}


    public bool isMouseLeftClick = false;
    public MousePos mousePosition = new MousePos();

    void Update(){
        mousePosition.x = (int)Input.mousePosition.x;
        mousePosition.y = (int)Input.mousePosition.y;

        if( Input.GetMouseButtonDown(0) ){
            isMouseLeftClick = true;
        }else{
            isMouseLeftClick = false;

        }

    }
	
}
