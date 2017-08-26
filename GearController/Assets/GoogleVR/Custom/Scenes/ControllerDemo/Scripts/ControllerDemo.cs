using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerDemo : MonoBehaviour
{
    public Text info;
    public InteractiveObject cube;

    private void Start ()
    {
        cube.onPointerClick = (go) =>
        {
            info.text = string.Format("clicked {0}", cube.name);
        };
        cube.onPointerEnter = (go) =>
        {
            info.text = string.Format("enterd {0}", cube.name);
        };
        cube.onPointerExit = (go) =>
        {
            info.text = string.Format("exit {0}", cube.name);
        };
    }
	
	
    private void Update ()
    {
        /*if (GvrControllerInput.ClickButton)
        {
            info.text = "ClickButton";
        }
        else if (GvrControllerInput.AppButton)
        {
            info.text = "AppButton";
        }
        else if (GvrControllerInput.HomeButtonState)
        {
            info.text = "HomeButton";
        }
        else if (GvrControllerInput.IsTouching)
        {
            info.text = string.Format("{0} {1}", GvrControllerInput.TouchPosCentered.x, GvrControllerInput.TouchPosCentered.y);
        }*/
        //info.text = string.Format("{0}", GvrControllerInput.Orientation.eulerAngles);
        //info.text = string.Format("{0} {1} {2} {3}", GvrControllerInput.Orientation.x, GvrControllerInput.Orientation.y, GvrControllerInput.Orientation.z, GvrControllerInput.Orientation.w);

    }
}
