namespace PanelTool.Custom.Forms
{
	partial class SettingForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.BtnDelete = new System.Windows.Forms.Button();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.listRegisterFamilies = new System.Windows.Forms.ListBox();
			this.BtnAdd = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.BtnOK = new System.Windows.Forms.Button();
			this.treeViewFamilies = new System.Windows.Forms.TreeView();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// BtnDelete
			// 
			this.BtnDelete.Location = new System.Drawing.Point(560, 148);
			this.BtnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.BtnDelete.Name = "BtnDelete";
			this.BtnDelete.Size = new System.Drawing.Size(75, 28);
			this.BtnDelete.TabIndex = 0;
			this.BtnDelete.Text = "Delete";
			this.BtnDelete.UseVisualStyleBackColor = true;
			this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BtnCancel.Location = new System.Drawing.Point(557, 323);
			this.BtnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(75, 28);
			this.BtnCancel.TabIndex = 1;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			// 
			// listRegisterFamilies
			// 
			this.listRegisterFamilies.FormattingEnabled = true;
			this.listRegisterFamilies.ItemHeight = 16;
			this.listRegisterFamilies.Location = new System.Drawing.Point(14, 28);
			this.listRegisterFamilies.Name = "listRegisterFamilies";
			this.listRegisterFamilies.Size = new System.Drawing.Size(204, 260);
			this.listRegisterFamilies.TabIndex = 3;
			// 
			// BtnAdd
			// 
			this.BtnAdd.Location = new System.Drawing.Point(246, 148);
			this.BtnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.BtnAdd.Name = "BtnAdd";
			this.BtnAdd.Size = new System.Drawing.Size(75, 28);
			this.BtnAdd.TabIndex = 0;
			this.BtnAdd.Text = "Add >>";
			this.BtnAdd.UseVisualStyleBackColor = true;
			this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.listRegisterFamilies);
			this.groupBox1.Location = new System.Drawing.Point(325, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(230, 300);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Registered Family List";
			// 
			// BtnOK
			// 
			this.BtnOK.Location = new System.Drawing.Point(476, 323);
			this.BtnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.BtnOK.Name = "BtnOK";
			this.BtnOK.Size = new System.Drawing.Size(75, 28);
			this.BtnOK.TabIndex = 1;
			this.BtnOK.Text = "OK";
			this.BtnOK.UseVisualStyleBackColor = true;
			this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
			// 
			// treeViewFamilies
			// 
			this.treeViewFamilies.Location = new System.Drawing.Point(15, 28);
			this.treeViewFamilies.Name = "treeViewFamilies";
			this.treeViewFamilies.Size = new System.Drawing.Size(200, 260);
			this.treeViewFamilies.TabIndex = 5;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.treeViewFamilies);
			this.groupBox2.Location = new System.Drawing.Point(12, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(230, 300);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Family List";
			// 
			// SettingForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(644, 361);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.BtnOK);
			this.Controls.Add(this.BtnCancel);
			this.Controls.Add(this.BtnAdd);
			this.Controls.Add(this.BtnDelete);
			this.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "SettingForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Register";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnDelete;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.ListBox listRegisterFamilies;
		private System.Windows.Forms.Button BtnAdd;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button BtnOK;
		private System.Windows.Forms.TreeView treeViewFamilies;
		private System.Windows.Forms.GroupBox groupBox2;
	}
}