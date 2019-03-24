namespace AoT_Vis
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using VRTK;

    public class ControllerEventHandler : MonoBehaviour {

        public VRTK_ControllerEvents leftController;
        public VRTK_ControllerEvents rightController;
        public DataManager dataManager;
        public ComponentManager componentManager;

        private bool hidden = false;

	    // Use this for initialization
	    void Start () {
            leftController.TouchpadPressed += OnTouchPadPressed;
            rightController.TouchpadPressed += OnTouchPadPressed;
	    }

        private void OnTouchPadPressed(object sender, ControllerInteractionEventArgs e)
        {
            if (!hidden)
            {
                componentManager.setVisualizationIsActive(false);
            }
            else
            {
                componentManager.setVisualizationIsActive(true);
            }
            hidden = !hidden;
        }
    }
}
