using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Displays an index, and rotates self to face the main camera object. 
/// </summary>
public class CellIndex : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialise reference to text renderer, and set it's text. 
        Vector3Int idx = gameObject.transform.parent.GetComponent<Cell>().index;
        txt = gameObject.GetComponent<TMP_Text>();
        UpdateText(idx);
    }

    // Update is called once per frame
    void Update()
    {
        // look at the camera
        transform.LookAt(Camera.main.transform);
        // note this is currently broken and displays the text backwards (facing directly away from the camera)
    }

    /// <summary>
    /// Makes the text renderer's text display the index as <i>"(x, y, z)"</i>
    /// </summary>
    /// <param name="index">The <i>(x, y, z)</i> index to display.</param>
    public void UpdateText(Vector3Int index) 
    {
        // validate txt reference
        if (txt==null) txt = gameObject.GetComponent<TMP_Text>();
        if (txt!=null) txt.SetText("({0}, {1}, {2})", index.x, index.y, index.z);
    }

    /// <summary> Reference to the object's text renderer component. </summary>
    private TMP_Text txt;
}
