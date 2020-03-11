using UnityEngine;

namespace Demo
{
	public static class DemoUtils
	{
		public static GameObject FindClosestWithTag(Vector3 position, string tag)
		{
			var gos = GameObject.FindGameObjectsWithTag(tag);
			GameObject closest = null;
			var distance = Mathf.Infinity;
			foreach (var go in gos) {
				var diff = go.transform.position - position;
				var curDistance = diff.sqrMagnitude;
				if (curDistance < distance) {
					closest = go;
					distance = curDistance;
				}
			}
			return closest;
		}
	}
}

