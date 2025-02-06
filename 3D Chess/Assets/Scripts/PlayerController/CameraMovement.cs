using UnityEngine;
using UnityEngine.InputSystem;
using Defs;
using Unity.VisualScripting;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private KeyCode panUp = KeyCode.I,
                    panDown = KeyCode.K,
                    panLeft = KeyCode.J,
                    panRight = KeyCode.L;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        graph = new CameraGraph();

        graph.AddNode(new Vector3(0,0,-25), new Vector3(0,0,0));    // 0    front face
        graph.AddNode(new Vector3(25,0,0), new Vector3(0,-90,0));   // 1    right face
        graph.AddNode(new Vector3(0,0,25), new Vector3(0,180,0));   // 2    back face
        graph.AddNode(new Vector3(-25,0,0), new Vector3(0,90,0));   // 3    left face
        graph.AddNode(new Vector3(0,25,0), new Vector3(90,0,0));    // 4    top face
        graph.AddNode(new Vector3(0,-25,0), new Vector3(-90,0,0));  // 5    bottom face

        // front face connections
        graph.AddEdge(0, 1, GraphDirection.Right);
        graph.AddEdge(0, 3, GraphDirection.Left);
        graph.AddEdge(0, 4, GraphDirection.Up);
        graph.AddEdge(0, 5, GraphDirection.Down);
        // right face connections
        graph.AddEdge(1, 0, GraphDirection.Left);
        graph.AddEdge(1, 2, GraphDirection.Right);
        graph.AddEdge(1, 4, GraphDirection.Up);
        graph.AddEdge(1, 5, GraphDirection.Down);
        // back face connections
        graph.AddEdge(2, 1, GraphDirection.Left);
        graph.AddEdge(2, 3, GraphDirection.Right);
        graph.AddEdge(2, 4, GraphDirection.Up);
        graph.AddEdge(2, 5, GraphDirection.Down);
        // left face connections
        graph.AddEdge(3, 0, GraphDirection.Right);
        graph.AddEdge(3, 2, GraphDirection.Left);
        graph.AddEdge(3, 4, GraphDirection.Up);
        graph.AddEdge(3, 5, GraphDirection.Down);
        // top face connections
        graph.AddEdge(4, 0, GraphDirection.Down);
        graph.AddEdge(4, 1, GraphDirection.Right);
        graph.AddEdge(4, 3, GraphDirection.Left);
        graph.AddEdge(4, 2, GraphDirection.Up);
        // bottom face connections
        graph.AddEdge(5, 0, GraphDirection.Up);
        graph.AddEdge(5, 1, GraphDirection.Right);
        graph.AddEdge(5, 3, GraphDirection.Left);
        graph.AddEdge(5, 2, GraphDirection.Down);

        curr_node = graph.GetNodeAt(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(panUp))    curr_node = graph.GetOut(curr_node.idx, GraphDirection.Up)[0];
        else if (Input.GetKeyDown(panDown))  curr_node = graph.GetOut(curr_node.idx, GraphDirection.Down)[0];
        else if (Input.GetKeyDown(panLeft))  curr_node = graph.GetOut(curr_node.idx, GraphDirection.Left)[0];
        else if (Input.GetKeyDown(panRight)) curr_node = graph.GetOut(curr_node.idx, GraphDirection.Right)[0];

        transform.position = curr_node.position;
        transform.eulerAngles = curr_node.eulerAngles;
    }

    private CameraGraph graph;
    private CameraNode curr_node;
}
