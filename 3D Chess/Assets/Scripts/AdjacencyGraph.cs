using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum GraphDirection 
{ 
    NoConnection, 
    // orthogonal directions (difference along one axis)
    Up, 
    Left, 
    Down, 
    Right,
}

/// <summary>
/// <para>
/// Directed weighted graph intended to allow for Up down left right connections between Vertices. 
/// </para>
/// 
/// <para>
/// Graph is implemented using an adjacency matrix.
/// </para>
/// </summary>
/// <typeparam name="T">The type of element that will serve as the graph's vertices</typeparam>
public class AdjacencyGraph<T>
{
    public AdjacencyGraph() {
        // initialise containers
        mat = new List<List<GraphDirection>>();
        vertices = new List<T>();
    }

    /// <summary>
    /// Adds the provided vertex to the graph with no connections.
    /// </summary>
    /// <param name="elem">The vertex to add to the graph.</param>
    /// <returns>The integer index of the vertex added</returns>
    public int AddVertex(T elem)
    {
        int n = vertices.Count;
        // create a new row in the matrix
        mat.Add(new List<GraphDirection>(new GraphDirection[n]));
        // for all existing entries, add a new connecttion (no connection) to represent the new vertex
        foreach (var row in mat) row.Add(GraphDirection.NoConnection);
        // add object to vertices array
        vertices.Add(elem);
        return n;
    }

    /// <summary>
    /// Adds an edge directed from src to dst. Does not add a connection from dst to src.
    /// </summary>
    /// <param name="src">The index of the source vertex</param>
    /// <param name="dst">The index of the destination vertex</param>
    /// <param name="dir">The "direction" of the connection</param>
    public void AddEdge(int src, int dst, GraphDirection dir)
    {
        mat[src][dst] = dir;
    }

    /// <summary>
    /// Removes a vertex from the adjacency matrix.
    /// </summary>
    /// <param name="node_idx">Index of the vertex to remove.</param>
    public void RemoveVertex(int node_idx)
    {
        mat.RemoveAt(node_idx);
        foreach (var row in mat) row.RemoveAt(node_idx);
        vertices.RemoveAt(node_idx);
    }

    /// <summary>
    /// Removes the connection from src to dst. Does not remove or otherwise modify any connection from dst to src.
    /// </summary>
    /// <param name="src">The index of the source vertex.</param>
    /// <param name="dst">The index of the destination vertex.</param>
    public void RemoveEdge(int src, int dst)
    {
        mat[src][dst] = GraphDirection.NoConnection;
    }

    /// <summary>
    /// Get a list of all connections originating from src whose weight is dir.
    /// </summary>
    /// <param name="src">The index of the source vertex.</param>
    /// <param name="dir">The type of connection to search for</param>
    /// <returns>An array of the vertices src has an outgoing connection of type dir with.</returns>
    public List<T> GetOutgoing(int src, GraphDirection dir)
    {
        List<T> res = new List<T>();
        for (int i=0; i<vertices.Count; i++) {
            // add matching vertices to to result
            if (mat[src][i]==dir) res.Add(vertices[i]);
        }
        return res;
    }

    /// <summary>
    /// Gets the first connection originating from src whose "weight" is dir.
    /// </summary>
    /// <param name="src">The index of the source vertex.</param>
    /// <param name="dir">The type of connection to search for.</param>
    /// <returns>The vertex terminating the first matching outgoing connection found.</returns>
    public T GetFirstOutgoing(int src, GraphDirection dir)
    {
        for (int i=0; i<vertices.Count; i++) {
            // return the first matching connection
            if (mat[src][i]==dir) return vertices[i];
        }
        return default;
    }

    /// <summary>
    /// Get the element stored in the vertex at a given index.
    /// </summary>
    /// <param name="idx">The index to search for a vertex at.</param>
    /// <returns>The element stored in the desired vertex</returns>
    public T GetVertexAt(int idx)
    {
        return vertices[idx];
    }

    /// <summary> Main adjacency matrix. </summary>
    private List<List<GraphDirection>> mat;
    /// <summary> Array of vertex data. Indices correspond to matrix indices. </summary>
    private List<T> vertices;
};
