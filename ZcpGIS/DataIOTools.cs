using MyMapObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace ZcpGIS
{
    internal class DataIOTools
    {
        #region 程序集方法
        internal static MyMapObjects.moMapLayer LoadLayerFromZshpFile(string zshpFileName)
        {
            // 1 读取文件
            string sBodyFileName = Path.GetFileNameWithoutExtension(zshpFileName);//不含路径和扩展名
            string sDirectoryName = Path.GetDirectoryName(zshpFileName); //目录名
            string zdbfFileName = sDirectoryName + Path.DirectorySeparatorChar + sBodyFileName + ".zdbf";

            // 2 将 JSON 文本解析为 JObject 对象
            string jsonText = File.ReadAllText(zshpFileName);
            JObject jsonObj = JObject.Parse(jsonText);

            // 3 检查读取格式
            if ((string)jsonObj["FileType"] != "zshp")
            {
                throw new Exception("请使用正确的zshp格式文件！");
            }

            // 4 确定图形的类型
            MyMapObjects.moGeometryTypeConstant sShapeType;
            if ((string)jsonObj["ShapeType"] == "1")
            {
                sShapeType = MyMapObjects.moGeometryTypeConstant.MultiPolyline;
            }
            else if ((string)jsonObj["ShapeType"] == "2")
            {
                sShapeType = MyMapObjects.moGeometryTypeConstant.MultiPolygon;
            }
            else if ((string)jsonObj["ShapeType"] == "0")
            {
                sShapeType = MyMapObjects.moGeometryTypeConstant.Point;
            }
            else
            {
                throw new Exception("不合法的zshp对象类型，请检查输入！");
            }

            // 5 获得属性表数据
            List<MyMapObjects.moAttributes> sAttributesList;
            MyMapObjects.moFields sFields;
            ReadzdbfFile(zdbfFileName, out sFields, out sAttributesList);

            // 6 添加几何信息
            List<MyMapObjects.moGeometry> sGeometries = new List<MyMapObjects.moGeometry>();  // 新建几何类
            // 赋值几何类
            if (sShapeType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                JArray mPoints = (JArray)jsonObj["Shapes"];
                foreach (JObject mPoint in mPoints)
                {
                    double x = (double)mPoint["X"];
                    double y = (double)mPoint["Y"];
                    MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(x, y);  //新建点图形
                    sGeometries.Add(sPoint);
                }
            }
            else if (sShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                JArray mMultiPolylines = (JArray)jsonObj["Shapes"];
                foreach (JArray mPolylines in mMultiPolylines)
                {
                    moParts sParts = new moParts();
                    foreach (JArray mPart in mPolylines)
                    {
                        moPoints sPoints = new moPoints();
                        foreach (JObject mPoint in mPart)
                        {
                            double x = (double)mPoint["X"];
                            double y = (double)mPoint["Y"];
                            MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(x, y);  //新建点图形
                            sPoints.Add(sPoint);
                        }
                        sParts.Add(sPoints);
                    }
                    moMultiPolyline sMultiPolyline = new moMultiPolyline(sParts);
                    sMultiPolyline.UpdateExtent();
                    sGeometries.Add(sMultiPolyline);
                }
            }
            else if (sShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                JArray mMultiPolygons = (JArray)jsonObj["Shapes"];
                foreach (JArray mPolygons in mMultiPolygons)
                {
                    moParts sParts = new moParts();
                    foreach (JArray mPart in mPolygons)
                    {
                        moPoints sPoints = new moPoints();
                        foreach (JObject mPoint in mPart)
                        {
                            double x = (double)mPoint["X"];
                            double y = (double)mPoint["Y"];
                            MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(x, y);  //新建点图形
                            sPoints.Add(sPoint);
                        }
                        sParts.Add(sPoints);
                    }
                    moMultiPolygon sMultiPolygon = new moMultiPolygon(sParts);
                    sMultiPolygon.UpdateExtent();
                    sGeometries.Add(sMultiPolygon);
                }
            }
            MyMapObjects.moFeatures sFeatures = new MyMapObjects.moFeatures();  // 新建多要素类
            Int32 sFeatureCount = sGeometries.Count;
            for (Int32 i = 0; i <= sFeatureCount - 1; i++)  // 将要素一个一个加入到多要素类中
            {
                MyMapObjects.moFeature sFeature = new MyMapObjects.moFeature
                    (sShapeType, sGeometries[i], sAttributesList[i]);
                sFeatures.Add(sFeature);
            }
            string sLayerName = sBodyFileName;
            MyMapObjects.moMapLayer sMapLayer = new MyMapObjects.moMapLayer(sLayerName, sShapeType, sFields);
            sMapLayer.Features = sFeatures;
            sMapLayer.UpdateExtent();
            return sMapLayer;
        }

        internal static MyMapObjects.moMapLayer LoadMapLayerFromShapeFile(string shapeFileName)
        {
            //说明：
            //（1）仅支持点、多线和多多边形三种图形类型（对应值为1,3,5）,如遇到上述三种类型以外的类型，则报错。
            //（2）根据shp文件路径，自动寻找相同路径下的dbf文件（存储属性数据的文件）。
            //（3）将自动寻找相应路径下的cpg文件，并根据该文件确定dbf文件的字符编码类型。如无cpg文件，
            //则将采用操作系统默认的字符编码类型，亦因此无法保证字符显示正确，如遇此种情况，
            //可以在本函数中设置相应的字符编码类型
            //（4）如果遇到日期型字段，则将其值转换为字符型
            //////////////以下为读取程序///////////////////
            //（1）读取.shp文件，返回几何图形的类型以及所有要素的几何图形
            Int32 sShapeTypeTag;   //几何类型
            List<MyMapObjects.moGeometry> sGeometries = ReadshpFile(shapeFileName, out sShapeTypeTag);
            //（2）读取.dbf文件
            //（2.1）获取同一路径下的.dbf文件和.cpg文件
            string sBodyFileName = Path.GetFileNameWithoutExtension(shapeFileName);//不含路径和扩展名
            string sDirectoryName = Path.GetDirectoryName(shapeFileName); //目录名
            string sdbfFileName = sDirectoryName + Path.DirectorySeparatorChar + sBodyFileName + ".dbf";
            string scpgFileName = sDirectoryName + Path.DirectorySeparatorChar + sBodyFileName + ".cpg";
            //（2.2）读取属性数据，返回字段集合以及所有要素的属性值集合
            MyMapObjects.moFields sFields;
            List<MyMapObjects.moAttributes> sAttributesList;
            ReaddbfFile(scpgFileName, sdbfFileName, out sFields, out sAttributesList);
            //（3）检验shp中的图形数量与dbf中的记录数是否相等，如否则报错，由调用程序处理
            if (sGeometries.Count != sAttributesList.Count)
            {
                throw new Exception("shp文件的图形数与dbf文件的记录数不等！");
            }
            //（3）建立要素集合
            //（3.1）确定要素几何类型
            MyMapObjects.moGeometryTypeConstant sShapeType = MyMapObjects.moGeometryTypeConstant.Point;
            if (sShapeTypeTag == 1)
            {
                sShapeType = MyMapObjects.moGeometryTypeConstant.Point;
            }
            else if (sShapeTypeTag == 3)
            {
                sShapeType = MyMapObjects.moGeometryTypeConstant.MultiPolyline;
            }
            else if (sShapeTypeTag == 5)
            {
                sShapeType = MyMapObjects.moGeometryTypeConstant.MultiPolygon;
            }
            else
            { }
            //（3.2）建立要素集合
            MyMapObjects.moFeatures sFeatures = new MyMapObjects.moFeatures();
            Int32 sFeatureCount = sGeometries.Count;
            for (Int32 i = 0; i <= sFeatureCount - 1; i++)
            {
                MyMapObjects.moFeature sFeature = new MyMapObjects.moFeature
                    (sShapeType, sGeometries[i], sAttributesList[i]);
                sFeatures.Add(sFeature);
            }
            //（4）建立图层
            //（4.1）从文件名获取图层名
            string sLayerName = sBodyFileName;
            //（4.2）建立图层
            MyMapObjects.moMapLayer sMapLayer = new MyMapObjects.moMapLayer
                (sLayerName, sShapeType, sFields);
            sMapLayer.Features = sFeatures;
            sMapLayer.UpdateExtent();
            return sMapLayer;
        }

        internal static List<MyMapObjects.moRenderer> LoadRendererFromZmxdFile(string zmxdFileName, moLayers sLayers)
        {
            
            string jsonText = File.ReadAllText(zmxdFileName);
            // 将 JSON 字符串解析为 JObject
            JObject obj = JObject.Parse(jsonText);

            List<moRenderer> sRenderers = new List<moRenderer>();
            // 获取 "Renderers" 键下的对象
            JArray layers = (JArray)obj["Layers"];
            JArray renderers = (JArray)obj["Renderers"];
            Int32 sLayerCount = layers.Count;
            for(int i = 0; i < sLayerCount; ++i)
            {
                JToken jRenderer = renderers[i];  // 保存的渲染对象
                moMapLayer sLayer = sLayers.GetItem(i);
                if (sLayer.ShapeType == moGeometryTypeConstant.Point)  // 点类型的图层
                {
                    if ((string)jRenderer["RendererType"] == "0")  // 简单渲染
                    {
                        moSimpleRenderer sRenderer = new moSimpleRenderer();
                        JToken jSymbol = jRenderer["Symbol"];
                        moSimpleMarkerSymbol sSymbol = new moSimpleMarkerSymbol();
                        sSymbol.Label = (string)jSymbol["Label"];
                        sSymbol.Style = (moSimpleMarkerSymbolStyleConstant)jSymbol["Style"].Value<Int32>();
                        string colorString = (string)jSymbol["Color"];
                        Color color;
                        if (colorString.Contains(","))
                        {
                            // 如果颜色字符串包含逗号，则按RGB模式处理
                            string[] colorParts = colorString.Split(',');
                            int r = int.Parse(colorParts[0]);
                            int g = int.Parse(colorParts[1]);
                            int b = int.Parse(colorParts[2]);
                            color = Color.FromArgb(r, g, b);
                        }
                        else
                        {
                            // 否则尝试按名称转换
                            color = Color.FromName(colorString);
                        }
                        sSymbol.Color = color;
                        sRenderer.Symbol = sSymbol;
                        sRenderers.Add(sRenderer);
                    }
                    else if ((string)jRenderer["RendererType"] == "1")  // 唯一值渲染
                    {
                        moUniqueValueRenderer sRenderer = new moUniqueValueRenderer();
                        sRenderer.Field = (string)jRenderer["Field"];
                        sRenderer.Values = jRenderer["Values"].Select(jv => (string)jv).ToList();
                        List<moSymbol> sSymbols = new List<moSymbol>();
                        foreach (JToken jSymbol in jRenderer["Symbols"])
                        {
                            moSimpleMarkerSymbol sSymbol = new moSimpleMarkerSymbol();
                            sSymbol.Label = (string)jSymbol["Label"];
                            sSymbol.Style = (moSimpleMarkerSymbolStyleConstant)jSymbol["Style"].Value<Int32>();
                            string colorString = (string)jSymbol["Color"];
                            Color color;
                            if (colorString.Contains(","))
                            {
                                // 如果颜色字符串包含逗号，则按RGB模式处理
                                string[] colorParts = colorString.Split(',');
                                int r = int.Parse(colorParts[0]);
                                int g = int.Parse(colorParts[1]);
                                int b = int.Parse(colorParts[2]);
                                color = Color.FromArgb(r, g, b);
                            }
                            else
                            {
                                // 否则尝试按名称转换
                                color = Color.FromName(colorString);
                            }
                            sSymbols.Add(sSymbol);
                        }
                        sRenderer.Symbols = sSymbols;
                        sRenderers.Add(sRenderer);
                    }
                    else if ((string)jRenderer["RendererType"] == "2")  // 分级渲染
                    {
                        moClassBreaksRenderer sRenderer = new moClassBreaksRenderer();
                        sRenderer.Field = (string)jRenderer["Field"];
                        sRenderer.BreakValues = jRenderer["BreakValues"].Select(jv => (double)jv).ToList();
                        List<moSymbol> sSymbols = new List<moSymbol>();
                        foreach (JToken jSymbol in jRenderer["Symbols"])
                        {
                            moSimpleMarkerSymbol sSymbol = new moSimpleMarkerSymbol();
                            sSymbol.Label = (string)jSymbol["Label"];
                            sSymbol.Style = (moSimpleMarkerSymbolStyleConstant)jSymbol["Style"].Value<Int32>();
                            string colorString = (string)jSymbol["Color"];
                            Color color;
                            if (colorString.Contains(","))
                            {
                                // 如果颜色字符串包含逗号，则按RGB模式处理
                                string[] colorParts = colorString.Split(',');
                                int r = int.Parse(colorParts[0]);
                                int g = int.Parse(colorParts[1]);
                                int b = int.Parse(colorParts[2]);
                                color = Color.FromArgb(r, g, b);
                            }
                            else
                            {
                                // 否则尝试按名称转换
                                color = Color.FromName(colorString);
                            }
                            sSymbols.Add(sSymbol);
                        }
                        sRenderer.Symbols = sSymbols;
                        sRenderers.Add(sRenderer);
                    }
                }
                else if (sLayer.ShapeType == moGeometryTypeConstant.MultiPolyline)  // 点类型的图层
                {
                    if ((string)jRenderer["RendererType"] == "0")  // 简单渲染
                    {
                        moSimpleRenderer sRenderer = new moSimpleRenderer();
                        JToken jSymbol = jRenderer["Symbol"];
                        moSimpleLineSymbol sSymbol = new moSimpleLineSymbol();
                        sSymbol.Label = (string)jSymbol["Label"];
                        sSymbol.Style = (moSimpleLineSymbolStyleConstant)jSymbol["Style"].Value<Int32>();
                        string colorString = (string)jSymbol["Color"];
                        Color color;
                        if (colorString.Contains(","))
                        {
                            // 如果颜色字符串包含逗号，则按RGB模式处理
                            string[] colorParts = colorString.Split(',');
                            int r = int.Parse(colorParts[0]);
                            int g = int.Parse(colorParts[1]);
                            int b = int.Parse(colorParts[2]);
                            color = Color.FromArgb(r, g, b);
                        }
                        else
                        {
                            // 否则尝试按名称转换
                            color = Color.FromName(colorString);
                        }
                        sSymbol.Color = color;
                        sRenderer.Symbol = sSymbol;
                        sRenderers.Add(sRenderer);
                    }
                    else if ((string)jRenderer["RendererType"] == "1")  // 唯一值渲染
                    {
                        moUniqueValueRenderer sRenderer = new moUniqueValueRenderer();
                        sRenderer.Field = (string)jRenderer["Field"];
                        sRenderer.Values = jRenderer["Values"].Select(jv => (string)jv).ToList();
                        List<moSymbol> sSymbols = new List<moSymbol>();
                        foreach (JToken jSymbol in jRenderer["Symbols"])
                        {
                            moSimpleLineSymbol sSymbol = new moSimpleLineSymbol();
                            sSymbol.Label = (string)jSymbol["Label"];
                            sSymbol.Style = (moSimpleLineSymbolStyleConstant)jSymbol["Style"].Value<Int32>();
                            string colorString = (string)jSymbol["Color"];
                            Color color;
                            if (colorString.Contains(","))
                            {
                                // 如果颜色字符串包含逗号，则按RGB模式处理
                                string[] colorParts = colorString.Split(',');
                                int r = int.Parse(colorParts[0]);
                                int g = int.Parse(colorParts[1]);
                                int b = int.Parse(colorParts[2]);
                                color = Color.FromArgb(r, g, b);
                            }
                            else
                            {
                                // 否则尝试按名称转换
                                color = Color.FromName(colorString);
                            }
                            sSymbols.Add(sSymbol);
                        }
                        sRenderer.Symbols = sSymbols;
                        sRenderers.Add(sRenderer);
                    }
                    else if ((string)jRenderer["RendererType"] == "2")  // 分级渲染
                    {
                        moClassBreaksRenderer sRenderer = new moClassBreaksRenderer();
                        sRenderer.Field = (string)jRenderer["Field"];
                        sRenderer.BreakValues = jRenderer["BreakValues"].Select(jv => (double)jv).ToList();
                        List<moSymbol> sSymbols = new List<moSymbol>();
                        foreach (JToken jSymbol in jRenderer["Symbols"])
                        {
                            moSimpleLineSymbol sSymbol = new moSimpleLineSymbol();
                            sSymbol.Label = (string)jSymbol["Label"];
                            sSymbol.Style = (moSimpleLineSymbolStyleConstant)jSymbol["Style"].Value<Int32>();
                            string colorString = (string)jSymbol["Color"];
                            Color color;
                            if (colorString.Contains(","))
                            {
                                // 如果颜色字符串包含逗号，则按RGB模式处理
                                string[] colorParts = colorString.Split(',');
                                int r = int.Parse(colorParts[0]);
                                int g = int.Parse(colorParts[1]);
                                int b = int.Parse(colorParts[2]);
                                color = Color.FromArgb(r, g, b);
                            }
                            else
                            {
                                // 否则尝试按名称转换
                                color = Color.FromName(colorString);
                            }
                            sSymbols.Add(sSymbol);
                        }
                        sRenderer.Symbols = sSymbols;
                        sRenderers.Add(sRenderer);
                    }
                }
                else if (sLayer.ShapeType == moGeometryTypeConstant.MultiPolygon)  // 面类型的图层
                {
                    if ((string)jRenderer["RendererType"] == "0")  // 简单渲染
                    {
                        moSimpleRenderer sRenderer = new moSimpleRenderer();
                        JToken jSymbol = jRenderer["Symbol"];
                        moSimpleFillSymbol sSymbol = new moSimpleFillSymbol();
                        sSymbol.Label = (string)jSymbol["Label"];
                        string colorString = (string)jSymbol["Color"];
                        Color color;
                        if (colorString.Contains(","))
                        {
                            // 如果颜色字符串包含逗号，则按RGB模式处理
                            string[] colorParts = colorString.Split(',');
                            int r = int.Parse(colorParts[0]);
                            int g = int.Parse(colorParts[1]);
                            int b = int.Parse(colorParts[2]);
                            color = Color.FromArgb(r, g, b);
                        }
                        else
                        {
                            // 否则尝试按名称转换
                            color = Color.FromName(colorString);
                        }
                        sSymbol.Color = color;

                        // 下面是专门处理outline的区域
                        moSimpleLineSymbol sOutline = new moSimpleLineSymbol();
                        JToken jOutline = jSymbol["Outline"];
                        sOutline.Style = (moSimpleLineSymbolStyleConstant)jOutline["Style"].Value<Int32>();
                        sOutline.Label = (string)jOutline["Label"];
                        string colorString2 = (string)jOutline["Color"];
                        Color color2;
                        if (colorString2.Contains(","))
                        {
                            string debugString = colorString2.Replace("'", "");  // 一些奇怪的Bug，需要将多余的字符去除
                            // 如果颜色字符串包含逗号，则按RGB模式处理
                            string[] colorParts = debugString.Split(',');
                            int r = int.Parse(colorParts[0]);
                            int g = int.Parse(colorParts[1]);
                            int b = int.Parse(colorParts[2]);
                            color2 = Color.FromArgb(r, g, b);
                        }
                        else
                        {
                            // 否则尝试按名称转换
                            color2 = Color.FromName(colorString2);
                        }
                        sOutline.Color = color2;
                        sSymbol.Outline = sOutline;
                        // 处理完毕

                        sRenderer.Symbol = sSymbol;
                        sRenderers.Add(sRenderer);
                    }
                    else if ((string)jRenderer["RendererType"] == "1")  // 唯一值渲染
                    {
                        moUniqueValueRenderer sRenderer = new moUniqueValueRenderer();
                        sRenderer.Field = (string)jRenderer["Field"];
                        sRenderer.Values = jRenderer["Values"].Select(jv => (string)jv).ToList();
                        List<moSymbol> sSymbols = new List<moSymbol>();
                        foreach (JToken jSymbol in jRenderer["Symbols"])
                        {
                            moSimpleFillSymbol sSymbol = new moSimpleFillSymbol();
                            sSymbol.Label = (string)jSymbol["Label"];
                            string colorString = (string)jSymbol["Color"];
                            Color color;
                            if (colorString.Contains(","))
                            {
                                // 如果颜色字符串包含逗号，则按RGB模式处理
                                string[] colorParts = colorString.Split(',');
                                int r = int.Parse(colorParts[0]);
                                int g = int.Parse(colorParts[1]);
                                int b = int.Parse(colorParts[2]);
                                color = Color.FromArgb(r, g, b);
                            }
                            else
                            {
                                // 否则尝试按名称转换
                                color = Color.FromName(colorString);
                            }
                            sSymbol.Color = color;

                            // 下面是专门处理outline的区域
                            moSimpleLineSymbol sOutline = new moSimpleLineSymbol();
                            JToken jOutline = jSymbol["Outline"];
                            sOutline.Style = (moSimpleLineSymbolStyleConstant)jOutline["Style"].Value<Int32>();
                            sOutline.Label = (string)jOutline["Label"];
                            string colorString2 = (string)jOutline["Color"];
                            Color color2;
                            if (colorString2.Contains(","))
                            {
                                string debugString = colorString2.Replace("'", "");
                                // 如果颜色字符串包含逗号，则按RGB模式处理
                                string[] colorParts = debugString.Split(',');
                                int r = int.Parse(colorParts[0]);
                                int g = int.Parse(colorParts[1]);
                                int b = int.Parse(colorParts[2]);
                                color2 = Color.FromArgb(r, g, b);
                            }
                            else
                            {
                                // 否则尝试按名称转换
                                color2 = Color.FromName(colorString2);
                            }
                            sOutline.Color = color2;
                            sSymbol.Outline = sOutline;
                            // 处理完毕
                            sSymbols.Add(sSymbol);
                        }
                        sRenderer.Symbols = sSymbols;
                        sRenderers.Add(sRenderer);
                    }
                    else if ((string)jRenderer["RendererType"] == "2")  // 分级渲染
                    {
                        moClassBreaksRenderer sRenderer = new moClassBreaksRenderer();
                        sRenderer.Field = (string)jRenderer["Field"];
                        sRenderer.BreakValues = jRenderer["BreakValues"].Select(jv => (double)jv).ToList();
                        List<moSymbol> sSymbols = new List<moSymbol>();
                        foreach (JToken jSymbol in jRenderer["Symbols"])
                        {
                            moSimpleFillSymbol sSymbol = new moSimpleFillSymbol();
                            sSymbol.Label = (string)jSymbol["Label"];
                            string colorString = (string)jSymbol["Color"];
                            Color color;
                            if (colorString.Contains(","))
                            {
                                // 如果颜色字符串包含逗号，则按RGB模式处理
                                string[] colorParts = colorString.Split(',');
                                int r = int.Parse(colorParts[0]);
                                int g = int.Parse(colorParts[1]);
                                int b = int.Parse(colorParts[2]);
                                color = Color.FromArgb(r, g, b);
                            }
                            else
                            {
                                // 否则尝试按名称转换
                                color = Color.FromName(colorString);
                            }
                            sSymbol.Color = color;

                            // 下面是专门处理outline的区域
                            moSimpleLineSymbol sOutline = new moSimpleLineSymbol();
                            JToken jOutline = jSymbol["Outline"];
                            sOutline.Style = (moSimpleLineSymbolStyleConstant)jOutline["Style"].Value<Int32>();
                            sOutline.Label = (string)jOutline["Label"];
                            string colorString2 = (string)jOutline["Color"];
                            Color color2;
                            if (colorString2.Contains(","))
                            {
                                string debugString = colorString2.Replace("'", "");
                                // 如果颜色字符串包含逗号，则按RGB模式处理
                                string[] colorParts = debugString.Split(',');
                                int r = int.Parse(colorParts[0]);
                                int g = int.Parse(colorParts[1]);
                                int b = int.Parse(colorParts[2]);
                                color2 = Color.FromArgb(r, g, b);
                            }
                            else
                            {
                                // 否则尝试按名称转换
                                color2 = Color.FromName(colorString2);
                            }
                            sOutline.Color = color2;
                            sSymbol.Outline = sOutline;

                            sSymbols.Add(sSymbol);
                        }
                        sRenderer.Symbols = sSymbols;
                        sRenderers.Add(sRenderer);
                    }
                }
            }
            
            return sRenderers;
            
        }

        #endregion

        #region 私有函数

        //本函数返回所有几何图形集合，并返回参数shapeType的值
        private static List<MyMapObjects.moGeometry> ReadshpFile(string shapeFileName, out Int32 shapeType)
        {
            //（1）设置相关临时变量
            Int32 sTempInt;         //临时变量，仅用于跳过某些字节
            double sTempDouble;     //临时变量，仅用于跳过某些字节
            //（2）打开文件
            FileStream sStream = new FileStream(shapeFileName, FileMode.Open);
            BinaryReader sr = new BinaryReader(sStream);
            //（3）读取文件头部分
            sTempInt = sr.ReadInt32();         //文件代码
            for (Int32 i = 0; i <= 4; i++)
            {
                sTempInt = sr.ReadInt32();      //未被使用
            }
            sTempInt = sr.ReadInt32();          //文件长度
            sTempInt = sr.ReadInt32();          //版本：值为1000
            Int32 sShapeType = sr.ReadInt32();        //图形类型
            //判断图形类型是否为支持的类型，如不是，则报错，由调用程序处理
            if (sShapeType != 1 && sShapeType != 3 && sShapeType != 5)
            {
                sr.Close();
                sStream.Close();
                throw new Exception("目前不支持该类型的图形！");
            }
            for (Int32 i = 0; i <= 7; i++)
            {
                sTempDouble = sr.ReadDouble(); //图层范围，Xmin、Ymin、Xmax、Ymax、Zmin、Zmax、Mmin、Mmax
            }
            //（4）定义几何图形集合，为读取所有要素的坐标做准备
            List<MyMapObjects.moGeometry> sGeometries = new List<MyMapObjects.moGeometry>();
            //（5）顺序读取所有要素的几何图形                                                                                                             
            while (sStream.Position < sStream.Length)
            {
                sTempInt = sr.ReadInt32();         //当前记录号
                sTempInt = sr.ReadInt32();         //当前记录长度
                if (sShapeType == 1)                //点
                {
                    sTempInt = sr.ReadInt32();      //Shape类型
                    double sX = sr.ReadDouble();    //读取当前点的X坐标
                    double sY = sr.ReadDouble();    //读取当前点的Y坐标
                    MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(sX, sY);//新建点图形
                    sGeometries.Add(sPoint);         //加入point集合
                }
                else if (sShapeType == 3)           //多线
                {
                    sTempInt = sr.ReadInt32();      //Shape类型
                    double sXmin = sr.ReadDouble();       //边界盒，下同
                    double sYmin = sr.ReadDouble();
                    double sXmax = sr.ReadDouble();
                    double sYmax = sr.ReadDouble();
                    Int32 sPartCount = sr.ReadInt32();        //部件的数目（即简单Polyline的数目）
                    Int32 sPointCount = sr.ReadInt32();       //点的数目
                    Int32[] sPartStarts = new Int32[sPartCount]; //当前图形每个部件第一个点在点序列中的索引号
                                                                 //读取所有部件第一个的索引号
                    for (Int32 i = 0; i <= sPartCount - 1; i++)
                    {
                        sPartStarts[i] = sr.ReadInt32();
                    }
                    //读取所有点
                    MyMapObjects.moPoint[] sPoints = new MyMapObjects.moPoint[sPointCount];
                    for (Int32 i = 0; i <= sPointCount - 1; i++)
                    {
                        double sX = sr.ReadDouble();
                        double sY = sr.ReadDouble();
                        MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(sX, sY);//新建一个点
                        sPoints[i] = sPoint;
                    }
                    //建立部件集合
                    MyMapObjects.moParts sParts = new MyMapObjects.moParts();
                    for (Int32 i = 0; i <= sPartCount - 1; i++)
                    {
                        MyMapObjects.moPoints sPart = new MyMapObjects.moPoints();
                        Int32 sStartIndex, sEndIndex;//当前部件的首点和尾点的索引号
                        sStartIndex = sPartStarts[i];
                        if (i < sPartCount - 1)
                        { sEndIndex = sPartStarts[i + 1] - 1; }
                        else
                        { sEndIndex = sPointCount - 1; }
                        for (Int32 j = sStartIndex; j <= sEndIndex; j++)
                        {
                            sPart.Add(sPoints[j]);
                        }
                        sParts.Add(sPart);
                    }
                    //新建多线并更新范围
                    MyMapObjects.moMultiPolyline sMultiPolyline = new MyMapObjects.moMultiPolyline(sParts);
                    sMultiPolyline.UpdateExtent();
                    //加入多线集合
                    sGeometries.Add(sMultiPolyline);
                }
                else if (sShapeType == 5)           //多多边形
                {
                    sTempInt = sr.ReadInt32();      //Shape类型
                    double sXmin = sr.ReadDouble();       //边界盒，下同
                    double sYmin = sr.ReadDouble();
                    double sXmax = sr.ReadDouble();
                    double sYmax = sr.ReadDouble();
                    Int32 sPartCount = sr.ReadInt32();        //部件的数目（即简单Polygon的数目）
                    Int32 sPointCount = sr.ReadInt32();       //点的数目
                    Int32[] sPartStarts = new Int32[sPartCount]; //当前图形每个部件第一个点在点序列中的索引号
                    //读取所有部件第一个点的索引号
                    for (Int32 i = 0; i <= sPartCount - 1; i++)
                    {
                        sPartStarts[i] = sr.ReadInt32();
                    }
                    //读取所有点
                    MyMapObjects.moPoint[] sPoints = new MyMapObjects.moPoint[sPointCount];
                    for (Int32 i = 0; i <= sPointCount - 1; i++)
                    {
                        double sX = sr.ReadDouble();
                        double sY = sr.ReadDouble();
                        MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(sX, sY);//新建一个点
                        sPoints[i] = sPoint;
                    }
                    //建立部件集合
                    MyMapObjects.moParts sParts = new MyMapObjects.moParts();
                    for (Int32 i = 0; i <= sPartCount - 1; i++)
                    {
                        MyMapObjects.moPoints sPart = new MyMapObjects.moPoints();
                        Int32 sStartIndex, sEndIndex;//当前部件的首点和尾点的索引号
                        sStartIndex = sPartStarts[i];
                        if (i < sPartCount - 1)
                        { sEndIndex = sPartStarts[i + 1] - 1; }
                        else
                        { sEndIndex = sPointCount - 1; }
                        for (Int32 j = sStartIndex; j <= sEndIndex; j++)
                        {
                            sPart.Add(sPoints[j]);
                        }
                        sParts.Add(sPart);
                    }
                    //新建多多边形并更新范围
                    MyMapObjects.moMultiPolygon sMultiPolygon = new MyMapObjects.moMultiPolygon(sParts);
                    sMultiPolygon.UpdateExtent();
                    //加入多多边形集合
                    sGeometries.Add(sMultiPolygon);
                }
            }
            //（5）返回相关参数
            sr.Close();
            sStream.Close();
            shapeType = sShapeType;
            return sGeometries;
        }

        private static void ReaddbfFile(string cpgFileName, string dbfFileName, out MyMapObjects.moFields fields, out List<MyMapObjects.moAttributes> attributesList)
        {
            //（1）读取cpg文件获得字符编码，如果文件不存在，则使用操作系统默认编码
            Encoding sEncoding;
            if (File.Exists(cpgFileName) == true)
            {
                StreamReader scpgr = new StreamReader(cpgFileName, Encoding.Default);
                string sLine = scpgr.ReadLine();
                if (sLine == "UTF-8")
                { sEncoding = Encoding.UTF8; }
                else if (sLine == "UTF-7")
                { sEncoding = Encoding.UTF7; }
                else if (sLine == "UTF-32")
                { sEncoding = Encoding.UTF32; }
                else
                { sEncoding = Encoding.Default; }
                scpgr.Dispose();
            }
            else
            { sEncoding = Encoding.Default; }
            //（2）打开dbf文件
            FileStream sStream = new FileStream(dbfFileName, FileMode.Open);
            BinaryReader sr = new BinaryReader(sStream);
            //（3）读取文件头部分
            byte sFileType = sr.ReadByte();         //文件类型
            byte sYear = sr.ReadByte();             // + 1900后为末次修改年份
            byte sMonth = sr.ReadByte();            //末次修改月份
            byte sDay = sr.ReadByte();              //末次修改日
            Int32 sRecordCount = sr.ReadInt32();    //记录数
            Int16 sFileHeadLength = sr.ReadInt16(); //文件头长度
            Int16 sRecordLength = sr.ReadInt16();   //记录长度
            byte sTempByte;     //用于读取保留字节
            Int32 sTempInt32;   //用于读取无用字节
            for (Int32 i = 0; i <= 19; i++)
            {
                sTempByte = sr.ReadByte();     //保留字节
            }
            //（4）读取所有字段信息
            Int32 sFieldCount = (sFileHeadLength - 33) / 32;  //根据文件头长度计算出字段数
            string[] sFieldNames = new string[sFieldCount];     //存储所有字段名称
            char[] sFieldTypes = new char[sFieldCount];         //存储所有字段类型字符
            Int32[] sFieldLengths = new Int32[sFieldCount];      //存储所有字段长度 
            for (Int32 i = 0; i <= sFieldCount - 1; i++)
            {
                //（4.1）字段名称
                byte[] sTempBytes = sr.ReadBytes(10);       // 字段名10个字节
                //转换为字符串，并去除首尾所有空字符(编码为0)
                string sFieldName = sEncoding.GetString(sTempBytes).Trim((char)0);
                sFieldNames[i] = sFieldName;
                //（4.2）系统保留
                sTempByte = sr.ReadByte();     //保留字节
                //（4.3）字段类型
                char sFieldTypeChar = sr.ReadChar();        //表示字段类型的字符
                sFieldTypes[i] = sFieldTypeChar;
                //（4.4）内存地址
                sTempInt32 = sr.ReadInt32();
                //（4.5）字段长度
                byte sFieldLength = sr.ReadByte();
                sFieldLengths[i] = sFieldLength;
                //（4.6）小数位
                byte sFieldScale = sr.ReadByte();
                //（4.7）系统保留
                for (Int32 j = 0; j <= 13; j++)
                {
                    sTempByte = sr.ReadByte();
                }
            }
            sTempByte = sr.ReadByte();         //结束标记，回车符 & HD
            //（5）建立moFields对象
            MyMapObjects.moFields sFields = new MyMapObjects.moFields();
            for (Int32 i = 0; i <= sFieldCount - 1; i++)
            {
                //（5.1）建立moField
                char sFieldTypeChar = sFieldTypes[i];
                MyMapObjects.moValueTypeConstant sFieldValueType = MyMapObjects.moValueTypeConstant.dInt32;
                if (sFieldTypeChar == 'C')  //字符型
                {
                    sFieldValueType = MyMapObjects.moValueTypeConstant.dText;
                }
                else if (sFieldTypeChar == 'N')  //数值型
                {
                    sFieldValueType = MyMapObjects.moValueTypeConstant.dDouble;
                }
                else if (sFieldTypeChar == 'D')  //日期型
                {
                    sFieldValueType = MyMapObjects.moValueTypeConstant.dText;
                }
                else if (sFieldTypeChar == 'F')  //单精度浮点型
                {
                    sFieldValueType = MyMapObjects.moValueTypeConstant.dSingle;
                }
                else if (sFieldTypeChar == 'B')  //双精度浮点型
                {
                    sFieldValueType = MyMapObjects.moValueTypeConstant.dDouble;
                }
                else if (sFieldTypeChar == 'I')  //整型
                {
                    sFieldValueType = MyMapObjects.moValueTypeConstant.dInt32;
                }
                else
                {
                    sFieldValueType = MyMapObjects.moValueTypeConstant.dText;
                }
                MyMapObjects.moField sField = new MyMapObjects.moField(sFieldNames[i], sFieldValueType);
                sFields.Append(sField);
            }
            //（5）读取所有记录，并为每条记录建立一个moAttributes对象
            List<MyMapObjects.moAttributes> sAttributesList = new List<MyMapObjects.moAttributes>();
            for (Int32 i = 0; i <= sRecordCount - 1; i++)
            {
                sTempByte = sr.ReadByte();  //记录删除标记，实为空格
                //新建一个属性集合对象moAttributes
                MyMapObjects.moAttributes sAttributes = new MyMapObjects.moAttributes();
                for (Int32 j = 0; j <= sFieldCount - 1; j++)
                {
                    byte[] sTempBytes = sr.ReadBytes(sFieldLengths[j]);
                    //转换为字符串，并去除首尾空格字符（编码为32）                    
                    string sValueString = sEncoding.GetString(sTempBytes).Trim((char)32);
                    char sFieldTypeChar = sFieldTypes[j];
                    if (sFieldTypeChar == 'C')  //字符型
                    {
                        string sValue = sValueString;
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeChar == 'N')  //数值型
                    {
                        double sValue = 0;
                        if (sValueString != "")
                        {
                            sValue = Double.Parse(sValueString);
                        }
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeChar == 'D')  //日期型
                    {
                        string sValue = sValueString;
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeChar == 'F')  //单精度浮点型
                    {
                        float sValue = 0;
                        if (sValueString != "")
                        {
                            sValue = float.Parse(sValueString);
                        }
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeChar == 'B')  //双精度浮点型
                    {
                        double sValue = 0;
                        if (sValueString != "")
                        {
                            sValue = Double.Parse(sValueString);
                        }
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeChar == 'I')  //整型
                    {
                        Int32 sValue = 0;
                        if (sValueString != "")
                        {
                            sValue = Int32.Parse(sValueString);
                        }
                        sAttributes.Append(sValue);
                    }
                    else
                    {
                        string sValue = sValueString;
                        sAttributes.Append(sValue);
                    }
                }
                sAttributesList.Add(sAttributes);
            }
            sTempByte = sr.ReadByte();         //结束标志 & H1A
            //（6）返回对象
            fields = sFields;
            attributesList = sAttributesList;
        }

        private static void ReadzdbfFile(string zdbfFileName, out MyMapObjects.moFields fields, out List<MyMapObjects.moAttributes> attributesList)
        {
            // 1 将 JSON 文本解析为 JObject 对象
            string jsonText = File.ReadAllText(zdbfFileName);
            JObject jsonObj = JObject.Parse(jsonText);

            // 2 检查读取格式
            if ((string)jsonObj["FileType"] != "zdbf")
            {
                throw new Exception("请使用正确的zshp格式文件！");
            }

            // 3 获取点类型
            MyMapObjects.moGeometryTypeConstant sShapeType;
            if ((string)jsonObj["ShapeType"] == "1")
            {
                sShapeType = MyMapObjects.moGeometryTypeConstant.MultiPolyline;
            }
            else if ((string)jsonObj["ShapeType"] == "2")
            {
                sShapeType = MyMapObjects.moGeometryTypeConstant.MultiPolygon;
            }
            else if ((string)jsonObj["ShapeType"] == "0")
            {
                sShapeType = MyMapObjects.moGeometryTypeConstant.Point;
            }
            else
            {
                throw new Exception("不合法的zshp对象类型，请检查输入！");
            }

            // 4 获取一个Fields对象
            JArray sFieldTypes = (JArray)jsonObj["FieldTypes"];
            JArray sFieldNames = (JArray)jsonObj["FieldNames"];
            if (sFieldTypes.Count != sFieldNames.Count)
            {
                throw new Exception("字段名称和字段类型长度不相等！");
            }
            Int32 sFieldCount = sFieldTypes.Count;
            MyMapObjects.moFields sFields = new MyMapObjects.moFields();
            for (Int32 i = 0; i <= sFieldCount - 1; i++)
            {
                // 建立moField
                int sFieldTypeInt = (int)sFieldTypes[i];
                MyMapObjects.moField sField = new MyMapObjects.moField((string)sFieldNames[i], (MyMapObjects.moValueTypeConstant)sFieldTypeInt);
                sFields.Append(sField);
            }
            fields = sFields;

            // 5 获取一个Attributes的List对象，也就是一个“二维表”
            List<MyMapObjects.moAttributes> sAttributesList = new List<moAttributes>();
            JArray sAttributesListValue = (JArray)jsonObj["Attributes"];  // 这是一个二维表
            foreach (JArray sAttributeValue in sAttributesListValue)  // 取出每一行的值，这里已经包含了强制转换，将JToken转换为JArray
            {
                MyMapObjects.moAttributes sAttributes = new MyMapObjects.moAttributes();  // 新建一行
                for (Int32 i = 0; i <= sFieldCount - 1; i++)  // 在该行中遍历
                {
                    int sFieldTypeInt = (int)sFieldTypes[i];  // 获取该行中某列的变量类型
                    if (sFieldTypeInt == 0)  // Int16
                    {
                        Int16 sValue = Int16.Parse((string)sAttributeValue[i]);
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeInt == 1)  // Int32
                    {
                        Int32 sValue = Int32.Parse((string)sAttributeValue[i]);
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeInt == 2)  // Int64
                    {
                        Int64 sValue = Int64.Parse((string)sAttributeValue[i]);
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeInt == 3)  // Single
                    {
                        Single sValue = Single.Parse((string)sAttributeValue[i]);
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeInt == 4)  // Double
                    {
                        double sValue = double.Parse((string)sAttributeValue[i]);
                        sAttributes.Append(sValue);
                    }
                    else if (sFieldTypeInt == 5)  // Text
                    {
                        sAttributes.Append((string)sAttributeValue[i]);
                    }
                }
                sAttributesList.Add(sAttributes);
            }
            attributesList = sAttributesList;
        }
        #endregion
    }
}
