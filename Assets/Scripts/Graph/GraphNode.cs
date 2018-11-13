using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode {
	protected int m_iIndex;
	public int Index {
		set { m_iIndex = value; }
		get { return m_iIndex; }
	}

	public GraphNode(int idx) {
		m_iIndex = idx;
	}

	public GraphNode() {
		m_iIndex = Config.InvalidIndex;
	}
}

public class NavGraphNode<T> : GraphNode where T : new() {
	private Vector2 m_vPosition;
	private T m_ExtraInfo;

	public Vector2 Pos {
		get { return m_vPosition; }
		set { m_vPosition = value; }
	}

	public T ExtraInfo {
		get { return m_ExtraInfo; }
		set { m_ExtraInfo = value; }
	}

	public NavGraphNode() {
		m_ExtraInfo = new T();
	}

	public NavGraphNode(int idx, Vector2 pos) : base(idx) {
		m_vPosition = pos;
		m_ExtraInfo = new T();
	}
}