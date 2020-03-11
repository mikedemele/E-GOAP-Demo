using EGoap.Source.Extensions.Unity;
using UnityEngine;

namespace Demo
{
	public class House : MonoBehaviour 
	{
		[SerializeField]
		private GameObject constructionSitePrefab;
		[SerializeField]
		private GameObject housePrefab;

		private GameObject constructionSite;
		private GameObject house;

		private void Start()
		{
			this.EnsureRequiredFieldsAreSetInEditor();

			var transform1 = transform;
			var position = transform1.position;
			var rotation = transform1.rotation;
			constructionSite = Instantiate(constructionSitePrefab, position, rotation);
			house = Instantiate(housePrefab, position, rotation);
		}

		private void Update()
		{
			constructionSite.GetComponentInChildren<Renderer>().enabled = !IsBuilt;
			house.GetComponentInChildren<Renderer>().enabled = IsBuilt;
		}

		public bool IsBuilt { get; set; }

		public Transform BuilderPosition { get; set; }
	}
}

