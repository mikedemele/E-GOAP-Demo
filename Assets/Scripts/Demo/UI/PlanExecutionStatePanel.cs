using System.Linq;
using EGoap.Source.Agents;
using UnityEngine;
using UnityEngine.UI;

namespace Demo.UI
{
	public class PlanExecutionStatePanel : MonoBehaviour
	{
		private const string CurrentGoalTemplate = "Current goal: {0}";
		private const string NoGoal = "<none>";
		private const string NoPlan = "<no plan>";
		private const string EmptyPlan = "<empty plan>";

		[SerializeField]
		private Text currentGoalText;
		[SerializeField]
		private Text planText;

		private IPlanExecution planExecution;
		private int lastActionIndex = -1;

		private void Update()
		{
			if(planExecution != null && planExecution.CurrentActionIndex != lastActionIndex)
			{
				RefreshText();
				lastActionIndex = planExecution.CurrentActionIndex;
			}
		}

		public IPlanExecution PlanExecution
		{
			get => planExecution;
			set {
				planExecution = value;
				RefreshText();
			}
		}

		private void RefreshText()
		{
			if(planExecution?.Plan == null)
			{
				currentGoalText.text = string.Format(CurrentGoalTemplate, NoGoal);
				planText.text = NoPlan;
			}
			else
			{
				currentGoalText.text = string.Format(CurrentGoalTemplate, planExecution.Plan.Goal.Name);
				var planDescription = planExecution.Plan.Length > 0 ? "" : EmptyPlan;
				for(var i = 0; i < planExecution.Plan.Length; i++)
				{
					planDescription += planExecution.Plan.Actions.ElementAt(i).Name;
					if(i < planExecution.CurrentActionIndex)
					{
						planDescription += " (Complete)";
					}
					else if(i == planExecution.CurrentActionIndex)
					{
						planDescription += " (In Progress)";
					}
					planDescription += "\n";
				}
				planText.text = planDescription;
			}
		}
	}
}

