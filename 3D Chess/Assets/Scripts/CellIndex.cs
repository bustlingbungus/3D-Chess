using System;
using TMPro;
using UnityEngine;

public class CellIndex : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3Int idx = gameObject.transform.parent.GetComponent<Cell>().index;
        gameObject.GetComponent<TMP_Text>().SetText("({0}, {1}, {2})", idx.x, idx.y, idx.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void UpdateText(Vector3 index) {
        gameObject.GetComponent<TMP_Text>().SetText("({0}, {1}, {2})", index.x, index.y, index.z);
    }
}
