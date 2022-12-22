using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._2002122020
{
    class Grid_Item_15 : B1Item
    {
        public Grid_Item_15()
        {
            FormType = "2002122020";
            ItemUID = "Item_15";
        }

        [B1Listener(BoEventTypes.et_MATRIX_LINK_PRESSED, true)]
        public virtual bool OnBeforeChooseFromList(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var grid = ((Grid)Form.Items.Item(ItemUID).Specific);

            if (pVal.ColUID == "Numero Factura/NC")
            {
                var tipo = (string)grid.DataTable.GetValue("Tipo de Documento", pVal.Row).ToString();
                var interno = (string)grid.DataTable.GetValue("Numero Interno", pVal.Row).ToString();

                if (tipo.Equals("Factura"))
                    B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_Invoice, "", interno);
                else if (tipo.Equals("NC"))
                    B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_InvoiceCreditMemo, "", interno);

                return false;
            }
            
            return true;
        }

    }
}
