using System.Linq;
using UnityEngine;

//Run Tests repeatedly to measure performance
public class Benchmark : MonoBehaviour
{

    //Number of test repetitions
    public int repetitions;
    public double[] weights;
    public Test[] testsToBenchmark;
    public bool quitOnFinish;
    
    private void Start()
    {
        foreach (var test in testsToBenchmark)
        {
            Debug.Log(test.Name()); 
            if(weights.Any())
            {
                foreach (var weight in weights)
                {
                     test.experienceWeight = weight;
                     Debug.Log($"Weight: {weight}");
                    RunRepetitions(test);
                }
            }
            else
            {
                RunRepetitions(test);    
            }
        }

        if (quitOnFinish)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    private void RunRepetitions(Test test)
    {
        for (var i = 0; i < repetitions; ++i)
        {
            test.Run();
        }
    }
}
