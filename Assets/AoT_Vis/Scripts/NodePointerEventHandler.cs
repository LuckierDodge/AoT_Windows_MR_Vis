namespace AoT_Vis
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using VRTK;
    using TMPro;

    public class NodePointerEventHandler : MonoBehaviour
    {

        public VRTK_DestinationMarker destination_marker_left;
        public VRTK_DestinationMarker destination_marker_right;
        public VRTK_Pointer pointer_left;
        public VRTK_Pointer pointer_right;
        public DataManager dataManager;

        void OnEnable()
        {
            if (destination_marker_left != null && destination_marker_right != null)
            {
                destination_marker_left.DestinationMarkerEnter += DestinationMarkerEnter;
                destination_marker_left.DestinationMarkerHover += DestinationMarkerHover;
                destination_marker_left.DestinationMarkerExit += DestinationMarkerExit;
                destination_marker_left.DestinationMarkerSet += DestinationMarkerSet;

                destination_marker_right.DestinationMarkerEnter += DestinationMarkerEnter;
                destination_marker_right.DestinationMarkerHover += DestinationMarkerHover;
                destination_marker_right.DestinationMarkerExit += DestinationMarkerExit;
                destination_marker_right.DestinationMarkerSet += DestinationMarkerSet;
            }
            else
            {
                print("Error while adding destination marker event handlers");
            }

            if (pointer_left != null && pointer_right != null)
            {
                pointer_left.SelectionButtonPressed += Pointer_SelectionButtonPressed;
                pointer_right.SelectionButtonPressed += Pointer_SelectionButtonPressed;
            }
        }

        private void Pointer_SelectionButtonPressed(object sender, ControllerInteractionEventArgs e)
        {
            if (dataManager.HighlightedNode != "" && dataManager.HighlightedNode != null)
            {
                dataManager.changeSelectedNode(dataManager.HighlightedNode);
            }
        }

        protected virtual void OnDisable()
        {
            if (destination_marker_right != null)
            {
                destination_marker_right.DestinationMarkerEnter -= DestinationMarkerEnter;
                destination_marker_right.DestinationMarkerHover -= DestinationMarkerHover;
                destination_marker_right.DestinationMarkerExit -= DestinationMarkerExit;
                destination_marker_right.DestinationMarkerSet -= DestinationMarkerSet;
            }
            if (destination_marker_left != null)
            {
                destination_marker_left.DestinationMarkerEnter -= DestinationMarkerEnter;
                destination_marker_left.DestinationMarkerHover -= DestinationMarkerHover;
                destination_marker_left.DestinationMarkerExit -= DestinationMarkerExit;
                destination_marker_left.DestinationMarkerSet -= DestinationMarkerSet;
            }
            if (pointer_left != null && pointer_right != null)
            {
                pointer_left.SelectionButtonPressed -= Pointer_SelectionButtonPressed;
                pointer_right.SelectionButtonPressed -= Pointer_SelectionButtonPressed;
            }
        }

        protected virtual void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
        {
            if (e.target.name == "MapMarker(Clone)")
            {
                dataManager.HighlightedNode = e.target.GetComponent<MapMarker>().node_id;
            }
            else if (e.target.name == "Capsule")
            {
                dataManager.HighlightedNode = e.target.parent.GetComponent<MapMarker>().node_id;
            }
        }

        protected virtual void DestinationMarkerHover(object sender, DestinationMarkerEventArgs e)
        {

        }

        protected virtual void DestinationMarkerExit(object sender, DestinationMarkerEventArgs e)
        {
            if (e.target.name == "MapMarker(Clone)")
            {
                dataManager.HighlightedNode = "";
            }
            else if (e.target.name == "Capsule")
            {
                dataManager.HighlightedNode = "";
            }
        }

        protected virtual void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {

        }
    }
}
