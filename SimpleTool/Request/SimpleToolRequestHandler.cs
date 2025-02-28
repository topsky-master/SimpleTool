using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace SimpleTool.Request
{
	//	A class with methods to execute requests made by the SplitSettings Dialog
	public class SimpleToolRequestHandler : IExternalEventHandler
	{
		//	The value of the latest request made by the form 
		public SimpleToolRequest Request { get; } = new SimpleToolRequest();

		public SimpleToolRequestId RequestId { get; set; }

		//	Controller Class Instance
		public Controller Instance { get; set; }

		//	A method to identify this External Event Handler
		public string GetName()
		{
			return "SimpleToolRequest";
		}

		public SimpleToolRequestHandler(SimpleToolRequestId reqId, UIApplication uiapp)
		{
			Initialize(reqId, uiapp);
		}

		protected void Initialize(SimpleToolRequestId reqId, UIApplication uiapp)
		{
			RequestId = reqId;
			switch (reqId)
			{
				case SimpleToolRequestId.SettingForm:
						Instance = (Controller)Application.thisApp.GetClassInstance("SettingController");
					break;
				default:
					break;
			}

			Instance.UIApp = uiapp;
		}

		//	The top method of the event handler.
		//	<remarks>
		//		This is called by Revit after the corresponding
		//		external event was raised (by the modeless form)
		//		and Revit reached the time at which it could call
		//		the event's handler (i.e. this object)
		//	</remarks>
		public void Execute(UIApplication uiapp)
		{
			bool bFinish = false;
			try
			{
				Document doc = uiapp.ActiveUIDocument.Document;

				SimpleToolRequestId reqId = Request.Take();

			}
			catch (Exception ex)
			{
				TaskDialog.Show("Error", ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
			finally
			{
				Application.thisApp.WakeRequestUp(RequestId, bFinish);
			}
		}
	}
}
