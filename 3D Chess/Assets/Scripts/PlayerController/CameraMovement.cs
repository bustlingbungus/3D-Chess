using UnityEngine;
using UnityEngine.InputSystem;
using Defs;
using Unity.VisualScripting;
using System.Collections.Generic;

/// <summary>
/// <para>Script to help control the movement of the camera.
/// Ensures the camera is always looking at the centre of the board.</para>
/// 
/// <para>Stores a directed graph whose vertices contain different viewing positions around the board. 
/// Interpolates the camera between these position vertices based on user input.</para>
/// </summary>
public class CameraMovement : MonoBehaviour
{
    /* ==========  MEMBERS  ========== */

    /// <summary> Keybinds to pan the camera in different connections. </summary>
    [SerializeField]
    private KeyCode panUp = KeyCode.I,
                    panDown = KeyCode.K,
                    panLeft = KeyCode.J,
                    panRight = KeyCode.L;
    
    /// <summary> The amount of time the camera takes to transition between viewing locations. </summary>
    [SerializeField]
    private float travelTime = 0.25f;

    /// <summary> Directed graph containing all viewing locations for the camera. </summary>
    private AdjacencyGraph<CameraNode> graph;
    /// <summary> The graph vertex (viewing location) the camera is currently positioned at. </summary>
    private CameraNode curr_node;

    /// <summary> The position of the camera's previous viewing location, used for interpolation. </summary>
    private Vector3 prev_pos;
    /// <summary> Timer used for interpolation between the previous location and the current one. </summary>
    private float move_timer;


    /* ==========  PROPERTIES  ========== */

    /// <summary>
    /// <para><b>get:</b> The graph vertex (viewing location) the camera is currently positioned at. </para>
    /// <para><b>set:</b> Assigns current node, and sets up <c>prev_pos</c> and <c>move_timer</c> to begin interpolation.</para>
    /// </summary>
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


    /* ==========  MAIN FUNCTIONS  ========== */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // initialise the vertex graph and add all the viewing locations.
        graph = new AdjacencyGraph<CameraNode>();

        graph.AddVertex(new CameraNode(new Vector3(0,0,-25),    0));    // 0    front face
        graph.AddVertex(new CameraNode(new Vector3(25,0,0),     1));    // 1    right face
        graph.AddVertex(new CameraNode(new Vector3(0,0,25),     2));    // 2    back face
        graph.AddVertex(new CameraNode(new Vector3(-25,0,0),    3));    // 3    left face
        graph.AddVertex(new CameraNode(new Vector3(0,25,0),     4));    // 4    top face
        graph.AddVertex(new CameraNode(new Vector3(0,-25,0),    5));    // 5    bottom face

        // add all vertex connections
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

        // initilaise camera position
        curr_node = graph.GetVertexAt(0);
        prev_pos = transform.position = curr_node.position;
    }

    // Update is called once per frame
    void Update()
    {
        // move to the next node in the direction selected by user input, if any
        if (Input.GetKeyDown(panUp))    Node = graph.GetFirstOutgoing(Node.idx, GraphDirection.Up);
        else if (Input.GetKeyDown(panDown))  Node = graph.GetFirstOutgoing(Node.idx, GraphDirection.Down);
        else if (Input.GetKeyDown(panLeft))  Node = graph.GetFirstOutgoing(Node.idx, GraphDirection.Left);
        else if (Input.GetKeyDown(panRight)) Node = graph.GetFirstOutgoing(Node.idx, GraphDirection.Right);

        // move to the position of the new node by interpolation
        float t = move_timer / travelTime;
        transform.position = interp(prev_pos, Node.position, t);
        // ensure the camera is looking at the centre of the board
        transform.LookAt(Vector3.zero);
        
        // update the timer used for interpolation
        move_timer = Mathf.Clamp(move_timer+Time.deltaTime, 0f, travelTime);
    }


    /* ==========  HELPER FUNCTIONS  ========== */

    // linear interpolation for Vector3
    private Vector3 interp(Vector3 a, Vector3 b, float t) {
        return a + t*(b - a);
    }
}
