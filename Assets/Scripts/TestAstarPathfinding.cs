using System;
using System.Linq;
using System.Collections.Generic;

using EGoap.Source.Graphs;
using EGoap.Source.Planning;
using EGoap.Source.Utils.Time;
using UnityEngine;

// Test case for the base A* pathfinder
public class TestAStarPathfinding : MonoBehaviour
{
	private class Node : IGraphNode<Node>
	{
		private readonly IList<IGraphEdge<Node>> outgoingEdges;

		public Node(string nodeName)
		{
			Name = nodeName;
			outgoingEdges = new List<IGraphEdge<Node>>();
		}

		#region IGraphNode implementation

		public IEnumerable<IGraphEdge<Node>> OutgoingEdges => outgoingEdges;

		#endregion

		public override string ToString()
		{
			return Name;
		}

		public void AddOutgoingEdge(IGraphEdge<Node> outgoingEdge)
		{
			if(!outgoingEdge.SourceNode.Equals(this))
			{
				throw new ArgumentException("outgoingEdge must have this Node as its source", nameof(outgoingEdge));
			}
			outgoingEdges.Add(outgoingEdge);
		}

		public override bool Equals(object obj)
		{
			return obj is Node otherNode && Name.Equals(otherNode.Name);
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		public string Name { get; }
	}

	private class Edge : IGraphEdge<Node>
	{
		#region IGraphEdge implementation

		public double Cost {get;}

		public PlanningAction GetAction()
		{
			throw new NotImplementedException();
		}

		public Node SourceNode {get;}

		public Node TargetNode {get;}

		#endregion

		public Edge(Node sourceNode, Node targetNode, double cost)
		{
			SourceNode = sourceNode;
			TargetNode = targetNode;
			Cost = cost;
		}

		public override string ToString()
		{
			return $"[Edge: Cost={Cost}, SourceNode={SourceNode}, TargetNode={TargetNode}]";
		}
	}

	private static double Heuristic(Node sourceNode, Node targetNode)
	{
		if(!targetNode.Name.Equals("F"))
		{
			throw new NotImplementedException(
				$"Heuristic is not implemented for any target node, except for F (got {targetNode.Name})"
			);
		}

		switch(sourceNode.Name)
		{
		default:
			throw new ArgumentException($"Unknown source node {sourceNode.Name}", nameof(sourceNode));
		case "A":
			return 4;
		case "B":
			return 3;
		case "C":
			return 3;
		case "D":
			return 2;
		case "E":
			return 2;
		case "F":
			return 0;
		}
	}

	// Use this for initialization
	private void Start()
	{
		var a = new Node("A");
		var b = new Node("B");
		var c = new Node("C");
		var d = new Node("D");
		var e = new Node("E");
		var f = new Node("F");

		a.AddOutgoingEdge(new Edge(a, b, 4));
		a.AddOutgoingEdge(new Edge(a, c, 2));
		a.AddOutgoingEdge(new Edge(a, f, 50));
		b.AddOutgoingEdge(new Edge(b, c, 5));
		b.AddOutgoingEdge(new Edge(b, d, 10));
		c.AddOutgoingEdge(new Edge(c, e, 3));
		e.AddOutgoingEdge(new Edge(e, d, 4));
		d.AddOutgoingEdge(new Edge(d, f, 11));

		var pathfinder = new AStarPathfinder<Node>(Heuristic);
		var timer = new StopwatchExecutionTimer();
		var path = pathfinder.FindPath(a, f);
		print(string.Format("In {1} seconds found the following path with cost {0} from A to F:", path.Cost, timer.ElapsedSeconds));
		print(path.Edges.Aggregate("", (soFar, edge) => soFar + (soFar.Length > 0 ? " -> " : "") + edge));
	}
}
