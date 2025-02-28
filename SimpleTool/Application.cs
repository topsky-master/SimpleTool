using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using SimpleTool.Request;
using SimpleTool.Utils;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Windows.Media.Imaging;
using SimpleTool.Forms;
using SimpleTool.Controllers;
using Autodesk.Revit.DB;
using static SimpleTool.Controllers.SOWBeamController;
using PanelTool.Custom.Forms;
using SimpleTool.UIController;
using SimpleTool.DBManager;

namespace SimpleTool
{
	public class Application : IExternalApplication
	{
		//	class instance
		public static Application thisApp = null;//	internal
		internal static UIControlledApplication UIContApp = null;

		private readonly List<IExternal> m_Forms = new();
		private readonly List<Assembly> m_Assemblies = new();

		private EventHandler<DocumentChangedEventArgs> m_hDocChanged = null;
		private EventHandler<DocumentOpenedEventArgs> m_hDocOpened = null;
		private EventHandler<ViewActivatedEventArgs> m_hViewActivated = null;

		private PushButton m_btnShowHideHLVoids = null;
		internal static object SimpleUpdater;

		public Result OnShutdown(UIControlledApplication application)
		{
			for (int i = 0; i < m_Forms.Count; i++)
			{
				if (m_Forms[i].IVisible())
					m_Forms[i].IClose();
			}
			return Result.Succeeded;
		}

		public Result OnStartup(UIControlledApplication application)
		{
			thisApp = this;
			UIContApp = application;

			PushButton btn;
			BitmapImage largeImage;

			SubscribeToOpen(GetUiApplication());
			SubscribeToChanges(GetUiApplication());

			m_hViewActivated = new EventHandler<ViewActivatedEventArgs>(OnViewActivated);
			application.ViewActivated += m_hViewActivated;

			//	Create a custom ribbon tab
			string tabName = Constants.BRAND;
			try
			{
				application.CreateRibbonTab(tabName);
			}
			catch (Exception) { }

			string url = Assembly.GetExecutingAssembly().Location;

			//	Get the url of the BasicSplit plugin
			url = url.Substring(0, url.LastIndexOf("\\")) + "\\" + "SimpleTool.dll";

			if (File.Exists(url))
			{
				//	Load the plugin
				Assembly assembly = Assembly.LoadFrom(url);
				m_Assemblies.Add(assembly);

				//  Create a ribbon panel
				RibbonPanel simpleTool = application.CreateRibbonPanel(tabName, "Settings");

				//	Create push buttons
				PushButtonData btnSetting = new("btnSimpleTool", "Register", url, "SimpleTool.Commands.SettingCommand");

				btn = simpleTool.AddItem(btnSetting) as PushButton;
				largeImage = GetEmbeddedImage("settings_32.png");
				btn.LargeImage = largeImage;
			}

			return Result.Succeeded;
		}

		public Type GetClassType(string sClassName)
		{
			foreach (Assembly assembly in m_Assemblies)
			{
				IEnumerable<Type> types = null;
				try { types = assembly.ExportedTypes; }
				catch (Exception) { continue; }
				foreach (Type t in types)
				{
					if (t.Name == sClassName)
					{
						return t;
					}
				}
			}
			return null;
		}

		public object GetClassInstance(string sClassName)
		{
			foreach (Assembly assembly in m_Assemblies)
			{
				IEnumerable<Type> types = null;
				try { types = assembly.ExportedTypes; }
				catch (Exception) { continue; }
				foreach (Type t in types)
				{
					if (t.Name == sClassName)
					{
						return Activator.CreateInstance(t);
					}
				}
			}
			return null;
		}

		public object GetClassInstance(string sClassName, params object[] args)
		{
			foreach (Assembly assembly in m_Assemblies)
			{
				foreach (Type t in assembly.ExportedTypes)
				{
					if (t.Name == sClassName)
					{
						return Activator.CreateInstance(t, args);
					}
				}
			}
			return null;
		}

		//	This method creates and shows a modeless dialog, unless it already exists.
		//	<remarks>
		//		The external command invokes this on the end-user's request
		//	</remarks>
		public void DoRequest(UIApplication uiapp, SimpleToolRequestId reqId)
		{
			for (int i = m_Forms.Count - 1; i >= 0; i--)
			{
				IExternal f = m_Forms[i];
				if (f.IIsDisposed())
				{
					f.IClose();
					m_Forms.RemoveAt(i);
					continue;
				}
			}

			// We give the objects to the new dialog;
			// The dialog becomes the owner responsible fore disposing them, eventually.
			IExternal form = null;
			BaseUIController controller = null;

			switch (reqId)
			{
				case SimpleToolRequestId.SettingForm:
					controller = new SettingUIController(reqId, uiapp);
					form = (IExternal)GetClassInstance("SettingForm", controller);
					m_Forms.Add(form);
					break;
				default:
					break;
			}

			if (form != null)
			{
				form.IShow();
				m_Forms.Add(form);
			}
		}

