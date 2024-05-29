namespace ZcpGIS
{
    partial class frmLayerRenderer
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSimpleRenderer = new System.Windows.Forms.TabPage();
            this.listBoxColors = new System.Windows.Forms.ListBox();
            this.tabUniqueValue = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxAttributes = new System.Windows.Forms.ListBox();
            this.tabClassBreaks = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.listBoxAttributes2 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelStart = new System.Windows.Forms.Panel();
            this.panelEnd = new System.Windows.Forms.Panel();
            this.textBreaks = new System.Windows.Forms.TextBox();
            this.btnRandomColor = new System.Windows.Forms.Button();
            this.btnEndColor = new System.Windows.Forms.Button();
            this.btnStartColor = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorDialogStart = new System.Windows.Forms.ColorDialog();
            this.colorDialogEnd = new System.Windows.Forms.ColorDialog();
            this.tabControl1.SuspendLayout();
            this.tabSimpleRenderer.SuspendLayout();
            this.tabUniqueValue.SuspendLayout();
            this.tabClassBreaks.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSimpleRenderer);
            this.tabControl1.Controls.Add(this.tabUniqueValue);
            this.tabControl1.Controls.Add(this.tabClassBreaks);
            this.tabControl1.Font = new System.Drawing.Font("隶书", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(390, 288);
            this.tabControl1.TabIndex = 0;
            // 
            // tabSimpleRenderer
            // 
            this.tabSimpleRenderer.Controls.Add(this.listBoxColors);
            this.tabSimpleRenderer.Location = new System.Drawing.Point(4, 24);
            this.tabSimpleRenderer.Name = "tabSimpleRenderer";
            this.tabSimpleRenderer.Padding = new System.Windows.Forms.Padding(3);
            this.tabSimpleRenderer.Size = new System.Drawing.Size(382, 260);
            this.tabSimpleRenderer.TabIndex = 0;
            this.tabSimpleRenderer.Text = "单一符号渲染";
            this.tabSimpleRenderer.UseVisualStyleBackColor = true;
            // 
            // listBoxColors
            // 
            this.listBoxColors.Dock = System.Windows.Forms.DockStyle.Right;
            this.listBoxColors.Font = new System.Drawing.Font("隶书", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBoxColors.FormattingEnabled = true;
            this.listBoxColors.ItemHeight = 14;
            this.listBoxColors.Items.AddRange(new object[] {
            "浅红",
            "浅黄",
            "浅蓝",
            "自定义"});
            this.listBoxColors.Location = new System.Drawing.Point(3, 3);
            this.listBoxColors.Name = "listBoxColors";
            this.listBoxColors.Size = new System.Drawing.Size(376, 254);
            this.listBoxColors.TabIndex = 0;
            this.listBoxColors.SelectedIndexChanged += new System.EventHandler(this.listBoxColors_SelectedIndexChanged);
            // 
            // tabUniqueValue
            // 
            this.tabUniqueValue.Controls.Add(this.label2);
            this.tabUniqueValue.Controls.Add(this.listBoxAttributes);
            this.tabUniqueValue.Location = new System.Drawing.Point(4, 24);
            this.tabUniqueValue.Name = "tabUniqueValue";
            this.tabUniqueValue.Padding = new System.Windows.Forms.Padding(3);
            this.tabUniqueValue.Size = new System.Drawing.Size(382, 260);
            this.tabUniqueValue.TabIndex = 1;
            this.tabUniqueValue.Text = "唯一值渲染";
            this.tabUniqueValue.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("隶书", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(7, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "请选择字段：";
            // 
            // listBoxAttributes
            // 
            this.listBoxAttributes.FormattingEnabled = true;
            this.listBoxAttributes.ItemHeight = 14;
            this.listBoxAttributes.Location = new System.Drawing.Point(3, 24);
            this.listBoxAttributes.Name = "listBoxAttributes";
            this.listBoxAttributes.Size = new System.Drawing.Size(373, 228);
            this.listBoxAttributes.TabIndex = 0;
            this.listBoxAttributes.SelectedIndexChanged += new System.EventHandler(this.listBoxAttributes_SelectedIndexChanged);
            // 
            // tabClassBreaks
            // 
            this.tabClassBreaks.Controls.Add(this.label3);
            this.tabClassBreaks.Controls.Add(this.listBoxAttributes2);
            this.tabClassBreaks.Controls.Add(this.label1);
            this.tabClassBreaks.Controls.Add(this.panelStart);
            this.tabClassBreaks.Controls.Add(this.panelEnd);
            this.tabClassBreaks.Controls.Add(this.textBreaks);
            this.tabClassBreaks.Controls.Add(this.btnRandomColor);
            this.tabClassBreaks.Controls.Add(this.btnEndColor);
            this.tabClassBreaks.Controls.Add(this.btnStartColor);
            this.tabClassBreaks.Location = new System.Drawing.Point(4, 24);
            this.tabClassBreaks.Name = "tabClassBreaks";
            this.tabClassBreaks.Padding = new System.Windows.Forms.Padding(3);
            this.tabClassBreaks.Size = new System.Drawing.Size(382, 260);
            this.tabClassBreaks.TabIndex = 2;
            this.tabClassBreaks.Text = "分级渲染";
            this.tabClassBreaks.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("隶书", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(6, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 14);
            this.label3.TabIndex = 9;
            this.label3.Text = "请选择字段：";
            // 
            // listBoxAttributes2
            // 
            this.listBoxAttributes2.FormattingEnabled = true;
            this.listBoxAttributes2.ItemHeight = 14;
            this.listBoxAttributes2.Location = new System.Drawing.Point(7, 31);
            this.listBoxAttributes2.Name = "listBoxAttributes2";
            this.listBoxAttributes2.Size = new System.Drawing.Size(150, 214);
            this.listBoxAttributes2.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("隶书", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(179, 213);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 14);
            this.label1.TabIndex = 7;
            this.label1.Text = "设置分隔数：";
            // 
            // panelStart
            // 
            this.panelStart.BackColor = System.Drawing.Color.Maroon;
            this.panelStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelStart.Location = new System.Drawing.Point(276, 44);
            this.panelStart.Name = "panelStart";
            this.panelStart.Size = new System.Drawing.Size(100, 22);
            this.panelStart.TabIndex = 6;
            // 
            // panelEnd
            // 
            this.panelEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.panelEnd.Location = new System.Drawing.Point(276, 100);
            this.panelEnd.Name = "panelEnd";
            this.panelEnd.Size = new System.Drawing.Size(100, 22);
            this.panelEnd.TabIndex = 5;
            // 
            // textBreaks
            // 
            this.textBreaks.Font = new System.Drawing.Font("隶书", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBreaks.Location = new System.Drawing.Point(276, 210);
            this.textBreaks.Name = "textBreaks";
            this.textBreaks.Size = new System.Drawing.Size(100, 23);
            this.textBreaks.TabIndex = 4;
            this.textBreaks.Text = "5";
            this.textBreaks.TextChanged += new System.EventHandler(this.textBreaks_TextChanged);
            // 
            // btnRandomColor
            // 
            this.btnRandomColor.Font = new System.Drawing.Font("隶书", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRandomColor.Location = new System.Drawing.Point(226, 141);
            this.btnRandomColor.Name = "btnRandomColor";
            this.btnRandomColor.Size = new System.Drawing.Size(107, 48);
            this.btnRandomColor.TabIndex = 2;
            this.btnRandomColor.Text = "随机颜色";
            this.btnRandomColor.UseVisualStyleBackColor = true;
            this.btnRandomColor.Click += new System.EventHandler(this.btnRandomColor_Click);
            // 
            // btnEndColor
            // 
            this.btnEndColor.Font = new System.Drawing.Font("隶书", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEndColor.Location = new System.Drawing.Point(163, 87);
            this.btnEndColor.Name = "btnEndColor";
            this.btnEndColor.Size = new System.Drawing.Size(107, 48);
            this.btnEndColor.TabIndex = 1;
            this.btnEndColor.Text = "设置最终颜色";
            this.btnEndColor.UseVisualStyleBackColor = true;
            this.btnEndColor.Click += new System.EventHandler(this.btnEndColor_Click);
            // 
            // btnStartColor
            // 
            this.btnStartColor.Font = new System.Drawing.Font("隶书", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStartColor.Location = new System.Drawing.Point(163, 33);
            this.btnStartColor.Name = "btnStartColor";
            this.btnStartColor.Size = new System.Drawing.Size(107, 48);
            this.btnStartColor.TabIndex = 0;
            this.btnStartColor.Text = "设置起始颜色";
            this.btnStartColor.UseVisualStyleBackColor = true;
            this.btnStartColor.Click += new System.EventHandler(this.btnStartColor_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(323, 308);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(242, 308);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmLayerRenderer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 343);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLayerRenderer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图层渲染";
            this.Load += new System.EventHandler(this.frmLayerRenderer_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabSimpleRenderer.ResumeLayout(false);
            this.tabUniqueValue.ResumeLayout(false);
            this.tabUniqueValue.PerformLayout();
            this.tabClassBreaks.ResumeLayout(false);
            this.tabClassBreaks.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSimpleRenderer;
        private System.Windows.Forms.TabPage tabUniqueValue;
        private System.Windows.Forms.TabPage tabClassBreaks;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ListBox listBoxColors;
        private System.Windows.Forms.ListBox listBoxAttributes;
        private System.Windows.Forms.Button btnRandomColor;
        private System.Windows.Forms.Button btnEndColor;
        private System.Windows.Forms.Button btnStartColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelStart;
        private System.Windows.Forms.Panel panelEnd;
        private System.Windows.Forms.TextBox textBreaks;
        private System.Windows.Forms.ListBox listBoxAttributes2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColorDialog colorDialogStart;
        private System.Windows.Forms.ColorDialog colorDialogEnd;
    }
}