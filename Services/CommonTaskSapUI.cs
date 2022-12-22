using B1Framework.B1Frame;
using B1Framework.RecordSet;
using SAPbouiCOM;
using System.Linq;
using System;
using System.Collections.Generic;
using SAPbobsCOM;

namespace bagant.Services
{
    class CommonTaskSapUI : B1Item
    {
        public void SetValuesFromSNChoosed(B1Forms FormParam, string CardCode)
        {
            Form = FormParam;

            ClearComboBoxValues("Item_21");
            ClearComboBoxValues("Item_23");
            ClearComboBoxValues("Item_25");

            var queryContact = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetContactPerson.sql"), CardCode);
            var queryAddress = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetAddresFE.sql"), CardCode);
            var queryShipment = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetAddresShip.sql"), CardCode);
            var queryHolding = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetBPHolding.sql"), CardCode);
            var respHolding = Record.Instance.Query(queryHolding).Execute().First();

            FillComboBoxWithQuery(queryContact, "Item_21");
            FillComboBoxWithQuery(queryAddress, "Item_23");
            FillComboBoxWithQuery(queryShipment, "Item_25");

            Form.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("U_BGT_CodHolding", 0, respHolding["U_IXX_HOLD"]);
            Form.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("U_BGT_NomHolding", 0, respHolding["CardName"]);
        }

        public void SetSearchMode(B1Forms FormParam, bool IsSearch, bool State)
        {
            Form = FormParam;
            Form.Freeze(true);

            try
            {
                if (Form.TypeEx == "2002122020")
                {
                    Form.Items.Item("Item_45").Enabled = true;
                    Form.Items.Item("Item_14").Enabled = State;
                    Form.Items.Item("Item_43").Enabled = State;
                    Form.Items.Item("Item_29").Enabled = State;
                    Form.Items.Item("Item_65").Enabled = State;
                    Form.Items.Item("Item_67").Enabled = State;
                    Form.Items.Item("Item_69").Enabled = State;
                    Form.Items.Item("Item_71").Enabled = State;
                    Form.Items.Item("Item_73").Enabled = State;

                    if (IsSearch)
                        Form.DataSources.DBDataSources.Item(0).SetValue("U_BGT_Status", 0, "");
                }
            }
            finally
            {
                Form.Freeze(false);
            }
        }

        public void SetTableData(B1Forms FormParam, string docEntry)
        {
            try
            {
                Form = FormParam;
                double valRepCont = 0, subPesObra = 0, subPesEnt = 0;
                var priceList = Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("U_BGT_PriceList", 0).Trim();
                var matrix = (SAPbouiCOM.Matrix)Form.Items.Item("Item_62").Specific;
                matrix.Clear();

                var queryPriceList = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetDetailsTable.sql"), docEntry, priceList);
                var resp = Record.Instance.Query(queryPriceList).Execute().All();
                for (int i = 0; i < resp.Length; i++)
                {
                    matrix.AddRow();
                    matrix.SetCellWithoutValidation(i + 1, "#", (i + 1).ToString());
                    matrix.SetCellWithoutValidation(i + 1, "Col_0", resp[i]["ItemCode"]);
                    matrix.SetCellWithoutValidation(i + 1, "Col_1", resp[i]["ItemName"]);
                    matrix.SetCellWithoutValidation(i + 1, "Col_2", resp[i]["Price"]);
                    matrix.SetCellWithoutValidation(i + 1, "Col_4", resp[i]["Quantity"]);
                    matrix.SetCellWithoutValidation(i + 1, "Col_6", resp[i]["Quantity"]);
                    matrix.SetCellWithoutValidation(i + 1, "Col_7", resp[i]["SWeight1"]);
                    matrix.SetCellWithoutValidation(i + 1, "Col_14", resp[i]["Price"]);
                    matrix.SetCellWithoutValidation(i + 1, "Col_15", resp[i]["Price"]);
                    matrix.SetCellWithoutValidation(i + 1, "Col_16", resp[i]["Price"]);

                    matrix.SetCellWithoutValidation(i + 1, "Col_11", (double.Parse(resp[i]["Price"]) * double.Parse(resp[i]["Quantity"])).ToString());
                    matrix.SetCellWithoutValidation(i + 1, "Col_13", (double.Parse(resp[i]["Price"]) * double.Parse(resp[i]["Quantity"])).ToString());

                    valRepCont += (double.Parse(resp[i]["PriceVenta"]) * double.Parse(resp[i]["Quantity"]));
                    subPesObra += (double.Parse(resp[i]["SWeight1"]) * double.Parse(resp[i]["Quantity"]));
                    subPesEnt += (double.Parse(resp[i]["SWeight1"]) * double.Parse(resp[i]["Quantity"]));
                }

                matrix.AutoResizeColumns();

                var cardCode = ((EditText)Form.Items.Item("Item_6").Specific).Value;
                var queryGta = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetGarantiaValue.sql"), cardCode);
                var rcorGta = Record.Instance.Query(queryGta).Execute().First();

                SetTableTotal(Form, 0, valRepCont, double.Parse(rcorGta["Valor"].Replace(",", ".")), subPesObra, subPesEnt);
            }
            catch (Exception ex)
            {
                Log.Info("(Exception) SetTableData => " + ex.Message);
            }
        }

        public void SetOpportunities(B1Forms FormParam, string DocEntryOF)
        {
            Form = FormParam;
            var queryOppor = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetOpportunities.sql"), DocEntryOF);
            var rcordOppor = Record.Instance.Query(queryOppor).Execute().First();

            SetFieldDb(rcordOppor["ClgCode"].Equals("0") ? "" : rcordOppor["ClgCode"], "U_BGT_ActRel", "@BGT_OCTR");
        }

        public void SetTableSummatoryTotal(B1Forms FormParam)
        {
            Form = FormParam;
            double valRepEqObr = 0, valRepCont = 0, subPesObra = 0, subPesEnt = 0;
            var matrix = (SAPbouiCOM.Matrix)Form.Items.Item("Item_62").Specific;

            for (int i = 1; i <= matrix.VisualRowCount; i++)
            {
                var ItemCode = ((EditText)matrix.GetCellSpecific("Col_0", i)).Value;

                if (!ItemCode.Contains("LOG"))
                {
                    var kilos = ((EditText)matrix.GetCellSpecific("Col_7", i)).Value.Replace(",", ".");
                    var cantPend = ((EditText)matrix.GetCellSpecific("Col_6", i)).Value.Replace(",", ".");
                    var cantObra = ((EditText)matrix.GetCellSpecific("Col_5", i)).Value.Replace(",", ".");
                    var cantCont = ((EditText)matrix.GetCellSpecific("Col_4", i)).Value.Replace(",", ".");
                    var precVnta = ((EditText)matrix.GetCellSpecific("Col_15", i)).Value.Replace(",", ".");
                    var precporDia = ((EditText)matrix.GetCellSpecific("Col_16", i)).Value.Replace(",", ".");

                    subPesEnt += double.Parse(cantPend) * double.Parse(kilos);
                    subPesObra += double.Parse(cantObra) * double.Parse(kilos);
                    valRepCont += double.Parse(cantCont) * double.Parse(precVnta);
                    valRepEqObr += double.Parse(cantObra) * double.Parse(precVnta);
                }

            }

            var cardCode = ((EditText)Form.Items.Item("Item_6").Specific).Value;
            var queryGta = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetGarantiaValue.sql"), cardCode);
            var rcorGta = Record.Instance.Query(queryGta).Execute().First();
            Log.Info("QueryGTA => " + queryGta);
            SetTableTotal(Form, valRepEqObr, valRepCont, double.Parse(rcorGta["Valor"].Replace(",", ".")), subPesObra, subPesEnt);
        }

        private void SetTableTotal(B1Forms FormParam, double valRepEqObr, double valRepCont, double valGarant, double subPesObra, double subPesEnt)
        {
            FormParam.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("U_BGT_ValRepEqOb", 0, valRepEqObr.ToString());
            FormParam.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("U_BGT_ValRepCont", 0, valRepCont.ToString());
            FormParam.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("U_BGT_ValGarant", 0, valGarant.ToString());
            FormParam.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("U_BGT_SubPesObra", 0, subPesObra.ToString());
            FormParam.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("U_BGT_SubPesEnt", 0, subPesEnt.ToString());
        }

        public void SetCategory(B1Forms FormParam, string categoryId)
        {
            Form = FormParam;
            ((ComboBox)Form.Items.Item("Item_84").Specific).Select(categoryId, BoSearchKey.psk_ByValue);
            Form.Items.Item("dummy").Click();
        }

        public void ClearComboBoxValues(string itemUID)
        {
            try
            {
                var combo = (ComboBox)Form.Items.Item(itemUID).Specific;
                var cantidad = combo.ValidValues.Count;
                for (int i = cantidad - 1; i >= 0; i--)
                    combo.ValidValues.Remove(i, BoSearchKey.psk_Index);
            }
            catch (Exception ex)
            {
                Log.Info("(Exception) (ClearComboBoxValues) => " + ex.Message);
            }
        }

