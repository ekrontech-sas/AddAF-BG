using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Form._2002122020
{
    class Grid_Item_101 : B1Item
    {
        public Grid_Item_101()
        {
            FormType = "2002122020";
            ItemUID = "Item_101";
        }

        [B1Listener(BoEventTypes.et_MATRIX_LINK_PRESSED, true)]
        public virtual bool OnBeforeChooseFromList(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var grid = ((Grid)Form.Items.Item(ItemUID).Specific);

            if (pVal.ColUID == "Numero Documento")
            {
                var row = grid.GetDataTableRowIndex(pVal.Row);
                if (row != -1) return true;

                row = grid.GetDataTableRowIndex(pVal.Row+1);

                var getType = grid.DataTable.GetValue("Type", row).ToString();
                var docEntry = grid.DataTable.GetValue("Numero Interno", row).ToString();

                Log.Info($"Type => {getType}");
                switch (getType)
                {
                    case "15":
                        B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_DeliveryNotes, "", docEntry);
                        //B1Connections.SboApp.Menus.Item("2051").Activate();
                        SAPbouiCOM.Form DeliveryNotes = B1Connections.SboApp.Forms.ActiveForm;

                        //((EditText)DeliveryNotes.Items.Item("8").Specific).Value = docEntry;
                        DeliveryNotes.Items.Item("U_BGT_FecFact").Enabled = false;
                        DeliveryNotes.Items.Item("U_BGT_ContAsoc").Enabled = false;
                        DeliveryNotes.Items.Item("U_NUM_GUIA").Enabled = false;
                        DeliveryNotes.Items.Item("U_BGT_DevProv").Enabled = false;
                        break;
                    case "13":
                        B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_Invoice, "", docEntry);
                        break;
                    case "14":
                        B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_InvoiceCreditMemo, "", docEntry);
                        break;
                    case "16":
                        B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_DeliveryNotesReturns, "", docEntry);
                        SAPbouiCOM.Form DeliveryNotesReturns = B1Connections.SboApp.Forms.ActiveForm;
                        DeliveryNotesReturns.Items.Item("U_BGT_FecFact").Enabled = false;
                        DeliveryNotesReturns.Items.Item("U_BGT_ContAsoc").Enabled = false;
                        DeliveryNotesReturns.Items.Item("U_NUM_GUIA").Enabled = false;
                        DeliveryNotesReturns.Items.Item("U_BGT_DevProv").Enabled = false;
                        break;
                    case "18":
                        B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_PurchaseInvoice, "", docEntry);
                        break;
                    case "19":
                        B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_PurchaseInvoiceCreditMemo, "", docEntry);
                        break;
                    case "20":
                        B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_GoodsReceiptPO, "", docEntry);
                        break;
                    case "21":
                        B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_GoodsReturns, "", docEntry);
                        break;
                    case "60":
                        B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_GoodsIssue, "", docEntry);
                        break;
                    case "59":
                        B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_GoodsReceipt, "", docEntry);
                        break;
                    case "67":
                        B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_StockTransfers, "", docEntry);
                        break;
                    case "112":
                        B1Connections.SboApp.OpenForm((BoFormObjectEnum)112, "", docEntry);
                        break;
                    case "116":
                        B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_DeliveryNotesReturns, "", docEntry);
                        break;
                }

                return true;
            }
            else if (pVal.ColUID == "Articulo")
            {
                var valueArt = ((string)((Grid)Form.Items.Item(ItemUID).Specific).DataTable.GetValue(pVal.ColUID, pVal.Row)).Split('-')[0].Trim();
                B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_Items, "", valueArt);

                return false;
            }


                return true;
        }
    }
}
