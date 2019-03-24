namespace AoT_Vis
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Mapbox.Unity.Map;
    using Mapbox.Unity.Utilities;
    using Mapbox.Utils;

    public class ComponentManager : MonoBehaviour
    {

        public DataManager dataManager;
        public GameObject worldComponent;
        public GameObject nodeInfoComponent;

        public void updateNode()
        {
            if (worldComponent != null)
            {
                worldComponent.GetComponent<AbstractMap>().SetCenterLatitudeLongitude(Conversions.StringToLatLon(dataManager.NodeLocations[dataManager.NodeIds.FindIndex(x => x == dataManager.SelectedNodeId)]));
                worldComponent.GetComponent<AbstractMap>().UpdateMap();
            }
            if(nodeInfoComponent != null)
            {
                nodeInfoComponent.GetComponent<NodeInfoComponent>().updateNodeInfo();
            }
        }
    }
}
