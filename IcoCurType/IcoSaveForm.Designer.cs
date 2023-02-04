using System.ComponentModel;
using System.Windows.Forms;

namespace IcoCurType;

internal partial class IcoSaveForm : Form {
	private IContainer components;

	protected override void Dispose(bool disposing) {
		if (disposing && components != null) {
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	private void InitializeComponent() {
		this.CancelBtn = new System.Windows.Forms.Button();
		this.OKBtn = new System.Windows.Forms.Button();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.clbFormats = new System.Windows.Forms.CheckedListBox();
		this.btnSelectAll = new System.Windows.Forms.Button();
		this.SelNoneBtn = new System.Windows.Forms.Button();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rbSeparate = new System.Windows.Forms.RadioButton();
		this.rbMerged = new System.Windows.Forms.RadioButton();
		this.groupBox4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		base.SuspendLayout();
		this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.CancelBtn.Location = new System.Drawing.Point(207, 309);
		this.CancelBtn.Name = "CancelBtn";
		this.CancelBtn.Size = new System.Drawing.Size(75, 23);
		this.CancelBtn.TabIndex = 4;
		this.CancelBtn.Text = "Cancel";
		this.CancelBtn.UseVisualStyleBackColor = true;
		this.OKBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.OKBtn.Location = new System.Drawing.Point(126, 309);
		this.OKBtn.Name = "OKBtn";
		this.OKBtn.Size = new System.Drawing.Size(75, 23);
		this.OKBtn.TabIndex = 3;
		this.OKBtn.Text = "OK";
		this.OKBtn.UseVisualStyleBackColor = true;
		this.groupBox4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox4.Controls.Add(this.clbFormats);
		this.groupBox4.Controls.Add(this.btnSelectAll);
		this.groupBox4.Controls.Add(this.SelNoneBtn);
		this.groupBox4.Location = new System.Drawing.Point(12, 141);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(270, 162);
		this.groupBox4.TabIndex = 1;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Copies to be saved";
		this.clbFormats.CheckOnClick = true;
		this.clbFormats.FormattingEnabled = true;
		this.clbFormats.Items.AddRange(new object[2] { "32x32, 32-bit", "32x32, 8-bit" });
		this.clbFormats.Location = new System.Drawing.Point(6, 19);
		this.clbFormats.Name = "clbFormats";
		this.clbFormats.Size = new System.Drawing.Size(258, 109);
		this.clbFormats.TabIndex = 3;
		this.btnSelectAll.Location = new System.Drawing.Point(6, 134);
		this.btnSelectAll.Name = "btnSelectAll";
		this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
		this.btnSelectAll.TabIndex = 2;
		this.btnSelectAll.Text = "Select All";
		this.btnSelectAll.UseVisualStyleBackColor = true;
		this.btnSelectAll.Click += new System.EventHandler(btnSelectAll_Click);
		this.SelNoneBtn.Location = new System.Drawing.Point(189, 134);
		this.SelNoneBtn.Name = "SelNoneBtn";
		this.SelNoneBtn.Size = new System.Drawing.Size(75, 23);
		this.SelNoneBtn.TabIndex = 1;
		this.SelNoneBtn.Text = "Select None";
		this.SelNoneBtn.UseVisualStyleBackColor = true;
		this.SelNoneBtn.Click += new System.EventHandler(SelNoneBtn_Click);
		this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox1.Controls.Add(this.rbSeparate);
		this.groupBox1.Controls.Add(this.rbMerged);
		this.groupBox1.Location = new System.Drawing.Point(12, 12);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(270, 123);
		this.groupBox1.TabIndex = 5;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Save Mode";
		this.rbSeparate.AutoSize = true;
		this.rbSeparate.Location = new System.Drawing.Point(16, 55);
		this.rbSeparate.Name = "rbSeparate";
		this.rbSeparate.Size = new System.Drawing.Size(251, 56);
		this.rbSeparate.TabIndex = 1;
		this.rbSeparate.Text = "Each layer as an image within the icon file.\r\nLayer names will be used to determine cropping \r\nwidths and must be in the form #x#. Examples:\r\n32x32, 64x64, etc.";
		this.rbSeparate.UseVisualStyleBackColor = true;
		this.rbSeparate.CheckedChanged += new System.EventHandler(ModeCheckChanged);
		this.rbMerged.AutoSize = true;
		this.rbMerged.Checked = true;
		this.rbMerged.Location = new System.Drawing.Point(16, 19);
		this.rbMerged.Name = "rbMerged";
		this.rbMerged.Size = new System.Drawing.Size(225, 30);
		this.rbMerged.TabIndex = 0;
		this.rbMerged.TabStop = true;
		this.rbMerged.Text = "Merged image (multiple, different resolution\r\nimage copies within the icon file)";
		this.rbMerged.UseVisualStyleBackColor = true;
		this.rbMerged.CheckedChanged += new System.EventHandler(ModeCheckChanged);
		base.AcceptButton = this.OKBtn;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.CancelBtn;
		base.ClientSize = new System.Drawing.Size(292, 342);
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.groupBox4);
		base.Controls.Add(this.OKBtn);
		base.Controls.Add(this.CancelBtn);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.Name = "IcoSaveForm";
		this.Text = "Icon Save Options";
		this.groupBox4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		base.Shown += new System.EventHandler(Form_Shown);
		base.ResumeLayout(false);
	}

	#endregion

	private Button CancelBtn;
	private Button OKBtn;
	private GroupBox groupBox4;
	private Button btnSelectAll;
	private Button SelNoneBtn;
	private CheckedListBox clbFormats;
	private GroupBox groupBox1;
	private RadioButton rbSeparate;
	private RadioButton rbMerged;
}
