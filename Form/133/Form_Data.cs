using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System.Xml;

namespace bagant.Form._133
{
    class Form_Data : B1Form
    {
        public Form_Data()
        {
            FormType = "133";
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnAfterFormDataAdd(BusinessObjectInfo pVal)
        {
            if (pVal.ActionSuccess)
            {
                var xml = new XmlDocument();
                xml.LoadXml(pVal.ObjectKey);

                new SAPServices().AddMovementData(xml.InnerText, "OINV", "INV1", "13");
                new SAPServices().AddInvoiceMovement(xml.InnerText);
                new SAPServices().AddInvoiceData(xml.InnerText);
            }
        }

        //[B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        //public virtual bool OnBeforeItemPressed(ItemEvent pVal)
        //{
        //    SAPbouiCOM.Form oForm = B1Connections.SboApp.Forms.Item(pVal.FormType);
        //    oForm.Items.Item("U_BGT_ContAsoc").Enabled = false;
        //    return true;
        //}



    }
}
