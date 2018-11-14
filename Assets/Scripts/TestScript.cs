using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestScript : MonoBehaviour {
    public int startNode;
    public int endNode;
	private SparseGraph graph;

	private void Start() {
		CreateGraph();
		//DijkstraSearch();
		//BreadthFirstSearch();
		//DepthFirstSearch();
	}

	private void CreateGraph() {
		graph = new SparseGraph(false);
        var objects = FindObjectsOfType<ObjectNode>().OrderBy( x=>x.index).ToArray();
        foreach (var o in objects) graph.AddNode(new GraphNode(o.index));
        foreach(var o in objects)
        {
            foreach(var n in o.Connects)
            {
                graph.AddEdge(new GraphEdge(o.index, n.index, Vector2.Distance(o.transform.position, n.transform.position)));
            }
        }
	}

	private void DijkstraSearch() {
		var dfs = new GraphSearchDijkstra(graph, startNode, endNode);
        ShowLog(dfs.GetPathToTarget());
	}

	private void BreadthFirstSearch() {
		var dfs = new GraphSerachBFS(graph, startNode, endNode);
        ShowLog(dfs.GetPathToTarget());
	}

	private void DepthFirstSearch () {
		var dfs = new GraphSearchDFS(graph, startNode, endNode);
        ShowLog(dfs.GetPathToTarget());
    }

    private void ShowLog(List<int> path)
    {
        string pathString = string.Empty;
        foreach (var nodeIdx in path) pathString += nodeIdx + ";";
        Debug.Log(pathString);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CreateGraph();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DijkstraSearch();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            BreadthFirstSearch();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DepthFirstSearch();
        }
    }
}
