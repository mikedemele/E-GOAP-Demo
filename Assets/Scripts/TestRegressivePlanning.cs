using EGoap.Source.Planning;

//Test case for the experience enhanced forward planner
public class TestRegressivePlanning : Test
{
	public override string Name()
	{
		return "Regressive Planner";
	}

	public override void Run()
	{
		var planner = new RegressivePlanner(maxSearchDepth, maxSearchTime, learn, experienceWeight);
     
		RunTests(planner);
	}
}