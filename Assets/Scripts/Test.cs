using System.Collections.Generic;

using UnityEngine;

using EGoap.Source.Planning;
using EGoap.Source.Planning.Effects;
using EGoap.Source.Planning.Preconditions;
using EGoap.Source.Utils.Time;

public abstract class Test : MonoBehaviour
{
    public bool standalone;
    
    public double experienceWeight;
    public float maxSearchTime;
    public int maxSearchDepth;
    public bool learn;

    private void Start()
    {
        if(standalone)
            Run();
    }
    public abstract string Name();
    public abstract void Run();

    private static WorldState GetInitialWorldState()
    {
        return new WorldState.Builder()
            .SetSymbol(new SymbolId("Wood"), 0)
            .SetSymbol(new SymbolId("Stone"), 8)
            .SetSymbol(new SymbolId("Iron"), 0)
            .SetSymbol(new SymbolId("HouseBuilt"), 0)
            .SetSymbol(new SymbolId("WoodInStorage"), 4)
            .SetSymbol(new SymbolId("StoneInStorage"), 3)
            .SetSymbol(new SymbolId("IronInStorage"), 1)
            .SetSymbol(new SymbolId("HasAxe"), 0)
            .SetSymbol(new SymbolId("HasPickaxe"), 0) 
            .SetSymbol(new SymbolId("AxesAvailable"), 0)
            .SetSymbol(new SymbolId("Money"), 0)
            .SetSymbol(new SymbolId("PickaxesAvailable"), 0)
            .Build();
    }

    private static HashSet<PlanningAction> GetAvailableActions()
    {
        return new HashSet<PlanningAction>
            {
     			new PlanningAction(
     				"BuildHouse",
     				new List<IPrecondition>
                    {
     					new IsNotSmaller(new SymbolId("Wood"), 20),
     					new IsNotSmaller(new SymbolId("Stone"), 5)
     				},
     				new List<IEffect>
                    {
     					new SetTrue(new SymbolId("HouseBuilt")),
     					new Subtract(new SymbolId("Wood"), 20),
     					new Subtract(new SymbolId("Stone"), 5)
     				},
     				5
     			),
     			new PlanningAction(
     				"GetWoodFromStorage",
     				new List<IPrecondition>
                    {
     					new IsNotSmaller(new SymbolId("WoodInStorage"), 1)
     				},
     				new List<IEffect>
                    {
     					new Add(new SymbolId("Wood"), 1),
     					new Subtract(new SymbolId("WoodInStorage"), 1),
     				},
     				2
                ),
     			new PlanningAction(
     				"GetStoneFromStorage",
     				new List<IPrecondition>
                    {
     					new IsNotSmaller(new SymbolId("StoneInStorage"), 1)
     				},
     				new List<IEffect>
                    {
     					new Add(new SymbolId("Stone"), 1),
     					new Subtract(new SymbolId("StoneInStorage"), 1),
     				},
     				2
     			),
     			new PlanningAction(
     				"GetIronFromStorage",
     				new List<IPrecondition>
                    {
     					new IsNotSmaller(new SymbolId("IronInStorage"), 1)
     				},
     				new List<IEffect>
                    {
     					new Add(new SymbolId("Iron"), 1),
     					new Subtract(new SymbolId("IronInStorage"), 1),
     				},
     				2
     			),
     			new PlanningAction(
     				"CutTrees",
     				new List<IPrecondition>
                    {
     					new IsTrue(new SymbolId("HasAxe"))
     				},
     				new List<IEffect>
                    {
     					new Add(new SymbolId("Wood"), 8)//,
     				},
     				3
     			),
     			new PlanningAction(
     				"BreakRocks",
     				new List<IPrecondition>
                    {
     					new IsTrue(new SymbolId("HasPickaxe"))
     				},
     				new List<IEffect>
                    {
     					new Add(new SymbolId("Stone"), 3)
     				},
     				3
     			),
     			new PlanningAction(
     				"MineOre",
     				new List<IPrecondition>
                    {
     					new IsTrue(new SymbolId("HasPickaxe"))
     				},
     				new List<IEffect>
                    {
     					new Add(new SymbolId("Iron"), 2)
     				},
     				3
     			),
     			new PlanningAction(
     				"TakeAxe",
     				new List<IPrecondition>
                    {
     					new IsFalse(new SymbolId("HasAxe")),
     					new IsNotSmaller(new SymbolId("AxesAvailable"), 1)
     				},
     				new List<IEffect>
                    {
     					new SetTrue(new SymbolId("HasAxe")),
     					new Subtract(new SymbolId("AxesAvailable"), 1)
     				},
     				1
     			),
     			new PlanningAction(
     				"TakePickaxe",
     				new List<IPrecondition>
                    {
     					new IsFalse(new SymbolId("HasPickaxe")),
     					new IsNotSmaller(new SymbolId("PickaxesAvailable"), 1)
     				},
     				new List<IEffect>
                    {
     					new SetTrue(new SymbolId("HasPickaxe")),
     					new Subtract(new SymbolId("PickaxesAvailable"), 1)
     				},
     				1
     			),
     			new PlanningAction(
     				"MakeAxe",
     				new List<IPrecondition>
                    {
     					new IsFalse(new SymbolId("HasAxe")),
     					new IsNotSmaller(new SymbolId("Wood"), 2),
     					new IsNotSmaller(new SymbolId("Iron"), 3)
     				},
     				new List<IEffect>
                    {
     					new Add(new SymbolId("AxesAvailable"), 1),
     					new Subtract(new SymbolId("Wood"), 2),
     					new Subtract(new SymbolId("Iron"), 3)
     				},
     				3
     			),
     			new PlanningAction(
     				"MakePickaxe",
     				new List<IPrecondition>
                    {
     					new IsFalse(new SymbolId("HasPickaxe")),
     					new IsNotSmaller(new SymbolId("Wood"), 2),
     					new IsNotSmaller(new SymbolId("Iron"), 1)
     				},
     				new List<IEffect>
                    {
     					new Add(new SymbolId("PickaxesAvailable"), 1),
     					new Subtract(new SymbolId("Wood"), 2),
     					new Subtract(new SymbolId("Iron"), 1)
     				},
     				3
     			),
                new PlanningAction(
                "Work",
                new List<IPrecondition>(),
                new List<IEffect>
                {
	                new Add(new SymbolId("Money"), 2)
                },
                5
                )
     		};
    }
    
