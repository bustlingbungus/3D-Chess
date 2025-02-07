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
        graph = new AdjacencyGraph<CameraNode>();

        graph.AddVertex(new CameraNode(new Vector3(0,0,-25),    0));    // 0    front face
        graph.AddVertex(new CameraNode(new Vector3(25,0,0),     1));    // 1    right face
        graph.AddVertex(new CameraNode(new Vector3(0,0,25),     2));    // 2    back face
        graph.AddVertex(new CameraNode(new Vector3(-25,0,0),    3));    // 3    left face
        graph.AddVertex(new CameraNode(new Vector3(0,25,0),     4));    // 4    top face
        graph.AddVertex(new CameraNode(new Vector3(0,-25,0),    5));    // 5    bottom face

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

        curr_node = graph.GetVertexAt(0);
        prev_pos = transform.position = curr_node.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(panUp))    Node = graph.GetFirstOutgoing(Node.idx, GraphDirection.Up);
        else if (Input.GetKeyDown(panDown))  Node = graph.GetFirstOutgoing(Node.idx, GraphDirection.Down);
        else if (Input.GetKeyDown(panLeft))  Node = graph.GetFirstOutgoing(Node.idx, GraphDirection.Left);
        else if (Input.GetKeyDown(panRight)) Node = graph.GetFirstOutgoing(Node.idx, GraphDirection.Right);


        float t = move_timer / travelTime;
        transform.position = interp(prev_pos, Node.position, t);
        transform.LookAt(Vector3.zero);
        
        move_timer = Mathf.Clamp(move_timer+Time.deltaTime, 0f, travelTime);
    }

    private AdjacencyGraph<CameraNode> graph;
    private CameraNode curr_node;


    private CameraNode Node
    {
        get => curr_node;
        set {
            if (value!=null) {
                prev_pos = curr_node.position;
                move_timer = 0f;
                curr_node = value;
            }
        }
    }

    private Vector3 interp(Vector3 a, Vector3 b, float t) {
        return a + t*(b - a);
    }


    private Vector3 prev_pos;

    private float move_timer;
    [SerializeField]
    private float travelTime = 0.25f;
}
