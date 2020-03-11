using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
	public class ToolBench : MonoBehaviour
	{
		[Serializable]
		private struct StackViewPair
		{
			public ToolType toolType;
			public StackView stackView;
		}

		[SerializeField]
		private int axes;
		[SerializeField]
		private int pickaxes;
		[SerializeField]
		private int hammers;
		[SerializeField]
		private int saws;

		[SerializeField]
		private Town town;
		[SerializeField]
		private StackViewPair[] stackViewPairs;

		private IDictionary<ToolType, StackView> stackViews;

		private void Start()
		{
			stackViews = new Dictionary<ToolType, StackView>();
			foreach(var pair in stackViewPairs)
			{
				pair.stackView.SingleViewPrefab = town.ToolPrefabs[pair.toolType];
				stackViews.Add(pair.toolType, pair.stackView);
			}
		}

		private void Update()
		{
			SafeUpdateStack(ToolType.Axe, axes);
			SafeUpdateStack(ToolType.Pickaxe, pickaxes);
			SafeUpdateStack(ToolType.Hammer, hammers);
			SafeUpdateStack(ToolType.Saw, saws);
		}

		private void SafeUpdateStack(ToolType toolType, int count)
		{
			if(stackViews.ContainsKey(toolType))
			{
				stackViews[toolType].Count = count;
			}
		}

		public int GetToolCount(ToolType toolType)
		{
			switch(toolType)
			{
			default:
				throw new ArgumentException($"unrecognized resource type {toolType}", nameof(toolType));
			case ToolType.None:
				throw new ArgumentException(
					$"Resource type {toolType} cannot be used as an argument for GetResourceCount()", nameof(toolType));
			case ToolType.Axe:
				return axes;
			case ToolType.Pickaxe:
				return pickaxes;
			case ToolType.Hammer:
				return hammers;
			case ToolType.Saw:
				return saws;
			}
		}

		public void SetToolCount(ToolType toolType, int count)
		{
			switch(toolType)
			{
			default:
				throw new ArgumentException($"unrecognized resource type {toolType}", nameof(toolType));
			case ToolType.None:
				throw new ArgumentException(
					$"Resource type {toolType} cannot be used as an argument for GetResourceCount()", nameof(toolType));
			case ToolType.Axe:
				axes = count;
				break;
			case ToolType.Pickaxe:
				pickaxes = count;
				break;
			case ToolType.Hammer:
				hammers = count;
				break;
			case ToolType.Saw:
				saws = count;
				break;
			}
		}

		public void Clear()
		{
			axes = 0;
			pickaxes = 0;
			hammers = 0;
			saws = 0;
		}

		public int Axes
		{
			get => axes;
			set => axes = value;
		}

		public int Pickaxes
		{
			get => pickaxes;
			set => pickaxes = value;
		}

		public int Hammers
		{
			get => hammers;
			set => hammers = value;
		}

		public int Saws
		{
			get => saws;
			set => saws = value;
		}
	}
}

