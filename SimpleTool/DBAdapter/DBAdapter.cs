using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Autodesk.Revit.DB;

namespace SimpleTool.DBManager
{
	public class DBAdapter
	{
		private static DBAdapter _instance;

		public static DBAdapter Instance 
		{
			get 
			{
				if(_instance == null )
					_instance = new DBAdapter();
				return _instance;
			}
		}

		/// <summary>
		/// Get database path of xml - C:\User\UserName\AppData\Roaming\SimpleTool\SimpleUpdater.xml
		/// </summary>
		/// <returns></returns>
		private string GetDBPath()
		{
			string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string dbPath = Path.Combine(appDataPath, "SimpleTool", "SimpleUpdater.xml");
			// Ensure the directory exists
			Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

			return dbPath;
		}

		/// <summary>
		/// Save families to xml
		/// </summary>
		/// <param name="registerFamilies"></param>
		public void SaveFamiliesToXml(List<string> registerFamilies)
		{
			var xmlDoc = new System.Xml.XmlDocument();
			var xmlRoot = xmlDoc.CreateElement("Families");

			foreach (var family in registerFamilies)
			{
				var xmlFamily = xmlDoc.CreateElement("Family");
				xmlFamily.InnerText = family;
				xmlRoot.AppendChild(xmlFamily);
			}

			xmlDoc.AppendChild(xmlRoot);
			xmlDoc.Save(GetDBPath());
		}

		/// <summary>
		/// Load family names from xml
		/// </summary>
		/// <returns></returns>
		public List<string> LoadFamiliesFromXml()
		{
			var registerFamilies = new List<string>();
			var filePath = GetDBPath();

			try
			{
				if (File.Exists(filePath))
				{
					var xmlDoc = new XmlDocument();
					xmlDoc.Load(filePath);

					var xmlRoot = xmlDoc.DocumentElement;
					if (xmlRoot != null)
					{
						foreach (XmlNode xmlFamily in xmlRoot.SelectNodes("Family"))
						{
							if (xmlFamily != null && !string.IsNullOrEmpty(xmlFamily.InnerText))
							{
								registerFamilies.Add(xmlFamily.InnerText);
							}
						}
					}
				}
			}
			catch
			{
			}


			return registerFamilies;
		}
	}
}
