using System;
using System.Collections.Generic;
using EGoap.Source.Agents;
using EGoap.Source.Debug;
using EGoap.Source.Planning;
using EGoap.Source.Planning.Effects;
using EGoap.Source.Planning.Preconditions;
using EGoap.Source.Utils.Time;
using UnityEngine;

namespace Demo
{
	public partial class Townsman
	{
		private IActionFactory GetActionFactory()
		{
			var actions = new Dictionary<PlanningAction, Func<IAction>>
			{
				{
					new PlanningAction(
						"BuildHouse",
						new List<IPrecondition>
						{
							new IsEqual(new SymbolId("MT"), (int)ToolType.Hammer),
							new IsNotSmaller(new SymbolId("CPlanks"), 4),
							new IsNotSmaller(new SymbolId("CIron"), 2)
						},
						new List<IEffect>
						{
							new SetTrue(new SymbolId("HouseBuilt")),
							new Subtract(new SymbolId("CPlanks"), 4),
							new Subtract(new SymbolId("CIron"), 2)
						},
						30
					),
					() => new ActionBuildHouse(this)
				},
				{
					new PlanningAction(
						"DestroyHouse",
						new List<IPrecondition>
						{
							new IsTrue(new SymbolId("HouseBuilt")),
						},
						new List<IEffect>
						{
							new SetFalse(new SymbolId("HouseBuilt")),
						},
						30
					),
					() => new ActionDestroyHouse(this)
				},
				{
					new PlanningAction(
						"GetPlanksFromStorage",
						new List<IPrecondition>
						{
							new IsEqual(new SymbolId("MR"), 0),
							new IsNotSmaller(new SymbolId("SPlanks"), 1)
						},
						new List<IEffect>
						{
							new SetValue(new SymbolId("MR"), (int)ResourceType.Planks),
							new Subtract(new SymbolId("SPlanks"), 1),
						},
						2
					),
					() => new ActionGetResource(this, ResourceType.Planks, town.MainStorage)
				},
				{
					new PlanningAction(
						"GetIronFromStorage",
						new List<IPrecondition>
						{
							new IsEqual(new SymbolId("MR"), 0),
							new IsNotSmaller(new SymbolId("SIron"), 1)
						},
						new List<IEffect>
						{
							new SetValue(new SymbolId("MR"), (int)ResourceType.Iron),
							new Subtract(new SymbolId("SIron"), 1),
						},
						2
					),
					() => new ActionGetResource(this, ResourceType.Iron, town.MainStorage)
				},
				{
					new PlanningAction(
						"PutPlanksIntoStorage",
						new List<IPrecondition>
						{
							new IsEqual(new SymbolId("MR"), (int)ResourceType.Planks)
						},
						new List<IEffect>
						{
							new SetValue(new SymbolId("MR"), (int)ResourceType.None),
							new Add(new SymbolId("SPlanks"), 1),
						},
						1
					),
					() => new ActionPutResource(this, ResourceType.Planks, town.MainStorage)
				},
				{
					new PlanningAction(
						"PutIronIntoStorage",
						new List<IPrecondition>
						{
							new IsEqual(new SymbolId("MR"), (int)ResourceType.Iron)
						},
						new List<IEffect>
						{
							new SetValue(new SymbolId("MR"), (int)ResourceType.None),
							new Add(new SymbolId("SIron"), 1),
						},
						1
					),
					() => new ActionPutResource(this, ResourceType.Iron, town.MainStorage)
				},
				{
					new PlanningAction(
						"PutPlanksIntoConstruction",
						new List<IPrecondition>
						{
							new IsEqual(new SymbolId("MR"), (int)ResourceType.Planks)
						},
						new List<IEffect>
						{
							new SetValue(new SymbolId("MR"), (int)ResourceType.None),
							new Add(new SymbolId("CPlanks"), 1),
						},
						1
					),
					() => new ActionPutResource(this, ResourceType.Planks, town.ConstructionStorage)
				},
				{
					new PlanningAction(
						"PutIronIntoConstruction",
						new List<IPrecondition>
						{
							new IsEqual(new SymbolId("MR"), (int)ResourceType.Iron)
						},
						new List<IEffect>
						{
							new SetValue(new SymbolId("MR"), (int)ResourceType.None),
							new Add(new SymbolId("CIron"), 1),
						},
						1
					),
					() => new ActionPutResource(this, ResourceType.Iron, town.ConstructionStorage)
				},
				{
					new PlanningAction(
						"CutTree",
						new List<IPrecondition>
						{
							new IsEqual(new SymbolId("MT"), (int)ToolType.Axe),
							new IsEqual(new SymbolId("MR"), (int)ResourceType.None)
						},
						new List<IEffect>
						{
							new SetValue(new SymbolId("MR"), (int)ResourceType.Planks)
						},
						5
					),
					() => new ActionCutTree(this)
				},
				{
					new PlanningAction(
						"TakeAxe",
						new List<IPrecondition>
						{
							new IsEqual(new SymbolId("MT"), (int)ToolType.None),
							new IsNotSmaller(new SymbolId("BAxe"), 1)
						},
						new List<IEffect>
						{
							new SetValue(new SymbolId("MT"), (int)ToolType.Axe),
							new Subtract(new SymbolId("BAxe"), 1)
						},
						1
					),
					() => new ActionGetTool(this, ToolType.Axe, town.ToolBench)
				},
				{
					new PlanningAction(
						"TakeHammer",
						new List<IPrecondition>
						{
							new IsEqual(new SymbolId("MT"), (int)ToolType.None),
							new IsNotSmaller(new SymbolId("BHammer"), 1)
						},
						new List<IEffect>
						{
							new SetValue(new SymbolId("MT"), (int)ToolType.Hammer),
							new Subtract(new SymbolId("BHammer"), 1)
						},
						1
					),
					() => new ActionGetTool(this, ToolType.Hammer, town.ToolBench)
				},
				{
					new PlanningAction(
						"PutBackAxe",
						new List<IPrecondition>
						{
							new IsEqual(new SymbolId("MT"), (int)ToolType.Axe),
						},
						new List<IEffect>
						{
							new SetValue(new SymbolId("MT"), (int)ToolType.None),
							new Add(new SymbolId("BAxe"), 1)
						},
						1
					),
					() => new ActionPutTool(this, ToolType.Axe, town.ToolBench)
				},
				{
					new PlanningAction(
						"PutBackHammer",
						new List<IPrecondition>
						{
							new IsEqual(new SymbolId("MT"), (int)ToolType.Hammer),
						},
						new List<IEffect>
						{
							new SetValue(new SymbolId("MT"), (int)ToolType.None),
							new Add(new SymbolId("BHammer"), 1)
						},
						1
					),
					() => new ActionPutTool(this, ToolType.Hammer, town.ToolBench)
				}
			};

			return new SimpleActionFactory(actions);
		}

