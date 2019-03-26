namespace AoT_Vis
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;

    public class RowComponent : MonoBehaviour {

        public string label;

        void Start()
        {
            setLabelText(label);
        }

        public void setLabelText(string text)
        {
            gameObject.GetComponentInChildren<TextMeshPro>().text = text;
        }
    }
}

