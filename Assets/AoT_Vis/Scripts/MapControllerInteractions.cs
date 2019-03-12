namespace AoT_VR_Visualization
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using VRTK;
    using Mapbox.Utils;
    using Mapbox.Unity.Map;
    using Mapbox.Unity.MeshGeneration.Factories;
    using Mapbox.Unity.Utilities;

    public class MapControllerInteractions : MonoBehaviour
    {

        [SerializeField]
        VRTK_ControllerEvents _left_ControllerEvents;
        [SerializeField]
        VRTK_ControllerEvents _right_ControllerEvents;
        [SerializeField]
        AbstractMap _map;
        [SerializeField]
        Transform playerTransform;

        // Use this for initialization
        void Start()
        {
            if (_left_ControllerEvents == null || _right_ControllerEvents == null || _map == null)
            {
                Console.print("Missing Components for MapControllerInteractions");
                return;
            }

            _left_ControllerEvents.ButtonOnePressed += ButtonOneEvent;
            _left_ControllerEvents.ButtonTwoPressed += ButtonTwoEvent;
            _right_ControllerEvents.ButtonOnePressed += ButtonOneEvent;
            _right_ControllerEvents.ButtonTwoPressed += ButtonTwoEvent;

            _right_ControllerEvents.TouchpadAxisChanged += ThumbstickEvent;

        }

        private void ThumbstickEvent(object sender, ControllerInteractionEventArgs e)
        {
            Vector2 axis = _right_ControllerEvents.GetTouchpadAxis();
            playerTransform.Translate(axis.x, 0, axis.y);
        }

        private void ButtonOneEvent(object sender, ControllerInteractionEventArgs e)
        {
         
            //_map.SetZoom(14);
            ////CameraBoundsTileProviderOptions options = new CameraBoundsTileProviderOptions();
            ////options.SetOptions(_camera,5,6);
            ////_map.SetExtent(MapExtentType.CameraBounds, options);
            ////RangeTileProviderOptions options = new RangeTileProviderOptions();
            ////options.SetOptions(1, 1, 1, 1);
            ////_map.SetExtent(MapExtentType.RangeAroundCenter, options);
            //_map.UpdateMap();
            //transform.localScale = new Vector3(.01f, .01f, .01f);
        }

        private void ButtonTwoEvent(object sender, ControllerInteractionEventArgs e)
        {
            //_map.SetZoom(12);
            ////RangeTileProviderOptions options = new RangeTileProviderOptions();
            ////options.SetOptions(2, 3, 2, 4);
            ////_map.SetExtent(MapExtentType.RangeAroundCenter, options);
            //_map.UpdateMap();
            //transform.localScale = new Vector3(.01f, .01f, .01f);
        }
    }
}
