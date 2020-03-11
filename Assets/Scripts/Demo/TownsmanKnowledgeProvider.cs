using System;
using EGoap.Source.Debug;
using EGoap.Source.Planning;

namespace Demo
{
	public partial class Townsman
	{
		private class KnowledgeProvider : IKnowledgeProvider
		{
			private readonly Townsman townsman;

			public KnowledgeProvider(Townsman townsman)
			{
				DebugUtils.Assert(townsman != null, "townsman must not be null");
				this.townsman = townsman;
			}

			#region IKnowledgeProvider implementation

			public int GetSymbolValue(SymbolId symbolId)
			{
				if(symbolId.Name[0] == 'M')
				{
					return symbolId.Name[1] == 'R'
						? (int)townsman.resourceType
						: (int)townsman.toolType;
				}

				if(symbolId.Name[0] == 'S')
				{
					return townsman.town.MainStorage.GetResourceCount((ResourceType)Enum.Parse(typeof(ResourceType), symbolId.Name.Substring(1)));
				}

				if(symbolId.Name[0] == 'C')
				{
					return townsman.town.ConstructionStorage.GetResourceCount((ResourceType)Enum.Parse(typeof(ResourceType), symbolId.Name.Substring(1)));
				}

				if(symbolId.Name[0] == 'B')
				{
					return townsman.town.ToolBench.GetToolCount((ToolType)Enum.Parse(typeof(ToolType), symbolId.Name.Substring(1)));
				}

				if(symbolId.Name == "HouseBuilt")
				{
					return townsman.town.House.IsBuilt ? 1 : 0;
				}

				throw new ArgumentException($"Unrecognized symbol {symbolId}", nameof(symbolId));
			}

			#endregion
		}
	}
}