        public void SetContractInitialValues(B1Forms FormParam)
        {
            try
            {
                Form = FormParam;
                Form.Freeze(true);
                Form.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("CreateDate", 0, DateTime.Now.ToString("yyyyMMdd"));

                var queryUsr = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetBranchUser.sql"), B1Connections.DiCompany.UserSignature);
                var respUser = Record.Instance.Query(queryUsr).Execute().First();

                ClearComboBoxValues("Item_78");
                FillComboBoxWithQuery(GetEmbeddedResource("bagant.Hana.GetPriceList.sql"), "Item_78");
                ClearComboBoxValues("Item_84");
                FillComboBoxWithQuery(GetEmbeddedResource("bagant.Hana.GetCategory.sql"), "Item_84");
                ClearComboBoxValues("Item_57");
                FillComboBoxWithQuery(GetEmbeddedResource("bagant.Hana.GetSeriesNumberContract.sql"), "Item_57");
                ClearComboBoxValues("Item_94");
                FillComboBoxWithQuery(B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetBranches.sql"), B1Connections.DiCompany.UserSignature), "Item_94");
                ClearComboBoxValues("Item_27");
                FillComboBoxWithQuery(GetEmbeddedResource("bagant.Hana.GetTerritory.sql"), "Item_27");

                var queryValueSel = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetPriceListSelectedUser.sql"), B1Connections.DiCompany.UserSignature);
                var respValueSel = Record.Instance.Query(queryValueSel).Execute().First();
                ((ComboBox)Form.Items.Item("Item_78").Specific).Select(respValueSel["Value"], BoSearchKey.psk_ByValue);

                var ramas = ((ComboBox)Form.Items.Item("Item_94").Specific);
                if (ramas.ValidValues.Count > 0)
                    ramas.Select(0, BoSearchKey.psk_Index);

                if (!string.IsNullOrEmpty(respUser["Name"]))
                    ((ComboBox)Form.Items.Item("Item_94").Specific).Select(respUser["Name"], BoSearchKey.psk_ByValue);

                var btnCmbo = ((ButtonCombo)Form.Items.Item("Item_96").Specific);
                if (btnCmbo.ValidValues.Count == 0)
                    btnCmbo.ValidValues.Add("23", "Oferta de ventas");

                Form.ChooseFromLists.Item("CFL_SN").SetConditions(null);
                Form.SetCflConditions("CFL_SN", new string[] { "CardType", "frozenFor" }, new BoConditionOperation[] { BoConditionOperation.co_EQUAL, BoConditionOperation.co_EQUAL }, new string[] { "C", "N" });
                Form.ChooseFromLists.Item("CFL_CH").SetConditions(null);
                Form.SetCflConditions("CFL_CH", new string[] { "CardType", "frozenFor", "QryGroup5" }, new BoConditionOperation[] { BoConditionOperation.co_EQUAL, BoConditionOperation.co_EQUAL, BoConditionOperation.co_EQUAL }, new string[] { "C", "N", "Y" });
                Form.ChooseFromLists.Item("CFL_COM").SetConditions(null);
                Form.SetCflConditions("CFL_COM", "U_CMS_FAC", BoConditionOperation.co_EQUAL, "A");

                Form.Items.Item("Item_99").Enabled = false;
                Form.Items.Item("Item_96").Enabled = true;

                var matrix = (SAPbouiCOM.Matrix)Form.Items.Item("Item_62").Specific;
                UtilServices.NewSetCflConditions(Form, "CFL_ITM",
                                                    new string[] { "ItemCode", "ItemCode" },
                                                    new BoConditionOperation[] { BoConditionOperation.co_CONTAIN, BoConditionOperation.co_CONTAIN },
                                                    new string[] { "AEA", "LOG" });
                //matrix.Columns.Item("Col_7").Visible = false;

                ((ComboBox)Form.Items.Item("Item_57").Specific).Select("Primary", BoSearchKey.psk_ByDescription);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Form.Freeze(false);
            }
        }

        public void DisableForm(B1Forms Form)
        {
            try
            {
                Form.Items.Item("dummy").Click();
                Form.Items.Item("Item_62").Enabled = false;
                Form.Items.Item("Item_6").Enabled = false;
                Form.Items.Item("Item_18").Enabled = false;
                Form.Items.Item("Item_21").Enabled = false;
                Form.Items.Item("Item_23").Enabled = false;
                Form.Items.Item("Item_25").Enabled = false;
                Form.Items.Item("Item_27").Enabled = false;
                Form.Items.Item("Item_8").Enabled = false;
                Form.Items.Item("Item_78").Enabled = false;
                Form.Items.Item("Item_94").Enabled = false;
                Form.Items.Item("Item_53").Enabled = false;
                Form.Items.Item("Item_95").Enabled = false;
                Form.Items.Item("Item_61").Enabled = false;
                //Form.Items.Item("Item_45").Enabled = false;
                Form.Items.Item("Item_14").Enabled = false;
                Form.Items.Item("Item_43").Enabled = false;
                Form.Items.Item("Item_62").Enabled = false;
                Form.Items.Item("Item_29").Enabled = false;
                Form.Items.Item("Item_37").Enabled = false;
                Form.Items.Item("Item_39").Enabled = false;
                Form.Items.Item("Item_41").Enabled = false;
                Form.Items.Item("Item_79").Enabled = false;
                Form.Items.Item("Item_51").Enabled = false;
                Form.Items.Item("Item_55").Enabled = false;
                Form.Items.Item("Item_84").Enabled = false;
                Form.Items.Item("Item_98").Enabled = false;
                Form.Items.Item("Item_88").Enabled = false;
                Form.Items.Item("Item_31").Enabled = false;
                Form.Items.Item("Item_33").Enabled = false;
                Form.Items.Item("Item_74").Enabled = false;
                Form.Items.Item("Item_65").Enabled = false;
                Form.Items.Item("Item_67").Enabled = false;
                Form.Items.Item("Item_69").Enabled = false;
                Form.Items.Item("Item_71").Enabled = false;
                Form.Items.Item("Item_73").Enabled = false;
                Form.Items.Item("Item_77").Enabled = false;
                Form.Items.Item("Item_9").Enabled = false;
                Form.Items.Item("Item_101").Enabled = false;
                Form.Items.Item("Item_15").Enabled = false;
                Form.Items.Item("Item_102").Enabled = false;
                Form.Items.Item("Item_57").Enabled = false;
                Form.Items.Item("Item_108").Enabled = false;
                Form.Items.Item("Item_47").Enabled = false;


            }
            catch (Exception ex)
            {
                Log.Info("(Exception)(2003112020) => " + ex.Message);
            }
        }

        public void EnableFields(B1Forms Form)
        {
            try
            {
                Form.Items.Item("dummy").Click();
                Form.Items.Item("Item_6").Enabled = true;
                Form.Items.Item("Item_18").Enabled = true;
                Form.Items.Item("Item_21").Enabled = true;
                Form.Items.Item("Item_23").Enabled = true;
                Form.Items.Item("Item_25").Enabled = true;
                Form.Items.Item("Item_27").Enabled = true;
                Form.Items.Item("Item_8").Enabled = true;
                Form.Items.Item("Item_78").Enabled = true;
                Form.Items.Item("Item_94").Enabled = true;
                Form.Items.Item("Item_57").Enabled = true;
                Form.Items.Item("Item_98").Enabled = true;
                Form.Items.Item("Item_88").Enabled = true;
                Form.Items.Item("Item_37").Enabled = true;
                Form.Items.Item("Item_39").Enabled = true;
                Form.Items.Item("Item_79").Enabled = true;
                Form.Items.Item("Item_47").Enabled = true;
                Form.Items.Item("Item_51").Enabled = true;
                Form.Items.Item("Item_55").Enabled = true;
                Form.Items.Item("Item_84").Enabled = true;
                Form.Items.Item("Item_53").Enabled = true;
                Form.Items.Item("Item_95").Enabled = true;
                Form.Items.Item("Item_62").Enabled = true;
                Form.Items.Item("Item_41").Enabled = true;
                Form.Items.Item("Item_33").Enabled = true;
                Form.Items.Item("Item_74").Enabled = true;
                Form.Items.Item("Item_108").Enabled = true;

                Form.Items.Item("Item_61").Enabled = false;
                //Form.Items.Item("Item_45").Enabled = false;
                Form.Items.Item("Item_14").Enabled = false;
                Form.Items.Item("Item_43").Enabled = false;
                Form.Items.Item("Item_29").Enabled = false;
                Form.Items.Item("Item_31").Enabled = false;
                Form.Items.Item("Item_65").Enabled = false;
                Form.Items.Item("Item_67").Enabled = false;
                Form.Items.Item("Item_69").Enabled = false;
                Form.Items.Item("Item_71").Enabled = false;
                Form.Items.Item("Item_73").Enabled = false;
                Form.Items.Item("Item_77").Enabled = false;
                Form.Items.Item("Item_9").Enabled = false;
                Form.Items.Item("Item_101").Enabled = false;
                Form.Items.Item("Item_15").Enabled = false;
                Form.Items.Item("Item_102").Enabled = false;
            }
            catch (Exception ex)
            {
                Log.Info("(Exception)(2003112020) => " + ex.Message);
            }
        }

        public void SetFilterProject(B1Forms FormParam, string cardCode)
        {
            Form = FormParam;
            var listado = new List<string>();
            var cantCol = new List<string>();
            var cantColCond = new List<BoConditionOperation>();

            var queryProject = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetProjectSN.sql"), cardCode);
            var respProject = Record.Instance.Query(queryProject).Execute().All();

            for (int i = 0; i < respProject.Length; i++)
            {
                listado.Add(respProject[i]["Address2"]);
                cantCol.Add("PrjCode");
                cantColCond.Add(BoConditionOperation.co_EQUAL);
            }

            UtilServices.SetCflConditions(Form, "CFL_PJ", cantCol.ToArray(), cantColCond.ToArray(), listado.ToArray());
        }

        public void SetFilterItemsTable(B1Forms FormParam, string sucursal, string distancia)
        {
            Form = FormParam;
            Form.ChooseFromLists.Item("CFL_ITM").SetConditions(null);
            UtilServices.NewSetCflConditions(Form, "CFL_ITM",
                                                            new string[] { "ItemCode", "U_BAG_DIST_TPE", "U_IXX_Suc_OCod" },
                                                            new BoConditionOperation[] { BoConditionOperation.co_CONTAIN, BoConditionOperation.co_EQUAL },
                                                            new string[] { "LOG", distancia, sucursal });
        }

        public void SetItemsContract(B1Forms FormParam)
        {
            Form = FormParam;
            var distObra = (ComboBox)Form.Items.Item("Item_47").Specific;
            if (distObra.Selected == null)
            {
                B1Connections.SboApp.SetStatusBarMessage("No ha seleccionado ninguna distancia de obra");
                return;
            }

            B1Connections.SboApp.SetStatusBarMessage("Comenzando proceso, por favor espere...", BoMessageTime.bmt_Medium, false);

            var matrix = (SAPbouiCOM.Matrix)Form.Items.Item("Item_62").Specific;
            var distObraValue = distObra.Selected.Value;
            var codSuc = (ComboBox)Form.Items.Item("Item_94").Specific;
            var valSuc = codSuc.Selected == null ? "" : codSuc.Selected.Value;
            var listSucPrec = (ComboBox)Form.Items.Item("Item_78").Specific;
            var valSucPrec = listSucPrec.Selected == null ? "" : listSucPrec.Selected.Value;
            var queryItemsDist = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetItemsDistanceObra.sql"), distObraValue, valSucPrec, valSuc);

            //    linea agregada, para validar query
            Log.Info("CFL DISTANCIA OBRA => " + queryItemsDist);

            var respItemDist = Record.Instance.Query(queryItemsDist).Execute().All();



            Form.Freeze(true);
            try
            {
                Log.Info("245");
                for (int i = matrix.RowCount; i >= 1; i--)
                {
                    var ItemCode = ((EditText)matrix.GetCellSpecific("Col_0", i)).Value;
                    var cantDev = ((EditText)matrix.GetCellSpecific("Col_9", i)).Value.Replace(",", ".");
                    var cantEnt = ((EditText)matrix.GetCellSpecific("Col_10", i)).Value.Replace(",", ".");

                    if (ItemCode.Contains("LOG"))
                    {
                        if (double.Parse(cantDev) == 0 && double.Parse(cantEnt) == 0)
                            matrix.DeleteRow(i);
                    }
                }

                Log.Info("246");
                for (int i = 0; i < respItemDist.Length; i++)
                {
                    AddRowToMatrix(Form, matrix, 1);

                    matrix.SetCellWithoutValidation(matrix.RowCount, "Col_0", respItemDist[i]["ItemCode"].ToString());
                    matrix.SetCellWithoutValidation(matrix.RowCount, "Col_1", respItemDist[i]["ItemName"].ToString());
                    matrix.SetCellWithoutValidation(matrix.RowCount, "Col_2", respItemDist[i]["Price"].ToString());
                    matrix.SetCellWithoutValidation(matrix.RowCount, "Col_15", respItemDist[i]["PriceVenta"].ToString());
                    matrix.SetCellWithoutValidation(matrix.RowCount, "Col_16", respItemDist[i]["PriceVenta"].ToString());
                }

                for (int i = 1; i <= matrix.VisualRowCount; i++)
                    matrix.SetCellWithoutValidation(i, "#", i.ToString());
            }
            catch (Exception ex)
            {
                Log.Info("Ex Mat => " + ex.Message);
            }
            finally
            {
                new CommonTaskSapUI().NumberingRowTable(matrix);
                Form.Freeze(false);
                B1Connections.SboApp.SetStatusBarMessage("Proceso Finalizado", BoMessageTime.bmt_Medium, false);
            }

        }

        public void SetDeliveryFinded(B1Forms FormParam)
        {
            try
            {
                Form = FormParam;
                B1Connections.SboApp.SetStatusBarMessage("Realizando Busqueda, por favor espere....", BoMessageTime.bmt_Medium, false);

                var grid = (Grid)Form.Items.Item("Item_5").Specific;
                var status = Form.DataSources.UserDataSources.Item("UD_Edo").Value;
                var oporDesde = Form.DataSources.UserDataSources.Item("UD_OPD").Value;
                var oporHasta = Form.DataSources.UserDataSources.Item("UD_OPH").Value;
                var proyDesde = Form.DataSources.UserDataSources.Item("UD_PRD").Value;
                var proyHasta = Form.DataSources.UserDataSources.Item("UD_PRH").Value;
                var oferDesde = Form.DataSources.UserDataSources.Item("UD_OFD").Value;
                var oferHasta = Form.DataSources.UserDataSources.Item("UD_OFH").Value;
                var clieDesde = Form.DataSources.UserDataSources.Item("UD_CLD").Value;
                var clieHasta = Form.DataSources.UserDataSources.Item("UD_CLH").Value;
                var holdDesde = Form.DataSources.UserDataSources.Item("UD_CHD").Value;
                var holdHasta = Form.DataSources.UserDataSources.Item("UD_CHH").Value;
                var fecDesde = Form.DataSources.UserDataSources.Item("UD_FED").Value;
                var fecHasta = Form.DataSources.UserDataSources.Item("UD_FEH").Value;
                var itemDesde = Form.DataSources.UserDataSources.Item("UD_ITD").Value;
                var itemHasta = Form.DataSources.UserDataSources.Item("UD_ITH").Value;
                var contDesde = Form.DataSources.UserDataSources.Item("UD_ContD").Value;
                var contHasta = Form.DataSources.UserDataSources.Item("UD_ContH").Value;

                var queryDelivery = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetDeliveryDocuments.sql"), status, oporDesde, oporHasta, proyDesde, proyHasta, oferDesde, oferHasta, clieDesde, clieHasta, holdDesde, holdHasta, fecDesde, fecHasta, itemDesde, itemHasta, contDesde, contHasta);
                Log.Info("QueryDelivery => " + queryDelivery);
                var entrData = Form.DataSources.DataTables.Item("DT_SelEnt");
                entrData.ExecuteQuery(queryDelivery);

                if (((string)grid.DataTable.GetValue("Nombre Cliente", 0)).Equals(""))
                {
                    var noDataQuery = GetEmbeddedResource("bagant.Hana.NoDataFound.sql");
                    entrData.ExecuteQuery(noDataQuery);
                    grid.Item.Enabled = false;
                    Form.Items.Item("Item_33").Enabled = false;
                    return;
                }

                ((EditTextColumn)grid.Columns.Item("Numero Contrato")).LinkedObjectType = "AGT_CTR";
                ((EditTextColumn)grid.Columns.Item("Nombre Cliente")).LinkedObjectType = "2";
                ((EditTextColumn)grid.Columns.Item("Codigo Articulo")).LinkedObjectType = "4";

                grid.Columns.Item("Nombre Cliente").Editable = false;
                grid.Columns.Item("Fecha Contrato").Editable = false;
                grid.Columns.Item("Codigo Articulo").Editable = false;
                grid.Columns.Item("Cantidad En Obra").Editable = false;
                grid.Columns.Item("Descripcion Articulo").Editable = false;
                grid.Columns.Item("Cantidad de Contrato").Editable = false;
                grid.Columns.Item("Stock Disponible").Editable = false;
                grid.Columns.Item("Stock Pendiente Contrato").Editable = false;
                grid.Columns.Item("Precio Unitario").Visible = false;
                grid.Columns.Item("Codigo Obra").Editable = false;
                grid.Columns.Item("Codigo Obra").Visible = false;
                grid.Columns.Item("Nombre Obra").Editable = false;
                grid.Columns.Item("Codigo Interno").Visible = false;
                grid.Columns.Item("Cliente").Visible = false;

                grid.Columns.Item("Opt").Type = BoGridColumnType.gct_CheckBox;
                grid.Columns.Item("Opt").TitleObject.Caption = "";
                grid.CollapseLevel = 2;

                Form.State = BoFormStateEnum.fs_Maximized;
                B1Connections.SboApp.SetStatusBarMessage("Proceso finalizado", BoMessageTime.bmt_Medium, false);
            }
            catch (Exception ex)
            {
                Log.Info("Exception (SetDeliveryFinded) => " + ex.Message);
            }
        }

        public bool ValidateDetailDevolutionFinded(B1Forms FormParam)
        {
            Form = FormParam;
            var matrixHeadDevo = (SAPbouiCOM.Matrix)Form.Items.Item("Item_6").Specific;

            for (int i = 1; i <= matrixHeadDevo.VisualRowCount; i++)
            {
                var prop = ((EditText)matrixHeadDevo.GetCellSpecific("Col_5", i)).Value;
                var guiaRem = ((EditText)matrixHeadDevo.GetCellSpecific("Col_6", i)).Value;
                var transp = ((EditText)matrixHeadDevo.GetCellSpecific("Col_7", i)).Value;
                var placa = ((EditText)matrixHeadDevo.GetCellSpecific("Col_8", i)).Value;

                if (string.IsNullOrEmpty(prop))
                {
                    B1Connections.SboApp.SetStatusBarMessage($"No ha indicado el campo de PROPIETARIO para la fila {i}");
                    return false;
                }

                if (string.IsNullOrEmpty(guiaRem))
                {
                    B1Connections.SboApp.SetStatusBarMessage($"No ha indicado el campo de GUIA DE REMISION para la fila {i}");
                    return false;
                }

                if (string.IsNullOrEmpty(transp))
                {
                    B1Connections.SboApp.SetStatusBarMessage($"No ha indicado el campo de TRANSPORTE para la fila {i}");
                    return false;
                }

                if (string.IsNullOrEmpty(placa))
                {
                    B1Connections.SboApp.SetStatusBarMessage($"No ha indicado el campo de PLACA para la fila {i}");
                    return false;
                }
            }

            return true;
        }

        public void SetDeliveryResume(B1Forms FormParam)
        {
            Form = FormParam;

            B1Connections.SboApp.SetStatusBarMessage("Comenzando proceso de resumen, por favor espere....", BoMessageTime.bmt_Medium, false);

            var listado = new Dictionary<string, List<Tuple<string, string, string, string, double, double>>>();
            var grid = (Grid)Form.Items.Item("Item_5").Specific;
            var gridRes = (Grid)Form.Items.Item("Item_37").Specific;
            var dtRes = gridRes.DataTable;

            for (int i = 0; i < grid.Rows.Count; i++)
            {
                if (grid.GetDataTableRowIndex(i) != -1)
                {
                    var fila = grid.GetDataTableRowIndex(i);
                    if (((string)grid.DataTable.GetValue("Opt", fila)).Equals("Y"))
                    {
                        var codigoCliente = ((string)grid.DataTable.GetValue("Nombre Cliente", fila).ToString()).Split('-')[0];
                        if (!listado.ContainsKey(grid.DataTable.GetValue("Numero Contrato", fila).ToString()))
                        {
                            var nroCont = (string)grid.DataTable.GetValue("Numero Contrato", fila).ToString();
                            var tuple = new Tuple<string, string, string, string, double, double>(nroCont, grid.DataTable.GetValue("Codigo Interno", fila).ToString(), ((string)grid.DataTable.GetValue("Nombre Cliente", fila).ToString()), grid.DataTable.GetValue("Codigo Articulo", fila), grid.DataTable.GetValue("A Entregar", fila), grid.DataTable.GetValue("A Entregar", fila));

                            listado.Add(nroCont, new List<Tuple<string, string, string, string, double, double>>());
                            listado[nroCont].Add(tuple);
                        }
                        else
                        {
                            var nroCont = (string)grid.DataTable.GetValue("Numero Contrato", fila).ToString();
                            listado[nroCont].Add(new Tuple<string, string, string, string, double, double>(nroCont, grid.DataTable.GetValue("Codigo Interno", fila).ToString(), ((string)grid.DataTable.GetValue("Nombre Cliente", fila).ToString()), grid.DataTable.GetValue("Codigo Articulo", fila), grid.DataTable.GetValue("A Entregar", fila), grid.DataTable.GetValue("A Entregar", fila)));
                        }
                    }
                }
            }


            dtRes.Rows.Clear();
            for (int i = 0; i < listado.Count; i++)
            {
                var indice = listado.ElementAt(i).Key;

                dtRes.Rows.Add();
                dtRes.SetValue("Numero Contrato", i, listado[indice][0].Item1);
                dtRes.SetValue("Numero Interno", i, listado[indice][0].Item2);
                dtRes.SetValue("Cliente", i, listado[indice][0].Item3);
            }

            ((EditTextColumn)gridRes.Columns.Item("Numero Contrato")).LinkedObjectType = "AGT_CTR";
            gridRes.Columns.Item("Numero Interno").Visible = false;
            gridRes.Columns.Item("Numero Contrato").Editable = false;
            gridRes.Columns.Item("Cliente").Editable = false;
            gridRes.AutoResizeColumns();

            B1Connections.SboApp.SetStatusBarMessage("Proceso Finalizado", BoMessageTime.bmt_Medium, false);
        }

        public void SetDeliveryToReturnHeaderFinded(B1Forms FormParam)
        {
            Form = FormParam;

            try
            {
                B1Connections.SboApp.SetStatusBarMessage("Realizando Busqueda, por favor espere....", BoMessageTime.bmt_Medium, false);

                Form.Freeze(true);
                var matrixNoData = (SAPbouiCOM.Matrix)Form.Items.Item("Item_17").Specific;
                matrixNoData.Item.FromPane = 10;
                matrixNoData.Item.ToPane = 10;
                matrixNoData.Item.Visible = false;

                var gridDevo = (Grid)Form.Items.Item("Item_35").Specific;
                gridDevo.Item.FromPane = 2;
                gridDevo.Item.ToPane = 2;
                gridDevo.Item.Visible = true;

                var status = Form.DataSources.UserDataSources.Item("UD_Edo").Value;
                var oporDesde = Form.DataSources.UserDataSources.Item("UD_OPD").Value;
                var oporHasta = Form.DataSources.UserDataSources.Item("UD_OPH").Value;
                var proyDesde = Form.DataSources.UserDataSources.Item("UD_PRD").Value;
                var proyHasta = Form.DataSources.UserDataSources.Item("UD_PRH").Value;
                var clieDesde = Form.DataSources.UserDataSources.Item("UD_CLD").Value;
                var clieHasta = Form.DataSources.UserDataSources.Item("UD_CLH").Value;
                var itemDesde = Form.DataSources.UserDataSources.Item("UD_ITD").Value;
                var itemHasta = Form.DataSources.UserDataSources.Item("UD_ITH").Value;
                var contDesde = Form.DataSources.UserDataSources.Item("UD_CNTDE").Value;
                var contHasta = Form.DataSources.UserDataSources.Item("UD_CNTHE").Value;
                var fecDesde = Form.DataSources.UserDataSources.Item("UD_FED").Value;
                var fecHasta = Form.DataSources.UserDataSources.Item("UD_FEH").Value;

                var queryDeliveryHeader = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetReturnsHeaderDocuments.sql"), status, oporDesde, oporHasta, proyDesde, proyHasta, itemDesde, itemHasta, contDesde, contHasta, clieDesde, clieHasta, fecDesde, fecHasta);
                Log.Info("queryDeliveryHeader => " + queryDeliveryHeader);
                var dtDev = Form.DataSources.DataTables.Item("DT_RsDvol");
                dtDev.ExecuteQuery(queryDeliveryHeader);

                for (int i = 0; i < gridDevo.Columns.Count; i++)
                {
                    if (i == 1)
                        continue;

                    gridDevo.Columns.Item(i).Editable = false;
                    gridDevo.Columns.Item(i).TitleObject.Sortable = true;
                }

                gridDevo.Columns.Item("Cantidad a Devolver").Editable = true;
                gridDevo.Columns.Item("Devolucion Especial 1").Editable = true;
                gridDevo.Columns.Item("Bodega Devolucion Especial 1").Editable = true;
                gridDevo.Columns.Item("Devolucion Especial 2").Editable = true;
                gridDevo.Columns.Item("Bodega Devolucion Especial 2").Editable = true;
                gridDevo.Columns.Item("Opt").Editable = true;

                gridDevo.Columns.Item("Cantidad a Devolver").RightJustified = true;
                gridDevo.Columns.Item("Cantidad en Obra").RightJustified = true;
                gridDevo.Columns.Item("Devolucion Especial 1").RightJustified = true;
                gridDevo.Columns.Item("Devolucion Especial 2").RightJustified = true;

                ((EditTextColumn)gridDevo.Columns.Item("Bodega Devolucion Especial 1")).ChooseFromListUID = "CFL_BE1";
                ((EditTextColumn)gridDevo.Columns.Item("Bodega Devolucion Especial 1")).ChooseFromListAlias = "WhsCode";
                ((EditTextColumn)gridDevo.Columns.Item("Bodega Devolucion Especial 2")).ChooseFromListUID = "CFL_BE2";
                ((EditTextColumn)gridDevo.Columns.Item("Bodega Devolucion Especial 2")).ChooseFromListAlias = "WhsCode";

                gridDevo.Columns.Item("Opt").Type = BoGridColumnType.gct_CheckBox;
                gridDevo.Columns.Item("Opt").TitleObject.Caption = "";
                gridDevo.Columns.Item("Opt").TitleObject.Sortable = false;
                gridDevo.Columns.Item("Interno Contrato").Visible = false;
                gridDevo.Columns.Item("U_BGT_LineNum").Visible = false;

                ((EditTextColumn)gridDevo.Columns.Item("Numero Contrato")).LinkedObjectType = "AGT_CTR";
                ((EditTextColumn)gridDevo.Columns.Item("Cliente")).LinkedObjectType = "2";

                gridDevo.AutoResizeColumns();
                gridDevo.CollapseLevel = 2;

                if (((string)dtDev.GetValue("Codigo Articulo", 0)).Equals(""))
                {
                    var noDataQuery = GetEmbeddedResource("bagant.Hana.NoDataFound.sql");
                    dtDev.ExecuteQuery(noDataQuery);
                    gridDevo.Item.Enabled = false;
                    Form.Items.Item("Item_35").Enabled = false;
                    return;
                }
            }
            finally
            {
                Form.Freeze(false);
                Form.State = BoFormStateEnum.fs_Maximized;
                B1Connections.SboApp.SetStatusBarMessage("Proceso finalizado", BoMessageTime.bmt_Medium, false);
            }
        }

        public void ShowResumeData(B1Forms FormParam)
        {
            Form = FormParam;

            try
            {
                var matrixNoData = (SAPbouiCOM.Matrix)Form.Items.Item("Item_17").Specific;
                matrixNoData.Item.FromPane = 10;
                matrixNoData.Item.ToPane = 10;
                matrixNoData.Item.Visible = false;

                var matrixRes = (SAPbouiCOM.Matrix)Form.Items.Item("Item_6").Specific;
                matrixRes.Item.FromPane = 3;
                matrixRes.Item.ToPane = 3;
                matrixRes.Item.Visible = true;

                var contrList = new List<string>();
                var listado = new Dictionary<string, List<Tuple<string, string, double>>>();
                var dt = Form.DataSources.DataTables.Item("DT_RsDvol");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var fila = i;
                    if (((string)dt.GetValue("Opt", fila)).Equals("Y"))
                    {
                        if (!listado.ContainsKey(dt.GetValue("Numero Contrato", fila).ToString()))
                        {
                            contrList.Add(dt.GetValue("Numero Contrato", fila).ToString());

                            var nroEnt = (string)dt.GetValue("Numero Contrato", fila).ToString();
                            var tuple = new Tuple<string, string, double>(((string)dt.GetValue("Cliente", fila).ToString()).Split('-')[0], dt.GetValue("Codigo Articulo", fila).ToString(), dt.GetValue("Cantidad a Devolver", fila));

                            listado.Add(nroEnt, new List<Tuple<string, string, double>>());
                            listado[nroEnt].Add(tuple);
                        }
                        else
                        {
                            var nroEnt = (string)dt.GetValue("Numero Contrato", fila).ToString();
                            listado[nroEnt].Add(new Tuple<string, string, double>(((string)dt.GetValue("Cliente", fila).ToString()).Split('-')[0], dt.GetValue("Codigo Articulo", fila).ToString(), dt.GetValue("Cantidad a Devolver", fila)));
                        }
                    }
                }

                if (listado.Count == 0)
                {
                    var noDataQuery = GetEmbeddedResource("bagant.Hana.NoDataFound.sql");
                    Form.DataSources.DataTables.Item("DT_DevRes").ExecuteQuery(noDataQuery);

                    SetNoDataMatrix(Form, matrixRes, dt);

                    matrixRes.Item.Enabled = false;
                    return;
                }

                var queryResumeReturns = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetResumeReturn.sql"), string.Join(",", contrList.ToArray()));
                var dtDevRes = Form.DataSources.DataTables.Item("DT_ResMas");
                dtDevRes.ExecuteQuery(queryResumeReturns);

                Log.Info("579");
                matrixRes.Columns.Item("#").DataBind.Bind("DT_ResMas", "RowNumber");
                matrixRes.Columns.Item("Col_0").DataBind.Bind("DT_ResMas", "Numero Contrato");
                matrixRes.Columns.Item("Col_1").DataBind.Bind("DT_ResMas", "Interno Contrato");
                matrixRes.Columns.Item("Col_2").DataBind.Bind("DT_ResMas", "Fecha Contrato");
                matrixRes.Columns.Item("Col_3").DataBind.Bind("DT_ResMas", "Fecha Facturacion");
                matrixRes.Columns.Item("Col_4").DataBind.Bind("DT_ResMas", "Comentarios");
                matrixRes.Columns.Item("Col_5").DataBind.Bind("DT_ResMas", "Propietario");
                matrixRes.Columns.Item("Col_6").DataBind.Bind("DT_ResMas", "Numero Guia Remision");
                matrixRes.Columns.Item("Col_7").DataBind.Bind("DT_ResMas", "Transportista");
                matrixRes.Columns.Item("Col_8").DataBind.Bind("DT_ResMas", "Placa Camion");
                matrixRes.Columns.Item("Col_9").DataBind.Bind("DT_ResMas", "Interno Propietario");

                matrixRes.Columns.Item("Col_5").ChooseFromListUID = "CFL_Prop";
                matrixRes.Columns.Item("Col_5").ChooseFromListAlias = "lastName";
                matrixRes.Columns.Item("Col_7").ChooseFromListUID = "CFL_Trspta";
                matrixRes.Columns.Item("Col_7").ChooseFromListAlias = "Code";
                matrixRes.Columns.Item("Col_8").ChooseFromListUID = "CFL_Trnsp";
                matrixRes.Columns.Item("Col_8").ChooseFromListAlias = "Code";

                matrixRes.Columns.Item("Col_1").Visible = false;
                matrixRes.Columns.Item("Col_9").Visible = false;

                matrixRes.LoadFromDataSource();
                matrixRes.AutoResizeColumns();

                for (int i = 0; i < 3; i++)
                    matrixRes.Columns.Item(i).Editable = false;

                matrixRes.Columns.Item("Col_2").Editable = true;
            }
            catch (Exception ex)
            {
                Log.Info("Exception (ShowResumeData) => " + ex.Message);
            }
        }

