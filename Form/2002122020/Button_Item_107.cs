using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Form._2002122020
{
    class Button_Item_107 : B1Item
    {
        public Button_Item_107()
        {
            FormType = "2002122020";
            ItemUID = "Item_107";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnBeforeClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            if (B1Connections.SboApp.MessageBox("Desea cargar los items de transporte ?", 1, "Si", "No") == 1)
            {
                new CommonTaskSapUI().SetItemsContract(Form);

                if( Form.Mode == BoFormMode.fm_OK_MODE ) 
                    Form.Mode = BoFormMode.fm_UPDATE_MODE;
            }
        }

    }
}
