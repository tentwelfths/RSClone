using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIcon : MonoBehaviour {

    public GameObject mapIcon;
    public Canvas miniMap;
    public int pixelSize;

    private GameObject Icon;
    private UnityEngine.UI.Image ico;

    private void Awake()
    {
        Icon = Instantiate(mapIcon);
        ico = Icon.GetComponent<UnityEngine.UI.Image>();
        ico.rectTransform.position = transform.position + Vector3.up;
        ico.rectTransform.localScale = new Vector3(1, 1, 1);
        ico.rectTransform.rotation = miniMap.transform.rotation;
        Icon.transform.SetParent(miniMap.transform);
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        ico.rectTransform.position = pos;
        pos = ico.rectTransform.localPosition;
        pos.z = -1;
        ico.rectTransform.localPosition = pos;
        ico.rectTransform.localScale = new Vector3(1, 1, 1);
    }

}
