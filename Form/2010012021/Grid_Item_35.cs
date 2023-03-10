using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Form._2010012021
{
    class Grid_Item_35 : B1Item
    {
        public Grid_Item_35()
        {
            FormType = "2010012021";
            ItemUID = "Item_35";
        }

        [B1Listener(BoEventTypes.et_VALIDATE, true)]
        public virtual bool OnBeforeValidate(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (pVal.ColUID == "Cantidad a Devolver")
            {
                var grid = (Grid)Form.Items.Item(ItemUID).Specific;
                var row = grid.GetDataTableRowIndex(pVal.Row);
                if (row != -1)
                {
                    var cantObra = (double)grid.DataTable.GetValue("Cantidad en Obra", row);
                    var cantMod = (double)grid.DataTable.GetValue("Cantidad a Devolver", row);
                    var item = (string)grid.DataTable.GetValue("Codigo Articulo", row).ToString();

                    //if (cantMod == 0)
                    //    return true;

                    //if (item.Contains("AEA"))
                    //{
                    if (cantMod > cantObra)
                    {
                        B1Connections.SboApp.SetStatusBarMessage("La cantidad no puede ser mayor a la cantidad de obra");
                        return false;
                    }
                    else
                    {
                        if (cantMod == 0)
                        {
                            B1Connections.SboApp.SetStatusBarMessage("La cantidad a devolver no puede ser 0.");
                            return false;
                        }
                    }
                    //}
                }
            }

            return true;
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            var grid = (Grid)Form.Items.Item(pVal.ItemUID).Specific;
            var row = grid.GetDataTableRowIndex(pVal.Row);
            if (row != -1)
            {
                if (pVal.ColUID == "Bodega Devolucion Especial 1")
                    grid.DataTable.SetValue(pVal.ColUID, row, ListChoiceListener(pVal, "WhsCode")[0].ToString());
                else if (pVal.ColUID == "Bodega Devolucion Especial 2")
                    grid.DataTable.SetValue(pVal.ColUID, row, ListChoiceListener(pVal, "WhsCode")[0].ToString());
            }
        }

        [B1Listener(BoEventTypes.et_DOUBLE_CLICK, false)]
        public virtual void OnAfterDoubleClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var grid = (Grid)Form.Items.Item(pVal.ItemUID).Specific;
            var opt = (string)grid.DataTable.GetValue("Opt", 1);

            if (pVal.ColUID == "Opt")
            {
                if (pVal.Row == -1)
                {
                    try
                    {
                        Form.Freeze(true);
                        for (int i = 0; i < grid.DataTable.Rows.Count; i++)
                            grid.DataTable.SetValue("Opt", i, opt.Equals("Y") ? "N" : "Y");
                    }
                    catch (Exception ex)
                    {
                        Log.Info("Exception ex => " + ex.Message);
                    }
                    finally
                    {
                        Form.Freeze(false);
                    }
                }
            }
        }

        [B1Listener(BoEventTypes.et_MATRIX_LINK_PRESSED, true)]
        public virtual bool OnBeforeMatrixLinkPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var grid = (Grid)Form.Items.Item(pVal.ItemUID).Specific;

            if (pVal.ColUID == "Numero Contrato")
            {
                var row = grid.GetDataTableRowIndex(pVal.Row);
                if (row == -1)
                {
                    var row2 = grid.GetDataTableRowIndex(pVal.Row + 2);
                    var cont = grid.DataTable.GetValue("Numero Contrato", row2).ToString();

                    CommonTaskSapUI.OpenContractForm(cont);
                }

                return false;
            }
            else if (pVal.ColUID == "Cliente")
            {
                var row = grid.GetDataTableRowIndex(pVal.Row);
                if (row == -1)
                {
                    var row2 = grid.GetDataTableRowIndex(pVal.Row + 1);
                    var cliente = (string)grid.DataTable.GetValue("Cliente", row2).ToString();
                    B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_BusinessPartner, "", cliente.Split('-')[0]);
                }

                return false;
            }

            return true;
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, true)]
        public virtual bool OnBeforeItemPressed(ItemEvent pVal)
        {
            var pDato = true;
            try
            {
                Form = new B1Forms(pVal.FormUID);
                if (pVal.FormTypeEx == "2010012021" && pVal.BeforeAction && pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED && pVal.ItemUID == "Item_35" && pVal.ColUID == "Opt")
                {
                    var oGrid = (Grid)Form.Items.Item("Item_35").Specific;
                    int newColor = oGrid.DataTable.GetValue("Opt", oGrid.GetDataTableRowIndex(pVal.Row)).ToString() == "Y" ? 0 : 8421504; //8421504 Colors are given as integer, 
                    oGrid.CommonSetting.SetRowFontColor(pVal.Row + 1, newColor);//row numbering in commonsettings is different
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