		//	Waking up the dialog from its waiting state.
		public void WakeRequestUp(SimpleToolRequestId reqId, bool bFinish = false)
		{
			foreach (IExternal f in m_Forms)
			{
				if (f.GetRequestId() == (int)reqId)
				{
					f.WakeUp(bFinish);
				}
			}
		}

		public UIControlledApplication GetUIContApp()
		{
			return UIContApp;
		}

		public static UIApplication GetUiApplication()
		{
			string versionNumber = UIContApp.ControlledApplication.VersionNumber;
			string fieldName = versionNumber switch
			{
				"2017" or "2018" or "2019" or "2020" or "2021" or "2022" or "2023" or "2024" or "2025" => "m_uiapplication",
				_ => "m_uiapplication",
			};
			var fieldInfo = UIContApp.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

			var uiApplication = (UIApplication)fieldInfo?.GetValue(UIContApp);

			return uiApplication;
		}

		/// <summary>
		///   Subscription to DocumentChanged
		/// </summary>
		/// <remarks>
		///   We hold the delegate to remember we we have subscribed.
		/// </remarks>
		/// 
		private void SubscribeToChanges(UIApplication uiapp)
		{
			if (m_hDocChanged == null)
			{
				m_hDocChanged = new EventHandler<DocumentChangedEventArgs>(DocChangedHandler);
				uiapp.Application.DocumentChanged += m_hDocChanged;
			}
		}

		private void SubscribeToOpen(UIApplication uiapp)
		{
			if (m_hDocOpened == null)
			{
				m_hDocOpened = new EventHandler<DocumentOpenedEventArgs>(DocOpenedHandler);
				uiapp.Application.DocumentOpened += m_hDocOpened;
			}
		}

		/// <summary>
		///   Unsubscribing from DocumentChanged event
		/// </summary>
		/// 
		private void UnsubscribeFromChanges(UIApplication uiapp)
		{
			if (m_hDocChanged != null)
			{
				uiapp.Application.DocumentChanged -= m_hDocChanged;
				m_hDocChanged = null;
			}
		}

		/// <summary>
		///   Unsubscribing from DocumentOpened event
		/// </summary>
		/// 
		private void UnsubscribeFromOpen(UIApplication uiapp)
		{
			if (m_hDocOpened != null)
			{
				uiapp.Application.DocumentOpened -= m_hDocOpened;
				m_hDocOpened = null;
			}
		}

		/// <summary>
		///   DocumentChanged Handler
		/// </summary>
		/// <remarks>
		///   It monitors changes to the element that is being analyzed.
		///   If the element was changed, we ask it to restart the analysis.
		///   If the element was deleted, we ask the analyzer to stop.
		/// </remarks>
		/// 
		public void DocChangedHandler(object sender, DocumentChangedEventArgs args)
		{
			try
			{
				MethodInfo mi = thisApp.GetClassType("SOWBeamController")?.GetMethod("DocChangedHandler");
				bool bHasSOW = false;
				if (mi != null)
				{
					bHasSOW = (bool)mi.Invoke(null, new object[] { args });
				}

				if (bHasSOW)
				{
					//thisApp.GetUIContApp().Idling += OnIdlingEvent;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Can not load tool. Please contact developer.");
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
			}
		}

		public void DocOpenedHandler(object sender, DocumentOpenedEventArgs args)
		{
			try
			{
				// Register Updater
				var su = new SimpleUpdater(args.Document, args.Document.Application.ActiveAddInId);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Can not load Simple tool. Please contact developer.");
				MessageBox.Show(ex.StackTrace);
			}
		}

		public void OnIdlingEvent(object sender, IdlingEventArgs e)
		{
			MethodInfo mi = thisApp.GetClassType("SOWBeamController").GetMethod("OnIdlingEvent");
			if (mi != null)
			{
				mi.Invoke(null, new object[] { sender, e });
			}

			thisApp.GetUIContApp().Idling -= OnIdlingEvent;
		}

		public void OnViewActivated(object sender, ViewActivatedEventArgs args)
		{
			for (int i = m_Forms.Count - 1; i >= 0; i--)
			{
				IExternal f = m_Forms[i];
				if (f.IIsDisposed())
				{
					f.IClose();
					m_Forms.RemoveAt(i);
					continue;
				}

				f.IClose();
				m_Forms.Remove(f);
			}
		}

		public BitmapImage GetEmbeddedImage(string resourceName)
		{
			var assembly = Assembly.GetExecutingAssembly();

			string[] names = assembly.GetManifestResourceNames();
			string prefix = "";
			foreach (string name in names)
			{
				if(name.IndexOf(".Resources.") > 0
					&& name.IndexOf(".Properties.Resources.") < 0)
				{
					prefix = name.Substring(0, name.IndexOf(".Resources"));
					prefix += ".Resources.";
					break;
				}
			}

			using (Stream stream = assembly.GetManifestResourceStream(prefix + resourceName))
			{
				if (stream == null)
					throw new FileNotFoundException($"Resource '{resourceName}' not found in assembly.");

				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.StreamSource = stream;
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.EndInit();
				return bitmapImage;
			}
		}
	}

