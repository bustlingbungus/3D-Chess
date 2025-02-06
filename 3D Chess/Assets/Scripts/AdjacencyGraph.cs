using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum GraphDirection { NoConnection, Up, Left, Down, Right }

public class AdjacencyGraph<T>
{
    public AdjacencyGraph() {
        mat = new List<List<GraphDirection>>();
        nodes = new List<T>();
    }

    public int AddNode(T elem)
    {
        int n = nodes.Count;
        mat.Add(new List<GraphDirection>(new GraphDirection[n]));
        foreach (var row in mat) {
            row.Add(GraphDirection.NoConnection);
        }
        nodes.Add(elem);
        return n;
    }

    public void AddEdge(int src, int dst, GraphDirection dir)
    {
        mat[src][dst] = dir;
    }

    public void RemoveNode(int node_idx)
    {
        mat.RemoveAt(node_idx);
        foreach (var row in mat) {
            row.RemoveAt(node_idx);
        }
        nodes.RemoveAt(node_idx);
    }

    public void RemoveEdge(int src, int dst)
    {
        mat[src][dst] = GraphDirection.NoConnection;
    }

    public List<T> GetOutgoing(int src, GraphDirection dir)
    {
        List<T> res = new List<T>();
        for (int i=0; i<nodes.Count; i++) {
            if (mat[src][i]==dir) res.Add(nodes[i]);
        }
        return res;
    }

    public T GetFirstOutgoing(int src, GraphDirection dir)
    {
        for (int i=0; i<nodes.Count; i++) {
            if (mat[src][i]==dir) return nodes[i];
        }
        return default;
    }

    public T GetElemAt(int idx)
    {
        return nodes[idx];
    }

    private List<List<GraphDirection>> mat;
    private List<T> nodes;
};
