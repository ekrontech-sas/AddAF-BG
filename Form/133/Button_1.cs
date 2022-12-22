using B1Framework.B1Frame;
using B1Framework.RecordSet;
using SAPbouiCOM;

namespace bagant.Form._133
{
    class Button_1 : B1Item
    {
        public Button_1()
        {
            FormType = "133";
            ItemUID = "1";
        }

        [B1Listener(BoEventTypes.et_CLICK, true)]
        public virtual bool OnBeforeClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            var rngIni = ((EditText)Form.Items.Item("U_BGT_RngIni").Specific).Value;
            var rngFin = ((EditText)Form.Items.Item("U_BGT_RngFin").Specific).Value;

            if (string.IsNullOrEmpty(rngIni) || rngIni.Equals("0") || rngIni.Equals("0.0"))
            {
                B1Connections.SboApp.SetStatusBarMessage("No puede dejar el campo de rango inicial de factura vacio");
                return false;
            }

            if (string.IsNullOrEmpty(rngFin) || rngFin.Equals("0") || rngFin.Equals("0.0"))
            {
                B1Connections.SboApp.SetStatusBarMessage("No puede dejar el campo de rango final de factura vacio");
                return false;
            }

            return true;
        }

        //[B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        //public virtual bool OnBeforeItemPressed(ItemEvent pVal)
        //{
        //    SAPbouiCOM.Form oForm = B1Connections.SboApp.Forms.Item(pVal.FormType);
        //    oForm.Items.Item("U_BGT_ContAsoc").Enabled = false;
        //    return true;
        //}

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, true)]
        public virtual bool OnBeforeItemPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
                SAPbouiCOM.Form oForm = B1Connections.SboApp.Forms.Item(pVal.FormType);
                oForm.Items.Item("U_BGT_ContAsoc").Enabled = false;

            if (Form.Mode == BoFormMode.fm_ADD_MODE)
            {
                var query = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.CheckInvoiceRestrain.sql", GetType().Assembly), B1Connections.DiCompany.UserSignature);
                var resp = Record.Instance.Query(query).Execute().First();

                if (!resp["Cant"].Equals("0"))
                {
                    if (resp["Tipo"].Equals("1"))
                    {
                        if (B1Connections.SboApp.MessageBox("Ya fue facturado el documento para el periodo actual, desea refacturarla ?", 1, "Si", "No") == 1)
                            return true;
                        else
                            return false;
                    }
                    else if (resp["Tipo"].Equals("-1"))
                    {
                        B1Connections.SboApp.SetStatusBarMessage("Ya fue facturado el documento para el periodo actual.");
                        return false;
                    }
                }
            }

            return true;
        }


    }
}
