using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Form._2010012021
{
    class Matrix_Item_15 : B1Item
    {
        public Matrix_Item_15()
        {
            FormType = "2010012021";
            ItemUID = "Item_15";
        }

        [B1Listener(BoEventTypes.et_VALIDATE, true)]
        public virtual bool OnBeforeValidate(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            if( pVal.ColUID == "Col_9" )
            {
                //if (pVal.ItemChanged)
                //{
                    var matrix = (SAPbouiCOM.Matrix)Form.Items.Item(ItemUID).Specific;
                    var cantObra = double.Parse(((EditText)matrix.GetCellSpecific("Col_10", pVal.Row)).Value);
                    var cantMod = double.Parse(((EditText)matrix.GetCellSpecific("Col_9", pVal.Row)).Value);

                    if (cantMod > cantObra)
                    {
                        B1Connections.SboApp.SetStatusBarMessage("La cantidad no puede ser mayor a la cantidad de obra");
                        return false;
                    }
                //}
            }

            return true;
        }

        [B1Listener(BoEventTypes.et_MATRIX_LINK_PRESSED, true)]
        public virtual bool OnBeforeMatrixLinkPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var matrix = (SAPbouiCOM.Matrix)Form.Items.Item(ItemUID).Specific;

            if ( pVal.ColUID == "Col_1")
            {
                var nroContrato = ((EditText)matrix.GetCellSpecific("Col_2", pVal.Row)).Value;
                B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_UserDefinedObject, "AGT_CTR", nroContrato);
                return false;
            }
            else if (pVal.ColUID == "Col_3")
            {
                var cardSN = ((EditText)matrix.GetCellSpecific("Col_3", pVal.Row)).Value.Split('-');
                B1Connections.SboApp.OpenForm(BoFormObjectEnum.fo_BusinessPartner, "", cardSN[0].Trim());
                return false;
            }

            return true;
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            var matrix = (SAPbouiCOM.Matrix)Form.Items.Item(ItemUID).Specific;
            if (pVal.ColUID == "Col_12")
                ((EditText)matrix.GetCellSpecific(pVal.ColUID, pVal.Row)).Value = ListChoiceListener(pVal, "WhsCode")[0].ToString();
            else if (pVal.ColUID == "Col_14")
                ((EditText)matrix.GetCellSpecific(pVal.ColUID, pVal.Row)).Value = ListChoiceListener(pVal, "WhsCode")[0].ToString();
        }

        [B1Listener(BoEventTypes.et_DOUBLE_CLICK, false)]
        public virtual void OnAfterDoubleClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            ((SAPbouiCOM.Matrix)Form.Items.Item(ItemUID).Specific).FlushToDataSource();
            var dt = Form.DataSources.DataTables.Item("DT_DevMas");
            var opt = dt.GetValue("Opt", 0).ToString();

            if (pVal.ColUID == "Col_0")
            {
                if (pVal.Row == 0)
                {
                    try
                    {
                        Form.Freeze(true);

                        for (int i = 0; i < dt.Rows.Count; i++)
                            dt.SetValue("Opt", i, opt.Equals("Y") ? "N" : "Y");

                        ((SAPbouiCOM.Matrix)Form.Items.Item(ItemUID).Specific).LoadFromDataSource();
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


    }
}
