namespace AoT_Vis
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;

    public class NodeInfoComponent : MonoBehaviour {

        public DataManager dataManager;
        public ComponentManager componentManager;

        TextMeshPro nodeID;
        TextMeshPro nodeAddress;
        TextMeshPro nodeOnline;
        TextMeshPro nodePosition;

	    // Use this for initialization
	    void Start () {
            nodeID = transform.Find("Text").Find("Node ID").GetComponent<TextMeshPro>();
            nodeAddress = transform.Find("Text").Find("Node Address").GetComponent<TextMeshPro>();
            nodeOnline = transform.Find("Text").Find("Node Online").GetComponent<TextMeshPro>();
            nodePosition = transform.Find("Text").Find("Node Position").GetComponent<TextMeshPro>();
            updateNodeInfo();
	    }

        public void updateNodeInfo ()
        {
            int index = dataManager.NodeIds.FindIndex(x => x == dataManager.SelectedNodeId);
            string nodeStart = dataManager.NodeStart[index];
            string nodeEnd = dataManager.NodeEnd[index];
            nodeID.SetText("Node ID: " + dataManager.SelectedNodeId);
            nodeAddress.SetText(dataManager.SelectedNodeAddress);
            nodeOnline.SetText(nodeEnd == "" ? "Online from " + nodeStart.Substring(0, 10) + " to " + "Present" : "Online from " + nodeStart.Substring(0, 10) + " to " + nodeEnd.Substring(0, 10));
            nodePosition.SetText(dataManager.NodeLat[index] + "°W, " + dataManager.NodeLon[index] + "°N");
        }
    }
}