        public void CreateReturnForm(B1Forms FormParam)
        {
            Form = FormParam;
            B1Connections.SboApp.SetStatusBarMessage("Comenzando la creacion de devoluciones, por favor espere ....", BoMessageTime.bmt_Medium, false);

            try
            {
                var valuesUDF = new Dictionary<string, Tuple<string, string, string, string, string, string, DateTime>>();
                var dtLog = Form.DataSources.DataTables.Item("DT_DevRes");
                var dtRes = Form.DataSources.DataTables.Item("DT_RsDvol");
                var dtFields = Form.DataSources.DataTables.Item("DT_ResMas");
                var matrixRes = (SAPbouiCOM.Matrix)Form.Items.Item("Item_6").Specific;
                matrixRes.FlushToDataSource();

                var listado = new Dictionary<string, List<Tuple<string, string, double, string, double, string, double>>>();
                var listAdic = new Dictionary<string, List<Tuple<string, double, string>>>();
                var gridLog = (Grid)Form.Items.Item("Item_16").Specific;

                for (int i = 0; i < dtRes.Rows.Count; i++)
                {
                    var fila = i;
                    if (((string)dtRes.GetValue("Opt", fila)).Equals("Y"))
                    {
                        var nroEnt = (string)dtRes.GetValue("Numero Contrato", fila).ToString();
                        if (dtRes.GetValue("Cantidad a Devolver", fila) > 0)
                        {
                            Log.Info("ItemDev => " + dtRes.GetValue("Codigo Articulo", fila).ToString());
                            Log.Info("CantDev => " + dtRes.GetValue("Cantidad a Devolver", fila));

                            if (!listado.ContainsKey(dtRes.GetValue("Numero Contrato", fila).ToString()))
                            {
                                listado.Add(nroEnt, new List<Tuple<string, string, double, string, double, string, double>>());
                                listado[nroEnt].Add(new Tuple<string, string, double, string, double, string, double>(dtRes.GetValue("Cliente", fila), dtRes.GetValue("Codigo Articulo", fila).ToString(), dtRes.GetValue("Cantidad a Devolver", fila), dtRes.GetValue("Bodega Devolucion Especial 1", fila).ToString(), dtRes.GetValue("Devolucion Especial 1", fila), dtRes.GetValue("Bodega Devolucion Especial 2", fila).ToString(), dtRes.GetValue("Devolucion Especial 2", fila)));
                            }
                            else
                            {
                                listado[nroEnt].Add(new Tuple<string, string, double, string, double, string, double>(dtRes.GetValue("Cliente", fila), dtRes.GetValue("Codigo Articulo", fila).ToString(), dtRes.GetValue("Cantidad a Devolver", fila), dtRes.GetValue("Bodega Devolucion Especial 1", fila).ToString(), dtRes.GetValue("Devolucion Especial 1", fila), dtRes.GetValue("Bodega Devolucion Especial 2", fila).ToString(), dtRes.GetValue("Devolucion Especial 2", fila)));
                            }
                        }


                        if (dtRes.GetValue("Devolucion Especial 1", fila) > 0 || dtRes.GetValue("Devolucion Especial 2", fila) > 0)
                        {
                            if (!listAdic.ContainsKey(dtRes.GetValue("Numero Contrato", fila).ToString()))
                            {
                                if (dtRes.GetValue("Devolucion Especial 1", fila) > 0)
                                {
                                    listAdic.Add(nroEnt, new List<Tuple<string, double, string>>());
                                    listAdic[nroEnt].Add(new Tuple<string, double, string>(dtRes.GetValue("Codigo Articulo", fila).ToString(), dtRes.GetValue("Devolucion Especial 1", fila), dtRes.GetValue("Bodega Devolucion Especial 1", fila).ToString()));
                                }
                                if (dtRes.GetValue("Devolucion Especial 2", fila) > 0)
                                {
                                    listAdic.Add(nroEnt, new List<Tuple<string, double, string>>());
                                    listAdic[nroEnt].Add(new Tuple<string, double, string>(dtRes.GetValue("Codigo Articulo", fila).ToString(), dtRes.GetValue("Devolucion Especial 2", fila), dtRes.GetValue("Bodega Devolucion Especial 2", fila).ToString()));
                                }
                            }
                            else
                            {
                                if (dtRes.GetValue("Devolucion Especial 1", fila) > 0)
                                    listAdic[nroEnt].Add(new Tuple<string, double, string>(dtRes.GetValue("Codigo Articulo", fila).ToString(), dtRes.GetValue("Devolucion Especial 1", fila), dtRes.GetValue("Bodega Devolucion Especial 1", fila).ToString()));

                                if (dtRes.GetValue("Devolucion Especial 2", fila) > 0)
                                    listAdic[nroEnt].Add(new Tuple<string, double, string>(dtRes.GetValue("Codigo Articulo", fila).ToString(), dtRes.GetValue("Devolucion Especial 2", fila), dtRes.GetValue("Bodega Devolucion Especial 2", fila).ToString()));

                            }
                        }
                    }

                }

                for (int i = 0; i < dtFields.Rows.Count; i++)
                {
                    var contrato = dtFields.GetValue("Numero Contrato", i).ToString();
                    var tupla = new Tuple<string, string, string, string, string, string, DateTime>(
                                                                                    dtFields.GetValue("Interno Contrato", i).ToString(),
                                                                                    dtFields.GetValue("Comentarios", i).ToString(),
                                                                                    dtFields.GetValue("Interno Propietario", i).ToString(),
                                                                                    dtFields.GetValue("Numero Guia Remision", i).ToString(),
                                                                                    dtFields.GetValue("Transportista", i).ToString(),
                                                                                    dtFields.GetValue("Placa Camion", i).ToString(),
                                                                                    dtFields.GetValue("Fecha Facturacion", i)
                                                                                 );

                    if (valuesUDF.ContainsKey(contrato))
                        valuesUDF.Add(contrato, tupla);
                    else
                        valuesUDF[contrato] = tupla;
                }

                var entrega = (Documents)B1Connections.DiCompany.GetBusinessObject(BoObjectTypes.oDeliveryNotes);
                for (int i = 0; i < listado.Count; i++)
                {
                    B1Connections.SboApp.SetStatusBarMessage("Creacion devolucion numero " + (i + 1) + " de " + listado.Count, BoMessageTime.bmt_Medium, false);

                    var contrato = listado.ElementAt(i).Key;
                    var querySerie = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetSerieFromContract.sql"), contrato, "'C_DV', 'G_DV', 'M_DV', 'U_DV'");
                    var queryVendedor = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetContractDataDevo.sql"), contrato);
                    var respSerie = Record.Instance.Query(querySerie).Execute().First();

                    var devolucion = (Documents)B1Connections.DiCompany.GetBusinessObject(BoObjectTypes.oReturns);
                    devolucion.CardCode = listado[contrato][0].Item1.Split('-')[0].Trim();
                    devolucion.Comments = valuesUDF[contrato].Item2;
                    devolucion.DocumentsOwner = int.Parse(valuesUDF[contrato].Item3);
                    devolucion.UserFields.Fields.Item("U_NUM_GUIA").Value = valuesUDF[contrato].Item4;
                    devolucion.UserFields.Fields.Item("U_TRANSPORTISTA").Value = valuesUDF[contrato].Item5;
                    devolucion.UserFields.Fields.Item("U_TRANSPORTE").Value = valuesUDF[contrato].Item6;
                    devolucion.UserFields.Fields.Item("U_BGT_ContAsoc").Value = contrato;
                    devolucion.UserFields.Fields.Item("U_BGT_FecFact").Value = valuesUDF[contrato].Item7;

                    if (!string.IsNullOrEmpty(respSerie["Serie"]))
                        devolucion.Series = int.Parse(respSerie["Serie"]);

                    var respVendedor = Record.Instance.Query(queryVendedor).Execute().First();
                    devolucion.SalesPersonCode = int.Parse(respVendedor["U_BGT_SlpCode"]);

                    for (int j = 0; j < listado[contrato].Count; j++)
                    {
                        var sn = listado[contrato][j].Item1;
                        var item = listado[contrato][j].Item2;
                        var cantDev = listado[contrato][j].Item3;
                        var checkDeliver = B1Util.Formato(GetEmbeddedResource("bagant.Hana.CheckDataReturnFromDelivery.sql"), contrato, item);
                        var respDeliver = Record.Instance.Query(checkDeliver).Execute().All();

                        for (int x = 0; x < respDeliver.Length; x++)
                        {
                            if (entrega.GetByKey(int.Parse(respDeliver[x]["DocEntry"])))
                            {
                                entrega.Lines.SetCurrentLine(int.Parse(respDeliver[x]["LineNum"]));

                                if (double.Parse(respDeliver[x]["OpenQty"]) >= cantDev)
                                {
                                    devolucion.Lines.ItemCode = entrega.Lines.ItemCode;
                                    devolucion.Lines.Quantity = cantDev;
                                    devolucion.Lines.WarehouseCode = entrega.Lines.WarehouseCode;
                                    devolucion.Lines.UnitPrice = 0;
                                    //devolucion.Lines.LineTotal = 0;
                                    devolucion.Lines.BaseEntry = entrega.DocEntry;
                                    devolucion.Lines.BaseLine = entrega.Lines.LineNum;
                                    devolucion.Lines.BaseType = 15;

                                    devolucion.Lines.Add();

                                    break;
                                }
                                else if (double.Parse(respDeliver[x]["OpenQty"]) < cantDev)
                                {
                                    cantDev -= double.Parse(respDeliver[x]["OpenQty"]);

                                    devolucion.Lines.ItemCode = entrega.Lines.ItemCode;
                                    devolucion.Lines.Quantity = double.Parse(respDeliver[x]["OpenQty"]); // cantDev;
                                    devolucion.Lines.WarehouseCode = entrega.Lines.WarehouseCode;
                                    devolucion.Lines.UnitPrice = 0;
                                    //devolucion.Lines.LineTotal = 0;
                                    devolucion.Lines.BaseEntry = entrega.DocEntry;
                                    devolucion.Lines.BaseLine = entrega.Lines.LineNum;
                                    devolucion.Lines.BaseType = 15;

                                    devolucion.Lines.Add();
                                }
                            }
                        }
                    }

                    for (int x = 0; x < listAdic.Count; x++)
                    {
                        var contratoKey = listAdic.ElementAt(x).Key;

                        for (int y = 0; y < listAdic[contratoKey].Count; y++)
                        {
                            devolucion.Lines.ItemCode = listAdic[contratoKey][y].Item1;
                            devolucion.Lines.Quantity = listAdic[contratoKey][y].Item2;
                            devolucion.Lines.WarehouseCode = listAdic[contratoKey][y].Item3;
                            devolucion.Lines.Add();
                        }
                    }

                    var codResp = devolucion.Add();
                    if (codResp == 0)
                    {
                        var docEntryDevol = B1Connections.DiCompany.GetNewObjectKey();
                        devolucion.GetByKey(int.Parse(docEntryDevol));

                        var sapServ = new SAPServices();
                        sapServ.AddMovementData(docEntryDevol, "ORDN", "RDN1", "16");
                        sapServ.CheckUDOContract(docEntryDevol, "ORDN", "16");

                        dtLog.Rows.Add();
                        dtLog.SetValue("Estado", dtLog.Rows.Count - 1, "Exito");
                        dtLog.SetValue("Interno Devolucion", dtLog.Rows.Count - 1, docEntryDevol.ToString());
                        dtLog.SetValue("Numero Devolucion", dtLog.Rows.Count - 1, devolucion.DocNum.ToString());
                        dtLog.SetValue("Codigo Error", dtLog.Rows.Count - 1, "0");
                        dtLog.SetValue("Mensaje Error", dtLog.Rows.Count - 1, "Creacion Exitosa");
                    }
                    else
                    {
                        dtLog.Rows.Add();
                        dtLog.SetValue("Estado", dtLog.Rows.Count - 1, "Fallo");
                        dtLog.SetValue("Interno Devolucion", dtLog.Rows.Count - 1, "");
                        dtLog.SetValue("Numero Devolucion", dtLog.Rows.Count - 1, "");
                        dtLog.SetValue("Codigo Error", dtLog.Rows.Count - 1, codResp);
                        dtLog.SetValue("Mensaje Error", dtLog.Rows.Count - 1, B1Connections.DiCompany.GetLastErrorDescription());
                    }
                }

                gridLog.Columns.Item("Interno Devolucion").Visible = false;
                ((EditTextColumn)gridLog.Columns.Item("Numero Devolucion")).LinkedObjectType = "16";
                Form.Items.Item("Item_2").Enabled = false;
                ((Button)Form.Items.Item("Item_34").Specific).Caption = "Finalizar";
            }
            catch (Exception ex)
            {
                Log.Info("(Exception) CreateReturnForm => " + ex.Message);
            }

            B1Connections.SboApp.SetStatusBarMessage("Proceso finalizado.", BoMessageTime.bmt_Medium, false);
        }

