using EGoap.Source.Planning;

//Test case for the experience enhanced forward planner
public class TestForwardPlanning : Test
{
	public override string Name()
	{
		return "Forward Planner";
	}

	public override void Run()
	{
		var planner = new ForwardPlanner(maxSearchDepth, maxSearchTime, learn, experienceWeight);
		
		RunTests(planner);
	}
}