namespace AoT_Vis
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;

    public class TemperatureComponent : MonoBehaviour
    {

        public DataManager dataManager;
        public ComponentManager componentManager;
        public string sensor;

        // Use this for initialization
        void Start()
        {
            gameObject.transform.Find("Sensor Label").GetComponent<TextMeshPro>().text = sensor.ToUpper();
            updateValue();
        }

        public void updateValue()
        {
            gameObject.transform.Find("Temp Label").GetComponent<TextMeshPro>().text = dataManager.TemperatureReadings[sensor] + "°C";
        }
    }
}