		private abstract class AbstractAction<TSubject> : IAction
		{
			protected readonly TSubject Subject;

			protected AbstractAction(TSubject subject)
			{
				this.Subject = subject;
			}

			#region IAction implementation

			public void StartExecution()
			{
				Debug.LogFormat("Executing {0}...", GetType().Name);
				try
				{
					StartExecutionImpl();
				}
				catch(InvalidOperationException e)
				{
					Debug.LogWarningFormat("{0} failed: {1}", GetType().Name, e.Message);
					Status = ExecutionStatus.Failed;
				}
				ReportStatus();
			}

			public void StartInterruption()
			{
				StartInterruptionImpl();
				ReportStatus();
			}

			public void Update()
			{
				try
				{
					UpdateImpl();
					if(Status.IsFinal())
					{
						ReportStatus();
					}
				}
				catch(InvalidOperationException e)
				{
					Debug.LogWarningFormat("{0} failed: {1}", GetType().Name, e.Message);
					Status = ExecutionStatus.Failed;
				}
			}

			public ExecutionStatus Status
			{
				get;
				protected set;
			}

			#endregion

			private void ReportStatus()
			{
				Debug.LogFormat("{0} is {1}", GetType().Name, Status);
			}

			protected abstract void StartExecutionImpl();

			protected abstract void StartInterruptionImpl();

			protected abstract void UpdateImpl();
		}

		private class ActionBuildHouse : AbstractAction<Townsman>
		{
			private const int PlanksRequired = 4;
			private const int IronRequired = 2;

			private const int ExecutionTime = 5;

			private readonly ITimer timer;

			public ActionBuildHouse(Townsman subject)
				: base(subject)
			{
				timer = new ResettableStopwatchExecutionTimer(true);
			}

			protected override void StartExecutionImpl()
			{
				if(Subject.town.House.IsBuilt)
				{
					throw new InvalidOperationException("already built");
				}

				if(Subject.town.ConstructionStorage.GetResourceCount(ResourceType.Planks) < PlanksRequired
				   || Subject.town.ConstructionStorage.GetResourceCount(ResourceType.Iron) < IronRequired)
				{
					throw new InvalidOperationException("not enough resources");
				}

				Subject.MoveTo(Subject.town.House.BuilderPosition.position);

				Status = ExecutionStatus.InProgress;
			}

