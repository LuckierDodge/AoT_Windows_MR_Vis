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
        float value = 0;
        TextMeshPro tempLabel;
        Transform column;

        // Use this for initialization
        void Start()
        {
            gameObject.transform.Find("Sensor Label").GetComponent<TextMeshPro>().text = sensor.ToUpper();
            tempLabel = gameObject.transform.Find("Temp Label").GetComponent<TextMeshPro>();
            column = gameObject.transform.Find("Indicator").Find("Column");
            updateValue();
        }

        public void updateValue()
        {
            try
            {
                tempLabel.text = dataManager.TemperatureReadings[sensor] + "°C";
                value = float.Parse(dataManager.TemperatureReadings[sensor]);
            } catch
            {
                tempLabel.text = "--°C";
                value = 0f;
            }

            if (value >= 50)
            {
                value = 50;
            } else if (value <= 0)
            {
                value = 0;
            }

            var barHeight = .275f * value / 50.0f;
            var barPosition = -.15f + value * .006f;

            column.transform.localScale = new Vector3(.05f, barHeight, .001f);
            column.transform.localPosition = new Vector3(0f, barPosition, -.05f);
        }
    }
}

