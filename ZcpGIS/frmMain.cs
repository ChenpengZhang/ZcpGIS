﻿using MyMapObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ZcpGIS
{
    public partial class frmMain : Form
    {
        #region 字段

        //选项变量
        private Color mZoomBoxColor = Color.DeepPink;  // 缩放盒
        private double mZoomBoxWidth = 0.53;
        private Color mSelectBoxColor = Color.DarkGreen;  // 选择盒
        private double mSelectBoxWidth = 0.53;
        private double mZoomRatioFixed = 2;  // 固定缩放系数
        private double mZoomRatioMouseWheel = 1.2;  // 滑轮缩放系数
        private double mSelectingTolerance = 3;  // 选择容限
        private MyMapObjects.moSimpleFillSymbol mSelectingBoxSymbol;
        private MyMapObjects.moSimpleFillSymbol mZoomBoxSymbol;
        private MyMapObjects.moSimpleFillSymbol mMovingPolygonSymbol;
        private MyMapObjects.moSimpleFillSymbol mEditingPolygonSymbol;
        private MyMapObjects.moSimpleMarkerSymbol mEditingVertexSymbol;
        private MyMapObjects.moSimpleLineSymbol mElasticSymbol;
        private bool mShowLngLat = false;

        //与地图操作有关的变量
        private Int32 mMapOpStyle = 0;  // 0：无 1：放大 2：缩小 3：漫游 4：选择 5：查询 6：移动 7：描绘 8：编辑
        private PointF mStartMouseLocation;
        private bool mIsInZoomIn = false;
        private bool mIsInZoomOut = false;
        private bool mIsInPan = false;
        private bool mIsInSelect = false;
        private bool mIsInIdentify = false;
        private bool mIsInMovingShapes = false;
        private List<MyMapObjects.moGeometry> mMovingGeometries = new List<MyMapObjects.moGeometry>();
        private MyMapObjects.moGeometry mEditingGeometry;
        private List<MyMapObjects.moPoints> mSketchingShape;

        #endregion

        #region 方法

        internal void NotifiedFeatureSelectionChanged(object sender)
        {
            moMap.RedrawTrackingShapes();
        }

        #endregion

        #region 构造函数

        public frmMain()
        {
            InitializeComponent();
            // 订阅moMap的MouseWheel事件
            moMap.MouseWheel += MoMap_MouseWheel;
        }

        #endregion

        #region 窗体和控件事件处理

        private void frmMain_Load(object sender, EventArgs e)
        {
            //（1）初始化符号
            InitializeSymbols();
            //（2）初始化描绘图形
            InitializeSketchingShape();
            //（3）显示比例尺
            ShowMapScale();
            //（4）初始化图层树菜单
            InitializeTreeView();
        }
        private void btnLoadLayer_Click(object sender, EventArgs e)
        {
            OpenFileDialog sDialog = new OpenFileDialog();
            sDialog.Filter = "Shape文件|*.shp";
            string sFileName = "";
            if (sDialog.ShowDialog() == DialogResult.OK)
            {
                sFileName = sDialog.FileName;
                sDialog.Dispose();
            }
            else
            {
                sDialog.Dispose();
                return;
            }
            MyMapObjects.moMapLayer sLayer;
            try
            {
                sLayer = DataIOTools.LoadMapLayerFromShapeFile(sFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            moMap.Layers.Add(sLayer);
            treeViewLayers_Update();
            if (moMap.Layers.Count == 1)
            {
                moMap.FullExtent();
            }
            else
            {
                moMap.RedrawMap();
            }
        }
        private void toolStripZoomIn_Click(object sender, EventArgs e)
        {
            mMapOpStyle = 1;
        }
        private void toolStripZoomOut_Click(object sender, EventArgs e)
        {
            mMapOpStyle = 2;
        }
        private void toolStripPan_Click(object sender, EventArgs e)
        {
            mMapOpStyle = 3;
        }
        private void toolStripSelect_Click(object sender, EventArgs e)
        {
            mMapOpStyle = 4;
        }
        private void toolStripIdentify_Click(object sender, EventArgs e)
        {
            mMapOpStyle = 5;
        }
        private void btnSimpleRenderer_Click(object sender, EventArgs e)
        {
            //（1）查找一个多边形图层
            MyMapObjects.moMapLayer sLayer = GetPolygonLayer();
            if (sLayer == null) return;
            //（2）新建一个简单渲染对象
            MyMapObjects.moSimpleRenderer sRenderer = new MyMapObjects.moSimpleRenderer();
            MyMapObjects.moSimpleFillSymbol sSymbol = new MyMapObjects.moSimpleFillSymbol();
            // 可以继续设置符号的形式
            sRenderer.Symbol = sSymbol;
            sLayer.Renderer = sRenderer;
            moMap.RedrawMap();
        }
        private void chkShowLngLat_CheckedChanged(object sender, EventArgs e)
        {
            mShowLngLat = chkShowLngLat.Checked;
        }
        private void treeViewLayers_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }
        private void treeViewLayers_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void treeViewLayers_DragDrop(object sender, DragEventArgs e)
        {
            // 获取拖放的节点
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            // 获取拖放位置的节点
            Point targetPoint = treeViewLayers.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = treeViewLayers.GetNodeAt(targetPoint);

            if (targetNode != null)
            {
                // 移动节点到目标位置
                if (draggedNode != targetNode)
                {
                    draggedNode.Remove();
                    moMap.Layers.MoveTo(draggedNode.Index, targetNode.Index);
                    treeViewLayers_Update();
                    moMap.RedrawMap();
                }
            }
        }
        private void treeViewLayers_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // 显示右键菜单
            if (e.Button == MouseButtons.Right)
            {
                treeViewLayers.SelectedNode = e.Node;
                treeViewLayers.ContextMenuStrip.Show(treeViewLayers, e.Location);
            }
        }
        private void BtnShowLabel_Click(object sender, EventArgs e)
        {
            //  获取选中图层
            MyMapObjects.moMapLayer sLayer = moMap.Layers.GetItem(treeViewLayers.SelectedNode.Index);
            // 检索第一个字符型字段，如果没有则直接选择第一个字段
            Int32 sFieldCount = sLayer.AttributeFields.Count;
            if (sFieldCount == 0) return;
            Int32 sFieldIndex = 0;
            for (Int32 i = 0; i < sFieldCount; ++i)
            {
                if (sLayer.AttributeFields.GetItem(i).ValueType == MyMapObjects.moValueTypeConstant.dText)
                {
                    sFieldIndex = i;
                    break;
                }
            }
            // 新建一个注记渲染对象
            MyMapObjects.moLabelRenderer sLabelRenderer = new MyMapObjects.moLabelRenderer();
            // 设定绑定字段
            sLabelRenderer.Field = sLayer.AttributeFields.GetItem(sFieldIndex).Name;
            // 设置注记符号
            Font sOldFont = sLabelRenderer.TextSymbol.Font;
            sLabelRenderer.TextSymbol.Font = new Font(sOldFont.Name, 12);
            sLabelRenderer.TextSymbol.UseMask = true;
            sLabelRenderer.LabelFeatures = true;
            // 赋给图层
            sLayer.LabelRenderer = sLabelRenderer;
            // 重绘地图
            moMap.RedrawMap();
        }
        private void BtnModifyRenderer_Click(object sender, EventArgs e)
        {
            // 1. 获得一个图层
            MyMapObjects.moMapLayer sLayer = moMap.Layers.GetItem(treeViewLayers.SelectedNode.Index);
            // 2. 新建一个frmLayerRenderer窗体
            if (sLayer.ShapeType == moGeometryTypeConstant.MultiPolygon)
            {
                frmLayerRenderer sfrmLayerRenderer = new frmLayerRenderer();
                // 3. 输入数据
                sfrmLayerRenderer.SetData(sLayer);
                // 4. 显示窗体，并根据对话框结果做相应处理
                if (sfrmLayerRenderer.ShowDialog(this) == DialogResult.OK)
                {
                    MyMapObjects.moRenderer sRenderer;
                    sfrmLayerRenderer.GetData(out sRenderer);
                    // TODO: 还没有返回正确的renderer
                    sLayer.Renderer = sRenderer;
                    moMap.RedrawMap();
                    sfrmLayerRenderer.Dispose();
                }
                else
                {
                    sfrmLayerRenderer.Dispose();
                }
            }
            else if (sLayer.ShapeType == moGeometryTypeConstant.MultiPolyline)
            {
                frmLineRenderer sfrmLineRenderer = new frmLineRenderer();
                // 3. 输入数据
                sfrmLineRenderer.SetData(sLayer);
                // 4. 显示窗体，并根据对话框结果做相应处理
                if (sfrmLineRenderer.ShowDialog(this) == DialogResult.OK)
                {
                    MyMapObjects.moRenderer sRenderer;
                    sfrmLineRenderer.GetData(out sRenderer);
                    // TODO: 还没有返回正确的renderer
                    sLayer.Renderer = sRenderer;
                    moMap.RedrawMap();
                    sfrmLineRenderer.Dispose();
                }
                else
                {
                    sfrmLineRenderer.Dispose();
                }
            }
            
        }
        private void BtnFullExtent_Click(object sender, EventArgs e)
        {
            moMap.FullExtent();
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            moMap.Layers.RemoveAt(treeViewLayers.SelectedNode.Index);
            treeViewLayers_Update();
            moMap.RedrawMap();
        }
        private void btnEditPolygon_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region 地图控件事件处理
        private void moMap_AfterTrackingLayerDraw(object sender, moUserDrawingTool drawTool)
        {
            DrawSketchingShapes(drawTool);  // 绘制描绘图形
            DrawEditingShapes(drawTool);  // 绘制正在编辑的图形
        }

        private void moMap_MapScaleChanged(object sender)
        {
            ShowMapScale();
        }

        private void moMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (mMapOpStyle == 1)
            {

            }
            else if (mMapOpStyle == 2)
            {
                MyMapObjects.moPoint sPoint = moMap.ToMapPoint(e.Location.X, e.Location.Y);
                moMap.ZoomByCenter(sPoint, 1 / mZoomRatioFixed);
            }
            else if (mMapOpStyle == 3)
            {

            }
            else if (mMapOpStyle == 4)
            {

            }
            else if (mMapOpStyle == 5)
            {

            }
            else if (mMapOpStyle == 6)
            {

            }
            else if (mMapOpStyle == 7)
            {
                if (e.Button != MouseButtons.Left) return;
                MyMapObjects.moPoint sPoint = moMap.ToMapPoint(e.Location.X, e.Location.Y);
                mSketchingShape.Last().Add(sPoint);
                moMap.RedrawTrackingShapes();
            }
            else if (mMapOpStyle == 8)
            {

            }
        }

        private void moMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (mMapOpStyle == 1)
            {
                OnZoomIn_MouseDown(e);
            }
            else if (mMapOpStyle == 2)
            {

            }
            else if (mMapOpStyle == 3)
            {
                OnPan_MouseDown(e);
            }
            else if (mMapOpStyle == 4)
            {
                OnSelect_MouseDown(e);
            }
            else if (mMapOpStyle == 5)
            {
                OnIndentify_MouseDown(e);
            }
            else if (mMapOpStyle == 6)
            {
                OnMoveShape_MouseDown(e);
            }
            else if (mMapOpStyle == 7)
            {

            }
            else if (mMapOpStyle == 8)
            {

            }
        }

        private void OnZoomIn_MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mStartMouseLocation = e.Location;
                mIsInZoomIn = true;
            }
        }


        private void OnPan_MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mStartMouseLocation = e.Location;
                mIsInPan = true;
            }
        }

        private void OnSelect_MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mStartMouseLocation = e.Location;
                mIsInSelect = true;
            }
        }

        private void OnIndentify_MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mStartMouseLocation = e.Location;
                mIsInIdentify = true;
            }
        }

        private void OnMoveShape_MouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            // 查找多边形图层
            MyMapObjects.moMapLayer sLayer = GetPolygonLayer();
            if (sLayer == null) return;
            // 判断是否有选中的要素
            Int32 sSelFeatureCount = sLayer.SelectedFeatures.Count;
            if (sSelFeatureCount == 0) return;
            mMovingGeometries.Clear();
            for (Int32 i = 0; i < sSelFeatureCount; ++i)
            {
                MyMapObjects.moMultiPolygon sOriPolygon = (MyMapObjects.moMultiPolygon)sLayer.SelectedFeatures.GetItem(i).Geometry;
                MyMapObjects.moMultiPolygon sDesPolygon = sOriPolygon.Clone();
                mMovingGeometries.Add(sDesPolygon);
            }
            mStartMouseLocation = e.Location;
            mIsInMovingShapes = true;
        }

        private void moMap_MouseMove(object sender, MouseEventArgs e)
        {
            ShowCoordinates(e.Location);
            if (mMapOpStyle == 1)
            {
                OnZoomIn_MouseMove(e);
            }
            else if (mMapOpStyle == 2)
            {

            }
            else if (mMapOpStyle == 3)
            {
                OnPan_MouseMove(e);
            }
            else if (mMapOpStyle == 4)
            {
                OnSelect_MouseMove(e);
            }
            else if (mMapOpStyle == 5)
            {
                OnIdentify_MouseMove(e);
            }
            else if (mMapOpStyle == 6)
            {
                OnMoveShape_MouseMove(e);
            }
            else if (mMapOpStyle == 7)
            {
                MyMapObjects.moPoint sCurPoint = moMap.ToMapPoint(e.Location.X, e.Location.Y);
                MyMapObjects.moPoints sLastPart = mSketchingShape.Last();
                Int32 sPointCount = sLastPart.Count;
                if (sPointCount == 0) { }
                else if (sPointCount == 1)
                {
                    moMap.Refresh();
                    //只有一个点，则绘制一条橡皮筋
                    MyMapObjects.moPoint sFirstPoint = sLastPart.GetItem(0);
                    MyMapObjects.moUserDrawingTool sDrawingTool = moMap.GetDrawingTool();
                    sDrawingTool.DrawLine(sFirstPoint, sCurPoint, mElasticSymbol);
                }
                else
                {
                    moMap.Refresh();
                    //两个以上点，则绘制两条橡皮筋
                    MyMapObjects.moPoint sFirstPoint = sLastPart.GetItem(0);
                    MyMapObjects.moPoint sLastPoint = sLastPart.GetItem(sPointCount - 1);
                    MyMapObjects.moUserDrawingTool sDrawingTool = moMap.GetDrawingTool();
                    sDrawingTool.DrawLine(sFirstPoint, sCurPoint, mElasticSymbol);
                    sDrawingTool.DrawLine(sCurPoint, sLastPoint, mElasticSymbol);
                }
            }
            else if (mMapOpStyle == 8)
            {

            }
        }

        private void OnZoomIn_MouseMove(MouseEventArgs e)
        {
            if (!mIsInZoomIn) return;
            moMap.Refresh();
            MyMapObjects.moRectangle sRect = GetMapRectByTwoPoints(mStartMouseLocation, e.Location);
            MyMapObjects.moUserDrawingTool sDrawingTool = moMap.GetDrawingTool();
            sDrawingTool.DrawRectangle(sRect, mZoomBoxSymbol);
        }

        private void OnPan_MouseMove(MouseEventArgs e)
        {
            if (!mIsInPan) return;
            moMap.PanMapImageTo(e.Location.X - mStartMouseLocation.X, e.Location.Y - mStartMouseLocation.Y);
        }

        private void OnSelect_MouseMove(MouseEventArgs e)
        {
            if (!mIsInSelect) return;
            moMap.Refresh();
            MyMapObjects.moRectangle sRect = GetMapRectByTwoPoints(mStartMouseLocation, e.Location);
            MyMapObjects.moUserDrawingTool sDrawingTool = moMap.GetDrawingTool();
            sDrawingTool.DrawRectangle(sRect, mSelectingBoxSymbol);
        }

        private void OnIdentify_MouseMove(MouseEventArgs e)
        {
            if (!mIsInIdentify) return;
            moMap.Refresh();
            MyMapObjects.moRectangle sRect = GetMapRectByTwoPoints(mStartMouseLocation, e.Location);
            MyMapObjects.moUserDrawingTool sDrawingTool = moMap.GetDrawingTool();
            sDrawingTool.DrawRectangle(sRect, mSelectingBoxSymbol);
        }

        private void OnMoveShape_MouseMove(MouseEventArgs e)
        {
            if (!mIsInMovingShapes) return;
            double sDeltaX = moMap.ToMapDistance(e.Location.X - mStartMouseLocation.X);
            double sDeltaY = moMap.ToMapDistance(mStartMouseLocation.Y - e.Location.Y);
            ModifyMovingGeometries(sDeltaX, sDeltaY);
            // 绘制移动图形
            moMap.Refresh();
            DrawMovingShapes();
            //重新设置鼠标位置
            mStartMouseLocation = e.Location;
        }

        private void moMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (mMapOpStyle == 1)
            {
                OnZoomIn_MouseUp(e);
            }
            else if (mMapOpStyle == 2)
            {

            }
            else if (mMapOpStyle == 3)
            {
                OnPan_MouseUp(e);
            }
            else if (mMapOpStyle == 4)
            {
                OnSelect_MouseUp(e);
            }
            else if (mMapOpStyle == 5)
            {
                OnIdentify_MouseUp(e);
            }
            else if (mMapOpStyle == 6)
            {
                OnMoveShape_MouseUp(e);
            }
            else if (mMapOpStyle == 7)
            {

            }
            else if (mMapOpStyle == 8)
            {

            }
        }

        private void OnZoomIn_MouseUp(MouseEventArgs e)
        {
            if (!mIsInZoomIn) return;
            mIsInZoomIn = false;
            if (mStartMouseLocation.X == e.Location.X && mStartMouseLocation.Y == e.Location.Y)
            {
                //单点放大
                MyMapObjects.moPoint sPoint = moMap.ToMapPoint(mStartMouseLocation.X, mStartMouseLocation.Y);
                moMap.ZoomByCenter(sPoint, mZoomRatioFixed);
            }
            else
            {
                //矩形放大
                MyMapObjects.moRectangle sBox = GetMapRectByTwoPoints(mStartMouseLocation, e.Location);
                moMap.ZoomToExtent(sBox);
            }
        }

        private void OnPan_MouseUp(MouseEventArgs e)
        {
            if (!mIsInPan) return;
            mIsInPan = false;
            double sDeltaX = moMap.ToMapDistance(e.Location.X - mStartMouseLocation.X);
            double sDeltaY = moMap.ToMapDistance(mStartMouseLocation.Y - e.Location.Y);
            moMap.PanDelta(sDeltaX, sDeltaY);
        }

        private void OnSelect_MouseUp(MouseEventArgs e)
        {
            if (!mIsInSelect) return;
            mIsInSelect = false;
            MyMapObjects.moRectangle sBox = GetMapRectByTwoPoints(mStartMouseLocation, e.Location);
            double sTolerance = moMap.ToMapDistance(mSelectingTolerance);
            moMap.SelectByBox(sBox, sTolerance, 0);
            moMap.RedrawTrackingShapes();
            // 告知子窗体，选择发生了变化
            ToNotifyFeatureSelectionChanged();
        }

        private void OnIdentify_MouseUp(MouseEventArgs e)
        {
            if (!mIsInIdentify) return;
            mIsInIdentify = false;
            moMap.Refresh();
            MyMapObjects.moRectangle sBox = GetMapRectByTwoPoints(mStartMouseLocation, e.Location);
            double sTolerance = moMap.ToMapDistance(mSelectingTolerance);
            if (moMap.Layers.Count == 0) return;
            MyMapObjects.moMapLayer sLayer = moMap.Layers.GetItem(0);
            MyMapObjects.moFeatures sFeatures = sLayer.SearchByBox(sBox, sTolerance);
            Int32 sSelFeatureCount = sFeatures.Count;
            if (sSelFeatureCount > 0)
            {
                MyMapObjects.moGeometry[] sGeometries = new MyMapObjects.moGeometry[sSelFeatureCount];
                for (int i = 0; i < sSelFeatureCount; ++i)
                {
                    sGeometries[i] = sFeatures.GetItem(i).Geometry;
                }
                moMap.FlashShapes(sGeometries, 3, 800);
                // 检测该图层的属性窗体是否已经打开
                frmLayerAttributes sfrmLayerAttributes = IsLayerAttributesFormOpened(sLayer);
                if (sfrmLayerAttributes == null)
                {
                    // 没有打开
                    sfrmLayerAttributes = new frmLayerAttributes();
                    sfrmLayerAttributes.SetData(sLayer);
                    sfrmLayerAttributes.SetIdentify(sFeatures);
                    sfrmLayerAttributes.Show(this);
                }
                else
                {
                    // 已经打开
                    sfrmLayerAttributes.Close();
                    sfrmLayerAttributes = new frmLayerAttributes();
                    sfrmLayerAttributes.SetData(sLayer);
                    sfrmLayerAttributes.SetIdentify(sFeatures);
                    sfrmLayerAttributes.Show(this);
                }
            }
        }

        private void OnMoveShape_MouseUp(MouseEventArgs e)
        {
            if (!mIsInMovingShapes) return;
            mIsInMovingShapes = false;
            //做相应的修改数据的操作，不再编写
            //清除移动图形列表
            mMovingGeometries.Clear();
            //重绘地图
            moMap.RedrawMap();
        }

        private void MoMap_MouseWheel(object sender, MouseEventArgs e)
        {
            //计算地图中心的地图坐标
            double sX = moMap.ClientRectangle.Width / 2;
            double sY = moMap.ClientRectangle.Height / 2;
            MyMapObjects.moPoint sPoint = moMap.ToMapPoint(sX, sY);
            //缩放
            if (e.Delta > 0)
                moMap.ZoomByCenter(sPoint, mZoomRatioMouseWheel);
            else
                moMap.ZoomByCenter(sPoint, 1 / mZoomRatioMouseWheel);
        }

        #endregion

        #region 私有函数

        //初始化符号
        private void InitializeSymbols()
        {
            mSelectingBoxSymbol = new MyMapObjects.moSimpleFillSymbol();
            mSelectingBoxSymbol.Color = Color.Transparent;
            mSelectingBoxSymbol.Outline.Color = mSelectBoxColor;
            mSelectingBoxSymbol.Outline.Size = mSelectBoxWidth;
            mZoomBoxSymbol = new MyMapObjects.moSimpleFillSymbol();
            mZoomBoxSymbol.Color = Color.Transparent;
            mZoomBoxSymbol.Outline.Color = mZoomBoxColor;
            mZoomBoxSymbol.Outline.Size = mZoomBoxWidth;
            mMovingPolygonSymbol = new MyMapObjects.moSimpleFillSymbol();
            mMovingPolygonSymbol.Color = Color.Transparent;
            mMovingPolygonSymbol.Outline.Color = Color.Black;
            mEditingPolygonSymbol = new MyMapObjects.moSimpleFillSymbol();
            mEditingPolygonSymbol.Color = Color.Transparent;
            mEditingPolygonSymbol.Outline.Color = Color.DarkGreen;
            mEditingPolygonSymbol.Outline.Size = 0.53;
            mEditingVertexSymbol = new MyMapObjects.moSimpleMarkerSymbol();
            mEditingVertexSymbol.Color = Color.DarkGreen;
            mEditingVertexSymbol.Style = MyMapObjects.moSimpleMarkerSymbolStyleConstant.SolidSquare;
            mEditingVertexSymbol.Size = 2;
            mElasticSymbol = new MyMapObjects.moSimpleLineSymbol();
            mElasticSymbol.Color = Color.DarkGreen;
            mElasticSymbol.Size = 0.52;
            mElasticSymbol.Style = MyMapObjects.moSimpleLineSymbolStyleConstant.Dash;
        }

        private void InitializeSketchingShape()
        {
            mSketchingShape = new List<moPoints>();
            moPoints sPoints = new moPoints();
            mSketchingShape.Add(sPoints);
        }

        private void ShowCoordinates(PointF point)
        {
            MyMapObjects.moPoint sPoint = moMap.ToMapPoint(point.X, point.Y);
            if (mShowLngLat == false)
            {
                double sX = Math.Round(sPoint.X, 2);
                double sY = Math.Round(sPoint.Y, 2);
                tssCoordinate.Text = "X: " + sX.ToString() + ",Y: " + sY.ToString();
            }
            else
            {
                MyMapObjects.moPoint sLngLat = moMap.ProjectionCS.TransferToLngLat(sPoint);
                double sX = Math.Round(sLngLat.X, 4);
                double sY = Math.Round(sLngLat.Y, 4);
                tssCoordinate.Text = "X: " + sX.ToString() + ",Y: " + sY.ToString();
            }
        }

        private void ShowMapScale()
        {
            tssMapScale.Text = "1:" + moMap.MapScale.ToString("0.00");
        }

        private void InitializeTreeView()
        {
            // 创建右键菜单
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

            // 添加菜单项
            ToolStripMenuItem btnFullExtent = new ToolStripMenuItem("全范围显示");
            ToolStripMenuItem btnModifyRenderer = new ToolStripMenuItem("调整渲染");
            ToolStripMenuItem btnShowLabel = new ToolStripMenuItem("添加注记");
            ToolStripMenuItem btnDelete = new ToolStripMenuItem("删除图层");
            btnFullExtent.Click += BtnFullExtent_Click;
            btnModifyRenderer.Click += BtnModifyRenderer_Click;
            btnShowLabel.Click += BtnShowLabel_Click;
            btnDelete.Click += BtnDelete_Click;
            contextMenuStrip.Items.Add(btnFullExtent);
            contextMenuStrip.Items.Add(btnModifyRenderer);
            contextMenuStrip.Items.Add(btnShowLabel);
            contextMenuStrip.Items.Add(btnDelete);

            // 将右键菜单关联到 TreeView 控件
            treeViewLayers.ContextMenuStrip = contextMenuStrip;

            // 默认隐藏右键菜单
            treeViewLayers.ContextMenuStrip.Visible = false;
        }





        //根据屏幕上两点返回一个矩形
        private MyMapObjects.moRectangle GetMapRectByTwoPoints(PointF point1, PointF point2)
        {
            MyMapObjects.moPoint sPoint1 = moMap.ToMapPoint(point1.X, point1.Y);
            MyMapObjects.moPoint sPoint2 = moMap.ToMapPoint(point2.X, point2.Y);
            double sMinX = Math.Min(sPoint1.X, sPoint2.X);
            double sMaxX = Math.Max(sPoint1.X, sPoint2.X);
            double sMinY = Math.Min(sPoint1.Y, sPoint2.Y);
            double sMaxY = Math.Max(sPoint1.Y, sPoint2.Y);
            MyMapObjects.moRectangle sRect = new MyMapObjects.moRectangle(sMinX, sMaxX, sMinY, sMaxY);
            return sRect;
        }

        //获取一个多边形图层
        private MyMapObjects.moMapLayer GetPolygonLayer()
        {
            Int32 sLayerCount = moMap.Layers.Count;
            MyMapObjects.moMapLayer sLayer = null;
            for (Int32 i = 0; i <= sLayerCount - 1; i++)
            {
                if (moMap.Layers.GetItem(i).ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
                {
                    sLayer = moMap.Layers.GetItem(i);
                    break;
                }
            }
            return sLayer;
        }

        //修改移动图形的坐标
        private void ModifyMovingGeometries(double deltaX, double deltaY)
        {
            Int32 sCount = mMovingGeometries.Count;
            for (Int32 i = 0; i <= sCount - 1; i++)
            {
                if (mMovingGeometries[i].GetType() == typeof(MyMapObjects.moMultiPolygon))
                {
                    MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)mMovingGeometries[i];
                    Int32 sPartCount = sMultiPolygon.Parts.Count;
                    for (Int32 j = 0; j <= sPartCount - 1; j++)
                    {
                        MyMapObjects.moPoints sPoints = sMultiPolygon.Parts.GetItem(j);
                        Int32 sPointCount = sPoints.Count;
                        for (Int32 k = 0; k <= sPointCount - 1; k++)
                        {
                            MyMapObjects.moPoint sPoint = sPoints.GetItem(k);
                            sPoint.X = sPoint.X + deltaX;
                            sPoint.Y = sPoint.Y + deltaY;
                        }
                    }
                    sMultiPolygon.UpdateExtent();
                }
            }
        }

        //绘制移动图形
        private void DrawMovingShapes()
        {
            MyMapObjects.moUserDrawingTool sDrawingTool = moMap.GetDrawingTool();
            Int32 sCount = mMovingGeometries.Count;
            for (Int32 i = 0; i <= sCount - 1; i++)
            {
                if (mMovingGeometries[i].GetType() == typeof(MyMapObjects.moMultiPolygon))
                {
                    MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)mMovingGeometries[i];
                    sDrawingTool.DrawMultiPolygon(sMultiPolygon, mMovingPolygonSymbol);
                }
            }
        }

        //绘制正在描绘的图形
        private void DrawSketchingShapes(MyMapObjects.moUserDrawingTool drawingTool)
        {
            if (mSketchingShape == null)
                return;
            Int32 sPartCount = mSketchingShape.Count;
            //绘制已经描绘完成的部分
            for (Int32 i = 0; i <= sPartCount - 2; i++)
            {
                drawingTool.DrawPolygon(mSketchingShape[i], mEditingPolygonSymbol);
            }
            //正在描绘的部分（只有一个Part）
            MyMapObjects.moPoints sLastPart = mSketchingShape.Last();
            if (sLastPart.Count >= 2)
                drawingTool.DrawPolyline(sLastPart, mEditingPolygonSymbol.Outline);
            //绘制所有顶点手柄
            for (Int32 i = 0; i <= sPartCount - 1; i++)
            {
                MyMapObjects.moPoints sPoints = mSketchingShape[i];
                drawingTool.DrawPoints(sPoints, mEditingVertexSymbol);
            }
        }

        //绘制正在编辑的图形
        private void DrawEditingShapes(MyMapObjects.moUserDrawingTool drawingTool)
        {
            if (mEditingGeometry == null)
                return;
            if (mEditingGeometry.GetType() == typeof(MyMapObjects.moMultiPolygon))
            {
                MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)mEditingGeometry;
                //绘制边界
                drawingTool.DrawMultiPolygon(sMultiPolygon, mEditingPolygonSymbol);
                //绘制顶点手柄
                Int32 sPartCount = sMultiPolygon.Parts.Count;
                for (Int32 i = 0; i <= sPartCount - 1; i++)
                {
                    MyMapObjects.moPoints sPoints = sMultiPolygon.Parts.GetItem(i);
                    drawingTool.DrawPoints(sPoints, mEditingVertexSymbol);
                }
            }
        }

        // 指定涂层的属性窗体是否打开
        private frmLayerAttributes IsLayerAttributesFormOpened(MyMapObjects.moMapLayer layer)
        {
            frmLayerAttributes sfrmLayerAttributes = null;
            foreach (Form sForm in this.OwnedForms)
            {
                if (sForm.GetType() == typeof(frmLayerAttributes))
                {
                    frmLayerAttributes sCurfrmLayerAttributes = (frmLayerAttributes)sForm;
                    if (sCurfrmLayerAttributes.GetLayer() == layer)
                    {
                        sfrmLayerAttributes = sCurfrmLayerAttributes;
                        break;
                    }
                }
            }
            return sfrmLayerAttributes;
        }

        private void ToNotifyFeatureSelectionChanged()
        {
            Int32 sLayerCount = moMap.Layers.Count;
            for (Int32 i = 0; i < sLayerCount; ++i)
            {
                MyMapObjects.moMapLayer sLayer = moMap.Layers.GetItem(i);
                frmLayerAttributes sfrmLayerAttributes = IsLayerAttributesFormOpened(sLayer);
                if (sfrmLayerAttributes != null)
                {
                    sfrmLayerAttributes.NotifiedFeatureSelectionChanged(this);
                }
            }
        }

        // 更新图层树中所有的图层
        private void treeViewLayers_Update()
        {
            treeViewLayers.Nodes.Clear();
            Int32 sLayerCount = moMap.Layers.Count;
            for (Int32 i = 0; i < sLayerCount; ++i)
            {
                MyMapObjects.moMapLayer sLayer = moMap.Layers.GetItem(i);
                string sLayerName = sLayer.Name;
                TreeNode layerNode = new TreeNode(sLayerName);
                treeViewLayers.Nodes.Add(layerNode);
            }
        }

        #endregion

    }
}