			protected override void StartInterruptionImpl()
			{
				if(!Status.IsFinal() && Status != ExecutionStatus.InInterruption)
				{
					Subject.AnimateTool = false;
					Status = ExecutionStatus.Interrupted;
				}
			}

			protected override void UpdateImpl()
			{
				if(Status == ExecutionStatus.InProgress)
				{
					if(timer.IsPaused && Subject.ReachedDestination)
					{
						Subject.town.ConstructionStorage.Planks -= PlanksRequired;
						Subject.town.ConstructionStorage.Iron -= IronRequired;

						Subject.AnimateTool = true;
						timer.Resume();
					}
					else if(!timer.IsPaused && timer.ElapsedSeconds > ExecutionTime)
					{
						Subject.AnimateTool = false;
						Status = ExecutionStatus.Complete;
					}
				}
			}
		}
		
		private class ActionDestroyHouse : AbstractAction<Townsman>
		{
			private const int ExecutionTime = 0;

			private readonly ITimer timer;

			public ActionDestroyHouse(Townsman subject)
				: base(subject)
			{
				timer = new ResettableStopwatchExecutionTimer(true);
			}

			protected override void StartExecutionImpl()
			{
				if(!Subject.town.House.IsBuilt)
				{
					throw new InvalidOperationException("Not built");
				}

				Subject.MoveTo(Subject.town.House.BuilderPosition.position);

				Status = ExecutionStatus.InProgress;
			}

			protected override void StartInterruptionImpl()
			{
				if(!Status.IsFinal() && Status != ExecutionStatus.InInterruption)
				{
					Subject.AnimateTool = false;
					Status = ExecutionStatus.Interrupted;
				}
			}

			protected override void UpdateImpl()
			{
				if(Status == ExecutionStatus.InProgress)
				{
					if(timer.IsPaused && Subject.ReachedDestination)
					{
						Subject.AnimateTool = true;
						timer.Resume();
					}
					else if(!timer.IsPaused && timer.ElapsedSeconds > ExecutionTime)
					{
						Subject.AnimateTool = false;
						Subject.town.House.IsBuilt = false;
						Status = ExecutionStatus.Complete;
					}
				}
			}
		}


		private class ActionGetResource : AbstractAction<Townsman>
		{
			private readonly ResourceType resourceType;
			private readonly Storage source;

			public ActionGetResource(Townsman subject, ResourceType resource, Storage source)
				: base(subject)
			{
				resourceType = resource;
				this.source = PreconditionUtils.EnsureNotNull(source, "source");
			}

			protected override void StartExecutionImpl()
			{
				if(source.GetResourceCount(resourceType) < 1)
				{
					throw new InvalidOperationException("not enough resources");
				}

				if(Subject.CurrentResource != ResourceType.None)
				{
					throw new InvalidOperationException("already carrying another resource");
				}

				Subject.MoveTo(source.transform.position);
				Status = ExecutionStatus.InProgress;
			}

			protected override void StartInterruptionImpl()
			{
				if(!Status.IsFinal() && Status != ExecutionStatus.InInterruption)
				{
					Status = ExecutionStatus.Interrupted;
				}
			}

			protected override void UpdateImpl()
			{
				if(Status == ExecutionStatus.InProgress && Subject.ReachedDestination)
				{
					Subject.CurrentResource = resourceType;
					Status = ExecutionStatus.Complete;
				}
			}
		}

		private class ActionPutResource : AbstractAction<Townsman>
		{
			private readonly ResourceType resourceType;
			private readonly Storage target;

			public ActionPutResource(Townsman subject, ResourceType resource, Storage target)
				: base(subject)
			{
				resourceType = resource;
				this.target = PreconditionUtils.EnsureNotNull(target, "target");
			}

			protected override void StartExecutionImpl()
			{
				if(Subject.CurrentResource != resourceType)
				{
					throw new InvalidOperationException("not carrying the necessary resource");
				}

				Subject.MoveTo(target.transform.position);
				Status = ExecutionStatus.InProgress;
			}

			protected override void StartInterruptionImpl()
			{
				if(!Status.IsFinal() && Status != ExecutionStatus.InInterruption)
				{
					Status = ExecutionStatus.Interrupted;
				}
			}

