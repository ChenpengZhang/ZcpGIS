using MyMapObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZcpGIS
{
    public partial class frmLayerAttributes : Form
    {
        #region 字段

        private MyMapObjects.moMapLayer _Layer;
        private MyMapObjects.moFeatures _IdentifiedFeatures;
        private bool mChangeByUser = true;  // 改变是否用户发起，用于网格控件的事件处理
        private Int32 mSearchIndex = -1;

        #endregion

        #region 方法

        internal void SetData(MyMapObjects.moMapLayer layer)
        {
            _Layer = layer;
        }

        internal void SetIdentify(moFeatures identifiedFeatures)
        {
            _IdentifiedFeatures = identifiedFeatures;
        }

        internal MyMapObjects.moMapLayer GetLayer()
        {
            return _Layer;
        }

        // 收到通知 图层变化
        internal void NotifiedLayerChanged(object sender)
        {
            // 不再编写
        }

        // 收到通知 字段变化
        internal void NotfiedFieldsChanged(object sender)
        {
            // 不再编写
        }

        // 收到通知 字段数目变化
        internal void NotifiedFieldCountChanged(object sender)
        {
            // 不再编写
        }

        // 收到通知 选择变化
        internal void NotifiedFeatureSelectionChanged(object sender)
        {
            mChangeByUser = false;
            SetRowSelections();
            mChangeByUser = true;
        }

        #endregion

        #region 私有函数

        private void ShowFormTitle()
        {
            this.Text = "属性：" + _Layer.Name;
        }

        private void CreateColumns()
        {
            MyMapObjects.moFields sFields = _Layer.AttributeFields;
            Int32 sFieldCount = sFields.Count;
            // 删除所有列
            dgvAttributes.Columns.Clear();
            // 新建
            for (Int32 i = 0; i < sFieldCount; ++i)
            {
                DataGridViewTextBoxColumn sColumns = new DataGridViewTextBoxColumn();
                dgvAttributes.Columns.Add(sColumns);
            }
        }

        // 设置列标题
        private void SetColumnTexts()
        {
            MyMapObjects.moFields sFields = _Layer.AttributeFields;
            Int32 sFieldCount = sFields.Count;
            for (Int32 i = 0; i < sFieldCount; ++i)
            {
                string sColumnText = "";
                MyMapObjects.moField sField = sFields.GetItem(i);
                sColumnText = sField.Name;
                dgvAttributes.Columns[i].HeaderText = sColumnText;
                tsSelectAttributes.Items.Add(sColumnText);
            }
        }

        //增加行
        private void CreateRows()
        {
            //删除所有行
            dgvAttributes.Rows.Clear();
            //根据要素数目增加行
            Int32 sRowCount = _Layer.Features.Count;
            if (sRowCount > 0)
            {
                dgvAttributes.Rows.Add(sRowCount);
            }
        }

        //设置选择行
        private void SetRowSelections()
        {
            // 先清除所有选择行
            dgvAttributes.ClearSelection();
            MyMapObjects.moFeatures sFeatures = _Layer.Features;
            MyMapObjects.moFeatures sSelectedFeatures = _Layer.SelectedFeatures;
            Int32 sSelectedCount = sSelectedFeatures.Count;
            for (Int32 i = 0; i < sSelectedCount; ++i)
            {
                MyMapObjects.moFeature sFeature = sSelectedFeatures.GetItem(i);
                Int32 sIndex = GetFeatureIndex(sFeatures, sFeature);
                if (sIndex > 0)
                {
                    dgvAttributes.Rows[sIndex].Selected = true;
                }
            }
        }

        // 设置查询行（如果有的话）
        private void SetIdentifiedFeatures()
        {
            if (_IdentifiedFeatures == null) return;
            MyMapObjects.moFeatures sFeatures = _Layer.Features;
            Int32 sIdentifiedCount = _IdentifiedFeatures.Count;
            for (Int32 i = 0; i < sIdentifiedCount; ++i)
            {
                Int32 sIndex = GetFeatureIndex(sFeatures, _IdentifiedFeatures.GetItem(i));
                if (sIndex >= 0)
                {
                    dgvAttributes.Rows[sIndex].DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }

        private Int32 GetFeatureIndex(MyMapObjects.moFeatures features, MyMapObjects.moFeature feature)
        {
            Int32 sIndex = -1;
            Int32 sFeatureCount = features.Count;
            for (Int32 i = 0; i < sFeatureCount; ++i)
            {
                MyMapObjects.moFeature sFeature = features.GetItem(i);
                if (sFeature == feature)
                {
                    sIndex = i;
                    break;
                }
            }
            return sIndex;
        }

        private void ToNotifyFeatureSelectionChanged()
        {
            frmMain sfrmMain = (frmMain)this.Owner;
            sfrmMain.NotifiedFeatureSelectionChanged(this);
        }

        #endregion

        #region 窗体与控件事件处理
        // 装载窗体
        private void frmLayerAttributes_Load(object sender, EventArgs e)
        {
            mChangeByUser = false;
            ShowFormTitle();
            CreateColumns();
            SetColumnTexts();
            CreateRows();
            SetRowSelections();
            SetIdentifiedFeatures();
            mChangeByUser = true;
        }

        private void dgvAttributes_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            Int32 sColumnIndex = e.ColumnIndex;
            Int32 sRowIndex = e.RowIndex;
            if (sColumnIndex < 0 || sRowIndex < 0)
            {
                return;
            }
            MyMapObjects.moFeature sFeature = _Layer.Features.GetItem(sRowIndex);
            object sValue = sFeature.Attributes.GetItem(sColumnIndex);
            e.Value = sValue.ToString();
        }

        private void dgvAttributes_SelectionChanged(object sender, EventArgs e)
        {
            if (!mChangeByUser) return;
            MyMapObjects.moFeatures sSelectedFeatures = new MyMapObjects.moFeatures();
            Int32 sFeatureCount = _Layer.Features.Count;
            for (Int32 i = 0; i < sFeatureCount; ++i)
            {
                if (dgvAttributes.Rows[i].Selected)
                {
                    MyMapObjects.moFeature sFeature = _Layer.Features.GetItem(i);
                    sSelectedFeatures.Add(sFeature);
                }
            }
            _Layer.SelectedFeatures = sSelectedFeatures;
            ToNotifyFeatureSelectionChanged();
        }

        private void tsSelectAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSearchIndex = tsSelectAttributes.SelectedIndex;
            MyMapObjects.moFields sFields = _Layer.AttributeFields;
            Int32 sFieldCount = sFields.Count;
            for (Int32 i = 0; i < sFieldCount; ++i)
            {
                string sColumnText = "";
                MyMapObjects.moField sField = sFields.GetItem(i);
                sColumnText = sField.Name;
                dgvAttributes.Columns[i].HeaderText = sColumnText;
                tsSelectAttributes.Items.Add(sColumnText);
            }
        }

        private void tsSearchText_TextChanged(object sender, EventArgs e)
        {
            if (mSearchIndex == -1 || tsSearchText.Text == null) return;
            Int32 sColumnIndex = mSearchIndex;
            Int32 sFeatureCount = _Layer.Features.Count;
            List<Int32> sFoundedIndexs = new List<Int32>();
            for (Int32 i = 0; i < sFeatureCount; ++i)
            {
                Int32 sIndex = i;
                dgvAttributes.Rows[sIndex].DefaultCellStyle.BackColor = SystemColors.Window;
                MyMapObjects.moFeature sFeature = _Layer.Features.GetItem(sIndex);
                object sValue = sFeature.Attributes.GetItem(sColumnIndex);
                string targetText = sValue.ToString();
                if (targetText.Contains(tsSearchText.Text))
                {
                    sFoundedIndexs.Add(sIndex);
                    dgvAttributes.Rows[sIndex].DefaultCellStyle.BackColor = Color.FromArgb(128, 255, 128);
                }
            }
            tsSearchResult.Text = "已找到记录：" + sFoundedIndexs.Count.ToString();
        }

        #endregion

        #region 构造函数
        public frmLayerAttributes()
        {
            InitializeComponent();
        }


        #endregion


    }
}
