using Autodesk.Revit.DB;
using System.Collections.Generic;
using Autodesk.Revit.UI;
using SimpleTool.Controllers;
using SimpleTool.Request;
using System.IO;
using System;
using SimpleTool.DBManager;
using System.Linq;

namespace SimpleTool.UIController
{
	public class SettingUIController : BaseUIController
	{
		// The dialog owns the handler and the event objects,
		// but it is not a requirement. They may as well be static properties
		// of the application.
		protected SimpleToolRequestHandler m_Handler;
		protected ExternalEvent m_ExEvent;

		public SimpleToolRequestHandler Handler { get => m_Handler; }

		public SimpleToolRequestId LastRequestId { get; set; } = SimpleToolRequestId.None;

		public Dictionary<string, List<Family>> FamilyGroups { get; set; } = [];

		public SettingUIController(SimpleToolRequestId reqId, UIApplication uiApp)
		{
			//	A new handler to handle request posting by the dialog
			m_Handler = new SimpleToolRequestHandler(reqId, uiApp);

			//	External Event for the dialog to use (to post requests)
			m_ExEvent = ExternalEvent.Create(m_Handler);

			//	Initialize Data Context
			SettingController ins = m_Handler.Instance as SettingController;

			ins.Initialize();

			FamilyGroups = ins.FamilyGroups;

			WakeUp();
		}

		#region IExternal implementation
		public override void MakeRequest(int request)
		{
			LastRequestId = (SimpleToolRequestId)request;

			m_Handler.Request.Make(LastRequestId);
			m_ExEvent.Raise();
		}

		public override void WakeUp(bool bFinish = false)
		{
		}

		public override int GetRequestId()
		{
			if (m_Handler == null)
			{
				return (int)SimpleToolRequestId.None;
			}
			return (int)m_Handler.RequestId;
		}
		#endregion

		#region Implement Controller
		public override void OnOK()
		{
			//	Post a request to the handler
			MakeRequest((int)SimpleToolRequestId.None);
		}

		// Save the registered families to the XML file
		public bool SaveFamiliesToXml(List<string> registerFamilies)
		{
			bool bFinish = true;

			DBAdapter.Instance.SaveFamiliesToXml(registerFamilies);

			return bFinish;
		}

		//private bool Validate(List<string> registerFamilies)
		//{
		//	bool bRes = true;
		//	// Get the top three categories from FamilyGroups
		//	var topThreeCategories = FamilyGroups.Keys.Take(3).ToList();

		//	// Check if registerFamilies are contained in the top three categories
		//	foreach (var family in registerFamilies)
		//	{
		//		bool familyFound = false;
		//		foreach (var category in topThreeCategories)
		//		{
		//			if (FamilyGroups[category].Any(f => f.Name == family))
		//			{
		//				familyFound = true;
		//				break;
		//			}
		//		}
		//		if (!familyFound)
		//		{
		//			bRes = false;
		//			break;
		//		}
		//	}

		//	return bRes;
		//}
		#endregion
	}
}
