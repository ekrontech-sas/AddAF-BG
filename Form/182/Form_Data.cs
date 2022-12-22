using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System.Xml;

namespace bagant.Form._182
{
    class Form_Data : B1Form
    {
        public Form_Data()
        {
            FormType = "182";
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnAfterFormDataAdd(BusinessObjectInfo pVal)
        {
            if (pVal.ActionSuccess)
            {
                var xml = new XmlDocument();
                xml.LoadXml(pVal.ObjectKey);

                var sapServ = new SAPServices();
                    sapServ.AddMovementData(xml.InnerText, "ORPD", "RPD1", "21");
                    sapServ.CreateDevolutionCapitalizationAsset("ORPD", "RPD1", int.Parse(xml.InnerText));
            }
        }
    }
}
