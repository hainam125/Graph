using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSearchDijkstra {
	private Graph m_Graph;
	//this vector contains the edges that comprise the shortest path tree -
	//a directed sub-tree of the graph that encapsulates the best paths from
	//every node on the SPT to the source node.
	private List<GraphEdge> m_ShortestPathTree = new List<GraphEdge>();

	//this is indexed into by node index and holds the total cost of the best
	//path found so far to the given node. For example, m_CostToThisNode[5]
	//will hold the total cost of all the edges that comprise the best path
	//to node 5 found so far in the search (if node 5 is present and has
	//been visited of course).
	private List<float> m_CostToThisNode = new List<float>();

	//this is an indexed (by node) vector of "parent" edges leading to nodes
	//connected to the SPT but that have not been added to the SPT yet.
	private List<GraphEdge> m_SearchFrontier = new List<GraphEdge>();

	private int m_iSource;
	private int m_iTarget;
	private bool m_bFound;

	public GraphSearchDijkstra(Graph graph, int source, int target = -1) {
		m_Graph = graph;
		m_iSource = source;
		m_iTarget = target;
		m_bFound = false;
		int graphNumNode = graph.NumNodes();
		for (int i = 0; i < graphNumNode; i++) {
			m_ShortestPathTree.Add(null);
			m_SearchFrontier.Add(null);
			m_CostToThisNode.Add(int.MaxValue);
		}
		Search();
	}

	private int Compare(GraphNode nodeA, GraphNode nodeB) {
		return -m_CostToThisNode[nodeA.Index].CompareTo(m_CostToThisNode[nodeB.Index]);
	}

	private void Search() {
		var pq = new Heap<GraphNode>(m_Graph.NumNodes(), Compare);
		pq.Add(m_Graph.GetNode(m_iSource));
		m_CostToThisNode[m_iSource] = 0;
		m_SearchFrontier[m_iSource] = new GraphEdge();
		while (pq.Count > 0) {
			//get the lowest cost node from the queue. Don't forget, the return value
			//is a *node index*, not the node itself. This node is the node not already
			//on the SPT that is the closest to the source node
			int nextClosestNode = pq.RemoveFirst().Index;

			//move this edge from the search frontier to the shortest path tree
			m_ShortestPathTree[nextClosestNode] = m_SearchFrontier[nextClosestNode];

			if (nextClosestNode == m_iTarget) {
				m_bFound = true;
				return;
			}
			//now to relax the edges. For each edge connected to the next closest node
			var edgeList = m_Graph.GetEdgesAt(nextClosestNode);
			var edgeNode = edgeList.First;

			while (edgeNode != null) {
				var edgeNodeVal = edgeNode.Value;
				float newCost = m_CostToThisNode[nextClosestNode] + edgeNodeVal.Cost;
				//if this edge has never been on the frontier make a note of the cost
				//to reach the node it points to, then add the edge to the frontier
				//and the destination node to the PQ.
				if (m_SearchFrontier[edgeNodeVal.To] == null) {
					m_CostToThisNode[edgeNodeVal.To] = newCost;
					pq.Add(m_Graph.GetNode(edgeNodeVal.To));
					m_SearchFrontier[edgeNodeVal.To] = edgeNodeVal;
				}
				//else test to see if the cost to reach the destination node via the
				//current node is cheaper than the cheapest cost found so far. If
				//this path is cheaper we assign the new cost to the destination
				//node, update its entry in the PQ to reflect the change, and add the
				//edge to the frontier
				else if (newCost < m_CostToThisNode[edgeNodeVal.To] && m_ShortestPathTree[edgeNodeVal.To] == null) {
					m_CostToThisNode[edgeNodeVal.To] = newCost;
					//because the cost is less than it was previously, the PQ must be
					//resorted to account for this
					pq.UpdateItem(m_Graph.GetNode(edgeNodeVal.To));
					m_SearchFrontier[edgeNodeVal.To] = edgeNodeVal;
				}
				edgeNode = edgeNode.Next;
			}
		}
	}

	public List<GraphEdge> GetSPT() {
		return m_ShortestPathTree;
	}

	public List<int> GetPathToTarget() {
		var path = new List<int>();
		if (m_iTarget < 0 || !m_bFound) return path;
		int nd = m_iTarget;
		path.Add(nd);
		while (nd != m_iSource && m_ShortestPathTree[nd] != null) {
			nd = m_ShortestPathTree[nd].From;
			path.Add(nd);
		}
		path.Reverse();
		return path;
	}

	public float GetCostToTarget() {
		return m_CostToThisNode[m_iTarget];
	}
}
