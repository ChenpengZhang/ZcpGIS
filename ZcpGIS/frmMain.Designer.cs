namespace ZcpGIS
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssMapScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLoadZshp = new System.Windows.Forms.Button();
            this.btnNewLayer = new System.Windows.Forms.Button();
            this.btnBitmap = new System.Windows.Forms.Button();
            this.btnLoadRender = new System.Windows.Forms.Button();
            this.btnSaveRender = new System.Windows.Forms.Button();
            this.chkShowLngLat = new System.Windows.Forms.CheckBox();
            this.btnEndEdit = new System.Windows.Forms.Button();
            this.btnEndSketch = new System.Windows.Forms.Button();
            this.btnLoadLayer = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripZoomIn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripZoomOut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripPan = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripIdentify = new System.Windows.Forms.ToolStripMenuItem();
            this.tsConditionIndicate = new System.Windows.Forms.ToolStripTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.treeViewLayers = new System.Windows.Forms.TreeView();
            this.moMap = new MyMapObjects.moMapcontrol();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssCoordinate,
            this.tssMapScale});
            this.statusStrip1.Location = new System.Drawing.Point(0, 583);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 9, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1080, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssCoordinate
            // 
            this.tssCoordinate.AutoSize = false;
            this.tssCoordinate.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssCoordinate.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssCoordinate.Name = "tssCoordinate";
            this.tssCoordinate.Size = new System.Drawing.Size(200, 17);
            // 
            // tssMapScale
            // 
            this.tssMapScale.AutoSize = false;
            this.tssMapScale.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssMapScale.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssMapScale.Name = "tssMapScale";
            this.tssMapScale.Size = new System.Drawing.Size(200, 17);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnLoadZshp);
            this.panel1.Controls.Add(this.btnNewLayer);
            this.panel1.Controls.Add(this.btnBitmap);
            this.panel1.Controls.Add(this.btnLoadRender);
            this.panel1.Controls.Add(this.btnSaveRender);
            this.panel1.Controls.Add(this.chkShowLngLat);
            this.panel1.Controls.Add(this.btnEndEdit);
            this.panel1.Controls.Add(this.btnEndSketch);
            this.panel1.Controls.Add(this.btnLoadLayer);
            this.panel1.Location = new System.Drawing.Point(957, 27);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(123, 554);
            this.panel1.TabIndex = 1;
            // 
            // btnLoadZshp
            // 
            this.btnLoadZshp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadZshp.Location = new System.Drawing.Point(15, 95);
            this.btnLoadZshp.Name = "btnLoadZshp";
            this.btnLoadZshp.Size = new System.Drawing.Size(90, 33);
            this.btnLoadZshp.TabIndex = 24;
            this.btnLoadZshp.Text = "载入.zshp";
            this.btnLoadZshp.UseVisualStyleBackColor = true;
            this.btnLoadZshp.Click += new System.EventHandler(this.btnLoadZshp_Click);
            // 
            // btnNewLayer
            // 
            this.btnNewLayer.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNewLayer.Location = new System.Drawing.Point(15, 19);
            this.btnNewLayer.Name = "btnNewLayer";
            this.btnNewLayer.Size = new System.Drawing.Size(90, 33);
            this.btnNewLayer.TabIndex = 23;
            this.btnNewLayer.Text = "新建图层";
            this.btnNewLayer.UseVisualStyleBackColor = true;
            this.btnNewLayer.Click += new System.EventHandler(this.btnNewLayer_Click);
            // 
            // btnBitmap
            // 
            this.btnBitmap.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBitmap.Location = new System.Drawing.Point(15, 292);
            this.btnBitmap.Name = "btnBitmap";
            this.btnBitmap.Size = new System.Drawing.Size(90, 32);
            this.btnBitmap.TabIndex = 22;
            this.btnBitmap.Text = "输出Bitmap";
            this.btnBitmap.UseVisualStyleBackColor = true;
            this.btnBitmap.Click += new System.EventHandler(this.btnBitmap_Click);
            // 
            // btnLoadRender
            // 
            this.btnLoadRender.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadRender.Location = new System.Drawing.Point(15, 251);
            this.btnLoadRender.Name = "btnLoadRender";
            this.btnLoadRender.Size = new System.Drawing.Size(90, 35);
            this.btnLoadRender.TabIndex = 21;
            this.btnLoadRender.Text = "载入渲染";
            this.btnLoadRender.UseVisualStyleBackColor = true;
            this.btnLoadRender.Click += new System.EventHandler(this.btnLoadRender_Click);
            // 
            // btnSaveRender
            // 
            this.btnSaveRender.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveRender.Location = new System.Drawing.Point(15, 209);
            this.btnSaveRender.Name = "btnSaveRender";
            this.btnSaveRender.Size = new System.Drawing.Size(90, 36);
            this.btnSaveRender.TabIndex = 20;
            this.btnSaveRender.Text = "保存渲染";
            this.btnSaveRender.UseVisualStyleBackColor = true;
            this.btnSaveRender.Click += new System.EventHandler(this.btnSaveRender_Click);
            // 
            // chkShowLngLat
            // 
            this.chkShowLngLat.AutoSize = true;
            this.chkShowLngLat.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkShowLngLat.Location = new System.Drawing.Point(15, 529);
            this.chkShowLngLat.Name = "chkShowLngLat";
            this.chkShowLngLat.Size = new System.Drawing.Size(99, 21);
            this.chkShowLngLat.TabIndex = 19;
            this.chkShowLngLat.Text = "显示地理坐标";
            this.chkShowLngLat.UseVisualStyleBackColor = true;
            this.chkShowLngLat.CheckedChanged += new System.EventHandler(this.chkShowLngLat_CheckedChanged);
            // 
            // btnEndEdit
            // 
            this.btnEndEdit.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEndEdit.Location = new System.Drawing.Point(15, 134);
            this.btnEndEdit.Name = "btnEndEdit";
            this.btnEndEdit.Size = new System.Drawing.Size(90, 32);
            this.btnEndEdit.TabIndex = 16;
            this.btnEndEdit.Text = "结束编辑";
            this.btnEndEdit.UseVisualStyleBackColor = true;
            this.btnEndEdit.Click += new System.EventHandler(this.btnEndEdit_Click);
            // 
            // btnEndSketch
            // 
            this.btnEndSketch.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEndSketch.Location = new System.Drawing.Point(15, 172);
            this.btnEndSketch.Name = "btnEndSketch";
            this.btnEndSketch.Size = new System.Drawing.Size(90, 31);
            this.btnEndSketch.TabIndex = 14;
            this.btnEndSketch.Text = "结束插入";
            this.btnEndSketch.UseVisualStyleBackColor = true;
            this.btnEndSketch.Click += new System.EventHandler(this.btnEndSketch_Click);
            // 
            // btnLoadLayer
            // 
            this.btnLoadLayer.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadLayer.Location = new System.Drawing.Point(15, 57);
            this.btnLoadLayer.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoadLayer.Name = "btnLoadLayer";
            this.btnLoadLayer.Size = new System.Drawing.Size(90, 33);
            this.btnLoadLayer.TabIndex = 0;
            this.btnLoadLayer.Text = "载入.shp";
            this.btnLoadLayer.UseVisualStyleBackColor = true;
            this.btnLoadLayer.Click += new System.EventHandler(this.btnLoadLayer_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripZoomIn,
            this.toolStripZoomOut,
            this.toolStripPan,
            this.toolStripSelect,
            this.toolStripIdentify,
            this.tsConditionIndicate});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1080, 27);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripZoomIn
            // 
            this.toolStripZoomIn.CheckOnClick = true;
            this.toolStripZoomIn.Name = "toolStripZoomIn";
            this.toolStripZoomIn.Size = new System.Drawing.Size(44, 23);
            this.toolStripZoomIn.Text = "放大";
            this.toolStripZoomIn.Click += new System.EventHandler(this.toolStripZoomIn_Click);
            // 
            // toolStripZoomOut
            // 
            this.toolStripZoomOut.CheckOnClick = true;
            this.toolStripZoomOut.Name = "toolStripZoomOut";
            this.toolStripZoomOut.Size = new System.Drawing.Size(44, 23);
            this.toolStripZoomOut.Text = "缩小";
            this.toolStripZoomOut.Click += new System.EventHandler(this.toolStripZoomOut_Click);
            // 
            // toolStripPan
            // 
            this.toolStripPan.CheckOnClick = true;
            this.toolStripPan.Name = "toolStripPan";
            this.toolStripPan.Size = new System.Drawing.Size(44, 23);
            this.toolStripPan.Text = "漫游";
            this.toolStripPan.Click += new System.EventHandler(this.toolStripPan_Click);
            // 
            // toolStripSelect
            // 
            this.toolStripSelect.CheckOnClick = true;
            this.toolStripSelect.Name = "toolStripSelect";
            this.toolStripSelect.Size = new System.Drawing.Size(44, 23);
            this.toolStripSelect.Text = "选择";
            this.toolStripSelect.Click += new System.EventHandler(this.toolStripSelect_Click);
            // 
            // toolStripIdentify
            // 
            this.toolStripIdentify.CheckOnClick = true;
            this.toolStripIdentify.Name = "toolStripIdentify";
            this.toolStripIdentify.Size = new System.Drawing.Size(44, 23);
            this.toolStripIdentify.Text = "查询";
            this.toolStripIdentify.Click += new System.EventHandler(this.toolStripIdentify_Click);
            // 
            // tsConditionIndicate
            // 
            this.tsConditionIndicate.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.tsConditionIndicate.Name = "tsConditionIndicate";
            this.tsConditionIndicate.ReadOnly = true;
            this.tsConditionIndicate.Size = new System.Drawing.Size(100, 23);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("隶书", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(46, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "图层树";
            // 
            // treeViewLayers
            // 
            this.treeViewLayers.AllowDrop = true;
            this.treeViewLayers.Font = new System.Drawing.Font("隶书", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeViewLayers.Location = new System.Drawing.Point(0, 46);
            this.treeViewLayers.Name = "treeViewLayers";
            this.treeViewLayers.Size = new System.Drawing.Size(136, 537);
            this.treeViewLayers.TabIndex = 0;
            this.treeViewLayers.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewLayers_ItemDrag);
            this.treeViewLayers.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewLayers_NodeMouseClick);
            this.treeViewLayers.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewLayers_DragDrop);
            this.treeViewLayers.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewLayers_DragEnter);
            // 
            // moMap
            // 
            this.moMap.BackColor = System.Drawing.Color.White;
            this.moMap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.moMap.FlashColor = System.Drawing.Color.Green;
            this.moMap.Location = new System.Drawing.Point(140, 27);
            this.moMap.Margin = new System.Windows.Forms.Padding(1);
            this.moMap.Name = "moMap";
            this.moMap.SelectionColor = System.Drawing.Color.Cyan;
            this.moMap.Size = new System.Drawing.Size(814, 556);
            this.moMap.TabIndex = 3;
            this.moMap.MapScaleChanged += new MyMapObjects.moMapcontrol.MapScaleChangedHandle(this.moMap_MapScaleChanged);
            this.moMap.AfterTrackingLayerDraw += new MyMapObjects.moMapcontrol.AfterTrackingLayerDrawHandle(this.moMap_AfterTrackingLayerDraw);
            this.moMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.moMap_MouseClick);
            this.moMap.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.moMap_MouseDoubleClick);
            this.moMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.moMap_MouseDown);
            this.moMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.moMap_MouseMove);
            this.moMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.moMap_MouseUp);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 605);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.treeViewLayers);
            this.Controls.Add(this.moMap);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ZcpGIS";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnLoadLayer;
        private MyMapObjects.moMapcontrol moMap;
        private System.Windows.Forms.ToolStripStatusLabel tssCoordinate;
        private System.Windows.Forms.ToolStripStatusLabel tssMapScale;
        private System.Windows.Forms.Button btnEndSketch;
        private System.Windows.Forms.CheckBox chkShowLngLat;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripZoomIn;
        private System.Windows.Forms.ToolStripMenuItem toolStripZoomOut;
        private System.Windows.Forms.ToolStripMenuItem toolStripPan;
        private System.Windows.Forms.ToolStripMenuItem toolStripSelect;
        private System.Windows.Forms.ToolStripMenuItem toolStripIdentify;
        private System.Windows.Forms.TreeView treeViewLayers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEndEdit;
        private System.Windows.Forms.Button btnLoadRender;
        private System.Windows.Forms.Button btnSaveRender;
        private System.Windows.Forms.Button btnBitmap;
        private System.Windows.Forms.Button btnNewLayer;
        private System.Windows.Forms.Button btnLoadZshp;
        private System.Windows.Forms.ToolStripTextBox tsConditionIndicate;
    }
}

