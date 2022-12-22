using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Form._2003112020
{
    class Matrix_Item_49 : B1Item
    {
        public Matrix_Item_49()
        {
            FormType = "2003112020";
            ItemUID = "Item_49";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnBeforeChooseFromList(ItemEvent pVal)
        {
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            Form = new B1Forms(pVal.FormUID);
            if (pVal.ColUID == "Col_0" )
            {
                var codigo = ListChoiceListener(pVal, "WhsCode")[0].ToString();
                var nombre = ListChoiceListener(pVal, "WhsName")[0].ToString();
                var matrix = (SAPbouiCOM.Matrix)Form.Items.Item(ItemUID).Specific;

                try
                {
                    Form.DataSources.DBDataSources.Item("@BGT_PAR4").SetValue("U_BGT_Warehouse", pVal.Row, codigo);
                    Form.DataSources.DBDataSources.Item("@BGT_PAR4").SetValue("U_BGT_WarehouseName", pVal.Row, nombre);

                    matrix.SetCellWithoutValidation(pVal.Row, "Col_0", codigo);
                    matrix.SetCellWithoutValidation(pVal.Row, "Col_1", nombre);
                    matrix.AutoResizeColumns();
                }
                catch(Exception ex)
                {
                    Log.Info("Exception => " + ex.Message);
                }
            }
        }

    }
}
