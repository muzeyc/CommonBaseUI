//using System;
//using System.Drawing;
//using System.Windows.Media;

//namespace CommonBaseUI.CommUtil
//{
//    public static class CommonUtil
//    {
//        public static string ToStr(this object v)
//        {
//            if (v is System.DBNull || v == null)
//            {
//                return string.Empty;
//            }
//            return Convert.ToString(v);
//        }

//        public static string ToStr(this object v, string reStr)
//        {
//            if (v is System.DBNull || v == null || v.Equals(""))
//            {
//                return reStr;
//            }
//            return Convert.ToString(v);
//        }

//        public static decimal ToDec(this object v)
//        {
//            if (v is System.DBNull || v == null)
//            {
//                return 0;
//            }
//            else if (Convert.ToString(v) == "")
//            {
//                return 0;
//            }
//            else
//            {
//                decimal outPut = 0;
//                bool result = decimal.TryParse(v.ToStr(), out outPut);
//                return result ? outPut : decimal.MinValue; ;
//            }
//        }

//        public static double ToDouble(this object v)
//        {
//            if (v is System.DBNull || v == null)
//            {
//                return 0;
//            }
//            else if (Convert.ToString(v) == "")
//            {
//                return 0;
//            }
//            else
//            {
//                double outPut = 0;
//                bool result = double.TryParse(v.ToStr(), out outPut);
//                return result ? outPut : double.MinValue; ;
//            }
//        }

//        public static long ToLong(this object v)
//        {
//            if (v is System.DBNull || v == null)
//            {
//                return 0;
//            }
//            else if (Convert.ToString(v) == "")
//            {
//                return 0;
//            }
//            else
//            {
//                long outPut = 0;
//                bool result = long.TryParse(v.ToStr(), out outPut);
//                return result ? outPut : long.MinValue; ;
//            }
//        }

//        public static int ToInt(this object v)
//        {
//            if (v is System.DBNull || v == null)
//            {
//                return 0;
//            }
//            else if (Convert.ToString(v) == "")
//            {
//                return 0;
//            }
//            else
//            {
//                int outPut = 0;
//                bool result = int.TryParse(v.ToStr(), out outPut);
//                return result ? outPut : int.MinValue; ;
//            }
//        }

//        public static bool Bool(this object v)
//        {
//            string s = ToStr(v).ToLower();
//            if (s == "true" || s == "1")
//            {
//                return true;
//            }
//            else if (s == "false" || s == "0")
//            {
//                return false;
//            }

//            return false;
//        }

//        public static bool IsNullOrEmpty(this string value)
//        {
//            if (value == null)
//            {
//                return true;
//            }

//            return string.IsNullOrWhiteSpace(value);
//        }

//        /// <summary>
//        /// 日期转换
//        /// </summary>
//        /// <param name="v"></param>
//        /// <returns></returns>
//        public static DateTime ToDateTime(this object v)
//        {
//            DateTime outDateTime;
//            if (DateTime.TryParse(CommonUtil.ToStr(v), out outDateTime))
//            {
//                return outDateTime;
//            }

//            return DateTime.MinValue;
//        }

//        /// <summary>
//        /// 16进制转画刷
//        /// </summary>
//        /// <param name="color16x"></param>
//        /// <returns></returns>
//        public static SolidColorBrush ToBrush(string color16x)
//        {
//            var color = ColorTranslator.FromHtml(color16x);
//            return new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
//        }

//        public static System.Windows.Media.Brush From16JinZhi(string color)
//        {
//            BrushConverter converter = new BrushConverter();
//            return (System.Windows.Media.Brush)converter.ConvertFromString(color);
//        }

//        public static System.Windows.Media.Color ToColor(string colorName)
//        {
//            if (colorName.StartsWith("#"))
//                colorName = colorName.Replace("#", string.Empty);
//            int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
//            return new System.Windows.Media.Color()
//            {
//                A = Convert.ToByte((v >> 24) & 255),
//                R = Convert.ToByte((v >> 16) & 255),
//                G = Convert.ToByte((v >> 8) & 255),
//                B = Convert.ToByte((v >> 0) & 255)
//            };
//        }
//    }
//}
