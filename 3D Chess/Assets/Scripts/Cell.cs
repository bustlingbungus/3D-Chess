using System.Threading.Tasks;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Toggle Index")]
    public void ToggleIndexDisplay() {
        var obj = gameObject.transform.GetChild(0).gameObject;
        obj.SetActive(!obj.activeSelf);
        if (obj.activeSelf) obj.GetComponent<CellIndex>().UpdateText(index);
    }

    public Vector3Int index = Vector3Int.zero;


    public bool Empty() { return occupant==null; }

    public Piece occupant = null;
}
