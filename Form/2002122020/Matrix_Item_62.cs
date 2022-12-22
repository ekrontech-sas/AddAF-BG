using B1Framework.B1Frame;
using B1Framework.RecordSet;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Form._2002122020
{
    class Matrix_Item_62 : B1Item
    {
        public Matrix_Item_62()
        {
            FormType = "2002122020";
            ItemUID = "Item_62";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnBeforeChooseFromList(ItemEvent pVal)
        {
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            Form = new B1Forms(pVal.FormUID);
            if ( pVal.ColUID == "Col_0" )
            {
                Log.Info("(OnBeforeChooseFromList) 26");
                var matrix = (SAPbouiCOM.Matrix)Form.Items.Item(pVal.ItemUID).Specific;

                var itemCode = ListChoiceListener(pVal, "ItemCode")[0].ToString();
                var itemName = ListChoiceListener(pVal, "ItemName")[0].ToString();
                var kilos = ListChoiceListener(pVal, "SWeight1")[0].ToString();
                var codPrec = Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("U_BGT_PriceList", 0);

                var queryPrecio = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetPriceContract.sql"), itemCode, codPrec);
                var respPrecio = Record.Instance.Query(queryPrecio).Execute().First();

                matrix.SetCellWithoutValidation(pVal.Row, "Col_0", itemCode);
                matrix.SetCellWithoutValidation(pVal.Row, "Col_1", itemName);
                matrix.SetCellWithoutValidation(pVal.Row, "Col_2", respPrecio["Price"]);
                matrix.SetCellWithoutValidation(pVal.Row, "Col_14", respPrecio["Price"]);
                matrix.SetCellWithoutValidation(pVal.Row, "Col_15", respPrecio["PriceVenta"]);
                matrix.SetCellWithoutValidation(pVal.Row, "Col_16", respPrecio["PriceVenta"]);
                matrix.SetCellWithoutValidation(pVal.Row, "Col_7", kilos);
                matrix.AutoResizeColumns();
               // matrix.AddRow();

                var table = (SAPbouiCOM.Matrix)Form.Items.Item("Item_62").Specific;
                var bdlevel = 1;
                Form.DataSources.DBDataSources.Item(bdlevel).InsertRecord(Form.DataSources.DBDataSources.Item(bdlevel).Size);
                Form.DataSources.DBDataSources.Item(bdlevel).Offset = Form.DataSources.DBDataSources.Item(bdlevel).Size - 1;
                //table.AddRow(1);
                for (int i = 1; i <= table.RowCount; i++)
                    table.SetCellWithoutValidation(table.RowCount, "#", i.ToString());

                new CommonTaskSapUI().SetTableSummatoryTotal(Form);
            }
        }

        [B1Listener(BoEventTypes.et_VALIDATE, false)]
        public virtual void OnAfterValidate(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var matrix = (SAPbouiCOM.Matrix)Form.Items.Item(pVal.ItemUID).Specific;

            if ( pVal.ColUID == "Col_3")
            {
                var cantidadOriginal = double.Parse(((EditText)matrix.GetCellSpecific("Col_4", pVal.Row)).Value);
                var cantidadModificar = double.Parse(((EditText)matrix.GetCellSpecific("Col_3", pVal.Row)).Value);
                var cantEntregado = double.Parse(((EditText)matrix.GetCellSpecific("Col_9", pVal.Row)).Value);
                var precio = double.Parse(((EditText)matrix.GetCellSpecific("Col_2", pVal.Row)).Value);

                matrix.SetCellWithoutValidation(pVal.Row, "Col_4", (cantidadOriginal + cantidadModificar).ToString());
                matrix.SetCellWithoutValidation(pVal.Row, "Col_6", ((cantidadOriginal + cantidadModificar) - cantEntregado).ToString());
                matrix.SetCellWithoutValidation(pVal.Row, "Col_11", (precio * ((cantidadOriginal + cantidadModificar))).ToString());
                matrix.SetCellWithoutValidation(pVal.Row, "Col_13", (precio * ((cantidadOriginal + cantidadModificar) - cantEntregado)).ToString());
                matrix.SetCellWithoutValidation(pVal.Row, "Col_3", "0");

                new CommonTaskSapUI().SetTableSummatoryTotal(Form);
            }
            else if( pVal.ColUID == "Col_2")
            {
                var cantidad = double.Parse(((EditText)matrix.GetCellSpecific("Col_4", pVal.Row)).Value);
                var precio = double.Parse(((EditText)matrix.GetCellSpecific("Col_2", pVal.Row)).Value);

                matrix.SetCellWithoutValidation(pVal.Row, "Col_11", (precio * cantidad).ToString());
                matrix.SetCellWithoutValidation(pVal.Row, "Col_13", (precio * cantidad).ToString());
                //matrix.SetCellWithoutValidation(pVal.Row, "Col_14", (precio).ToString());

                new CommonTaskSapUI().SetTableSummatoryTotal(Form);
            }
            else if (pVal.ColUID == "Col_8")
            {
                try
                {
                    var dscto = double.Parse(((EditText)matrix.GetCellSpecific("Col_8", pVal.Row)).Value);
                    var cantidad = double.Parse(((EditText)matrix.GetCellSpecific("Col_4", pVal.Row)).Value);
                    var precio = double.Parse(((EditText)matrix.GetCellSpecific("Col_14", pVal.Row)).Value);
                    var precioDcto = precio - ((precio * dscto) / 100);

                    Log.Info($"dscto {dscto} - cantidad {cantidad} - precio {precio} - precioDcto {precioDcto}");

                    matrix.SetCellWithoutValidation(pVal.Row, "Col_2", precioDcto.ToString());
                    matrix.SetCellWithoutValidation(pVal.Row, "Col_11", (precioDcto * cantidad).ToString());
                    matrix.SetCellWithoutValidation(pVal.Row, "Col_13", (precioDcto * cantidad).ToString());

                    new CommonTaskSapUI().SetTableSummatoryTotal(Form);
                }
                catch (Exception ex)
                {
                    Log.Info("OnAfterValidate => " + ex.Message);
                }
            }
            //if (pVal.ColUID == "Col_3")
            //{
            //    matrix.AddRow();
            //    matrix.SelectRow(matrix.GetNextSelectedRow(), true, false);
            //    //matrix.DeleteRow(matrix.GetNextSelectedRow()-1);
            //}
        }

    }
}
