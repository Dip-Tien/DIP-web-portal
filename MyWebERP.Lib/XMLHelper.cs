using System.Xml;

namespace MyWebERP.Lib
{
    public class XMLHelper
    {
        public static XmlDocument ReadXmlFile(string p_sFilename)
        {
            string _sFilename = p_sFilename;
            XmlDocument _xmlDoc = new XmlDocument();

            try
            {
                _xmlDoc.Load(_sFilename);
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("CF.ReadXmlFile: {0}", _ex.Message), _ex);

            }

            return _xmlDoc;
        }

        public static string GetLabel(string p_sDefaultLabel, string p_sCultureCode, XmlDocument p_doc)
        {
            string _sResult = p_sDefaultLabel;
            string _sCultureCode = p_sCultureCode;
            XmlDocument _xmlDoc = p_doc;

            try
            {
                bool _blnHaveTranslated = false;

                string _sUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZĐÊẾỀỆỂỄĂẮẰẶẲẴÂẤẦẬẪẨIÍÌỊĨỈÔỐỒỘỖỔƠỚỜỢỠỞÙÚỤỦŨ";
                string _sLower = "abcdefghijklmnopqrstuvwxyzđêếềệểễăắằặẳẵâấầậẫẩiíìịĩỉôốồộỗổơớờợỡởùúụủũ";
                //string _strPath = @"hi2.Label/Label[@Name='" + p_sDefaultLabel + "']";
                string _strPath = @"hi2.Label/Label[translate(@Name, '" + _sUpper + "', '" + _sLower + "')='" + p_sDefaultLabel.ToLower() + "']";
                XmlNode _xmlNode = _xmlDoc.SelectSingleNode(_strPath);

                if (_xmlNode != null)
                {
                    XmlNode _nodeLang = _xmlNode.SelectSingleNode(_sCultureCode);
                    if (_nodeLang != null)
                    {
                        _sResult = _nodeLang.InnerText;
                        _blnHaveTranslated = true;
                    }
                }
                else if (p_sDefaultLabel.Trim().Contains(":"))
                {
                    string _sDefault = p_sDefaultLabel.Trim();

                    if (_sDefault.Substring(_sDefault.Length - 1) == ":")
                    {
                        //_strPath = @"hi2.Label/Label[@Name='" + _sDefault.Substring(0, _sDefault.Length - 1) + "']";
                        _strPath = @"hi2.Label/Label[translate(@Name, '" + _sUpper + "', '" + _sLower + "')='" + _sDefault.Substring(0, _sDefault.Length - 1).ToLower() + "']";
                        _xmlNode = _xmlDoc.SelectSingleNode(_strPath);

                        if (_xmlNode != null)
                        {
                            XmlNode _nodeLang = _xmlNode.SelectSingleNode(_sCultureCode);

                            if (_nodeLang != null)
                            {
                                _sResult = _nodeLang.InnerText;

                                if (_sResult.Trim().Length > 0 && _sResult.Trim().Substring(_sResult.Trim().Length - 1) != ":")
                                {
                                    _sResult += ":";
                                }

                                _blnHaveTranslated = true;
                            }
                        }
                    }
                }
                else if (p_sDefaultLabel.Trim().Contains("..."))
                {
                    string _sDefault = p_sDefaultLabel.Trim();

                    if (_sDefault.Substring(_sDefault.Length - 3) == "...")
                    {
                        _strPath = @"hi2.Label/Label[translate(@Name, '" + _sUpper + "', '" + _sLower + "')='" + _sDefault.Substring(0, _sDefault.Length - 3).ToLower() + "']";
                        _xmlNode = _xmlDoc.SelectSingleNode(_strPath);

                        if (_xmlNode != null)
                        {
                            XmlNode _nodeLang = _xmlNode.SelectSingleNode(_sCultureCode);

                            if (_nodeLang != null)
                            {
                                _sResult = _nodeLang.InnerText;

                                if (_sResult.Trim().Length > 3 && _sResult.Trim().Substring(_sResult.Trim().Length - 3) != "...")
                                {
                                    _sResult += "...";
                                }

                                _blnHaveTranslated = true;
                            }
                        }
                    }
                }

                if (_blnHaveTranslated)
                {
                    // Xử lý upper
                    bool _blnAllUpper = true;

                    string _sOld = p_sDefaultLabel;
                    for (int i = 0; i < _sOld.Length; i++)
                    {
                        if (char.IsLetter(_sOld[i]) && !char.IsUpper(_sOld[i]))
                        {
                            _blnAllUpper = false;
                            break;
                        }
                    }

                    if (_blnAllUpper)
                    {
                        _sResult = _sResult.ToUpper();
                    }
                }
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("GetLabel: ", _ex.Message), _ex);
            }

            if (string.IsNullOrEmpty(_sResult))
            {
                _sResult = p_sDefaultLabel;
            }

            return _sResult;
        }

        public static List<string> GetLabels(string[] p_sDefaultLabels, string p_sCultureCode, string p_sFilename)
        {
            List<string> _lstResult = new List<string>();
            string _sCultureCode = p_sCultureCode;
            string[] _sDefaultLabels = p_sDefaultLabels;

            if (string.IsNullOrEmpty(_sCultureCode))
            {
                _sCultureCode = "vi-VN";
            }

            try
            {
                //bool _blnHaveTranslated = false;
                XmlDocument _xmlDoc = ReadXmlFile(p_sFilename);

                if (_xmlDoc != null)
                {
                    foreach (string _sDefaultValue in p_sDefaultLabels)
                    {
                        string _sResult = GetLabel(_sDefaultValue, _sCultureCode, _xmlDoc);
                        _lstResult.Add(_sResult);
                    }
                }
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("GetLabels: ", _ex.Message), _ex);
            }

            return _lstResult;
        }
        public static List<string> GetLabels0(string[] p_sTexts, string p_sLanguageCode)
        {
            string _sFilename = Path.Combine(AppContext.BaseDirectory, "xmldata", "label.xml");
            string _sLanguageCode = p_sLanguageCode;

            return GetLabels(p_sTexts, _sLanguageCode, _sFilename);
        }

        public static string GetLabel(string p_sDefaultLabel, string p_sCultureCode, string p_sFilename)
        {
            List<string> _lstResult = new List<string>();
            string _sCultureCode = p_sCultureCode;
            string _sDefaultLabel = p_sDefaultLabel;

            if (string.IsNullOrEmpty(_sCultureCode))
            {
                _sCultureCode = "vi-VN";
            }

            try
            {
                XmlDocument _xmlDoc = ReadXmlFile(p_sFilename);

                if (_xmlDoc != null)
                {
                    return GetLabel(_sDefaultLabel, _sCultureCode, _xmlDoc);
                }
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("GetLabel: ", _ex.Message), _ex);
            }

            return _sDefaultLabel;
        }

        public static string GetLabel0(string p_sText, string p_sLanguageCode)
        {
            string _sFilename = Path.Combine(AppContext.BaseDirectory, "xmldata", "label.xml");
            string _sLanguageCode = p_sLanguageCode;

            return GetLabel(p_sText, _sLanguageCode, _sFilename);
        }
    }
}
