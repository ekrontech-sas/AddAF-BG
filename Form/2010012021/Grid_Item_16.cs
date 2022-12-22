using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._2010012021
{
    class Grid_Item_16 : B1Item
    {
        public Grid_Item_16()
        {
            FormType = "2010012021";
            ItemUID = "Item_16";
        }

        [B1Listener(BoEventTypes.et_MATRIX_LINK_PRESSED, true)]
        public virtual bool OnBeforeMatrixLinkPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var grid = (Grid)Form.Items.Item(pVal.ItemUID).Specific;

            if (pVal.ColUID == "Numero Devolucion")
            {
                var fila = grid.GetDataTableRowIndex(pVal.Row);
                var interno = (string)grid.DataTable.GetValue("Interno Devolucion", fila).ToString();
                B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_DeliveryNotesReturns, "", interno);
                SAPbouiCOM.Form DeliveryNotesReturns = B1Connections.SboApp.Forms.ActiveForm;
                DeliveryNotesReturns.Items.Item("U_BGT_FecFact").Enabled = false;
                DeliveryNotesReturns.Items.Item("U_BGT_ContAsoc").Enabled = false;
                DeliveryNotesReturns.Items.Item("U_NUM_GUIA").Enabled = false;
                DeliveryNotesReturns.Items.Item("U_BGT_DevProv").Enabled = false;
                return false;
            }

            return false;
        }

    }
}
