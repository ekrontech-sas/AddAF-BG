using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System.Xml;

namespace bagant.Form._940
{
    class Form_Data : B1Form
    {
        public Form_Data()
        {
            FormType = "940";
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnAfterFormDataAdd(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (pVal.ActionSuccess)
            {
                var xml = new XmlDocument();
                    xml.LoadXml(pVal.ObjectKey);

                new SAPServices().AddMovementData(xml.InnerText, "OWTR", "WTR1", "67");
            }
        }

    }
}