        private void SetNoDataMatrix(B1Forms Form, SAPbouiCOM.Matrix matrix, DataTable dt)
        {
            var matrixNoData = (SAPbouiCOM.Matrix)Form.Items.Item("Item_17").Specific;
            var noDataQuery = GetEmbeddedResource("bagant.Hana.NoDataFoundMatrix.sql");
            dt.ExecuteQuery(noDataQuery);

            matrix.Item.FromPane = 10;
            matrix.Item.Top = 10;
            matrix.Item.Visible = false;

            matrixNoData.Item.Visible = true;
            matrixNoData.Item.Width = matrix.Item.Width;
            matrixNoData.Item.Height = matrix.Item.Height;
            matrixNoData.Item.Left = matrix.Item.Left;
            matrixNoData.Item.Top = matrix.Item.Top;
            matrixNoData.Item.Visible = true;
            matrixNoData.Item.FromPane = 2;
            matrixNoData.Item.ToPane = 2;

            matrixNoData.Columns.Item("#").DataBind.Bind("DT_DevMas", "RowNumber");
            matrixNoData.Columns.Item("Col_0").DataBind.Bind("DT_DevMas", "Mensaje");
            matrixNoData.Item.Enabled = false;
            matrixNoData.LoadFromDataSource();
        }

        public void SetDeliveryToReturnDetailFinded(B1Forms FormParam)
        {
            try
            {
                Form = FormParam;
                B1Connections.SboApp.SetStatusBarMessage("Realizando Busqueda, por favor espere....", BoMessageTime.bmt_Medium, false);

                var grid = (Grid)Form.Items.Item("Item_15").Specific;
                grid.Item.Enabled = true;

                var entrData = Form.DataSources.DataTables.Item("DT_SelDev");
                var listEntry = new List<string>();

                for (int i = 0; i < entrData.Rows.Count; i++)
                {
                    if (((string)entrData.GetValue("Opt", i)).Equals("Y"))
                        listEntry.Add(entrData.GetValue("Interno Entrega", i).ToString());
                }

                var cadena = string.Join(",", listEntry.ToArray());
                var queryDetailDelivery = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetReturnsDocuments.sql"), string.IsNullOrEmpty(cadena) ? "-1" : cadena);
                var devDetData = Form.DataSources.DataTables.Item("DT_DevDet");
                devDetData.ExecuteQuery(queryDetailDelivery);

                if (((string)grid.DataTable.GetValue("Codigo Obra", 0).ToString()).Equals(""))
                {
                    var noDataQuery = GetEmbeddedResource("bagant.Hana.NoDataFound.sql");
                    devDetData.ExecuteQuery(noDataQuery);
                    grid.Item.Enabled = false;
                    Form.Items.Item("Item_33").Enabled = false;
                    return;
                }

                ((EditTextColumn)grid.Columns.Item("Numero Contrato")).LinkedObjectType = "AGT_CTR";
                ((EditTextColumn)grid.Columns.Item("Nombre Cliente")).LinkedObjectType = "2";
                ((EditTextColumn)grid.Columns.Item("Codigo Articulo")).LinkedObjectType = "4";
                ((EditTextColumn)grid.Columns.Item("Numero Entrega")).LinkedObjectType = "15";

                grid.Columns.Item("Nombre Cliente").Editable = false;
                grid.Columns.Item("Fecha Contrato").Editable = false;
                grid.Columns.Item("Codigo Articulo").Editable = false;
                grid.Columns.Item("Cantidad En Obra").Editable = false;
                grid.Columns.Item("Descripcion Articulo").Editable = false;
                grid.Columns.Item("Cantidad de Contrato").Editable = false;
                grid.Columns.Item("Cantidad a Devolver").Editable = true;
                grid.Columns.Item("Stock Disponible").Editable = false;
                grid.Columns.Item("Stock Pendiente Contrato").Editable = false;
                grid.Columns.Item("Precio Unitario").Visible = false;
                grid.Columns.Item("Codigo Obra").Editable = false;
                grid.Columns.Item("Codigo Obra").Visible = false;
                grid.Columns.Item("Nombre Obra").Editable = false;
                grid.Columns.Item("Interno Entrega").Visible = false;
                grid.Columns.Item("Interno Contrato").Visible = false;
                grid.Columns.Item("Opt").Type = BoGridColumnType.gct_CheckBox;
                grid.Columns.Item("Opt").TitleObject.Caption = "";
                grid.CollapseLevel = 3;

                Form.State = BoFormStateEnum.fs_Maximized;
                B1Connections.SboApp.SetStatusBarMessage("Proceso finalizado", BoMessageTime.bmt_Medium, false);
            }
            catch (Exception ex)
            {
                Log.Info("Exception (SetDeliveryFinded) => " + ex.Message);
            }
        }

