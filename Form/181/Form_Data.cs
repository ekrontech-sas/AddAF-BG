using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System.Xml;

namespace bagant.Form._181
{
    class Form_Data : B1Form
    {
        public Form_Data()
        {
            FormType = "181";
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnAfterFormDataAdd(BusinessObjectInfo pVal)
        {
            if (pVal.ActionSuccess)
            {
                var xml = new XmlDocument();
                xml.LoadXml(pVal.ObjectKey);

                var sapServ = new SAPServices();
                    sapServ.AddMovementData(xml.InnerText, "ORPC", "RPC1", "19");
                    sapServ.CreateDevolutionCapitalizationAsset("ORPC", "RPC1", int.Parse(xml.InnerText));
            }
        }
    }
}
