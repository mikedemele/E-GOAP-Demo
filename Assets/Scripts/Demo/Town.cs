using System;
using System.Collections.Generic;
using EGoap.Source.Extensions.Unity;
using UnityEngine;

namespace Demo
{
	public class Town : MonoBehaviour
	{
		[Serializable]
		private struct ToolPrefabPair
		{
			public ToolType toolType;
			public GameObject prefab;
		}

		[Serializable]
		private struct ResourcePrefabPair
		{
			public ResourceType resourceType;
			public GameObject prefab;
		}

		[SerializeField]
		private Storage mainStorage;
		[SerializeField]
		private Storage constructionStorage;
		[SerializeField]
		private ToolBench toolBench;
		[SerializeField]
		private House house;
		[SerializeField]
		private ToolPrefabPair[] toolPrefabPairs;
		[SerializeField]
		private ResourcePrefabPair[] resourcePrefabPairs;

		private void Start()
		{
			this.EnsureRequiredFieldsAreSetInEditor();

			ToolPrefabs = new Dictionary<ToolType, GameObject>();
			foreach(var pair in toolPrefabPairs)
			{
				ToolPrefabs.Add(pair.toolType, pair.prefab);
			}

			ResourcePrefabs = new Dictionary<ResourceType, GameObject>();
			foreach(var pair in resourcePrefabPairs)
			{
				ResourcePrefabs.Add(pair.resourceType, pair.prefab);
			}
		}

		public Storage MainStorage => mainStorage;

		public Storage ConstructionStorage => constructionStorage;

		public ToolBench ToolBench
		{
			get => toolBench;
			set => toolBench = value;
		}

		public House House
		{
			get => house;
			set => house = value;
		}

		public IDictionary<ToolType, GameObject> ToolPrefabs { get; private set; }

		public IDictionary<ResourceType, GameObject> ResourcePrefabs { get; private set; }
	}
}

