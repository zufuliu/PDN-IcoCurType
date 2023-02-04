using System.ComponentModel;
using System.Windows.Forms;

namespace IcoCurType;

internal partial class ANISaveOptForm : Form {
	private IContainer components;

	protected override void Dispose(bool disposing) {
		if (disposing && components != null) {
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	private void InitializeComponent() {
		this.components = new System.ComponentModel.Container();
		this.YUpDown = new System.Windows.Forms.NumericUpDown();
		this.label2 = new System.Windows.Forms.Label();
		this.XUpDown = new System.Windows.Forms.NumericUpDown();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.AnimPictureBox = new System.Windows.Forms.PictureBox();
		this.label3 = new System.Windows.Forms.Label();
		this.CancelBtn = new System.Windows.Forms.Button();
		this.OKBtn = new System.Windows.Forms.Button();
		this.FPSCombo = new System.Windows.Forms.ComboBox();
		this.AnimTimer = new System.Windows.Forms.Timer(this.components);
		this.label4 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.YUpDown).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.XUpDown).BeginInit();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.AnimPictureBox).BeginInit();
		base.SuspendLayout();
		this.YUpDown.Location = new System.Drawing.Point(87, 35);
		System.Windows.Forms.NumericUpDown yUpDown = this.YUpDown;
		yUpDown.Maximum = new decimal(new int[4] { 31, 0, 0, 0 });
		this.YUpDown.Name = "YUpDown";
		this.YUpDown.Size = new System.Drawing.Size(56, 20);
		this.YUpDown.TabIndex = 9;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(12, 35);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(57, 13);
		this.label2.TabIndex = 8;
		this.label2.Text = "Hotspot Y:";
		this.XUpDown.Location = new System.Drawing.Point(87, 9);
		System.Windows.Forms.NumericUpDown xUpDown = this.XUpDown;
		xUpDown.Maximum = new decimal(new int[4] { 31, 0, 0, 0 });
		this.XUpDown.Name = "XUpDown";
		this.XUpDown.Size = new System.Drawing.Size(56, 20);
		this.XUpDown.TabIndex = 7;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(12, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(57, 13);
		this.label1.TabIndex = 6;
		this.label1.Text = "Hotspot X:";
		this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel1.Controls.Add(this.AnimPictureBox);
		this.panel1.Location = new System.Drawing.Point(172, 35);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(36, 36);
		this.panel1.TabIndex = 10;
		this.AnimPictureBox.Location = new System.Drawing.Point(1, 1);
		this.AnimPictureBox.Name = "AnimPictureBox";
		this.AnimPictureBox.Size = new System.Drawing.Size(32, 32);
		this.AnimPictureBox.TabIndex = 0;
		this.AnimPictureBox.TabStop = false;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(12, 61);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 13);
		this.label3.TabIndex = 11;
		this.label3.Text = "Speed (fps):";
		this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.CancelBtn.Location = new System.Drawing.Point(141, 102);
		this.CancelBtn.Name = "CancelBtn";
		this.CancelBtn.Size = new System.Drawing.Size(75, 23);
		this.CancelBtn.TabIndex = 13;
		this.CancelBtn.Text = "Cancel";
		this.CancelBtn.UseVisualStyleBackColor = true;
		this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.OKBtn.Location = new System.Drawing.Point(60, 102);
		this.OKBtn.Name = "OKBtn";
		this.OKBtn.Size = new System.Drawing.Size(75, 23);
		this.OKBtn.TabIndex = 14;
		this.OKBtn.Text = "OK";
		this.OKBtn.UseVisualStyleBackColor = true;
		this.FPSCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.FPSCombo.FormattingEnabled = true;
		this.FPSCombo.Items.AddRange(new object[9] { "1", "2", "3", "4", "5", "10", "20", "30", "60" });
		this.FPSCombo.Location = new System.Drawing.Point(87, 63);
		this.FPSCombo.Name = "FPSCombo";
		this.FPSCombo.Size = new System.Drawing.Size(56, 21);
		this.FPSCombo.TabIndex = 15;
		this.FPSCombo.SelectedIndexChanged += new System.EventHandler(FPSCombo_SelectedIndexChanged);
		this.AnimTimer.Tick += new System.EventHandler(AnimTimer_Tick);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(169, 11);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(45, 13);
		this.label4.TabIndex = 16;
		this.label4.Text = "Preview";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(228, 132);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.FPSCombo);
		base.Controls.Add(this.OKBtn);
		base.Controls.Add(this.CancelBtn);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.YUpDown);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.XUpDown);
		base.Controls.Add(this.label1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.Name = "ANISaveOptForm";
		this.Text = "Animated Cursor Options";
		((System.ComponentModel.ISupportInitialize)this.YUpDown).EndInit();
		((System.ComponentModel.ISupportInitialize)this.XUpDown).EndInit();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.AnimPictureBox).EndInit();
		base.Shown += new System.EventHandler(Form_Shown);
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	#endregion

	private NumericUpDown YUpDown;
	private Label label2;
	private NumericUpDown XUpDown;
	private Label label1;
	private Panel panel1;
	private Label label3;
	private Button CancelBtn;
	private Button OKBtn;
	private ComboBox FPSCombo;
	private Timer AnimTimer;
	private PictureBox AnimPictureBox;
	private Label label4;
}
