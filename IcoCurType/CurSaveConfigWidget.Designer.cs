using System.Windows.Forms;
using PaintDotNet;

namespace IcoCurType;

internal partial class CurSaveConfigWidget : SaveConfigWidget {
	#region Windows Form Designer generated code

	private void InitializeComponent() {
		this.label1 = new System.Windows.Forms.Label();
		this.XUpDown = new System.Windows.Forms.NumericUpDown();
		this.label2 = new System.Windows.Forms.Label();
		this.YUpDown = new System.Windows.Forms.NumericUpDown();
		this.Bit32RBtn = new System.Windows.Forms.RadioButton();
		this.Bit8RBtn = new System.Windows.Forms.RadioButton();
		((System.ComponentModel.ISupportInitialize)this.XUpDown).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.YUpDown).BeginInit();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(3, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(57, 13);
		this.label1.TabIndex = 2;
		this.label1.Text = "Hotspot X:";
		this.XUpDown.Location = new System.Drawing.Point(91, 11);
		System.Windows.Forms.NumericUpDown xUpDown = this.XUpDown;
		xUpDown.Maximum = new decimal(new int[4] { 31, 0, 0, 0 });
		this.XUpDown.Name = "XUpDown";
		this.XUpDown.Size = new System.Drawing.Size(56, 20);
		this.XUpDown.TabIndex = 3;
		this.XUpDown.ValueChanged += new System.EventHandler(HotSpotChange);
		this.XUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(HotSpotKeyPress);
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(3, 36);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(57, 13);
		this.label2.TabIndex = 4;
		this.label2.Text = "Hotspot Y:";
		this.YUpDown.Location = new System.Drawing.Point(91, 37);
		System.Windows.Forms.NumericUpDown yUpDown = this.YUpDown;
		yUpDown.Maximum = new decimal(new int[4] { 31, 0, 0, 0 });
		this.YUpDown.Name = "YUpDown";
		this.YUpDown.Size = new System.Drawing.Size(56, 20);
		this.YUpDown.TabIndex = 5;
		this.YUpDown.ValueChanged += new System.EventHandler(HotSpotChange);
		this.YUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(HotSpotKeyPress);
		this.Bit32RBtn.AutoSize = true;
		this.Bit32RBtn.Checked = true;
		this.Bit32RBtn.Location = new System.Drawing.Point(6, 76);
		this.Bit32RBtn.Name = "Bit32RBtn";
		this.Bit32RBtn.Size = new System.Drawing.Size(51, 17);
		this.Bit32RBtn.TabIndex = 6;
		this.Bit32RBtn.TabStop = true;
		this.Bit32RBtn.Text = "32-bit";
		this.Bit32RBtn.UseVisualStyleBackColor = true;
		this.Bit32RBtn.CheckedChanged += new System.EventHandler(DepthCheckChanged);
		this.Bit8RBtn.AutoSize = true;
		this.Bit8RBtn.Location = new System.Drawing.Point(91, 76);
		this.Bit8RBtn.Name = "Bit8RBtn";
		this.Bit8RBtn.Size = new System.Drawing.Size(45, 17);
		this.Bit8RBtn.TabIndex = 7;
		this.Bit8RBtn.Text = "8-bit";
		this.Bit8RBtn.UseVisualStyleBackColor = true;
		this.Bit8RBtn.CheckedChanged += new System.EventHandler(DepthCheckChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.Controls.Add(this.Bit8RBtn);
		base.Controls.Add(this.Bit32RBtn);
		base.Controls.Add(this.YUpDown);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.XUpDown);
		base.Controls.Add(this.label1);
		base.Name = "CurSaveConfigWidget";
		((System.ComponentModel.ISupportInitialize)this.XUpDown).EndInit();
		((System.ComponentModel.ISupportInitialize)this.YUpDown).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	#endregion

	private Label label1;
	private NumericUpDown XUpDown;
	private Label label2;
	private RadioButton Bit32RBtn;
	private RadioButton Bit8RBtn;
	private NumericUpDown YUpDown;
}
