using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
	public class StackView : MonoBehaviour
	{
		[SerializeField]
		private GameObject singleViewPrefab;
		[SerializeField]
		private int count;
		[SerializeField]
		private int viewsPerRow = 3;
		[SerializeField]
		private float horizontalSpacing;
		[SerializeField]
		private float verticalSpacing;

		private bool dirtyView = true;
		private Stack<GameObject> views;

		private void Start()
		{
			views = new Stack<GameObject>();
		}

		private void Update()
		{
			if(dirtyView && singleViewPrefab != null)
			{
				if(count > views.Count)
				{
					var newViewsNeeded = count - views.Count;
					for(var i = 0; i < newViewsNeeded; i++)
					{
						var newView = Instantiate(singleViewPrefab, transform, true);
						newView.transform.localRotation = Quaternion.identity;
						var newViewRenderer = newView.GetComponentInChildren<Renderer>();
						var bounds = newViewRenderer.bounds;
						newView.transform.localPosition 
							= ((views.Count + 1) % viewsPerRow) * (bounds.size.x + horizontalSpacing)* Vector3.right
								+
							((float)(views.Count + 1) / viewsPerRow) * (bounds.size.y + verticalSpacing) * Vector3.up;

						views.Push(newView);

					}
				}
				else
				{
					var extraViews = views.Count - count;
					for(var i = 0; i < extraViews; i++)
					{
						Destroy(views.Pop());
					}
				}
				dirtyView = false;
			}
		}

		public GameObject SingleViewPrefab
		{
			get => singleViewPrefab;
			set {
				singleViewPrefab = value;
				dirtyView = true;
				views.Clear();
			}
		}

		public int Count
		{
			get => count;
			set {
				count = value;
				dirtyView = true;
			}
		}

		public int ViewsPerRow
		{
			get => viewsPerRow;
			set {
				viewsPerRow = value;
				dirtyView = true;
			}
		}
	}
}