	public class SimpleUpdater : IUpdater
	{
		private UpdaterId m_updaterId;

		public static List<SOWDefaultParam> SOWDefaultParams;

		public static List<string> RegisteredFamilies;

		public SimpleUpdater(Document doc, AddInId addInId)
		{
			m_updaterId = new UpdaterId(addInId, new Guid("d42d28af-d2cd-4f07-8873-e7cfb61903d8"));
			RegisterUpdater(doc);
			RegisterTriggers();

			SOWDefaultParams = new List<SOWDefaultParam>
			{
				new SOWDefaultParam(0.0, 4.0, 1, 1),
				new SOWDefaultParam(4.0, 8.0, 2, 1),
				new SOWDefaultParam(8.0, 10.0, 3, 1),
				new SOWDefaultParam(10.0, 14.0, 3, 2)
			};

			RegisteredFamilies = DBAdapter.Instance.LoadFamiliesFromXml();
		}

		public void Execute(UpdaterData data)
		{
			Document document = data.GetDocument();
			ICollection<ElementId> addedElementIds = data.GetAddedElementIds();
			if (addedElementIds.Count > 0)
			{
				foreach (ElementId elementId in addedElementIds)
				{
					FamilyInstance familyInstance = document.GetElement(elementId) as FamilyInstance;
					if (familyInstance != null)
					{
						try
						{
							if (familyInstance.Symbol.FamilyName == "HDR")
							{
								Parameter parameter = familyInstance.LookupParameter("TrueLength");
								double fTrueLength = parameter.AsDouble();
								SOWDefaultParam sowdefaultParam = (from x in SOWDefaultParams where x.MinLengthOfBeam < fTrueLength && x.MaxLengthOfBeam > fTrueLength select x).FirstOrDefault<SOWDefaultParam>();
								if (sowdefaultParam == null)
								{
									sowdefaultParam = SOWDefaultParams.Last<SOWDefaultParam>();
								}
								parameter = familyInstance.LookupParameter("TrimmerStudCount");
								parameter.Set(sowdefaultParam.Trimmers);
								parameter = familyInstance.LookupParameter("KingStudCount");
								parameter.Set(sowdefaultParam.KingStuds);
							}
							else
							{
								if (RegisteredFamilies.Contains(familyInstance.Symbol.FamilyName))
								{
									// Retrieve the "HD" type parameter value
									Parameter hdParameter = familyInstance.Symbol.LookupParameter("HD le");
									if (hdParameter != null)
									{
										double hdValue = hdParameter.AsDouble();

										// Set the "Elevation From Level" instance parameter
										Parameter elevationParameter = familyInstance.LookupParameter("Elevation from Level");
										if (elevationParameter != null)
										{
											elevationParameter.Set(-hdValue);
										}
									}
								}
							}
						}
						catch (Exception ex)
						{
							MessageBox.Show("Error", ex.Message);
						}
					}
				}
			}
		}

		public UpdaterId GetUpdaterId()
		{
			return this.m_updaterId;
		}

		public ChangePriority GetChangePriority()
		{
			return ChangePriority.Structure;
		}

		public string GetUpdaterName()
		{
			return "SimpleUpdater";
		}

		public string GetAdditionalInformation()
		{
			return "NA";
		}
		public void RegisterUpdater(Document doc)
		{
			if (UpdaterRegistry.IsUpdaterRegistered(this.m_updaterId, doc))
			{
				UpdaterRegistry.RemoveAllTriggers(this.m_updaterId);
				UpdaterRegistry.UnregisterUpdater(this.m_updaterId, doc);
			}
			UpdaterRegistry.RegisterUpdater(this, doc);
		}

		public void RegisterTriggers()
		{
			ElementCategoryFilter structuralFramingFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
			ElementCategoryFilter structuralColumnFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns);
			ElementCategoryFilter genericModelFilter = new ElementCategoryFilter(BuiltInCategory.OST_GenericModel);

			LogicalOrFilter combinedFilter = new LogicalOrFilter(new List<ElementFilter>
			 {
					 structuralFramingFilter,
					 structuralColumnFilter,
					 genericModelFilter
			 });
			if (this.m_updaterId != null && UpdaterRegistry.IsUpdaterRegistered(this.m_updaterId))
			{
				UpdaterRegistry.RemoveAllTriggers(this.m_updaterId);
				UpdaterRegistry.AddTrigger(this.m_updaterId, combinedFilter, Element.GetChangeTypeElementAddition());
			}
		}
	}
}
