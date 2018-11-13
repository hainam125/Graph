using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSerachBFS {
	private enum NodeStatus { Visited, UnVisited }
	private Graph m_Graph;
	//this records the indexes of all the nodes that are visited as the search progresses
	private List<NodeStatus> m_Visted = new List<NodeStatus>();
	//this holds the route taken to the target.
	private List<int> m_Route = new List<int>();
	//the source and target node indices
	private int m_iSource;
	private int m_iTarget;
	private bool m_bFound;

	public bool Found { get { return m_bFound; } }

	public GraphSerachBFS(Graph graph, int source, int target = -1) {
		m_Graph = graph;
		m_iSource = source;
		m_iTarget = target;
		m_bFound = false;
		int graphNumNode = graph.NumNodes();
		for (int i = 0; i < graphNumNode; i++) {
			m_Visted.Add(NodeStatus.UnVisited);
			m_Route.Add(-1);//-1 ~ No Parent
		}
		m_bFound = Search();
	}

	public List<int> GetPathToTarget() {
		var path = new List<int>();
		if (!m_bFound || m_iTarget < 0) return path;
		int nd = m_iTarget;
		path.Add(nd);
		while (nd != m_iSource) {
			nd = m_Route[nd];
			path.Add(nd);
		}
		path.Reverse();
		return path;
	}

	private bool Search() {
		var queue = new Queue<GraphEdge>();
		var dummy = new GraphEdge(m_iSource, m_iSource, 0);
		queue.Enqueue(dummy);
		m_Visted[m_iSource] = NodeStatus.Visited;

		while (queue.Count > 0) {
			GraphEdge next = queue.Peek();
			queue.Dequeue();
			//make a note of the parent of the node this edge points to
			m_Route[next.To] = next.From;
			//if the target has been found the method can return success
			if (next.To == m_iTarget) return true;

			var edgeList = m_Graph.GetEdgesAt(next.To);
			var edge = edgeList.First;
			while (edge != null) {
				if (m_Visted[edge.Value.To] == NodeStatus.UnVisited) {
					queue.Enqueue(edge.Value);
					m_Visted[edge.Value.To] = NodeStatus.Visited;
				}
				edge = edge.Next;
			}
		}
		return false;
	}
}