        private void AddAditionalRows(ref Documents document, B1Forms formParam, string nroCont)
        {
            Form = formParam;
            var matrixAd = (SAPbouiCOM.Matrix)Form.Items.Item("Item_42").Specific;
            for (int i = 1; i <= matrixAd.RowCount; i++)
            {
                matrixAd.GetLineData(i);
                if (Form.DataSources.UserDataSources.Item("UD_Cont").Value.Equals(nroCont))
                {
                    document.Lines.ItemCode = Form.DataSources.UserDataSources.Item("UD_Art").Value;
                    document.Lines.Quantity = double.Parse(Form.DataSources.UserDataSources.Item("UD_Qty").Value);
                    document.Lines.Add();
                }

            }
        }

        private Tuple<string, string, double> WasItemSelected(Dictionary<string, List<Tuple<string, string, double>>> listado, string indice, string itemCode)
        {
            for (int i = 0; i < listado[indice].Count; i++)
            {
                if (listado[indice][i].Item2.Equals(itemCode))
                    return listado[indice][i];
            }

            return null;
        }

        public void CreateInvoiceForm(B1Forms FormParam, string fecIni, string fecFin)
        {
            Form = FormParam;
            B1Connections.SboApp.Menus.Item("2053").Activate();
            var formFactura = B1Connections.SboApp.Forms.ActiveForm;

            try
            {
                formFactura.Freeze(true);
                ((EditText)formFactura.Items.Item("U_BGT_ContAsoc").Specific).Value = Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("DocNum", 0);
                var formUDF = B1Connections.SboApp.Forms.Item(formFactura.UDFFormUID);
                var comboSucursal = GetSucursalCode(((ComboBox)Form.Items.Item("Item_94").Specific).Selected.Value);


                //((EditText)formUDF.Items.Item("U_BGT_ContAsoc").Specific).Active = true;
                //((EditText)formUDF.Items.Item("U_BGT_ContAsoc").Specific).Value = Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("DocNum", 0);                                
                //((EditText)formUDF.Items.Item("U_BGT_ContAsoc").Specific).Active = false;
                
                Log.Info("1042");
                ((ComboBox)formUDF.Items.Item("U_DOC_DECLARABLE").Specific).Select("S", BoSearchKey.psk_ByValue);
                //((ComboBox)formUDF.Items.Item("U_EXX_FE_TIPCOM").Specific).Select("01", BoSearchKey.psk_ByValue);
                Log.Info("1045");
                //((ComboBox)formUDF.Items.Item("U_EXX_FE_TIPAMB").Specific).Select("1", BoSearchKey.psk_ByValue);
                Log.Info("1047");
                ((EditText)formUDF.Items.Item("U_EXX_FE_CODNUM").Specific).Value = "9999999999";
                Log.Info("1048");
                //((ComboBox)formUDF.Items.Item("U_EXX_FE_TIPEMI").Specific).Select("1", BoSearchKey.psk_ByValue);
                Log.Info("1049");
                ((EditText)formUDF.Items.Item("U_SER_EST").Specific).Value = comboSucursal;
                Log.Info("1050");
                //Rangos de fecha para guardar en la factura
                formUDF.Items.Item("U_IXX_COD_COM").Enabled = true;
                formUDF.Items.Item("U_BGT_RngIni").Enabled = true;
                formUDF.Items.Item("U_BGT_RngFin").Enabled = true;
                ((EditText)formUDF.Items.Item("U_BGT_RngIni").Specific).Value = fecIni;
                ((EditText)formUDF.Items.Item("U_BGT_RngFin").Specific).Value = fecFin;
                ((EditText)formUDF.Items.Item("U_IXX_COD_COM").Specific).Value = ((EditText)Form.Items.Item("Item_95").Specific).Value;
                ((EditText)formUDF.Items.Item("U_OrdendeCompra").Specific).Value = ((EditText)Form.Items.Item("Item_8").Specific).Value;

                formFactura.Items.Item("114").Click();
                Log.Info("1052");
                ((ComboBox)formFactura.Items.Item("40").Specific).Select(Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("U_BGT_DestObra", 0), BoSearchKey.psk_ByValue);
                ((ComboBox)formFactura.Items.Item("226").Specific).Select(Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("U_BGT_DirecFE", 0), BoSearchKey.psk_ByValue);
                formFactura.Items.Item("138").Click();
                Log.Info("1053");
                ((EditText)formFactura.Items.Item("157").Specific).Value = ((EditText)Form.Items.Item("Item_53").Specific).Value;
                Log.Info("1054");
                ((ComboBox)formFactura.Items.Item("47").Specific).Select("-1", BoSearchKey.psk_ByValue);
                Log.Info("1055");
                formFactura.Items.Item("112").Click();
                formFactura.Items.Item("16").Click();
                var OwnAndSlp = GetOwner(Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("DocNum", 0));
                ((ComboBox)formFactura.Items.Item("20").Specific).Select(OwnAndSlp.Item4, BoSearchKey.psk_ByValue);
                //((EditText)formFactura.Items.Item("222").Specific).Value = OwnAndSlp.Item1;
                formFactura.Items.Item("U_BGT_ContAsoc").Enabled = false;
                formUDF.Items.Item("U_BGT_RngIni").Enabled = false;
                formUDF.Items.Item("U_BGT_RngFin").Enabled = false;
                formUDF.Items.Item("U_IXX_COD_COM").Enabled = false;
                ((EditText)formUDF.Items.Item("U_SER_PE").Specific).Value = "002";
                ((EditText)formUDF.Items.Item("U_NUM_AUTOR").Specific).Value = "9999999999";
                ((EditText)formUDF.Items.Item("U_tipo_comprob").Specific).Value = "18";

                /*CARGUE DEL GRID*/
                ((EditText)formFactura.Items.Item("4").Specific).Value = ((EditText)Form.Items.Item("Item_6").Specific).Value;
                var matrixCont = (SAPbouiCOM.Matrix)Form.Items.Item("Item_62").Specific;
                var matrixFact = (SAPbouiCOM.Matrix)formFactura.Items.Item("38").Specific;
                var queryFact = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetDataToInvoiceCalc.sql"), Form.DataSources.DBDataSources.Item(0).GetValue("DocEntry", 0), fecIni, fecFin);
                var resoFact = Record.Instance.Query(queryFact).Execute().All();

                //SAPbouiCOM.Columns OCols;
                //SAPbouiCOM.Column ocol;
                //SAPbouiCOM.Item oitem;
                //SAPbouiCOM.Matrix OMat;
                //oitem = formFactura.Items.Item("38");
                //OMat = oitem.Specific;
                //OCols = OMat.Columns;
                //ocol = OCols.Item("16");
                //ocol.Editable = true;
                for (int i = 0; i < resoFact.Length; i++)
                {

                    double total = double.Parse(resoFact[i]["Total"]) < 0 ? -1 * double.Parse(resoFact[i]["Total"]) : double.Parse(resoFact[i]["Total"]);
                    double totalporDia = double.Parse(resoFact[i]["Total"]) < 0 ? -1 * double.Parse(resoFact[i]["Total"]) : double.Parse(resoFact[i]["Total"]);
                    total = totalporDia * Convert.ToInt32(resoFact[i]["Dias"]);
                    Console.WriteLine(total);
                    ((EditText)matrixFact.GetCellSpecific("1", i + 1)).Value = resoFact[i]["ItemCode"];
                    ((EditText)matrixFact.GetCellSpecific("11", i + 1)).Value = resoFact[i]["Cantidad"];
                    ((EditText)matrixFact.GetCellSpecific("14", i + 1)).Value = total.ToString();
                    ((EditText)matrixFact.GetCellSpecific("15", i + 1)).Value = resoFact[i]["Dcto"];
                    ((EditText)matrixFact.GetCellSpecific("U_BGT_PrecporDia", i + 1)).Value = totalporDia.ToString();
                    ((EditText)matrixFact.GetCellSpecific("U_BGT_Dias", i + 1)).Value = resoFact[i]["Dias"];
                    ((EditText)matrixFact.GetCellSpecific("U_BGT_Tipo", i + 1)).Value = resoFact[i]["Tipo"];
                    ((EditText)matrixFact.GetCellSpecific("U_BGT_NumGui", i + 1)).Value = resoFact[i]["Guia"];
                    if (resoFact[i]["Guia"] != "")
                    {
                        ((EditText)matrixFact.GetCellSpecific("U_BGT_DocEntry2", i + 1)).Value = resoFact[i]["Tipo"] == "ENTREGA" ? resoFact[i]["DocEntry"] : null;
                        ((EditText)matrixFact.GetCellSpecific("U_BGT_DocEntry3", i + 1)).Value = resoFact[i]["Tipo"] == "DEVOLUCION" ? resoFact[i]["DocEntry"] : null;
                        ((EditText)matrixFact.GetCellSpecific("U_BGT_DocEntry4", i + 1)).Value = resoFact[i]["Tipo"] == "ENTREGA PRELIMINAR" ? resoFact[i]["DocEntry"] : null;
                    }
                }
                //ocol.Editable = false;


            }
            catch (Exception ex)
            {
                Log.Info("(CreateInvoiceForm) Exception => " + ex.Message);
            }
            finally
            {
                formFactura.Freeze(false);
            }
        }

