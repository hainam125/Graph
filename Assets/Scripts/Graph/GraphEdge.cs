using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEdge {
	protected int m_iFrom;
	protected int m_iTo;
	protected float m_dCost;

	public int From { 
		set { m_iFrom = value; }
		get { return m_iFrom; }
	}

	public int To {
		set { m_iTo = value; }
		get { return m_iTo; }
	}

	public float Cost {
		set { m_dCost = value; }
		get { return m_dCost; }
	}

	public GraphEdge(int from, int to, float cost = 1.0f) {
		m_iFrom = from;
		m_iTo = to;
		m_dCost = cost;
	}

	public GraphEdge(float cost = 1.0f) {
		m_iFrom = Config.InvalidIndex;
		m_iTo = Config.InvalidIndex;
		m_dCost = cost;
	}

	public static bool Equals(GraphEdge obj1, GraphEdge obj2) {
		return obj2 != null &&
					 obj1.m_iFrom == obj2.m_iFrom &&
	         obj1.m_iTo == obj2.m_iTo &&
	         obj1.m_dCost == obj2.m_dCost;
	}
}

public class NavGraphEdge : GraphEdge {
	public enum Flag {
		normal = 0,
		swim = 1<<0,
		crawl = 1<<1, 
		creep = 1<<2,
		jump = 1<<3,
		fly = 1<<4,
		grapple = 1<<5,
		goes_through_door = 1<<6
	}

	protected int m_iFlags;
	protected int m_iIDofIntersectingEntity;

	public NavGraphEdge(int from, int to, float cost, int flags = 0, int id = -1) : base(from, to, cost) {
		m_iFlags = flags;
		m_iIDofIntersectingEntity = id;
	}

	public int Flags {
		get { return m_iFlags; }
		set { m_iFlags = value; }
	}

	public int IDofIntersectingEntity {
		get { return m_iIDofIntersectingEntity; }
		set { m_iIDofIntersectingEntity = value; }
	}
}
