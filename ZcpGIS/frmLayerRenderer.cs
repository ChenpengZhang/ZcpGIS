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
    public partial class frmLayerRenderer : Form
    {
        #region 字段

        private MyMapObjects.moMapLayer _Layer;
        private MyMapObjects.moRenderer _Renderer;
        private Color _SimpleColor = Color.White;
        private int _Breaks = 5;
        private Color _StartColor = Color.Maroon;
        private Color _EndColor = Color.FromArgb(255, 192, 192);
        private bool _IsPolygon = false;

        #endregion

        #region 方法

        //设置图层
        internal void SetData(MyMapObjects.moMapLayer layer)
        {
            _Layer = layer;
            MyMapObjects.moFields sFields = _Layer.AttributeFields;
            Int32 sFieldCount = sFields.Count;
            for (Int32 i = 0; i < sFieldCount; ++i)
            {
                string sColumnText = "";
                MyMapObjects.moField sField = sFields.GetItem(i);
                sColumnText = sField.Name;
                listBoxAttributes.Items.Add(sColumnText);
                if (sField.ValueType == moValueTypeConstant.dDouble)
                {
                    listBoxAttributes2.Items.Add(sColumnText);
                }
            }
            _Renderer = _Layer.Renderer;  // 以防有人没设置就点确定了
        }

        internal void GetData(out MyMapObjects.moRenderer renderer)
        {
            renderer = _Renderer;
        }

        #endregion

        #region 字段

        #endregion

        public frmLayerRenderer()
        {
            InitializeComponent();
        }

        private void frmLayerRenderer_Load(object sender, EventArgs e)
        {
            // 这时已经获得了输入的图层对象_Layer，为接下来的用户交互做相应的准备工作
            if (_Layer.ShapeType != moGeometryTypeConstant.MultiPolygon)
            {
                throw new Exception("Invalid Shape Type");
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // 1. 根据用户交互的结果新建一个Renderer的实例，并赋给_Renderer变量
            if (tabControl1.SelectedIndex == 0)
            {
                MyMapObjects.moSimpleRenderer sRenderer = new MyMapObjects.moSimpleRenderer();
                MyMapObjects.moSimpleFillSymbol sSymbol = new MyMapObjects.moSimpleFillSymbol();
                sSymbol.Color = _SimpleColor;
                // 可以继续设置符号的形式
                sRenderer.Symbol = sSymbol;
                _Renderer = sRenderer;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                Int32 sFieldIndex = listBoxAttributes.SelectedIndex;
                // 新建一个渲染对象
                MyMapObjects.moUniqueValueRenderer sRenderer = new MyMapObjects.moUniqueValueRenderer();
                sRenderer.Field = (string)listBoxAttributes.Items[sFieldIndex];
                List<string> sValues = new List<string>();
                Int32 sFeaturesCount = _Layer.Features.Count;
                for (int i = 0; i < sFeaturesCount; ++i)
                {
                    object sBlock = _Layer.Features.GetItem(i).Attributes.GetItem(sFieldIndex);
                    string sValue = sBlock.ToString();
                    sValues.Add(sValue);
                }
                // 去除重复
                sValues.Distinct().ToList();
                // 生成符号
                Int32 sValueCount = sValues.Count;
                for (int i = 0; i < sValueCount; ++i)
                {
                    MyMapObjects.moSimpleFillSymbol sSymbol = new MyMapObjects.moSimpleFillSymbol();
                    sRenderer.AddUniqueValue(sValues[i], sSymbol);
                }
                sRenderer.DefaultSymbol = new MyMapObjects.moSimpleFillSymbol();
                _Renderer = sRenderer;
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                if (listBoxAttributes2.SelectedIndex == -1) return;
                string sFieldName = listBoxAttributes2.SelectedItem.ToString();
                Int32 sFieldIndex = _Layer.AttributeFields.FindField(sFieldName);
                if (sFieldIndex == -1) return;
                if (_Layer.AttributeFields.GetItem(sFieldIndex).ValueType != MyMapObjects.moValueTypeConstant.dDouble) return;
                // 新建一个分级渲染对象
                MyMapObjects.moClassBreaksRenderer sRenderer = new MyMapObjects.moClassBreaksRenderer();
                sRenderer.Field = sFieldName;
                List<double> sValues = new List<double>();
                Int32 sFeaturesCount = _Layer.Features.Count;
                for (int i = 0; i < sFeaturesCount; ++i)
                {
                    double sValue = (double)_Layer.Features.GetItem(i).Attributes.GetItem(sFieldIndex);
                    sValues.Add(sValue);
                }
                // 获取最小最大值，并分为5级
                double sMinValue = sValues.Min();
                double sMaxValue = sValues.Max();
                for (Int32 i = 0; i < _Breaks; ++i)
                {
                    double sValue = sMinValue + (sMaxValue - sMinValue) * (i + 1) / _Breaks;
                    MyMapObjects.moSimpleFillSymbol sSymbol = new MyMapObjects.moSimpleFillSymbol();
                    sRenderer.AddBreakValue(sValue, sSymbol);
                }
                sRenderer.RampColor(_StartColor, _EndColor);
                sRenderer.DefaultSymbol = new MyMapObjects.moSimpleFillSymbol();
                // 赋给图层
                _Renderer = sRenderer;
            }
            // 2. 关闭窗体，并返回对话框结果
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void listBoxColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxColors.SelectedIndex == 0)
            {
                _SimpleColor = Color.Pink;
            }
            else if (listBoxColors.SelectedIndex == 1)
            {
                _SimpleColor = Color.LightYellow;
            }
            else if (listBoxColors.SelectedIndex == 2)
            {
                _SimpleColor = Color.LightBlue;
            }
            else if (listBoxColors.SelectedIndex == 3)
            {
                DialogResult result = colorDialog1.ShowDialog();

                // 检查用户是否点击了“确定”按钮
                if (result == DialogResult.OK)
                {
                    // 获取用户选择的颜色
                    _SimpleColor = colorDialog1.Color;
                }
            }
        }

        private void listBoxAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void textBreaks_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBreaks.Text)) return;
            int result;
            if (int.TryParse(textBreaks.Text, out result))
            {
                _Breaks = result;
            }
            else
            {
                // 转换失败
                MessageBox.Show("请输入一个整数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnStartColor_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialogStart.ShowDialog();
            // 检查用户是否点击了“确定”按钮
            if (result == DialogResult.OK)
            {
                // 获取用户选择的颜色
                _StartColor = colorDialogStart.Color;
                panelStart.BackColor = _StartColor;
            }
        }

        private void btnEndColor_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialogEnd.ShowDialog();
            // 检查用户是否点击了“确定”按钮
            if (result == DialogResult.OK)
            {
                // 获取用户选择的颜色
                _EndColor = colorDialogEnd.Color;
                panelEnd.BackColor = _EndColor;
            }
        }
    }
}