	private static HashSet<PlanningAction> GetManyActions()
    {
	    return new HashSet<PlanningAction>
	    {
		    new PlanningAction(
			    "DoStuff",
			    new List<IPrecondition>(),
			    new List<IEffect> {
				    new Add(new SymbolId("Money"), 1)
				    },
			    1
		    ),new PlanningAction(
			    "DoStuff1",
			    new List<IPrecondition>(),
			    new List<IEffect> {
					new Add(new SymbolId("Money"), 2)
				},
			    2
		    ),
		    new PlanningAction(
			    "DoStuff2",
			    new List<IPrecondition>(),
			    new List<IEffect> {
				    new Add(new SymbolId("Money"), 3)
			    },
			    3
		    ),new PlanningAction(
			    "DoStuff3",
			    new List<IPrecondition>(),
			    new List<IEffect> {
				    new Add(new SymbolId("Money"), 4)
			    },
			    4
		    ),
		    new PlanningAction(
			    "DoStuff4",
			    new List<IPrecondition>(),
			    new List<IEffect> {
				    new Add(new SymbolId("Money"), 5)
			    },
			    5
		    ),new PlanningAction(
			    "DoStuff5",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 15),
				    new IsNotSmaller(new SymbolId("Stone"), 15)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HouseBuilt")),
				    new Subtract(new SymbolId("Wood"), 15),
				    new Subtract(new SymbolId("Stone"), 15)
			    },
			    1
		    ),
		    new PlanningAction(
			    "DoStuff6",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 15),
				    new IsNotSmaller(new SymbolId("Stone"), 10)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HouseBuilt")),
				    new Subtract(new SymbolId("Wood"), 15),
				    new Subtract(new SymbolId("Stone"), 10)
			    },
			    2
		    ),new PlanningAction(
			    "DoStuff7",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 40)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HouseBuilt")),
				    new Subtract(new SymbolId("Wood"), 40)
			    },
			    3
		    ),
		    new PlanningAction(
			    "DoStuff8",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 20),
				    new IsNotSmaller(new SymbolId("Iron"), 10)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HouseBuilt")),
				    new Subtract(new SymbolId("Wood"), 20),
				    new Subtract(new SymbolId("Iron"), 10)
			    },
			    4
		    ),new PlanningAction(
			    "DoStuff9",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Iron"), 50)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HouseBuilt")),
				    new Subtract(new SymbolId("Iron"), 50)
			    },
			    5
		    ),
		    new PlanningAction(
			    "DoStuff10",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 25),
				    new IsNotSmaller(new SymbolId("Iron"), 10)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HouseBuilt")),
				    new Subtract(new SymbolId("Wood"), 25),
				    new Subtract(new SymbolId("Iron"), 10)
			    },
			    1
		    ),new PlanningAction(
			    "DoStuff11",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 20),
				    new IsNotSmaller(new SymbolId("Iron"), 15)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HouseBuilt")),
				    new Subtract(new SymbolId("Wood"), 20),
				    new Subtract(new SymbolId("Iron"), 15)
			    },
			    2
		    ),
		    new PlanningAction(
			    "DoStuff12",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 15),
				    new IsNotSmaller(new SymbolId("Iron"), 20)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HouseBuilt")),
				    new Subtract(new SymbolId("Wood"), 15),
				    new Subtract(new SymbolId("Iron"), 20)
			    },
			    3
		    ),
		    new PlanningAction(
			    "DoStuff13",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 10),
				    new IsNotSmaller(new SymbolId("Stone"), 25)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HouseBuilt")),
				    new Subtract(new SymbolId("Wood"), 10),
				    new Subtract(new SymbolId("Stone"), 25)
			    },
			    4
		    ),
		    new PlanningAction(
			    "DoStuff14",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 30),
				    new IsNotSmaller(new SymbolId("Stone"), 5)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HouseBuilt")),
				    new Subtract(new SymbolId("Wood"), 30),
				    new Subtract(new SymbolId("Stone"), 5)
			    },
			    5
		    ),new PlanningAction(
			    "DoStuff15",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 5),
				    new IsNotSmaller(new SymbolId("Iron"), 30)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HouseBuilt")),
				    new Subtract(new SymbolId("Wood"), 5),
				    new Subtract(new SymbolId("Iron"), 30)
			    },
			    1
		    ),
		    new PlanningAction(
			    "DoStuff16",
			    new List<IPrecondition>
			    {
				    new IsFalse(new SymbolId("HasAxe")),
				    new IsNotSmaller(new SymbolId("Wood"), 2),
				    new IsNotSmaller(new SymbolId("Stone"), 3)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HasAxe")),
				    new Subtract(new SymbolId("Wood"), 2),
				    new Subtract(new SymbolId("Stone"), 3)
			    },
			    2
		    ),new PlanningAction(
			    "DoStuff17",
			    new List<IPrecondition>
			    {
				    new IsFalse(new SymbolId("HasAxe")),
				    new IsNotSmaller(new SymbolId("Iron"), 2),
				    new IsNotSmaller(new SymbolId("Stone"), 3)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HasAxe")),
				    new Subtract(new SymbolId("Iron"), 2),
				    new Subtract(new SymbolId("Stone"), 3)
			    },
			    3
		    ),
		    new PlanningAction(
			    "DoStuff18",
			    new List<IPrecondition>
			    {
				    new IsFalse(new SymbolId("HasAxe")),
				    new IsNotSmaller(new SymbolId("Stone"), 10)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HasAxe")),
				    new Subtract(new SymbolId("Stone"), 10)
			    },
			    4
		    ),new PlanningAction(
			    "DoStuff19",
			    new List<IPrecondition>
			    {
				    new IsFalse(new SymbolId("HasAxe")),
				    new IsNotSmaller(new SymbolId("Iron"), 8)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HasAxe")),
				    new Subtract(new SymbolId("Iron"), 8)
			    },
			    5
		    ),
		    new PlanningAction(
			    "DoStuff20",
			    new List<IPrecondition>(),
			    new List<IEffect>
			    {
				    new Add(new SymbolId("WoodInStorage"), 2)
			    },
			    1
		    ),new PlanningAction(
			    "DoStuff21",
			    new List<IPrecondition>(),
			    new List<IEffect>
			    {
				    new Add(new SymbolId("WoodInStorage"), 4)
			    },
			    2
		    ),
		    new PlanningAction(
			    "DoStuff22",
			    new List<IPrecondition>(),
			    new List<IEffect>
			    {
				    new Add(new SymbolId("WoodInStorage"), 6)
			    },
			    3
		    ),new PlanningAction(
			    "DoStuff23",
			    new List<IPrecondition>(),
			    new List<IEffect>
			    {
				    new Add(new SymbolId("WoodInStorage"), 8)
			    },
			    4
		    ),
		    new PlanningAction(
			    "DoStuff24",
			    new List<IPrecondition>(),
			    new List<IEffect>
			    {
				    new Add(new SymbolId("WoodInStorage"), 10)
			    },
			    5
		    ),new PlanningAction(
			    "DoStuff25",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasPickaxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Iron"), 2)
			    },
			    1
		    ),
		    new PlanningAction(
			    "DoStuff26",new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasPickaxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Iron"), 4)
			    },
			    
			    2
		    ),new PlanningAction(
			    "DoStuff27",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasPickaxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Iron"), 6)
			    },
			    3
		    ),
		    new PlanningAction(
			    "DoStuff28",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasPickaxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Iron"), 8)
			    },
			    4
		    ),new PlanningAction(
			    "DoStuff29",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasPickaxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Iron"), 10)
			    },
			    5
		    ),
		    new PlanningAction(
			    "DoStuff30",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasPickaxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Stone"), 2)
			    },
			    1
		    ),new PlanningAction(
			    "DoStuff31",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasPickaxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Stone"), 4)
			    },
			    2
		    ),
		    new PlanningAction(
			    "DoStuff32",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasPickaxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Stone"), 6)
			    },
			    3
		    ),new PlanningAction(
			    "DoStuff33",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasPickaxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Stone"), 8)
			    },
			    4
		    ),
		    new PlanningAction(
			    "DoStuff34",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasPickaxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Stone"), 10)
			    },
			    5
		    ),new PlanningAction(
			    "DoStuff35",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasAxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Wood"), 1)
			    },
			    1
		    ),
		    new PlanningAction(
			    "DoStuff36",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasAxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Wood"), 3)
			    },
			    2
		    ),new PlanningAction(
			    "DoStuff1",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasAxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Wood"), 5)
			    },
			    3
		    ),
		    new PlanningAction(
			    "DoStuff37",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasAxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Wood"), 7)
			    },
			    4
		    ),new PlanningAction(
			    "DoStuff38",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasAxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Wood"), 9)
			    },
			    5
		    ),
		    new PlanningAction(
			    "DoStuff39",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasAxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Wood"), 11)
			    },
			    6
		    ),new PlanningAction(
			    "DoStuff40",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasAxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Wood"), 13)
			    },
			    7
		    ),
		    new PlanningAction(
			    "DoStuff41",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasAxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Wood"), 15)
			    },
			    8
		    ),new PlanningAction(
			    "DoStuff42",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasAxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Wood"), 17)
			    },
			    9
		    ),
		    new PlanningAction(
			    "DoStuff43",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasAxe"))
			    },
			    new List<IEffect>
			    {
				    new Add(new SymbolId("Wood"), 19)
			    },
			    10
		    ),new PlanningAction(
			    "DoStuff44",
			    new List<IPrecondition>(),
			    new List<IEffect>
			    {
				    new Add(new SymbolId("IronInStorage"), 1)
			    },
			    1
		    ),
		    new PlanningAction(
			    "DoStuff45",
			    new List<IPrecondition>(),
			    new List<IEffect>
			    {
				    new Add(new SymbolId("IronInStorage"), 2)
			    },
			    2
		    ),new PlanningAction(
			    "DoStuff46",
			    new List<IPrecondition>(),
			    new List<IEffect>
			    {
				    new Add(new SymbolId("IronInStorage"), 3)
			    },
			    3
		    ),
		    new PlanningAction(
			    "DoStuff47",
			    new List<IPrecondition>(),
			    new List<IEffect>
			    {
				    new Add(new SymbolId("IronInStorage"), 4)
			    },
			    4
		    ),new PlanningAction(
			    "DoStuff48",
			    new List<IPrecondition>(),
			    new List<IEffect>
			    {
				    new Add(new SymbolId("IronInStorage"), 5)
			    },
			    5
		    ),
		    new PlanningAction(
			    "DoStuff49",
			    new List<IPrecondition>(),

			    new List<IEffect>
			    {
				    new Add(new SymbolId("StoneInStorage"), 3)
			    },			    1
		    ),new PlanningAction(
			    "DoStuff50",
			    new List<IPrecondition>(),
			    new List<IEffect>
			    {
				    new Add(new SymbolId("StoneInStorage"), 6)
			    },
			    2
		    ),
		    new PlanningAction(
			    "DoStuff51",
			    new List<IPrecondition>(),
			    new List<IEffect>
			    {
				    new Add(new SymbolId("StoneInStorage"), 9)
			    },			    3
		    ),new PlanningAction(
			    "DoStuff52",
			    new List<IPrecondition>(),
			    new List<IEffect>
			    {
				    new Add(new SymbolId("StoneInStorage"), 10)
			    },
	    4
		    ),
		    new PlanningAction(
			    "DoStuff53",
			    new List<IPrecondition>(),
			    new List<IEffect>
			    {
				    new Add(new SymbolId("StoneInStorage"), 11)
			    },
			    5
		    ),new PlanningAction(
			    "DoStuff54",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasPickaxe"))
			    },
			    new List<IEffect>
			    {
				    new SetFalse(new SymbolId("HasPickaxe")),
				    new Add(new SymbolId("Money"), 10)
			    },
			    3
		    ),
		    new PlanningAction(
			    "DoStuff55",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasAxe"))
			    },
			    new List<IEffect>
			    {
				    new SetFalse(new SymbolId("HasAxe")),
				    new Add(new SymbolId("Money"), 15)
			    },
			    3
		    ),new PlanningAction(
			    "DoStuff56",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Iron"), 5)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Iron"), 5),
				    new Add(new SymbolId("Money"), 10)
			    },
			    3
		    ),
		    new PlanningAction(
			    "DoStuff57",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Stone"), 5)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Stone"), 5),
				    new Add(new SymbolId("Money"), 8)
			    },
			    3
		    ),new PlanningAction(
			    "DoStuff58",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 5)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Wood"), 5),
				    new Add(new SymbolId("Money"), 5)
			    },
			    3
		    ),
		    new PlanningAction(
			    "DoStuff59",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 10)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Wood"), 10),
				    new Add(new SymbolId("Money"), 10)
			    },
			    3
		    ),new PlanningAction(
			    "DoStuff60",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Iron"), 10)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Iron"), 10),
				    new Add(new SymbolId("Money"), 22)
			    },
			    3
		    ),
		    new PlanningAction(
			    "DoStuff61",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Stone"), 10)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Stone"), 10),
				    new Add(new SymbolId("Money"), 15)
			    },
			    3
		    ),new PlanningAction(
			    "DoStuff62",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 100)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Wood"), 100),
				    new Add(new SymbolId("Money"), 101)
			    },
			    3
		    ),
		    new PlanningAction(
			    "DoStuff63",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Iron"), 100)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Iron"), 100),
				    new Add(new SymbolId("Money"), 230)
			    },
			    3
		    ),new PlanningAction(
			    "DoStuff64",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Stone"), 100)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Stone"), 100),
				    new Add(new SymbolId("Money"), 190)
			    },
			    3
		    ),
		    new PlanningAction(
			    "DoStuff65",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 3),
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Wood"), 3),
				    new Add(new SymbolId("Iron"), 1)
			    },
			    1
		    ),new PlanningAction(
			    "DoStuff66",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Stone"), 2),
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Stone"), 2),
				    new Add(new SymbolId("Iron"), 1)
			    },
			    2
			),
		    new PlanningAction(
			    "DoStuff67",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Iron"), 1),
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Iron"), 1),
				    new Add(new SymbolId("Stone"), 2)
			    },
			    3
		    ),new PlanningAction(
			    "DoStuff68",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Iron"), 1),
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Iron"), 1),
				    new Add(new SymbolId("Wood"), 3)
			    },
			    4
		    ),
		    new PlanningAction(
			    "DoStuff69",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Stone"), 1),
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Stone"), 1),
				    new Add(new SymbolId("Wood"), 2)
			    },
			    5
		    ),new PlanningAction(
			    "DoStuff70",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasPickaxe"))
			    },
			    new List<IEffect>
			    {
				    new SetFalse(new SymbolId("HasPickaxe")),
				    new Add(new SymbolId("Wood"), 1)
			    },
			    5
		    ),
		    new PlanningAction(
			    "DoStuff71",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasAxe"))
			    },
			    new List<IEffect>
			    {
				    new SetFalse(new SymbolId("HasAxe")),
				    new Add(new SymbolId("Wood"), 1)
			    },
			    5
		    ),new PlanningAction(
			    "DoStuff72",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HouseBuilt"))
			    },
			    new List<IEffect>
			    {
				    new SetFalse(new SymbolId("HouseBuilt")),
				    new Add(new SymbolId("Money"), 100)
			    },
			    3
		    ),
		    new PlanningAction(
			    "DoStuff73",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HouseBuilt"))
			    },
			    new List<IEffect>
			    {
				    new SetFalse(new SymbolId("HouseBuilt")),
				    new Add(new SymbolId("Wood"), 2),
				    new Add(new SymbolId("Stone"), 5)
			    },
			    4
		    ),new PlanningAction(
			    "DoStuff74",
			    new List<IPrecondition>(),
			    new List<IEffect>(),
			    5
		    ),
		    new PlanningAction(
			    "DoStuff75",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 5)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Wood"), 5)
			    },
			    1
		    ),new PlanningAction(
			    "DoStuff76",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasPickaxe"))
			    },
			    new List<IEffect>
			    {
				    new SetFalse(new SymbolId("HasPickaxe"))
			    },
			    2
		    ),
		    new PlanningAction(
			    "DoStuff77",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HasAxe"))
			    },
			    new List<IEffect>
			    {
				    new SetFalse(new SymbolId("HasAxe"))
			    },
			    3
		    ),new PlanningAction(
			    "DoStuff78",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HouseBuilt"))
			    },
			    new List<IEffect>
			    {
				    new SetFalse(new SymbolId("HouseBuilt"))
			    },
			    4
		    ),
		    new PlanningAction(
			    "DoStuff79",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 1)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Wood"), 1)
			    },
			    5
		    ),new PlanningAction(
			    "DoStuff80",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Stone"), 5)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Stone"), 5)
			    },
			    1
		    ),
		    new PlanningAction(
			    "DoStuff81",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Iron"), 5)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Iron"), 5)
			    },
			    
			    2
		    ),new PlanningAction(
			    "DoStuff82",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 3)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Wood"), 3)
			    },
			    3
		    ),
		    new PlanningAction(
			    "DoStuff83",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 10)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Wood"), 10)
			    },
			    4
		    ),new PlanningAction(
			    "DoStuff84",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Stone"), 1)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Stone"), 1)
			    },
			    5
		    ),
		    new PlanningAction(
			    "DoStuff85",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Iron"), 2)
			    },
			    new List<IEffect>
			    {
				    new Subtract(new SymbolId("Iron"), 2)
			    },
			    1
		    ),
		    new PlanningAction(
			    "DoStuff86",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Wood"), 5),
				    new IsNotSmaller(new SymbolId("Stone"), 10),
				    new IsNotSmaller(new SymbolId("Iron"), 1)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HouseBuilt")),
				    new Subtract(new SymbolId("Wood"), 5),
				    new Subtract(new SymbolId("Stone"), 10),
				    new Subtract(new SymbolId("Iron"), 1)
			    },
			    9
		    ),new PlanningAction(
			    "DoStuff87",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Stone"), 40)
			    },
			    new List<IEffect>
			    {
				    new SetTrue(new SymbolId("HouseBuilt")),
				    new Subtract(new SymbolId("Stone"), 40)
			    },
			    10
		    ),

	    };
    }


    protected static void RunTests(IPlanner planner)
    {
	    try
	    {
		    var currentWorldState = GetInitialWorldState();

		    var availableActions = GetAvailableActions();

		    var buildHouseGoal = new Goal(
			    "BuildHouse",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HouseBuilt"))
			    }
		    );
	            
		    var buildHousePlusGoal = new Goal(
			    "BuildHousePlus",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HouseBuilt")),
				    new IsNotSmaller(new SymbolId("Iron"), 5),
				    new IsNotSmaller(new SymbolId("Wood"), 2)
			    }
		    );
	            
		    var getRichGoal = new Goal(
			    "GetRich",
			    new List<IPrecondition>
			    {
				    new IsNotSmaller(new SymbolId("Money"), 10),
				    new IsNotSmaller(new SymbolId("Iron"), 15),
			    }
		    ); 
		    
		    var farGoal = new Goal(
			    "FarGoal",
			    new List<IPrecondition>
			    {
				    new IsTrue(new SymbolId("HouseBuilt")),
				    new IsNotSmaller(new SymbolId("Iron"), 500),
				    new IsNotSmaller(new SymbolId("Wood"), 100),
				    new IsNotSmaller(new SymbolId("Stone"), 200),
				    new IsNotSmaller(new SymbolId("Money"), 200)
			    }
			);
		    
		    print("-----------------DEFAULT PLAN---------------------------");
		    var timer = new ResettableStopwatchExecutionTimer();
		    var plan = planner.FormulatePlan(currentWorldState, availableActions, buildHouseGoal);
		    print(
			    $"{planner.GetType()} has found a plan of length {plan.Length} and cost {plan.Cost} to satisfy \"{buildHouseGoal.Name}\" in {timer.ElapsedSeconds} seconds"
		    );
		    
		    // print(plan.ToString());
		    print("-----------------IDENTICAL PLAN---------------------------");
		    timer.Reset(false);
		    plan = planner.FormulatePlan(currentWorldState, availableActions, buildHouseGoal);
		    print(
			    $"{planner.GetType()} has found a plan of length {plan.Length} and cost {plan.Cost} to satisfy \"{buildHouseGoal.Name}\" in {timer.ElapsedSeconds} seconds"
		    );
		    // print(plan.ToString());
		    print("-----------------PARTIAL REUSE PLAN---------------------------");
		    timer.Reset(false);
		    plan = planner.FormulatePlan(currentWorldState, availableActions, buildHousePlusGoal);
		    print(
			    $"{planner.GetType()} has found a plan of length {plan.Length} and cost {plan.Cost} to satisfy \"{buildHousePlusGoal.Name}\" in {timer.ElapsedSeconds} seconds"
		    );
		    // print(plan.ToString());
		    print("-----------------COMPLETELY DIFFERENT PLAN---------------------------");
		    timer.Reset(false);
		    plan = planner.FormulatePlan(currentWorldState, availableActions, getRichGoal);
		    print(
			    $"{planner.GetType()} has found a plan of length {plan.Length} and cost {plan.Cost} to satisfy \"{getRichGoal.Name}\" in {timer.ElapsedSeconds} seconds"
		    );
		    // print(plan.ToString());
		    print("-----------------COMPLETELY DIFFERENT PLAN FRESH---------------------------");
		    planner.ClearExperience();
		    timer.Reset(false);
		    plan = planner.FormulatePlan(currentWorldState, availableActions, getRichGoal);
		    print(
			    $"{planner.GetType()} has found a plan of length {plan.Length} and cost {plan.Cost} to satisfy \"{getRichGoal.Name}\" in {timer.ElapsedSeconds} seconds"
		    );
		    // print(plan.ToString());
		    print("-----------------PARTIAL REUSE PLAN FRESH---------------------------");
		    planner.ClearExperience();
		    timer.Reset(false);
		    plan = planner.FormulatePlan(currentWorldState, availableActions, buildHousePlusGoal);
		    print(
			    $"{planner.GetType()} has found a plan of length {plan.Length} and cost {plan.Cost} to satisfy \"{buildHousePlusGoal.Name}\" in {timer.ElapsedSeconds} seconds"
		    );
		    // print(plan.ToString());
		    print("-----------------DEEP PLAN---------------------------");
		    timer.Reset(false);
		    planner.ClearExperience();
		    plan = planner.FormulatePlan(currentWorldState, availableActions, farGoal);
		    print(
			    $"{planner.GetType()} has found a plan of length {plan.Length} and cost {plan.Cost} to satisfy \"{getRichGoal.Name}\" in {timer.ElapsedSeconds} seconds"
		    );
		    // print(plan.ToString());
		    print("-----------------DEEP PLAN AGAIN---------------------------");
		    timer.Reset(false);
		    plan = planner.FormulatePlan(currentWorldState, availableActions, farGoal);
		    print(
			    $"{planner.GetType()} has found a plan of length {plan.Length} and cost {plan.Cost} to satisfy \"{getRichGoal.Name}\" in {timer.ElapsedSeconds} seconds"
		    );
		    // print(plan.ToString());
		    
		    print("-----------------WIDE PLAN---------------------------");
		    planner.ClearExperience();
		    availableActions.UnionWith(GetManyActions());
		    timer.Reset(false);
		    plan = planner.FormulatePlan(currentWorldState, availableActions, getRichGoal);
		    print(
			    $"{planner.GetType()} has found a plan of length {plan.Length} and cost {plan.Cost} to satisfy \"{getRichGoal.Name}\" in {timer.ElapsedSeconds} seconds"
		    );
		    // print(plan.ToString());
		    print("-----------------WIDE PLAN AGAIN---------------------------");
		    timer.Reset(false);
		    plan = planner.FormulatePlan(currentWorldState, availableActions, getRichGoal);
		    print(
			    $"{planner.GetType()} has found a plan of length {plan.Length} and cost {plan.Cost} to satisfy \"{getRichGoal.Name}\" in {timer.ElapsedSeconds} seconds"
		    );
		    // print(plan.ToString());
		    print("-----------------DEEP WIDE PLAN---------------------------");
		    timer.Reset(false);
		    planner.ClearExperience();
		    plan = planner.FormulatePlan(currentWorldState, availableActions, farGoal);
		    print(
			    $"{planner.GetType()} has found a plan of length {plan.Length} and cost {plan.Cost} to satisfy \"{getRichGoal.Name}\" in {timer.ElapsedSeconds} seconds"
		    );
		    // print(plan.ToString());
		    print("-----------------DEEP WIDE PLAN AGAIN---------------------------");
		    timer.Reset(false);
		    plan = planner.FormulatePlan(currentWorldState, availableActions, farGoal);
		    print(
			    $"{planner.GetType()} has found a plan of length {plan.Length} and cost {plan.Cost} to satisfy \"{getRichGoal.Name}\" in {timer.ElapsedSeconds} seconds"
		    );
		    // print(plan.ToString());
	    }
	    catch(PlanNotFoundException e)
	    {
		    Debug.LogWarningFormat("{0}: {1}", e.Message, e.InnerException?.Message);
	    }
    }
}