        private string GetSucursalCode(string codSuc)
        {
            switch (codSuc)
            {
                case "UIO":
                    return "001";
                case "GYE":
                    return "002";
                case "CUE":
                    return "005";
                case "MTA":
                    return "007";
                default:
                    return string.Empty;
            }
        }


        public bool ValidateHeaderDevolutionFinded(B1Forms FormParam)
        {
            Form = FormParam;
            var matrixDataDevo = (SAPbouiCOM.Matrix)Form.Items.Item("Item_15").Specific;
            var gridDataDevo = (SAPbouiCOM.Grid)Form.Items.Item("Item_35").Specific;

            for (int i = 1; i <= matrixDataDevo.RowCount; i++)
            {
                var item = ((EditText)matrixDataDevo.GetCellSpecific("Col_3", i)).Value;
                if (string.IsNullOrEmpty(item))
                    continue;

                var txtCant1 = ((EditText)matrixDataDevo.GetCellSpecific("Col_11", i)).Value;
                var txtCant2 = ((EditText)matrixDataDevo.GetCellSpecific("Col_13", i)).Value;

                var seleccionado = ((CheckBox)matrixDataDevo.GetCellSpecific("Col_0", i)).Checked;
                var valueCant1 = string.IsNullOrEmpty(txtCant1) || txtCant1.Equals("0") || txtCant1.Equals("0.0") ? 0 : double.Parse(txtCant1);
                var valueCant2 = string.IsNullOrEmpty(txtCant2) || txtCant2.Equals("0") || txtCant2.Equals("0.0") ? 0 : double.Parse(txtCant2);

                if (seleccionado)
                {
                    if (valueCant1 > 0)
                    {
                        var alm1 = ((EditText)matrixDataDevo.GetCellSpecific("Col_12", i)).Value;
                        if (string.IsNullOrEmpty(alm1))
                        {
                            B1Connections.SboApp.SetStatusBarMessage("No se ha indicado el almacen para la cantidad de devolucion especial 1  - Fila => " + i);
                            return false;
                        }
                    }
                    if (valueCant2 > 0)
                    {
                        var alm2 = ((EditText)matrixDataDevo.GetCellSpecific("Col_14", i)).Value;
                        if (string.IsNullOrEmpty(alm2))
                        {
                            B1Connections.SboApp.SetStatusBarMessage("No se ha indicado el almacen para la cantidad de devolucion especial 2 - Fila => " + i);
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public void CreateDeliveryForm(B1Forms FormParam)
        {
            B1Connections.SboApp.SetStatusBarMessage("Realizando proceso de mostrar entregas, por favor espere....", BoMessageTime.bmt_Medium, false);

            try
            {
                Form = FormParam;
                //Form.Freeze(true);
                //Form.Mode = BoFormMode.fm_ADD_MODE;
                var listado = new Dictionary<string, List<Tuple<string, string, double, double>>>();
                var grid = (Grid)Form.Items.Item("Item_5").Specific;
                for (int i = 0; i < grid.Rows.Count; i++)
                {
                    if (grid.GetDataTableRowIndex(i) != -1)
                    {
                        var fila = grid.GetDataTableRowIndex(i);
                        if (((string)grid.DataTable.GetValue("Opt", fila)).Equals("Y"))
                        {
                            var codigoCliente = ((string)grid.DataTable.GetValue("Nombre Cliente", fila).ToString()).Split('-')[0];
                            if (!listado.ContainsKey(grid.DataTable.GetValue("Numero Contrato", fila).ToString()))
                            {
                                var nroCont = (string)grid.DataTable.GetValue("Numero Contrato", fila).ToString();
                                var tuple = new Tuple<string, string, double, double>(codigoCliente, grid.DataTable.GetValue("Codigo Articulo", fila), grid.DataTable.GetValue("A Entregar", fila), grid.DataTable.GetValue("Precio Unitario", fila));

                                listado.Add(nroCont, new List<Tuple<string, string, double, double>>());
                                listado[nroCont].Add(tuple);
                            }
                            else
                            {
                                Log.Info("Test Price => " + grid.DataTable.GetValue("Precio Unitario", fila));
                                var nroCont = (string)grid.DataTable.GetValue("Numero Contrato", fila).ToString();
                                listado[nroCont].Add(new Tuple<string, string, double, double>(codigoCliente, grid.DataTable.GetValue("Codigo Articulo", fila), grid.DataTable.GetValue("A Entregar", fila), grid.DataTable.GetValue("Precio Unitario", fila)));
                            }
                        }
                    }
                }

                for (int i = 0; i < listado.Count; i++)
                {
                    B1Connections.SboApp.Menus.Item("2051").Activate();

                    var indice = listado.ElementAt(i).Key;
                    var formEnt = B1Connections.SboApp.Forms.ActiveForm;
                    formEnt.Freeze(true);

                    var queryValues = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetBPProject.sql"), indice);
                    var respValues = Record.Instance.Query(queryValues).Execute().First();

                    try
                    {
                        var matrix = (SAPbouiCOM.Matrix)formEnt.Items.Item("38").Specific;
                        ((EditText)formEnt.Items.Item("4").Specific).Value = listado[indice][0].Item1;

                        for (int j = 1; j <= listado[indice].Count; j++)
                        {
                            ((EditText)matrix.GetCellSpecific("1", j)).Value = listado[indice][j - 1].Item2;
                            ((EditText)matrix.GetCellSpecific("11", j)).Value = listado[indice][j - 1].Item3.ToString();
                            ((EditText)matrix.GetCellSpecific("14", j)).Value = listado[indice][j - 1].Item4.ToString();
                            ((EditText)matrix.GetCellSpecific("15", j)).Item.Click();
                        }

                        formEnt.Items.Item("138").Click();
                        ((EditText)formEnt.Items.Item("157").Specific).Value = respValues["U_BGT_Project"];
                    }
                    finally
                    {
                        formEnt.Items.Item("U_BGT_FecFact").Enabled = true;
                        ((EditText)formEnt.Items.Item("U_BGT_FecFact").Specific).Value = GetDateFact(Form, indice);
                        formEnt.Items.Item("U_BGT_ContAsoc").Enabled = true;
                        ((EditText)formEnt.Items.Item("U_BGT_ContAsoc").Specific).Value = indice;

                        formEnt.Items.Item("114").Click();
                        ((ComboBox)formEnt.Items.Item("40").Specific).Select(respValues["U_BGT_DestObra"], BoSearchKey.psk_ByValue);
                        formEnt.Items.Item("112").Click();
                        formEnt.Items.Item("U_BGT_FecFact").Enabled = false;
                        formEnt.Items.Item("U_BGT_ContAsoc").Enabled = false;
                        var OwnAndSlp = GetOwner(indice);
                        ((ComboBox)formEnt.Items.Item("20").Specific).Select(OwnAndSlp.Item4, BoSearchKey.psk_ByValue);
                        //((EditText)formEnt.Items.Item("222").Specific).Value = OwnAndSlp.Item1;
                        //var querySerie = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetSerieFromContract.sql"), indice, "'ENT'");
                        //var respSerie = Record.Instance.Query(querySerie).Execute().First();
                        //if (!string.IsNullOrEmpty(respSerie["Serie"]))
                        //    ((ComboBox)formEnt.Items.Item("88").Specific).Select(respSerie["Serie"], BoSearchKey.psk_ByValue);

                        formEnt.Freeze(false);

                    }

                    Log.Info("UDF => " + formEnt.UDFFormUID);
                }

            }
            catch (Exception ex)
            {
                Log.Info("Exception (CreateDeliveryForm) => " + ex.Message);
            }
            finally
            {
                //Form.Freeze(false);
                Form.Close();
            }

            B1Connections.SboApp.SetStatusBarMessage("Proceso finalizado.", BoMessageTime.bmt_Medium, false);
        }

        private string GetDateFact(B1Forms FormParam, string nroContract)
        {
            Form = FormParam;

            var gridRes = (Grid)Form.Items.Item("Item_37").Specific;
            var dtRes = gridRes.DataTable;

            for (int i = 0; i < dtRes.Rows.Count; i++)
            {
                if (dtRes.GetValue("Numero Contrato", i).ToString().Equals(nroContract))
                    return dtRes.GetValue("Fecha Facturacion", i).ToString("yyyyMMdd");
            }

            return "";
        }

        private Tuple<string, string, string, string> GetOwner(string nroContract)
        {
            var queryValues = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetOwnerAndSlp.sql"), nroContract);
            var respValues = Record.Instance.Query(queryValues).Execute().First();
            return new Tuple<string, string, string, string>(
                respValues["U_BGT_OwnerId"],
                respValues["U_BGT_Owner"],
                respValues["U_BGT_SlpCode"],
                respValues["U_BGT_SlpName"]);
        }

        public static void OpenContractForm(string nroContract)
        {
            B1Connections.SboApp.Menus.Item("BGT_Contr").Activate();
            var formActiv = B1Connections.SboApp.Forms.ActiveForm;
            formActiv.Mode = BoFormMode.fm_OK_MODE;
            B1Connections.SboApp.Menus.Item("1281").Activate();
            ((EditText)formActiv.Items.Item("Item_45").Specific).Value = nroContract;
            formActiv.Items.Item("1").Click();
        }

        public void AddRowToMatrix(B1Forms FormParam, SAPbouiCOM.Matrix table, int bdlevel)
        {
            Form = FormParam;
            Form.DataSources.DBDataSources.Item(bdlevel).InsertRecord(Form.DataSources.DBDataSources.Item(bdlevel).Size);
            Form.DataSources.DBDataSources.Item(bdlevel).Offset = Form.DataSources.DBDataSources.Item(bdlevel).Size - 1;
            table.AddRow(1);

            NumberingRowTable(table);
        }

        private void NumberingRowTable(SAPbouiCOM.Matrix table)
        {
            for (int i = 1; i <= table.RowCount; i++)
                table.SetCellWithoutValidation(table.RowCount, "#", i.ToString());
        }

        public void CopyUDFFieldsForm(B1Forms formOrigUDF, B1Forms FormExcesoUDF)
        {
            try
            {
                FormExcesoUDF.Freeze(true);
                for (int i = 0; i < formOrigUDF.Items.Count; i++)
                {
                    if (formOrigUDF.Items.Item(i).UniqueID.Contains("U_"))
                    {
                        if (formOrigUDF.Items.Item(i).Type == BoFormItemTypes.it_EDIT || formOrigUDF.Items.Item(i).Type == BoFormItemTypes.it_EXTEDIT)
                        {
                            if (formOrigUDF.Items.Item(i).Enabled)
                                ((EditText)FormExcesoUDF.Items.Item(i).Specific).Value = ((EditText)formOrigUDF.Items.Item(i).Specific).Value;
                        }
                        else if (formOrigUDF.Items.Item(i).Type == BoFormItemTypes.it_COMBO_BOX)
                        {
                            if (((ComboBox)FormExcesoUDF.Items.Item(i).Specific).Selected != null)
                                ((ComboBox)FormExcesoUDF.Items.Item(i).Specific).Select(((ComboBox)formOrigUDF.Items.Item(i).Specific).Selected.Value, BoSearchKey.psk_ByValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info("(CopyUDFFieldsForm) Exception => " + ex.Message);
            }
            finally
            {
                FormExcesoUDF.Freeze(false);
            }
        }


    }
}