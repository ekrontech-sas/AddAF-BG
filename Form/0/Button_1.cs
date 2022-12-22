using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System.Xml;

namespace bagant.Form._0
{
    class Button_1 : B1Item
    {
        public static string RngIni = "";
        public static string RngFin = "";

        public Button_1()
        {
            FormType = "0";
            ItemUID = "1";
        }

        [B1Listener(BoEventTypes.et_CLICK, false)]
        public virtual void OnAfterClick(ItemEvent pVal)
        {
            var frm = B1Connections.SboApp.Forms.Item(pVal.FormUID);
            if (_2002122020.Button_Item_99.ProvieneContrato)
            {
                RngIni = ((EditText)Form.Items.Item("txtFecIn").Specific).Value;
                RngFin = ((EditText)Form.Items.Item("txtFecFin").Specific).Value;
            }
            return;
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnBeforeFormDataAdd(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (pVal.ActionSuccess)
            {
                var xml = new XmlDocument();
                xml.LoadXml(pVal.ObjectKey);

                var sapServ = new SAPServices();

                if (pVal.Type == "15")
                {
                    sapServ.AddMovementData(xml.InnerText, "ODLN", "DLN1", "15");
                    sapServ.CheckUDOContract(xml.InnerText, "ODLN", "15");
                }
                else if (pVal.Type == "112")
                {
                    sapServ.AddMovementData(xml.InnerText, "ODRF", "DRF1", "112");
                    sapServ.CheckUDOContract(xml.InnerText, "ODRF", "112");
                }

            }
            return;

        }

    }
}
