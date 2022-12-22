using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Form._2002122020
{
    class Button_Item_99 : B1Item
    {
        public static bool ProvieneContrato = false;

        public Button_Item_99()
        {
            FormType = "2002122020";
            ItemUID = "Item_99";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnBeforeClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            ProvieneContrato = true;

            if (B1Connections.SboApp.MessageBox("Prueba ", 1, "Ok") == 1)
            {
                ProvieneContrato = false;

                new CommonTaskSapUI().CreateInvoiceForm(new B1Forms(pVal.FormUID), _0.Button_1.RngIni, _0.Button_1.RngFin);
            }

            ProvieneContrato = false;
        }

    }
}
