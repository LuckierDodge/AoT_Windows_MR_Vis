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
        public GameObject mapComponent;

        public void updateNode()
        {
            if (worldComponent)
            {
                worldComponent.GetComponent<AbstractMap>().SetCenterLatitudeLongitude(Conversions.StringToLatLon(dataManager.NodeLocations[dataManager.NodeIds.FindIndex(x => x == dataManager.SelectedNodeId)]));
                worldComponent.GetComponent<AbstractMap>().UpdateMap();
            }
            if(nodeInfoComponent)
            {
                nodeInfoComponent.GetComponent<NodeInfoComponent>().updateNodeInfo();
            }
        }

        public void setVisualizationIsActive(bool isActive)
        {
            if (mapComponent)
            {
                mapComponent.SetActive(isActive);
                foreach (GameObject spawnedObject in mapComponent.GetComponent<MapComponent>().spawnedObjects)
                {
                    spawnedObject.SetActive(isActive);
                }
                mapComponent.GetComponent<MapComponent>().selectedMarkerInstance.SetActive(isActive);
            }
            if (nodeInfoComponent)
            {
                nodeInfoComponent.SetActive(isActive);
            }
        }
    }
}