			protected override void UpdateImpl()
			{
				if(Status == ExecutionStatus.InProgress && Subject.ReachedDestination)
				{
					Subject.CurrentResource = ResourceType.None;
					target.SetResourceCount(resourceType, target.GetResourceCount(resourceType) + 1);
					Status = ExecutionStatus.Complete;
				}
			}
		}

		private class ActionGetTool : AbstractAction<Townsman>
		{
			private readonly ToolType toolType;
			private readonly ToolBench source;

			public ActionGetTool(Townsman subject, ToolType toolType, ToolBench source)
				: base(subject)
			{
				this.toolType = toolType;
				this.source = PreconditionUtils.EnsureNotNull(source, "source");
			}

			protected override void StartExecutionImpl()
			{
				if(source.GetToolCount(toolType) < 1)
				{
					throw new InvalidOperationException("not enough tools");
				}

				if(Subject.CurrentTool != ToolType.None)
				{
					throw new InvalidOperationException("already carrying another tool");
				}
				
				Subject.MoveTo(source.transform.position);
				Status = ExecutionStatus.InProgress;
			}

			protected override void StartInterruptionImpl()
			{
				if(!Status.IsFinal() && Status != ExecutionStatus.InInterruption)
				{
					Status = ExecutionStatus.Interrupted;
				}
			}

			protected override void UpdateImpl()
			{
				if(Status == ExecutionStatus.InProgress && Subject.ReachedDestination)
				{
					Subject.CurrentTool = toolType;
					source.SetToolCount(toolType, source.GetToolCount(toolType) - 1);
					Status = ExecutionStatus.Complete;
				}
			}
		}

		private class ActionPutTool : AbstractAction<Townsman>
		{
			private readonly ToolType toolType;
			private readonly ToolBench target;

			public ActionPutTool(Townsman subject, ToolType toolType, ToolBench target)
				: base(subject)
			{
				this.toolType = toolType;
				this.target = PreconditionUtils.EnsureNotNull(target, "target");
			}

			protected override void StartExecutionImpl()
			{
				if(Subject.CurrentTool != toolType)
				{
					throw new InvalidOperationException("not carrying the necessary tool");
				}

				Subject.MoveTo(target.transform.position);
				Status = ExecutionStatus.InProgress;
			}

			protected override void StartInterruptionImpl()
			{
				if(!Status.IsFinal() && Status != ExecutionStatus.InInterruption)
				{
					Status = ExecutionStatus.Interrupted;
				}
			}

			protected override void UpdateImpl()
			{
				if(Status == ExecutionStatus.InProgress && Subject.ReachedDestination)
				{
					Subject.CurrentTool = ToolType.None;
					target.SetToolCount(toolType, target.GetToolCount(toolType) + 1);
					Status = ExecutionStatus.Complete;
				}
			}
		}

		private class ActionCutTree : AbstractAction<Townsman>
		{
			private const float ExecutionTime = 3.0f;

			private readonly ITimer timer;

			private GameObject selectedTree;

			public ActionCutTree(Townsman subject)
				: base(subject)
			{
				timer = new ResettableStopwatchExecutionTimer(true);
			}

			#region implemented abstract members of AbstractAction

			protected override void StartExecutionImpl()
			{
				if(Subject.toolType != ToolType.Axe)
				{
					throw new InvalidOperationException("has no axe");
				}

				selectedTree = Subject.FindClosestTree();
				Subject.MoveTo(selectedTree.transform.position);
				Status = ExecutionStatus.InProgress;
			}

			protected override void StartInterruptionImpl()
			{
				if(!Status.IsFinal() && Status != ExecutionStatus.InInterruption)
				{
					Subject.AnimateTool = false;
					Status = ExecutionStatus.Interrupted;
				}
			}

			protected override void UpdateImpl()
			{
				if(Status == ExecutionStatus.InProgress)
				{
					if(timer.IsPaused && Subject.ReachedDestination)
					{
						timer.Resume();
						Subject.AnimateTool = true;
					}
					else if(!timer.IsPaused && timer.ElapsedSeconds > ExecutionTime)
					{
						Subject.AnimateTool = false;
						if(Subject.CurrentResource != ResourceType.None)
						{
							throw new InvalidOperationException("already carrying another resource");
						}
						Subject.CurrentResource = ResourceType.Planks;
//						Destroy(selectedTree);
						Status = ExecutionStatus.Complete;
					}
				}
			}

			#endregion
		}
	}
}

