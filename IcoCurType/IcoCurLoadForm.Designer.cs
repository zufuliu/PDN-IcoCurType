using System.ComponentModel;
using System.Windows.Forms;

namespace IcoCurType;

internal partial class IcoCurLoadForm : Form {
	private IContainer components;

	protected override void Dispose(bool disposing) {
		if (disposing && components != null) {
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	private void InitializeComponent() {
		this.RadioItemsGBox = new System.Windows.Forms.GroupBox();
		this.LoadAllRBtn = new System.Windows.Forms.RadioButton();
		this.LoadOneRBtn = new System.Windows.Forms.RadioButton();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.CancelBtn = new System.Windows.Forms.Button();
		this.listBox1 = new System.Windows.Forms.ListBox();
		this.OKBtn = new System.Windows.Forms.Button();
		this.RadioItemsGBox.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.RadioItemsGBox.Controls.Add(this.LoadAllRBtn);
		this.RadioItemsGBox.Controls.Add(this.LoadOneRBtn);
		this.RadioItemsGBox.Location = new System.Drawing.Point(12, 12);
		this.RadioItemsGBox.Name = "RadioItemsGBox";
		this.RadioItemsGBox.Size = new System.Drawing.Size(200, 70);
		this.RadioItemsGBox.TabIndex = 0;
		this.RadioItemsGBox.TabStop = false;
		this.RadioItemsGBox.Text = "There are (x) images available";
		this.LoadAllRBtn.AutoSize = true;
		this.LoadAllRBtn.Location = new System.Drawing.Point(16, 42);
		this.LoadAllRBtn.Name = "LoadAllRBtn";
		this.LoadAllRBtn.Size = new System.Drawing.Size(143, 17);
		this.LoadAllRBtn.TabIndex = 1;
		this.LoadAllRBtn.TabStop = true;
		this.LoadAllRBtn.Text = "Load all available images";
		this.LoadAllRBtn.UseVisualStyleBackColor = true;
		this.LoadAllRBtn.CheckedChanged += new System.EventHandler(LoadAllRBtn_CheckedChanged);
		this.LoadOneRBtn.AutoSize = true;
		this.LoadOneRBtn.Checked = true;
		this.LoadOneRBtn.Location = new System.Drawing.Point(16, 19);
		this.LoadOneRBtn.Name = "LoadOneRBtn";
		this.LoadOneRBtn.Size = new System.Drawing.Size(145, 17);
		this.LoadOneRBtn.TabIndex = 0;
		this.LoadOneRBtn.TabStop = true;
		this.LoadOneRBtn.Text = "Load only selected image";
		this.LoadOneRBtn.UseVisualStyleBackColor = true;
		this.LoadOneRBtn.CheckedChanged += new System.EventHandler(LoadOneRBtn_CheckedChanged);
		this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBox1.Location = new System.Drawing.Point(218, 12);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(256, 256);
		this.pictureBox1.TabIndex = 1;
		this.pictureBox1.TabStop = false;
		this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.CancelBtn.Location = new System.Drawing.Point(399, 274);
		this.CancelBtn.Name = "CancelBtn";
		this.CancelBtn.Size = new System.Drawing.Size(75, 23);
		this.CancelBtn.TabIndex = 2;
		this.CancelBtn.Text = "Cancel";
		this.CancelBtn.UseVisualStyleBackColor = true;
		this.listBox1.FormattingEnabled = true;
		this.listBox1.Location = new System.Drawing.Point(12, 95);
		this.listBox1.Name = "listBox1";
		this.listBox1.Size = new System.Drawing.Size(200, 173);
		this.listBox1.TabIndex = 3;
		this.listBox1.SelectedIndexChanged += new System.EventHandler(listBox1_SelectedIndexChanged);
		this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.OKBtn.Location = new System.Drawing.Point(318, 274);
		this.OKBtn.Name = "OKBtn";
		this.OKBtn.Size = new System.Drawing.Size(75, 23);
		this.OKBtn.TabIndex = 4;
		this.OKBtn.Text = "OK";
		this.OKBtn.UseVisualStyleBackColor = true;
		base.AcceptButton = this.OKBtn;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.CancelBtn;
		base.ClientSize = new System.Drawing.Size(485, 307);
		base.Controls.Add(this.OKBtn);
		base.Controls.Add(this.listBox1);
		base.Controls.Add(this.CancelBtn);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.RadioItemsGBox);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.Name = "IcoCurLoadForm";
		this.Text = "Icon/Cursor Load Options";
		base.Shown += new System.EventHandler(IcoCurLoadForm_Shown);
		this.RadioItemsGBox.ResumeLayout(false);
		this.RadioItemsGBox.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
	}

	#endregion

	private GroupBox RadioItemsGBox;
	private RadioButton LoadAllRBtn;
	private RadioButton LoadOneRBtn;
	private PictureBox pictureBox1;
	private Button CancelBtn;
	private ListBox listBox1;
	private Button OKBtn;
}
