using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;
using System.Threading;
using static bagant.Program;

namespace bagant.Form._3002
{
    class Grid_Item_3 : B1Item
    {
        public Grid_Item_3()
        {
            FormType = "3002";
            ItemUID = "Item_3";
        }

        [B1Listener(BoEventTypes.et_VALIDATE, true)]
        public virtual bool OnBeforeValidate(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (pVal.ColUID == "A Entregar")
            {
                var grid = (Grid)Form.Items.Item(ItemUID).Specific;
                var row = grid.GetDataTableRowIndex(pVal.Row);
                if (row != -1)
                {
                    var cantEntr = (double)grid.DataTable.GetValue("A Entregar", row);
                    var stockPend = (double)grid.DataTable.GetValue("Stock Pendiente Contrato", row);
                    var item = (string)grid.DataTable.GetValue("Codigo Articulo", row).ToString();

                    //if ( item.Contains("LOG") )
                    //   return true;

                    // if (item.Contains("AEA"))
                    // {
                    if (cantEntr > stockPend)
                    {
                        B1Connections.SboApp.SetStatusBarMessage("La cantidad a entregar no puede ser mayor a la cantidad pendiente");
                        return false;
                    }
                    // }
                }


            }
            return true;
        }


        //[B1Listener(BoEventTypes.et_DOUBLE_CLICK, false)]
        //public virtual void OnAfterDoubleClick(ItemEvent pVal)
        //{
        //    Form = new B1Forms(pVal.FormUID);
        //    var grid = (Grid)Form.Items.Item(pVal.ItemUID).Specific;
        //    var opt = (string)grid.DataTable.GetValue("Opt", 1);

        //    if (pVal.ColUID == "Opt")
        //    {
        //        if (pVal.Row == -1)
        //        {
        //            try
        //            {
        //                Form.Freeze(true);
        //                for (int i = 0; i < grid.DataTable.Rows.Count; i++)
        //                    grid.DataTable.SetValue("Opt", i, opt.Equals("Y") ? "N" : "Y");
        //            }
        //            catch (Exception ex)
        //            {
        //                Log.Info("Exception ex => " + ex.Message);
        //            }
        //            finally
        //            {
        //                Form.Freeze(false);
        //            }
        //        }
        //    }
        //}

        //[B1Listener(BoEventTypes.et_MATRIX_LINK_PRESSED, true)]
        //public virtual bool OnBeforeMatrixLinkPressed(ItemEvent pVal)
        //{
        //    Form = new B1Forms(pVal.FormUID);
        //    var grid = (Grid)Form.Items.Item(pVal.ItemUID).Specific;

        //    if (pVal.ColUID == "Numero Contrato")
        //    {
        //        var row = grid.GetDataTableRowIndex(pVal.Row);
        //        if (row == -1)
        //        {
        //            var row2 = grid.GetDataTableRowIndex(pVal.Row + 2);
        //            var cont = grid.DataTable.GetValue("Numero Contrato", row2).ToString();

        //            CommonTaskSapUI.OpenContractForm(cont);
        //            return false;
        //        }
        //    }
        //    else if (pVal.ColUID == "Nombre Cliente")
        //    {
        //        var row = grid.GetDataTableRowIndex(pVal.Row);
        //        if (row == -1)
        //        {
        //            var row2 = grid.GetDataTableRowIndex(pVal.Row + 1);
        //            var cliente = (string)grid.DataTable.GetValue("Nombre Cliente", row2).ToString();

        //            B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_BusinessPartner, "", cliente.Split('-')[0]);
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, true)]
        public virtual bool OnBeforeItemPressed(ItemEvent pVal)
        {
            var pDato = true;
            try
            {
                Form = new B1Forms(pVal.FormUID);
                if (pVal.FormTypeEx == "3002" && pVal.BeforeAction && pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED && pVal.ItemUID == "Item_3" )//&& pVal.ColUID == "Opt")
                {
                    var oGrid = (Grid)Form.Items.Item("Item_3").Specific;
                    int newColor = oGrid.DataTable.GetValue("U_NUM_GUIA", oGrid.GetDataTableRowIndex(pVal.Row)).ToString() == "Y" ? 0 : 8421504; 
                    oGrid.CommonSetting.SetRowFontColor(pVal.Row + 1, newColor);
                }
            }
            catch (System.Exception ex)
            {
                Log.Info("Excp (SboApp_ItemEvent) => " + ex.Message);
            }
            return pDato;
        }


    }
}
