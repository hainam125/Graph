using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Graph {
	protected List<GraphNode> m_Nodes = new List<GraphNode>();
	protected List<LinkedList<GraphEdge>> m_Edges = new List<LinkedList<GraphEdge>>();
	protected bool m_bDigraph;
	protected int m_iNextNodeIndex;

	public int NumNodes() {
		return m_Nodes.Count;
	}

	public LinkedList<GraphEdge> GetEdgesAt(int nodeIdx) {
		return m_Edges[nodeIdx];
	}

	public GraphNode GetNode(int idx) {
		return m_Nodes[idx];
	}
}

public class SparseGraph : Graph {
	public SparseGraph(bool digraph) {
		m_iNextNodeIndex = 0;
		m_bDigraph = digraph;
	}

	private bool UniqueEdge(int from, int to) {
		var curEdge = m_Edges[from].First;
		while (curEdge != null) {
			if (curEdge.Value.To == to) return false;
			curEdge = curEdge.Next;
		}
		return true;
	}

	public GraphEdge GetEdge(int from, int to) {
		var curEdge = m_Edges[from].First;
		while (curEdge != null) {
			if (curEdge.Value.To == to) return curEdge.Value;
			curEdge = curEdge.Next;
		}
		throw new Exception("Not Exist!");
	}

	public int GetNextFreeNodeIndex() {
		return m_iNextNodeIndex;
	}

	private void CullInvalidEdges() {
		foreach (var curEdgeList in m_Edges) {
			var curEdge = curEdgeList.First;
			while (curEdge != null) {
				if (m_Nodes[curEdge.Value.To].Index == Config.InvalidIndex || m_Nodes[curEdge.Value.From].Index == Config.InvalidIndex) {
					var nextEdge = curEdge.Next;
					curEdgeList.Remove(curEdge);
					curEdge = nextEdge;
					continue;
				}
				curEdge = curEdge.Next;
			}
		}
	}

	public int AddNode(GraphNode node) {
		if (node.Index < m_Nodes.Count) {
			m_Nodes[node.Index] = node;
			return m_iNextNodeIndex;
		}
		else {
			m_Nodes.Add(node);
			m_Edges.Add(new LinkedList<GraphEdge>());
			return m_iNextNodeIndex++;
		}
	}

	public void RemoveNode(int node) {
		//set this node's index to invalid_node_index
		m_Nodes[node].Index = Config.InvalidIndex;
		//if the graph is not directed remove all edges leading to this node and then
		//clear the edges leading from the node
		if (!m_bDigraph) {
			var curEdge = m_Edges[node].First;
			while (curEdge != null) {
				var curE = m_Edges[curEdge.Value.To].First;
				while (curE != null) {
					if (curE.Value.To == node) {
						m_Edges[curEdge.Value.To].Remove(curE);
						break;
					}
					curE = curE.Next;
				}
				curEdge = curEdge.Next;
			}
			m_Edges[node].Clear();
		}
		//else remove the edges the slow way
		else {
			CullInvalidEdges();
		}
	}

	public void AddEdge(GraphEdge edge) {
		if (m_Nodes[edge.To].Index == Config.InvalidIndex || m_Nodes[edge.From].Index == Config.InvalidIndex) {
			throw new Exception("Wrong edge");
		}

		//add the edge, first make sure it is unique
		if (UniqueEdge(edge.From, edge.To)) {
			m_Edges[edge.From].AddLast(edge);
		}
		//if the graph is undirected we must add another connection in the opposite direction
		if (!m_bDigraph) {
			if (UniqueEdge(edge.To, edge.From)) {
				var newEdge = new GraphEdge();
				newEdge.Cost = edge.Cost;
				newEdge.To = edge.From;
				newEdge.From = edge.To;
				m_Edges[edge.To].AddLast(newEdge);
			}
		}
	}

	public void RemoveEdge(int from, int to) {
		if (!m_bDigraph) {
			var curEdge = m_Edges[to].First;
			while (curEdge != null) {
				if (curEdge.Value.To == from) {
					m_Edges[to].Remove(curEdge);
					break;
				}
				curEdge = curEdge.Next;
			}
		}
		{
			var curEdge = m_Edges[from].First;
			while (curEdge != null) {
				if (curEdge.Value.To == to) {
					m_Edges[to].Remove(curEdge);
					break;
				}
				curEdge = curEdge.Next;
			}
		}
	}

	private void SetEdgeCost(int from, int to, float cost) {
		var curEdge = m_Edges[from].First;
		while (curEdge != null) {
			if (curEdge.Value.To == to) {
				curEdge.Value.Cost = cost;
			}
			curEdge = curEdge.Next;
		}
	}

	public int NumActiveNodes() {
		int count = 0;
		for (int i = 0; i < m_Nodes.Count; i++) {
			if (m_Nodes[i].Index != Config.InvalidIndex) count++;
		}
		return count;
	}

	public int NumEdges() {
		int total = 0;
		for (int i = 0; i < m_Edges.Count; i++) {
			total += m_Edges[i].Count;
		}
		return total;
	}

	public bool isDigraph() {
		return m_bDigraph;
	}

	public bool isEmpty() {
		return m_Nodes.Count == 0;
	}

	public bool isNodePresent(int nd) {
		return m_Nodes[nd].Index != Config.InvalidIndex && nd < m_Nodes.Count;
	}

	public bool isEdgePresent(int from, int to) {
		if (isNodePresent(from) && isNodePresent(to)) {
			var curEdge = m_Edges[from].First;
			while (curEdge != null) {
				if (curEdge.Value.To == to) return true;
				curEdge = curEdge.Next;
			}
		}
		return false;
	}

	public void Clear() {
		m_iNextNodeIndex = 0;
		m_Nodes.Clear();
		m_Edges.Clear();
	}

	public void RemoveEdges() {
		for (int i = 0; i < m_Edges.Count; i++) {
			m_Edges[i].Clear();
		}
	}
}
