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

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void updateWorldPosition()
        {
            if (worldComponent != null)
            {
                worldComponent.GetComponent<AbstractMap>().SetCenterLatitudeLongitude(Conversions.StringToLatLon(dataManager.NodeLocations[dataManager.NodeIds.FindIndex(x => x == dataManager.SelectedNodeId)]));
                worldComponent.GetComponent<AbstractMap>().UpdateMap();
            }
        }
    }
}
