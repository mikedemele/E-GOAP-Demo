using System;
using System.Collections.Generic;
using Demo.UI;
using EGoap.Source.Agents;
using EGoap.Source.Debug;
using EGoap.Source.Extensions.Unity;
using EGoap.Source.Planning;
using EGoap.Source.Planning.Preconditions;
using EGoap.Source.Utils.Time;
using UnityEngine;
using UnityEngine.AI;

namespace Demo
{
	public partial class Townsman : MonoBehaviour
	{
		private const float ToolAnimationPeriod = 0.5f;
		private const float IdleToolAngle = 75f;
		private const float MaxToolAngle = 190f;

		[SerializeField]
		private PlanExecutionStatePanel planExecutionStatePanel;

		[SerializeField]
		private Town town;
		[SerializeField]
		private Transform toolTransform;
		[SerializeField]
		private Transform resourceTransform;

		[SerializeField]
		private ToolType toolType;
		[SerializeField]
		private ResourceType resourceType;

		[SerializeField]
		private bool learn;

		[SerializeField]
		private double experienceWeight;

		private Agent agent;
		
		private NavMeshAgent navMeshAgent;

		private IDictionary<ToolType, GameObject> toolViews;
		private IDictionary<ResourceType, GameObject> resourceViews;

		private GameObject currentToolView;
		private GameObject currentResourceView;

		private bool dirtyView = true;
		private bool animateTool;

		private IResettableTimer toolAnimationTimer;

		private void Start()
		{
			this.EnsureRequiredFieldsAreSetInEditor();

			// Init AI
			var knowledgeProvider = new KnowledgeProvider(this);
			var goalSelector = new GoalSelector.Builder()
				.WithReevaluationPeriod(3.0f)
				.WithGoal(
					new Goal("BuildHouse",
						new List<IPrecondition>
						{
							new IsTrue(new SymbolId("HouseBuilt"))
						}
					),
					() => town.House.IsBuilt ? -1 : 1
				)
				.WithGoal(
					new Goal("DestroyHouse",
						new List<IPrecondition>
						{
							new IsTrue(new SymbolId("HouseBuilt"))
						}
					),
					() => town.House.IsBuilt ? 1 : -1
				)
				.WithGoal(
					new Goal("BuildHouse",
						new List<IPrecondition>
						{
							new IsTrue(new SymbolId("HouseBuilt"))
						}
					),
					() => town.House.IsBuilt ? -1 : 1
				)
				.Build();
			var actionFactory = GetActionFactory();

			var planner = new ForwardPlanner(100, 200, learn, experienceWeight);
			var planExecutor = new PlanExecutor(actionFactory);

			agent = new Agent(
				new AgentConfiguration.Builder()
				.WithKnowledgeProvider(knowledgeProvider)
				.WithGoalSelector(goalSelector)
				.WithPlanner(planner)
				.WithPlanExecutor(planExecutor)
				.Build()
			);

			navMeshAgent = GetComponent<NavMeshAgent>();
			DebugUtils.Assert(navMeshAgent != null, "{0} requires {1} to be present", GetType(), typeof(NavMeshAgent));

			// Init view
			toolAnimationTimer = new ResettableStopwatchExecutionTimer(true);

			// Init UI
			planExecutionStatePanel.PlanExecution = planExecutor.CurrentExecution;
		}

		private void Update()
		{
			// Update AI
			agent.Update();

			// Init view (if needed)
			if(toolViews == null)
			{
				toolViews = new Dictionary<ToolType, GameObject>();
				foreach(var kvp in town.ToolPrefabs)
				{
					var view = Instantiate(kvp.Value, toolTransform.position, toolTransform.rotation);
					view.transform.parent = toolTransform;
					toolViews.Add(
						kvp.Key,
						view
					);
					SetVisible(view, false);
				}
			}
			if(resourceViews == null)
			{
				resourceViews = new Dictionary<ResourceType, GameObject>();
				foreach(var kvp in town.ResourcePrefabs)
				{
					var view = Instantiate(kvp.Value, resourceTransform.position, resourceTransform.rotation);
					view.transform.parent = resourceTransform;
					resourceViews.Add(
						kvp.Key,
						view
					);
					SetVisible(view, false);
				}
			}

			// Update view
			if(dirtyView)
			{
				foreach(var kvp in toolViews)
				{
					SetVisible(kvp.Value, (kvp.Key == toolType));
				}
				foreach(var kvp in resourceViews)
				{
					SetVisible(kvp.Value, (kvp.Key == resourceType));
				}
				dirtyView = false;
			}

			var animSeconds = toolAnimationTimer.ElapsedSeconds;
			var animSecondsRemainder = (float)(animSeconds - Math.Floor(animSeconds / (ToolAnimationPeriod))*ToolAnimationPeriod);
			var currentToolAngle = Mathf.Lerp(
				IdleToolAngle,
				MaxToolAngle, 
				1 - Math.Abs(2*(animSecondsRemainder / ToolAnimationPeriod) - 1)
			);
			toolTransform.localRotation = Quaternion.Euler(currentToolAngle, 0, -90);
		}

		private static void SetVisible(GameObject gameObject, bool visible)
		{
			var renderers = gameObject.GetComponentsInChildren<Renderer>();
			foreach(var currentRenderer in renderers)
			{
				currentRenderer.enabled = visible;
			}
		}

		private GameObject FindClosestTree()
		{
			return DemoUtils.FindClosestWithTag(transform.position, "src_tree");
		}

		private void MoveTo(Vector3 position)
		{
			navMeshAgent.destination = position;
		}

		private bool ReachedDestination => !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.0001f;

		private ToolType CurrentTool
		{
			get => toolType;
			set {
				toolType = value;
				dirtyView = true;
			}
		}

		private ResourceType CurrentResource
		{
			get => resourceType;
			set {
				resourceType = value;
				dirtyView = true;
			}
		}

		private bool AnimateTool
		{
			set {
				if(!animateTool && value)
				{
					toolAnimationTimer.Reset(false);
				}
				else if(animateTool && !value)
				{
					toolAnimationTimer.Reset();
				}
				animateTool = value;
			}
		}
	}
}

