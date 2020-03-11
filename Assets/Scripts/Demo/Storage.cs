using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
	public class Storage : MonoBehaviour
	{
		[Serializable]
		private struct StackViewPair
		{
			public ResourceType resourceType;
			public StackView stackView;
		}

		[SerializeField]
		private int logs;
		[SerializeField]
		private int planks;
		[SerializeField]
		private int ore;
		[SerializeField]
		private int iron;
		[SerializeField]
		private int stone;
		[SerializeField]
		private Town town;
		[SerializeField]
		private StackViewPair[] stackViewPairs;

		private IDictionary<ResourceType, StackView> stackViews;

		private void Start()
		{
			stackViews = new Dictionary<ResourceType, StackView>();
			foreach(var pair in stackViewPairs)
			{
				pair.stackView.SingleViewPrefab = town.ResourcePrefabs[pair.resourceType];
				stackViews.Add(pair.resourceType, pair.stackView);
			}
		}

		private void Update()
		{
			SafeUpdateStack(ResourceType.Logs, logs);
			SafeUpdateStack(ResourceType.Planks, planks);
			SafeUpdateStack(ResourceType.Ore, ore);
			SafeUpdateStack(ResourceType.Iron, iron);
			SafeUpdateStack(ResourceType.Stone, stone);
		}

		private void SafeUpdateStack(ResourceType resourceType, int count)
		{
			if(stackViews.ContainsKey(resourceType))
			{
				stackViews[resourceType].Count = count;
			}
		}

		public int GetResourceCount(ResourceType resourceType)
		{
			switch(resourceType)
			{
			default:
				throw new ArgumentException($"unrecognized resource type {resourceType}", nameof(resourceType));
			case ResourceType.None:
				throw new ArgumentException(
					$"Resource type {resourceType} cannot be used as an argument for GetResourceCount()", nameof(resourceType));
			case ResourceType.Logs:
				return logs;
			case ResourceType.Planks:
				return planks;
			case ResourceType.Ore:
				return ore;
			case ResourceType.Iron:
				return iron;
			case ResourceType.Stone:
				return stone;
			}
		}

		public void SetResourceCount(ResourceType resourceType, int count)
		{
			switch(resourceType)
			{
			default:
				throw new ArgumentException($"unrecognized resource type {resourceType}", nameof(resourceType));
			case ResourceType.None:
				throw new ArgumentException(
					$"Resource type {resourceType} cannot be used as an argument for GetResourceCount()", nameof(resourceType));
			case ResourceType.Logs:
				logs = count;
				break;
			case ResourceType.Planks:
				planks = count;
				break;
			case ResourceType.Ore:
				ore = count;
				break;
			case ResourceType.Iron:
				iron = count;
				break;
			case ResourceType.Stone:
				stone = count;
				break;
			}
		}

		public void Clear()
		{
			logs = 0;
			planks = 0;
			ore = 0;
			iron = 0;
			stone = 0;
		}

		public int Logs
		{
			get {
				return logs;
			}
			set {
				logs = value;
			}
		}

		public int Planks
		{
			get {
				return planks;
			}
			set {
				planks = value;
			}
		}

		public int Ore
		{
			get {
				return ore;
			}
			set {
				ore = value;
			}
		}

		public int Iron
		{
			get {
				return iron;
			}
			set {
				iron = value;
			}
		}

		public int Stone
		{
			get {
				return stone;
			}
			set {
				stone = value;
			}
		}
	}
}

