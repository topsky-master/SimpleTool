using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleTool;
using SimpleTool.Forms;
using SimpleTool.Request;
using SimpleTool.UIController;
using Family = Autodesk.Revit.DB.Family;

namespace PanelTool.Custom.Forms
{
	public partial class SettingForm : Form, IExternal
	{
		private SettingUIController _formController;

		private bool _isDisposed = false;

		public SettingForm()
		{
			InitializeComponent();
		}

		public SettingForm(SettingUIController controller)
		{
			InitializeComponent();

			_formController = controller;

			LoadFamiliesIntoTreeView();

			LoadRegisteredFamiliesToListBox();
		}

		#region IExternal interface implementation

		private void OnClosed(object sender, EventArgs e)
		{
			_isDisposed = true;
		}

		public int GetRequestId()
		{
			return -1;
		}

		public void MakeRequest(int request)
		{
			DozeOff();
		}

		public void DozeOff()
		{

		}

		public void WakeUp(bool bFinish = false)
		{
			if (bFinish)
			{
				Close();
				return;
			}
		}

		public void IClose()
		{
			if (!_isDisposed)
			{
				Close();
				_isDisposed = true;
			}
		}

		public bool IVisible()
		{
			return true;
		}

		public bool IIsDisposed()
		{
			return _isDisposed;
		}

		public void IShow()
		{
			if (!_isDisposed)
			{
				ShowDialog();
			}
		}

		#endregion

		#region Event Handlers


		private void LoadFamiliesIntoTreeView()
		{
			treeViewFamilies.Nodes.Clear();  // Clear previous items if any

			Dictionary<string, List<Family>> familyGroups = _formController.FamilyGroups;

			foreach (var category in familyGroups)
			{
				TreeNode categoryNode = new TreeNode(category.Key); // Add category as parent node

				foreach (Family family in category.Value)
				{
					TreeNode familyNode = new TreeNode(family.Name); // Add family as child node
					categoryNode.Nodes.Add(familyNode);
				}

				treeViewFamilies.Nodes.Add(categoryNode);
			}
		}

		private void LoadRegisteredFamiliesToListBox()
		{
			listRegisterFamilies.Items.Clear();

			List<string> registeredFamilies = SimpleUpdater.RegisteredFamilies;

			foreach (var family in registeredFamilies)
			{
				listRegisterFamilies.Items.Add(family);
			}
		}

		#endregion

		private void BtnAdd_Click(object sender, EventArgs e)
		{
			if (treeViewFamilies.SelectedNode != null)
			{
				TreeNode selectedNode = treeViewFamilies.SelectedNode;
				if (selectedNode.Parent != null)
				{
					listRegisterFamilies.Items.Add(selectedNode.Text);
				}
			}
		}

		private void BtnDelete_Click(object sender, EventArgs e)
		{
			int selectedIndex = listRegisterFamilies.SelectedIndex;

			if (selectedIndex != -1) // Ensure an item is selected
			{
				listRegisterFamilies.Items.RemoveAt(selectedIndex);
			}
			else
			{
				MessageBox.Show("Please select an item to delete.");
			}
		}

		private void BtnOK_Click(object sender, EventArgs e)
		{
			List<string> registerFamilies = [];
			
			foreach(var item in listRegisterFamilies.Items)
			{
				registerFamilies.Add(item.ToString());
			}

			try
			{
				var res = _formController.SaveFamiliesToXml(registerFamilies);

				if (res)
				{
					MessageBox.Show("Families registered successfully.", "Notice");

					SimpleUpdater.RegisteredFamilies = registerFamilies;

					Close();
				}
				else
				{
					// TODO : Handle error

				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error", ex.Message);
			}
		}
	}
}
