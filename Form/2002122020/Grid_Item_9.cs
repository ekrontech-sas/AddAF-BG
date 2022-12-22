using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Form._2002122020
{
    class Grid_Item_9 : B1Item
    {
        public Grid_Item_9()
        {
            FormType = "2002122020";
            ItemUID = "Item_9";
        }

        [B1Listener(BoEventTypes.et_MATRIX_LINK_PRESSED, true)]
        public virtual bool OnBeforeChooseFromList(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            if (pVal.ColUID == "Numero Contrato")
            {
                try
                {
                    var numCont = ((Grid)Form.Items.Item(ItemUID).Specific).DataTable.GetValue("Numero Contrato", pVal.Row).ToString();
                    CommonTaskSapUI.OpenContractForm(numCont);
                }
                catch(Exception ex)
                {
                    Log.Info("(OnBeforeChooseFromList) => " + ex.Message);
                }

                return false;
            }
            else if (pVal.ColUID == "Num. Oferta")
            {
                var numInt = ((Grid)Form.Items.Item(ItemUID).Specific).DataTable.GetValue("Num. Interno Oferta", pVal.Row-1);
                B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_Quotation, "", numInt);
                return false;
            }
            else if (pVal.ColUID == "Nombre Socio de Nego")
            {

            }

            return true;
        }

    }
}
