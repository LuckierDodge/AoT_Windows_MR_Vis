namespace AoT_VR_Visualization
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using VRTK;
    using TMPro;

    public class NodePointerEventHandler : MonoBehaviour
    {

        public VRTK_DestinationMarker pointer;

        void OnEnable()
        {
            pointer = (pointer == null ? GetComponent<VRTK_DestinationMarker>() : pointer);

            if (pointer != null)
            {
                pointer.DestinationMarkerEnter += DestinationMarkerEnter;
                pointer.DestinationMarkerHover += DestinationMarkerHover;
                pointer.DestinationMarkerExit += DestinationMarkerExit;
                pointer.DestinationMarkerSet += DestinationMarkerSet;
            }
            else
            {
                print("Error while adding pointer event handlers");
            }
        }

        protected virtual void OnDisable()
        {
            if (pointer != null)
            {
                pointer.DestinationMarkerEnter -= DestinationMarkerEnter;
                pointer.DestinationMarkerHover -= DestinationMarkerHover;
                pointer.DestinationMarkerExit -= DestinationMarkerExit;
                pointer.DestinationMarkerSet -= DestinationMarkerSet;
            }
        }

        protected virtual void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
        {
            if (e.target.parent != null)
                e.target.parent.GetComponentInChildren<TextMeshPro>().enabled = true;
        }

        protected virtual void DestinationMarkerHover(object sender, DestinationMarkerEventArgs e)
        {

        }

        protected virtual void DestinationMarkerExit(object sender, DestinationMarkerEventArgs e)
        {
            if (e.target.parent != null)
                e.target.parent.GetComponentInChildren<TextMeshPro>().enabled = false;
        }

        protected virtual void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {

        }
    }
}
