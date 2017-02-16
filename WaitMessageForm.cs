/* 项目:不用部署就运行网站
 * 日期:2009.10.1
   来源:奎宇工作室 整理
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace StartExamples
{
	public class WaitMessageForm : Form
	{
		private IContainer components;

		private Label label1;

		public WaitMessageForm()
		{
			this.InitializeComponent();
		}

		private void closeTimer_Tick(object sender, EventArgs e)
		{
			((Timer)sender).Stop();
			((Timer)sender).Dispose();
			base.Close();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(WaitMessageForm));
			this.label1 = new Label();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.label1.Location = new Point(26, 26);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x205, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "请稍等,正在启动web服务器...";
			this.label1.UseWaitCursor = true;
			base.AccessibleDescription = "ASP.NET Development server loader";
			base.AccessibleName = "Start Examples";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CausesValidation = false;
			base.ClientSize = new Size(0x23f, 70);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.None;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "WaitMessageForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "加载中...";
			base.UseWaitCursor = true;
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			Timer timer = new Timer();
			timer.Interval = 0xfa0;
			timer.Tick += new EventHandler(this.closeTimer_Tick);
			timer.Start();
		}
	}
}