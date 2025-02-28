using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SimpleTool.Request;

namespace SimpleTool.Commands
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	public class SettingCommand : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIApplication uiApp = commandData.Application;
			Application.thisApp.DoRequest(uiApp, SimpleToolRequestId.SettingForm);

			return Result.Succeeded;
		}
	}
}
