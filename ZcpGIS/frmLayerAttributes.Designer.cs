namespace ZcpGIS
{
    partial class frmLayerAttributes
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvAttributes = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.tsSelectAttributes = new System.Windows.Forms.ToolStripComboBox();
            this.tsSearchText = new System.Windows.Forms.ToolStripTextBox();
            this.tsSearchResult = new System.Windows.Forms.ToolStripTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttributes)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 410);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(654, 39);
            this.panel1.TabIndex = 0;
            // 
            // dgvAttributes
            // 
            this.dgvAttributes.AllowUserToAddRows = false;
            this.dgvAttributes.AllowUserToDeleteRows = false;
            this.dgvAttributes.AllowUserToResizeColumns = false;
            this.dgvAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAttributes.Location = new System.Drawing.Point(0, 29);
            this.dgvAttributes.Name = "dgvAttributes";
            this.dgvAttributes.RowTemplate.Height = 23;
            this.dgvAttributes.Size = new System.Drawing.Size(654, 381);
            this.dgvAttributes.TabIndex = 1;
            this.dgvAttributes.VirtualMode = true;
            this.dgvAttributes.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvAttributes_CellValueNeeded);
            this.dgvAttributes.SelectionChanged += new System.EventHandler(this.dgvAttributes_SelectionChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1,
            this.tsSelectAttributes,
            this.tsSearchText,
            this.tsSearchResult});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(654, 29);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.ReadOnly = true;
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 25);
            this.toolStripTextBox1.Text = "查询属性：";
            this.toolStripTextBox1.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tsSelectAttributes
            // 
            this.tsSelectAttributes.Name = "tsSelectAttributes";
            this.tsSelectAttributes.Size = new System.Drawing.Size(121, 25);
            this.tsSelectAttributes.SelectedIndexChanged += new System.EventHandler(this.tsSelectAttributes_SelectedIndexChanged);
            // 
            // tsSearchText
            // 
            this.tsSearchText.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tsSearchText.Name = "tsSearchText";
            this.tsSearchText.Size = new System.Drawing.Size(100, 25);
            this.tsSearchText.TextChanged += new System.EventHandler(this.tsSearchText_TextChanged);
            // 
            // tsSearchResult
            // 
            this.tsSearchResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tsSearchResult.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tsSearchResult.Name = "tsSearchResult";
            this.tsSearchResult.ReadOnly = true;
            this.tsSearchResult.Size = new System.Drawing.Size(130, 25);
            this.tsSearchResult.Text = "请输入属性和条件";
            this.tsSearchResult.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmLayerAttributes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 449);
            this.Controls.Add(this.dgvAttributes);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmLayerAttributes";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmLayerAttributes";
            this.Load += new System.EventHandler(this.frmLayerAttributes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttributes)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvAttributes;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripComboBox tsSelectAttributes;
        private System.Windows.Forms.ToolStripTextBox tsSearchText;
        private System.Windows.Forms.ToolStripTextBox tsSearchResult;
    }
}