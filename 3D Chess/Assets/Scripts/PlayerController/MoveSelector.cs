using UnityEngine;
using System.Collections.Generic;

public class MoveSelector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetMoves(Piece piece)
    {
        highlights = new List<GameObject>();
    }

    private List<GameObject> highlights;
}
