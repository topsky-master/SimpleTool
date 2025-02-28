using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using SimpleTool.Request;
using SimpleTool.Utils;

namespace SimpleTool.Controllers
{
	public class SettingController : Controller
	{
		public Dictionary<string, List<Family>> FamilyGroups { get; set; } = [];

		public static List<string> RegisteredFamilies { get; set;} = new List<string>();

		public override void Initialize()
		{
			// Get all the Families in the Document

			Document doc = GetDocument();

			FamilyGroups = RevitHelper.GetFamiliesGroupedByCategory(doc);
		}

		public override bool ProcessRequest(SimpleToolRequestId reqId)
		{
			bool bFinish = false;
			Document doc = GetDocument();

			switch (reqId)
			{
				case SimpleToolRequestId.None:
					return bFinish;
				default:
					break;
			}

			return bFinish;
		}

	}
}
