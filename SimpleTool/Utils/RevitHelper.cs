using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace SimpleTool.Utils
{
	public class RevitHelper
	{
		public static Dictionary<string, List<Family>> GetFamiliesGroupedByCategory(Document doc)
		{
			Dictionary<string, List<Family>> groupedFamilies = new Dictionary<string, List<Family>>();

			// Get all families in the document
			List<Family> families = new FilteredElementCollector(doc)
															.OfClass(typeof(Family))
															.Cast<Family>()
															.ToList();

			foreach (var family in families)
			{
				string categoryName = family.FamilyCategory != null ? family.FamilyCategory.Name : "Uncategorized";

				if (!groupedFamilies.ContainsKey(categoryName))
				{
					groupedFamilies[categoryName] = new List<Family>();
				}

				groupedFamilies[categoryName].Add(family);
			}

			// Return a dictionary sorted by category name
			return groupedFamilies.OrderBy(kvp => kvp.Key)
														.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}
	}
}
