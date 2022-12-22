using B1Framework.B1Frame;
using B1Framework.RecordSet;
using bagant.Services;
using SAPbouiCOM;
using System;
using static bagant.Program;

namespace bagant.Form._2002122020
{
    class Form_Data : B1Form
    {

        public Form_Data()
        {
            FormType = "2002122020";
        }

        //[B1Listener(BoEventTypes.et_MENU_CLICK, false)]
        //public virtual void OnBeforeMenuClick(BusinessObjectInfo pVal)
        //{
        //    Globals.pFormD = "2002122020";
        //}

        [B1Listener(BoEventTypes.et_VALIDATE, true)]
        public virtual bool OnBeforeValidate(ItemEvent pVal)
        {
            Globals.pFormD = "2002122020";
            Form = new B1Forms(pVal.FormUID);
            Globals.pFormD = "2002122020";
            Frd.FormRD = Form;
            return true;
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_LOAD, false)]
        public virtual void OnBeforeFormDataLoad(BusinessObjectInfo pVal)
        {
            Globals.pFormD = "2002122020";
            SAPbouiCOM.Form FormR = B1Connections.SboApp.Forms.Item(pVal.FormUID);

            Form = new B1Forms(pVal.FormUID);
            Globals.pFormD = "2002122020";
            Frd.FormRD = Form;
            try
            {
                if (pVal.ActionSuccess)
                {
                    Form.Items.Item("Item_99").Enabled = true;
                    Form.Items.Item("Item_96").Enabled = false;

                    Form.Items.Item("Item_45").Enabled = false;
                    Form.Items.Item("Item_78").Enabled = false;
                    Form.Items.Item("Item_94").Enabled = false;
                    Form.Items.Item("Item_88").Enabled = false;
                    Form.Items.Item("Item_108").Enabled = false;

                    if (Form.Mode == BoFormMode.fm_ADD_MODE)
                    {
                        Form.Items.Item("Item_78").Enabled = true;
                        Form.Items.Item("Item_94").Enabled = true;
                        Form.Items.Item("Item_88").Enabled = true;
                        Form.Items.Item("Item_108").Enabled = true;
                        Form.Items.Item("Item_45").Enabled = true;
                        //Form.Items.Item("Item_62").SetAutoManagedAttribute.
                        //var matrixDataD = (SAPbouiCOM.Matrix)Form.Items.Item("Item_62").Specific;
                        

                    }
                    else
                    {
                        Form.Items.Item("Item_78").Enabled = false;
                        Form.Items.Item("Item_94").Enabled = false;
                        Form.Items.Item("Item_88").Enabled = false;
                        Form.Items.Item("Item_108").Enabled = false;
                        Form.Items.Item("Item_45").Enabled = false;
                    }

                    var docEntry = Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("DocEntry", 0).Trim();
                    var cardCode = Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("U_BGT_CardCode", 0).Trim();
                    var oqutDocEntry = Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("U_BGT_DocEntryOF", 0).Trim();
                    var status = Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("U_BGT_Status", 0).Trim();

                    //if (status.Equals("C"))
                    //    new CommonTaskSapUI().DisableForm(Form);
                    //else
                    //    new CommonTaskSapUI().EnableFields(Form);

                    var matrixData = (SAPbouiCOM.Matrix)Form.Items.Item("Item_62").Specific;
                    matrixData.AutoResizeColumns();

                    var queryContractResume = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetContractResume.sql"), cardCode, docEntry);
                    var dt = Form.DataSources.DataTables.Item("DT_Garnt");
                    dt.ExecuteQuery(queryContractResume);

                    if (string.IsNullOrEmpty(((string)dt.GetValue("Estado", 0))))
                    {
                        var noDataQuery = GetEmbeddedResource("bagant.Hana.NoDataFound.sql");
                        dt.ExecuteQuery(noDataQuery);
                    }
                    else
                    {
                        var gridRes = (Grid)Form.Items.Item("Item_9").Specific;
                        gridRes.Columns.Item("Numero Interno").Visible = false;
                        gridRes.Columns.Item("Num. Interno Oferta").Visible = false;

                        var gridColRes = (EditTextColumn)gridRes.Columns.Item("Numero Contrato");
                        gridColRes.LinkedObjectType = "AGT_CTR";
                        gridColRes = (EditTextColumn)gridRes.Columns.Item("Num. Oferta");
                        gridColRes.LinkedObjectType = "23";
                        //gridColRes = (EditTextColumn)gridRes.Columns.Item("Socio de Negocio");
                        //gridColRes.LinkedObjectType = "2";
                    }

                    if (!string.IsNullOrEmpty(oqutDocEntry))
                    {
                        var queryObraResume = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetResumeObra.sql"), oqutDocEntry);
                        var dtObra = Form.DataSources.DataTables.Item("DT_ResOb");
                        dtObra.ExecuteQuery(queryObraResume);

                        if (((int)dtObra.GetValue("OpprId", 0)) == 0)
                        {
                            var noDataQuery = GetEmbeddedResource("bagant.Hana.NoDataFound.sql");
                            dtObra.ExecuteQuery(noDataQuery);
                        }
                    }
                    new CommonTaskSapUI().SetTableSummatoryTotal(Form);

                    #region PESTAÑA DE GARANTIA
                    var queryGarantia = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetDataGarantia.sql"), cardCode);
                    var dtGta = Form.DataSources.DataTables.Item("DT_GTA");
                    dtGta.ExecuteQuery(queryGarantia);

                    if (string.IsNullOrEmpty(((string)dtGta.GetValue("Estado de Garantia", 0))))
                    {
                        var noDataQuery = GetEmbeddedResource("bagant.Hana.NoDataFound.sql");
                        dtGta.ExecuteQuery(noDataQuery);

                        Form.DataSources.UserDataSources.Item("UD_VlrCdo").Value = "0";
                        Form.DataSources.UserDataSources.Item("UD_VlrCon").Value = "0";
                        Form.DataSources.UserDataSources.Item("UD_CupGar").Value = "0";

                        //return;
                    }
                    else
                    {
                        var grid = (Grid)Form.Items.Item("Item_77").Specific;
                        var gridCol = (EditTextColumn)grid.Columns.Item("Numero Garantia");
                        gridCol.LinkedObjectType = "BAG_GARANTIAS";

                        var gridColSuma = (EditTextColumn)grid.Columns.Item("Valor");
                        gridColSuma.ColumnSetting.SumType = BoColumnSumType.bst_Auto;

                        grid.Columns.Item("U_GRT_D_TIP").Visible = false;
                        grid.Columns.Item("U_GRT_D_EST").Visible = false;
                        grid.AutoResizeColumns();

                        var queryGarantiaHeader = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetGarantiaHeader.sql"), docEntry, cardCode);
                        Log.Info("queryGarantiaHeader => " + queryGarantiaHeader);
                        var respGarHead = Record.Instance.Query(queryGarantiaHeader).Execute().First();
                        Form.DataSources.UserDataSources.Item("UD_VlrCdo").Value = respGarHead["GarConsumObra"];
                        Form.DataSources.UserDataSources.Item("UD_VlrCon").Value = respGarHead["GarConsumCont"];
                        Form.DataSources.UserDataSources.Item("UD_CupGar").Value = respGarHead["GarCup"];
                    }
                    #endregion

                    #region PESTAÑA DE MOVIMIENTO
                    var queryMov = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetMovement.sql"), docEntry);
                    Log.Info("queryMov => " + queryMov);
                    var dtMov = Form.DataSources.DataTables.Item("DT_Mvm");
                    dtMov.ExecuteQuery(queryMov);

                    if (string.IsNullOrEmpty(((string)dtMov.GetValue("Tipo Objeto", 0))))
                    {
                        var noDataQuery = GetEmbeddedResource("bagant.Hana.NoDataFound.sql");
                        dtMov.ExecuteQuery(noDataQuery);
                        if (Form.Mode == BoFormMode.fm_ADD_MODE)
                        {
                            Form.Items.Item("Item_78").Enabled = true;
                            Form.Items.Item("Item_94").Enabled = true;
                            Form.Items.Item("Item_88").Enabled = true;
                            Form.Items.Item("Item_108").Enabled = true;
                            SetFieldDb("1", "U_BGT_CicloFact", "@BGT_OCTR");
                        }
                        if (Form.Mode == BoFormMode.fm_UPDATE_MODE)
                        {
                            Form.Items.Item("Item_78").Enabled = true;
                            Form.Items.Item("Item_94").Enabled = true;
                            Form.Items.Item("Item_88").Enabled = true;
                            Form.Items.Item("Item_108").Enabled = true;
                            //SetFieldDb(ListChoiceListener(pVal, "Code")[0].ToString(), "U_BGT_CicloFact", "@BGT_OCTR");
                        }
                        //return;
                    }
                    else
                    {
                        if (Form.Mode == BoFormMode.fm_ADD_MODE)
                        {
                            Form.Items.Item("Item_78").Enabled = true;
                            Form.Items.Item("Item_94").Enabled = true;
                            Form.Items.Item("Item_88").Enabled = true;
                            Form.Items.Item("Item_108").Enabled = true;
                            //SetFieldDb(ListChoiceListener(pVal, "Code")[0].ToString(), "U_BGT_CicloFact", "@BGT_OCTR");
                        }
                        //else
                        //{
                        //    Form.Items.Item("Item_78").Enabled = false;
                        //    Form.Items.Item("Item_94").Enabled = false;
                        //    Form.Items.Item("Item_88").Enabled = false;
                        //    Form.Items.Item("Item_108").Enabled = false;
                        //}
                        var gridMov = (Grid)Form.Items.Item("Item_101").Specific;
                        gridMov.Columns.Item("Numero Interno").Visible = false;
                        gridMov.Columns.Item("Type").Visible = false;
                        gridMov.Columns.Item("Hora").Visible = false;
                        gridMov.CollapseLevel = 1;


                        var gridMovCol = (EditTextColumn)gridMov.Columns.Item("Numero Documento");
                        gridMovCol.LinkedObjectType = "13";

                        var gridMovItem = (EditTextColumn)gridMov.Columns.Item("Articulo");
                        gridMovItem.LinkedObjectType = "4";
                    }

                    #endregion

                    #region PESTAÑA DE MOVIMIENTO POR ITEM
                    var queryMovByItem = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetMovementByItem.sql"), docEntry);
                    Log.Info("queryMovByItem => " + queryMovByItem);
                    var dtMovByItm = Form.DataSources.DataTables.Item("DT_MvmItm");
                    dtMovByItm.ExecuteQuery(queryMovByItem);

                    if (string.IsNullOrEmpty(((string)dtMovByItm.GetValue("Tipo Objeto", 0))))
                    {
                        var noDataQuery = GetEmbeddedResource("bagant.Hana.NoDataFound.sql");
                        dtMovByItm.ExecuteQuery(noDataQuery);
                        //return;
                    }
                    else
                    {
                        var gridMovByItem = (Grid)Form.Items.Item("Item_111").Specific;
                        gridMovByItem.Columns.Item("Numero Interno").Visible = false;
                        gridMovByItem.Columns.Item("Type").Visible = false;
                        gridMovByItem.Columns.Item("Hora").Visible = false;
                        gridMovByItem.CollapseLevel = 1;

                        var gridMovColByItem = (EditTextColumn)gridMovByItem.Columns.Item("Numero Documento");
                        gridMovColByItem.LinkedObjectType = "13";

                        var gridMovItemByItem = (EditTextColumn)gridMovByItem.Columns.Item("Articulo");
                        gridMovItemByItem.LinkedObjectType = "4";
                    }

                    #endregion

                    #region PESTAÑA DE ESTADO DE CUENTA
                    var queryEdoCta = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetEdoCuenta.sql"), docEntry);
                    var dtEdoCta = Form.DataSources.DataTables.Item("DT_EdoCta");
                    dtEdoCta.ExecuteQuery(queryEdoCta);

                    if (string.IsNullOrEmpty(((string)dtEdoCta.GetValue("Tipo de Documento", 0))))
                    {
                        var noDataQuery = GetEmbeddedResource("bagant.Hana.NoDataFound.sql");
                        dtEdoCta.ExecuteQuery(noDataQuery);
                    }
                    else
                    {
                        var gridEdoCta = (Grid)Form.Items.Item("Item_15").Specific;
                        gridEdoCta.Columns.Item("Numero Interno").Visible = false;

                        var gridColEdoCta = (EditTextColumn)gridEdoCta.Columns.Item("Numero Factura/NC");
                        gridColEdoCta.LinkedObjectType = "13";
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Log.Info("Excp => " + ex.Message);
            }
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnBeforeFormDataAdd(BusinessObjectInfo pVal)
        {
            if (pVal.ActionSuccess)
            {
                Form = new B1Forms(pVal.FormTypeEx);
                if (Form.Mode == BoFormMode.fm_ADD_MODE)
                {
                    Form.Items.Item("Item_78").Enabled = true;
                    Form.Items.Item("Item_94").Enabled = true;
                    Form.Items.Item("Item_88").Enabled = true;
                    Form.Items.Item("Item_108").Enabled = true;
                    SetFieldDb("1", "U_BGT_CicloFact", "@BGT_OCTR");
                }
            }
        }

       


    }
}
