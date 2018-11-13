using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {
	public List<Transform> nodeObjects;
	private SparseGraph graph;

	private void Start() {
		CreateGraph();
		DijkstraSearch();
		//BreadthFirstSearch();
		//DepthFirstSearch();
	}

	private void CreateGraph() {
		graph = new SparseGraph(true);
		for (int i = 0; i < 6; i++) {
			graph.AddNode(new GraphNode(i));
		}
		graph.AddEdge(new GraphEdge(0, 1));
		graph.AddEdge(new GraphEdge(0, 2));
		graph.AddEdge(new GraphEdge(1, 0));
		graph.AddEdge(new GraphEdge(1, 4));
		graph.AddEdge(new GraphEdge(2, 0));
		graph.AddEdge(new GraphEdge(2, 3));
		graph.AddEdge(new GraphEdge(3, 1));
		graph.AddEdge(new GraphEdge(3, 4));
		graph.AddEdge(new GraphEdge(3, 5));
		graph.AddEdge(new GraphEdge(4, 3));
		graph.AddEdge(new GraphEdge(4, 5));
		graph.AddEdge(new GraphEdge(5, 4));
		graph.AddEdge(new GraphEdge(5, 3));
	}

	private void DijkstraSearch() {
		var dfs = new GraphSearchDijkstra(graph, 4, 2);
		var path = dfs.GetPathToTarget();
		string pathString = string.Empty;
		foreach (var nodeIdx in path) pathString += nodeIdx + ";";
		Debug.Log(pathString);
	}

	private void BreadthFirstSearch() {
		var dfs = new GraphSerachBFS(graph, 4, 2);
		var path = dfs.GetPathToTarget();
		string pathString = string.Empty;
		foreach (var nodeIdx in path) pathString += nodeIdx + ";";
		Debug.Log(pathString);
	}

	private void DepthFirstSearch () {
		var dfs = new GraphSearchDFS(graph, 4, 2);
		var path = dfs.GetPathToTarget();
		string pathString = string.Empty;
		foreach (var nodeIdx in path) pathString += nodeIdx + ";";
		Debug.Log(pathString);
	}
}
