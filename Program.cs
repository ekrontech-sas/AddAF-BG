using B1Framework.B1Frame;
using B1Framework.RecordSet;
//using bagant.Services;
using System;
using System.Reflection;
using System.Xml;
using SAPbouiCOM;
using SAPbobsCOM;

namespace bagant
{
    class Program : B1AddOn
    {
        public static class Globals
        {
            public static string pFormD { get; set; }
            public static string pRowD { get; set; }
            public static string pColD { get; set; }
            public static string DocEntryDR { get; set; }
            public static string DocNumD { get; set; }
            public static string NoGuiaD { get; set; }
            public static string pNumCtoDR { get; set; }
            public static string pEntrPre { get; set; }
        }

        public static class Frd
        {
            public static SAPbouiCOM.Form FormRD { get; set; }
            public static B1Forms FormRDN { get; set; }
        }

        public string pFormDN = "";
        public SAPbouiCOM.Form FormR;


        public void LoadProgram(Assembly asm, string connStr, int appid)
        {
            SetAssembly(asm);
            loadConnection(connStr, appid);
            loadResources(connStr, appid, asm, "bagant.Menu.xml.addMenus.xml");

            B1Connections.SboApp.SetStatusBarMessage("Addin Rental AF Iniciado", SAPbouiCOM.BoMessageTime.bmt_Medium, false);
            //B1Connections.SboApp.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SboApp_ItemEvent);
            B1Connections.SboApp.MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(sboApp_MenuItemEvent);
            //B1Connections.SboApp.FormDataEvent += SboApp_FormDataEvent;
        }
        private void sboApp_MenuItemEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            var pDatoD = true;
            var pError = "0";
            var pNumCto = "";
            var pErrorDesc = "";

            if (pVal.MenuUID == "RemRowC")
            {
                if (Globals.pFormD == "2002122020")
                {
                    //FormR = Frd.FormRD;
                    //var pValorD = FormR.Items.Item("Item_43").Specific;
                    //string pDatoN = pValorD.Value;
                    //if (pDatoN.Replace(" ", "") == "Activo" || pDatoN.Replace(" ", "") == "A")
                    //{
                    //    pErrorDesc = "No es posible Eliminar partidas porque el Contrato esta Activo..!!";
                    //    B1Connections.SboApp.SetStatusBarMessage(pErrorDesc, BoMessageTime.bmt_Medium);
                    //    pDatoD = false;
                    //}
                    SAPbouiCOM.Form FormR = Frd.FormRD;
                    var pValorD = FormR.Items.Item("Item_43").Specific;
                    string pDatoN = pValorD.Value;

                    var pNumCtoD = ((EditText)FormR.Items.Item("Item_45").Specific).Value.ToString();
                    var pItem = "";
                    var matrix = (SAPbouiCOM.Matrix)FormR.Items.Item("Item_62").Specific;
                    int selrow1 = matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
                    if (selrow1 != -1)
                    {
                        FormR.Mode = BoFormMode.fm_UPDATE_MODE;
                        pItem = matrix.Columns.Item(1).Cells.Item(selrow1).Specific.Value;
                        if (pNumCtoD != "" && pItem != "")
                        {
                            var queryContractData = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetMovmentByItemDelRow.sql", GetType().Assembly), pNumCtoD, pItem);
                            var resp = Record.Instance.Query(queryContractData).Execute().First();
                            if (resp.Count > 0)
                            {
                                var pDato = resp["Articulo"].ToString();
                                if (pDato == pItem)
                                {
                                    pErrorDesc = "No es posible Eliminar partidas porque se tienen articulos asignados en el Contrato..!!";
                                    B1Connections.SboApp.SetStatusBarMessage(pErrorDesc, BoMessageTime.bmt_Medium);
                                    pDatoD = false;
                                }
                                else
                                {
                                    //matrix.AddRow();
                                    FormR.Mode = BoFormMode.fm_UPDATE_MODE;
                                }
                            }
                        }
                    }


                }
            }

            if (pVal.MenuUID == "DuplicarCont1")
            {
                var pUser = B1Connections.SboApp.Company.UserName;
                SAPbouiCOM.Form FormR = Frd.FormRD;
                //FormR = B1Connections.SboApp.Forms.Item(Globals.pFormD);
                //FormR = new B1Forms(Globals.pFormD);
                var pNumCtoD = ((EditText)FormR.Items.Item("Item_45").Specific).Value.ToString();
            }

            if (pVal.MenuUID == "1283")
            {
                var pUser = B1Connections.SboApp.Company.UserName;
                //SAPbouiCOM.Form FormR = Frd.FormRD;

                //var queryCheckPre = (string)B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetEntregaPreliminarNoGuiaRemove.sql", GetType().Assembly), Globals.pNumCtoDR, Globals.NoGuiaD, int.Parse(Globals.DocEntryDR));
                //var respCheckPre = Record.Instance.Query(queryCheckPre).Execute().First();
                //if (int.Parse(respCheckPre["TotalNoGuia"]) == 1)
                //{
                //    Services.UDT_Table.RemoveRecordUDT("BGT_RECEPMAT", int.Parse(respCheckPre["Code"]));
                //}


            }

            if (pVal.MenuUID == "DuplicarCont1")
            {
                if (Globals.pFormD == "2002122020")
                {
                    pErrorDesc = "Menu..";
                    FormR = Frd.FormRD;

                    try
                    {
                        var pUser = B1Connections.SboApp.Company.UserName;
                        SAPbouiCOM.Form FormR = Frd.FormRD;
                        //FormR = B1Connections.SboApp.Forms.Item(Globals.pFormD);
                        //FormR = new B1Forms(Globals.pFormD);
                        var pNumCtoD = ((EditText)FormR.Items.Item("Item_45").Specific).Value.ToString();
                        FormR.Mode = BoFormMode.fm_ADD_MODE;
                        var queryContractData = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetDataContractAll.sql", GetType().Assembly), pNumCtoD, pUser);
                        var resp = Record.Instance.Query(queryContractData).Execute().First();
                        var pDato = resp["U_BGT_OrdComp"].ToString();

                        //var queryUser = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetDataContractAll.sql", GetType().Assembly), pNumCtoD);
                        //var respUser = Record.Instance.Query(queryContractData).Execute().First();


                        ((EditText)FormR.Items.Item("Item_8").Specific).Value = pDato;
                    //    var matrix = (SAPbouiCOM.Matrix)FormR.Items.Item("Item_62").Specific;
                    //    matrix.Clear();
                       // B1Forms Form = new B1Forms(B1Connections.SboApp.Forms.ActiveForm.UniqueID);

                        var matrix = (SAPbouiCOM.Matrix)FormR.Items.Item("Item_62").Specific;
                        
                        string pFechaDN = "";
                        pFechaDN = DateTime.Now.Year.ToString();
                        if (DateTime.Now.Month.ToString().Length == 2)
                        {
                            pFechaDN += DateTime.Now.Month.ToString();
                        }
                        else
                        {
                            pFechaDN += "0" + DateTime.Now.Month.ToString();
                        }
                        if (DateTime.Now.Day.ToString().Length == 2)
                        {
                            pFechaDN += DateTime.Now.Day.ToString();
                        }
                        else
                        {
                            pFechaDN += "0" + DateTime.Now.Day.ToString();
                        }

                        ((EditText)FormR.Items.Item("Item_14").Specific).Value = pFechaDN;

                        ((EditText)FormR.Items.Item("Item_6").Specific).Value = resp["U_BGT_CardCode"].ToString();

                        ((EditText)FormR.Items.Item("Item_18").Specific).Value = resp["U_BGT_CodHolding"].ToString();

                        SAPbouiCOM.ComboBox oComboBox21 = FormR.Items.Item("Item_21").Specific;
                        oComboBox21.Select(resp["U_BGT_CntcPerson"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                        SAPbouiCOM.ComboBox oComboBox23 = FormR.Items.Item("Item_23").Specific;
                        oComboBox23.Select(resp["U_BGT_DirecFE"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                        SAPbouiCOM.ComboBox oComboBox25 = FormR.Items.Item("Item_25").Specific;
                        oComboBox25.Select(resp["U_BGT_DestObra"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                        SAPbouiCOM.ComboBox oComboBox27 = FormR.Items.Item("Item_27").Specific;
                        oComboBox27.Select(resp["U_BGT_Territor"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                        FormR.Items.Item("Item_78").Enabled = true;
                        FormR.Items.Item("Item_94").Enabled = true;
                        FormR.Items.Item("Item_88").Enabled = true;
                        FormR.Items.Item("Item_108").Enabled = true;

                        SAPbouiCOM.ComboBox oComboBox78 = FormR.Items.Item("Item_78").Specific;
                        oComboBox78.Select(resp["U_BGT_PriceList"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                        SAPbouiCOM.ComboBox oComboBox94 = FormR.Items.Item("Item_94").Specific;
                        oComboBox94.Select(resp["U_BGT_CodSurc"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                        SAPbouiCOM.ComboBox oComboBox98 = FormR.Items.Item("Item_98").Specific;
                        oComboBox98.Select(resp["U_BGT_PerFact"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                        SAPbouiCOM.ComboBox oComboBox88 = FormR.Items.Item("Item_88").Specific;
                        oComboBox88.Select(resp["U_BGT_TipCalc"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                        ((EditText)FormR.Items.Item("Item_108").Specific).Value = resp["U_BGT_CicloFact"].ToString();

                        SAPbouiCOM.ComboBox oComboBox43 = FormR.Items.Item("Item_43").Specific;
                        oComboBox43.Select(resp["U_BGT_Status"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                        ((EditText)FormR.Items.Item("Item_37").Specific).Value = resp["U_BGT_DurCont"].ToString();
                        ((EditText)FormR.Items.Item("Item_39").Specific).Value = resp["U_BGT_DocNumOF"].ToString();

                        SAPbouiCOM.ComboBox oComboBox47 = FormR.Items.Item("Item_47").Specific;
                        oComboBox47.Select(resp["U_BGT_DistObra"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                        SAPbouiCOM.ComboBox oComboBox79 = FormR.Items.Item("Item_79").Specific;
                        oComboBox79.Select(resp["U_BGT_Transporte"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                        SAPbouiCOM.ComboBox oComboBox57 = FormR.Items.Item("Item_57").Specific;
                        oComboBox57.Select(resp["Series"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                        ((EditText)FormR.Items.Item("Item_51").Specific).Value = resp["U_BGT_NumOport"].ToString();
                        ((EditText)FormR.Items.Item("Item_55").Specific).Value = resp["U_BGT_ActRel"].ToString();

                        SAPbouiCOM.ComboBox oComboBox84 = FormR.Items.Item("Item_84").Specific;
                        oComboBox84.Select(resp["U_BGT_Categor"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                        ((EditText)FormR.Items.Item("Item_53").Specific).Value = resp["U_BGT_Project"].ToString();
                        ((EditText)FormR.Items.Item("Item_31").Specific).Value = resp["U_BGT_ProjectName"].ToString();
                        ((EditText)FormR.Items.Item("Item_95").Specific).Value = resp["U_BGT_NomComis"].ToString();
                        ((EditText)FormR.Items.Item("Item_41").Specific).Value = resp["U_BGT_SlpName"].ToString();
                        ((EditText)FormR.Items.Item("Item_33").Specific).Value = resp["OwnerD"].ToString();
                        ((EditText)FormR.Items.Item("Item_74").Specific).Value = resp["U_BGT_Comments"].ToString();
                        ((EditText)FormR.Items.Item("Item_65").Specific).Value = resp["U_BGT_ValRepEqOb"].ToString();
                        ((EditText)FormR.Items.Item("Item_67").Specific).Value = resp["U_BGT_ValRepCont"].ToString();
                        ((EditText)FormR.Items.Item("Item_69").Specific).Value = resp["U_BGT_ValGarant"].ToString();
                        ((EditText)FormR.Items.Item("Item_71").Specific).Value = resp["U_BGT_SubPesObra"].ToString();
                        ((EditText)FormR.Items.Item("Item_73").Specific).Value = resp["U_BGT_SubPesEnt"].ToString();

                          var table = (SAPbouiCOM.Matrix)FormR.Items.Item("Item_62").Specific;
                           var bdlevel = 1;
                           FormR.DataSources.DBDataSources.Item(bdlevel).InsertRecord(FormR.DataSources.DBDataSources.Item(bdlevel).Size);
                           FormR.DataSources.DBDataSources.Item(bdlevel).Offset = FormR.DataSources.DBDataSources.Item(bdlevel).Size - 1;
                           table.AddRow(1);
                           for (int i = 1; i <= table.RowCount; i++)
                               table.SetCellWithoutValidation(table.RowCount, "#", i.ToString());

                        FormR.ChooseFromLists.Item("CFL_ITM").SetConditions(null);
                        Services.UtilServices.NewSetCflConditionsR(FormR, "CFL_ITM",
                                                            new string[] { "ItemCode", "ItemCode" },
                                                            new BoConditionOperation[] { BoConditionOperation.co_CONTAIN, BoConditionOperation.co_CONTAIN },
                                                            new string[] { "AEA", "LOG" });
                    }
                    catch (System.Exception ex)
                    {
                        //Log.Info("Excp (SboApp_ItemEvent) => " + ex.Message);
                        pDatoD = false;
                    }
                    pDatoD = false;

                    //try
                    //{
                    //    var pNumCtoD = ((EditText)FormR.Items.Item("Item_45").Specific).Value.ToString();
                    //    FormR.Mode = BoFormMode.fm_ADD_MODE;
                    //    var queryContractData = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetDataContractAll.sql", GetType().Assembly), pNumCtoD);
                    //    var resp = Record.Instance.Query(queryContractData).Execute().First();
                    //    var pDato = resp["U_BGT_OrdComp"].ToString();
                    //    ((EditText)FormR.Items.Item("Item_8").Specific).Value = pDato;
                    //    ((EditText)FormR.Items.Item("Item_18").Specific).Value = resp["U_BGT_CodHolding"].ToString();

                    //    string pFechaDN = "";
                    //    pFechaDN = DateTime.Now.Year.ToString();
                    //    if (DateTime.Now.Month.ToString().Length == 2)
                    //    {
                    //        pFechaDN += DateTime.Now.Month.ToString();
                    //    }
                    //    else
                    //    {
                    //        pFechaDN += "0" + DateTime.Now.Month.ToString();
                    //    }
                    //    if (DateTime.Now.Day.ToString().Length == 2)
                    //    {
                    //        pFechaDN += DateTime.Now.Day.ToString();
                    //    }
                    //    else
                    //    {
                    //        pFechaDN += "0" + DateTime.Now.Day.ToString();
                    //    }

                    //    ((EditText)FormR.Items.Item("Item_14").Specific).Value = pFechaDN;

                    //    SAPbouiCOM.ComboBox oComboBox27 = FormR.Items.Item("Item_27").Specific;
                    //    oComboBox27.Select(resp["U_BGT_Territor"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                    //    SAPbouiCOM.ComboBox oComboBox78 = FormR.Items.Item("Item_78").Specific;
                    //    oComboBox78.Select(resp["U_BGT_PriceList"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                    //    SAPbouiCOM.ComboBox oComboBox94 = FormR.Items.Item("Item_94").Specific;
                    //    oComboBox94.Select(resp["U_BGT_CodSurc"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                    //    SAPbouiCOM.ComboBox oComboBox98 = FormR.Items.Item("Item_98").Specific;
                    //    oComboBox98.Select(resp["U_BGT_PerFact"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                    //    SAPbouiCOM.ComboBox oComboBox88 = FormR.Items.Item("Item_88").Specific;
                    //    oComboBox88.Select(resp["U_BGT_TipCalc"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                    //    ((EditText)FormR.Items.Item("Item_108").Specific).Value = resp["U_BGT_CicloFact"].ToString();

                    //    SAPbouiCOM.ComboBox oComboBox43 = FormR.Items.Item("Item_43").Specific;
                    //    oComboBox43.Select(resp["U_BGT_Status"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                    //    ((EditText)FormR.Items.Item("Item_37").Specific).Value = resp["U_BGT_DurCont"].ToString();
                    //    ((EditText)FormR.Items.Item("Item_39").Specific).Value = resp["U_BGT_DocNumOF"].ToString();

                    //    SAPbouiCOM.ComboBox oComboBox47 = FormR.Items.Item("Item_47").Specific;
                    //    oComboBox47.Select(resp["U_BGT_DistObra"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                    //    SAPbouiCOM.ComboBox oComboBox79 = FormR.Items.Item("Item_79").Specific;
                    //    oComboBox79.Select(resp["U_BGT_Transporte"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                    //    SAPbouiCOM.ComboBox oComboBox57 = FormR.Items.Item("Item_57").Specific;
                    //    oComboBox57.Select(resp["Series"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                    //    ((EditText)FormR.Items.Item("Item_51").Specific).Value = resp["U_BGT_NumOport"].ToString();
                    //    ((EditText)FormR.Items.Item("Item_55").Specific).Value = resp["U_BGT_ActRel"].ToString();

                    //    SAPbouiCOM.ComboBox oComboBox84 = FormR.Items.Item("Item_84").Specific;
                    //    oComboBox84.Select(resp["U_BGT_Categor"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

                    //    ((EditText)FormR.Items.Item("Item_53").Specific).Value = resp["U_BGT_Project"].ToString();
                    //    ((EditText)FormR.Items.Item("Item_31").Specific).Value = resp["U_BGT_ProjectName"].ToString();
                    //    ((EditText)FormR.Items.Item("Item_95").Specific).Value = resp["U_BGT_NomComis"].ToString();
                    //    ((EditText)FormR.Items.Item("Item_74").Specific).Value = resp["U_BGT_Comments"].ToString();
                    //    ((EditText)FormR.Items.Item("Item_65").Specific).Value = resp["U_BGT_ValRepEqOb"].ToString();
                    //    ((EditText)FormR.Items.Item("Item_67").Specific).Value = resp["U_BGT_ValRepCont"].ToString();
                    //    ((EditText)FormR.Items.Item("Item_69").Specific).Value = resp["U_BGT_ValGarant"].ToString();
                    //    ((EditText)FormR.Items.Item("Item_71").Specific).Value = resp["U_BGT_SubPesObra"].ToString();
                    //    ((EditText)FormR.Items.Item("Item_73").Specific).Value = resp["U_BGT_SubPesEnt"].ToString();
                    //}
                    //catch (System.Exception ex)
                    //{
                    //    //Log.Info("Excp (SboApp_ItemEvent) => " + ex.Message);
                    //    pDatoD = false;
                    //}
                    //pDatoD = false;
                }
            }

            if (pVal.MenuUID == "5907")
            {
                if (Globals.pFormD == "140")
                {
                    if (Globals.pEntrPre == "1")
                    {
                        Globals.pEntrPre = "0";
                        try
                        {
                            //Frd.FormRDN = new B1Forms("140");
                            //FormR = new B1Forms(Globals.pFormD);
                            FormR = Frd.FormRD;
                            FormR.Items.Item("U_BGT_FecFact").Enabled = false;
                            FormR.Items.Item("U_BGT_ContAsoc").Enabled = false;
                            //FormR.Items.Item("U_NUM_GUIA").Enabled = false;
                            FormR.Items.Item("U_BGT_DevProv").Enabled = false;
                            pNumCto = ((EditText)FormR.Items.Item("U_BGT_ContAsoc").Specific).Value;
                            var fecFact = ((EditText)FormR.Items.Item("U_BGT_FecFact").Specific).Value;

                            if (string.IsNullOrEmpty(fecFact) || fecFact.Equals("0") || fecFact.Equals("0.0"))
                            {
                                pErrorDesc = "No puede dejar el campo de Fecha de Facturacion Vacio.";
                                pError = "1";
                            }


                            var iNumGuiaD = ((EditText)FormR.Items.Item("U_NUM_GUIA").Specific);
                            var iNumeroGuia = ((EditText)FormR.Items.Item("U_NUM_GUIA").Specific).Value;
                            int valor = 0;
                            if (!int.TryParse(iNumeroGuia.ToString(), out valor))
                            {
                                pErrorDesc = "Solo se admite numeros";
                                iNumGuiaD.Value = "0";
                                pError = "1";
                            }

                            if (iNumeroGuia.ToString().Length > 9)
                            {
                                pErrorDesc = "Numero de caracteres permitidos 9";
                                iNumGuiaD.Value = "0";
                                pError = "1";
                            }
                            if (iNumeroGuia.ToString().Length > 0 && iNumeroGuia.ToString().Length < 9)
                            {
                                iNumGuiaD.Value = iNumeroGuia.ToString().PadLeft(9, '0');
                            }

                            var fecEnt = ((EditText)FormR.Items.Item("10").Specific).Value;
                            var queryContractData = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetContractData.sql", GetType().Assembly), pNumCto);
                            var resp = Record.Instance.Query(queryContractData).Execute().First();
                            var pDato = resp["CreateDate"].ToString();
                            var pFecha = "";
                            var pFechaD = "";
                            pFecha = pDato.Substring(6, 2) + "/" + pDato.Substring(4, 2) + "/" + pDato.Substring(0, 4);
                            pFechaD = fecEnt.Substring(6, 2) + "/" + fecEnt.Substring(4, 2) + "/" + fecEnt.Substring(0, 4);
                            if (Convert.ToDateTime(pFechaD) < Convert.ToDateTime(pFecha))
                            {
                                pErrorDesc = "Error: No puede seleccionar una fecha anterior a la Fecha del Contrato..!! ";
                                pError = "1";
                            }
                            if (pError == "1")
                            {
                                B1Connections.SboApp.SetStatusBarMessage(pErrorDesc);
                                pDatoD = false;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //Log.Info("Excp (SboApp_ItemEvent) => " + ex.Message);
                            pDatoD = true;
                        }
                    }
                }
            }
            BubbleEvent = pDatoD;
        }

        //private void SboApp_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        //{
        //    bool pDatoD = true;
        //    Globals.pFormD = BusinessObjectInfo.FormUID;
        //    pFormDN = BusinessObjectInfo.FormUID;
        //    BubbleEvent = pDatoD;
        //}

        //private void SboApp_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        //{
        //    BubbleEvent = true;
        //    try
        //    {
        //        pFormDN = pVal.FormTypeEx;
        //        Globals.pFormD = pVal.FormTypeEx;
        //        if (pVal.FormTypeEx == "2002122020")
        //        {
        //            FormR = B1Connections.SboApp.Forms.Item(FormUID);
        //            //if (pVal.ItemUID.Equals("1") && pVal.BeforeAction)
        //            //{
        //            //    if (pVal.EventType == BoEventTypes.et_FORM_DATA_ADD)
        //            //    {
        //            //        if (!new Form._2002122020.Button_1().PassValidations(new B1Forms(pVal.FormUID)))
        //            //            BubbleEvent = false; return;
        //            //    }


        //            //}
        //            if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED && pVal.BeforeAction == false && pVal.ItemUID.Equals("1"))
        //            {
        //                B1Connections.SboApp.Menus.Item("1304").Activate();
        //            }

        //        }
        //        if (pVal.FormTypeEx == "133")
        //        {
        //            if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED && pVal.BeforeAction == false)
        //            {
        //                SAPbouiCOM.Form oForm = B1Connections.SboApp.Forms.Item(FormUID);
        //                oForm.Items.Item("U_BGT_ContAsoc").Enabled = false;
        //            }
        //        }
        //        if (pVal.FormTypeEx == "140")
        //        {
        //            FormR = B1Connections.SboApp.Forms.Item(FormUID);
        //            if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED && pVal.BeforeAction == false && pVal.ItemUID.Equals("57"))
        //            {
        //                FormR.Items.Item("U_BGT_FecFact").Enabled = false;
        //                FormR.Items.Item("U_BGT_ContAsoc").Enabled = false;
        //                FormR.Items.Item("U_NUM_GUIA").Enabled = false;
        //                FormR.Items.Item("U_BGT_DevProv").Enabled = false;
        //            }
        //            if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED && pVal.BeforeAction == true && pVal.ItemUID.Equals("1"))
        //            {
        //                FormR.Items.Item("U_BGT_FecFact").Enabled = false;
        //                FormR.Items.Item("U_BGT_ContAsoc").Enabled = false;
        //                FormR.Items.Item("U_NUM_GUIA").Enabled = false;
        //                FormR.Items.Item("U_BGT_DevProv").Enabled = false;

        //                var pError = "0";
        //                var pNumCto = "";
        //                var pErrorDesc = "";
        //                pNumCto = ((EditText)FormR.Items.Item("U_BGT_ContAsoc").Specific).Value;
        //                var fecFact = ((EditText)FormR.Items.Item("U_BGT_FecFact").Specific).Value;

        //                if (string.IsNullOrEmpty(fecFact) || fecFact.Equals("0") || fecFact.Equals("0.0"))
        //                {
        //                    pErrorDesc = "No puede dejar el campo de Fecha de Facturacion Vacio.";
        //                    pError = "1";
        //                }


        //                var iNumGuiaD = ((EditText)FormR.Items.Item("U_NUM_GUIA").Specific);
        //                var iNumeroGuia = ((EditText)FormR.Items.Item("U_NUM_GUIA").Specific).Value;
        //                int valor = 0;
        //                if (!int.TryParse(iNumeroGuia.ToString(), out valor))
        //                {
        //                    pErrorDesc = "Solo se admite numeros";
        //                    iNumGuiaD.Value = "0";
        //                    pError = "1";
        //                }

        //                if (iNumeroGuia.ToString().Length > 9)
        //                {
        //                    pErrorDesc = "Numero de caracteres permitidos 9";
        //                    iNumGuiaD.Value = "0";
        //                    pError = "1";
        //                }
        //                //else if (iNumeroGuia.ToString().Length > 0 && iNumeroGuia.ToString().Length < 9)
        //                //{
        //                //    iNumGuiaD.Value = iNumeroGuia.ToString().PadLeft(9, '0');
        //                //}

        //                var fecEnt = ((EditText)FormR.Items.Item("10").Specific).Value;
        //                var queryContractData = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetContractData.sql", GetType().Assembly), pNumCto);
        //                var resp = Record.Instance.Query(queryContractData).Execute().First();
        //                var pDato = resp["CreateDate"].ToString();
        //                var pFecha = "";
        //                var pFechaD = "";
        //                pFecha = pDato.Substring(6, 2) + "/" + pDato.Substring(4, 2) + "/" + pDato.Substring(0, 4);
        //                pFechaD = fecEnt.Substring(6, 2) + "/" + fecEnt.Substring(4, 2) + "/" + fecEnt.Substring(0, 4);
        //                if (Convert.ToDateTime(pFecha) < Convert.ToDateTime(pFechaD))
        //                {
        //                    pErrorDesc = "Error: No puede seleccionar una fecha anterior a la Fecha del Contrato..!! ";
        //                    pError = "1";
        //                }
        //                if (pError == "1")
        //                {
        //                    B1Connections.SboApp.SetStatusBarMessage(pErrorDesc);
        //                    BubbleEvent = false; return;
        //                }

        //            }
        //        }
        //        if (pVal.FormTypeEx == "112")
        //        {
        //            if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED && pVal.BeforeAction == true && pVal.ItemUID.Equals("5907"))
        //            {
        //                SAPbouiCOM.Form oForm = B1Connections.SboApp.Forms.Item(FormUID);
        //                oForm.Items.Item("U_BGT_FecFact").Enabled = false;
        //                oForm.Items.Item("U_BGT_ContAsoc").Enabled = false;
        //                oForm.Items.Item("U_NUM_GUIA").Enabled = false;
        //                oForm.Items.Item("U_BGT_DevProv").Enabled = false;

        //                var pError = "0";
        //                var pNumCto = "";
        //                var pErrorDesc = "";
        //                pNumCto = ((EditText)oForm.Items.Item("U_BGT_ContAsoc").Specific).Value;
        //                var fecFact = ((EditText)oForm.Items.Item("U_BGT_FecFact").Specific).Value;

        //                if (string.IsNullOrEmpty(fecFact) || fecFact.Equals("0") || fecFact.Equals("0.0"))
        //                {
        //                    pErrorDesc = "No puede dejar el campo de Fecha de Facturacion Vacio.";
        //                    pError = "1";
        //                }


        //                var iNumGuiaD = ((EditText)oForm.Items.Item("U_NUM_GUIA").Specific);
        //                var iNumeroGuia = ((EditText)oForm.Items.Item("U_NUM_GUIA").Specific).Value;
        //                int valor = 0;
        //                if (!int.TryParse(iNumeroGuia.ToString(), out valor))
        //                {
        //                    pErrorDesc = "Solo se admite numeros";
        //                    iNumGuiaD.Value = "0";
        //                    pError = "1";
        //                }

        //                if (iNumeroGuia.ToString().Length > 9)
        //                {
        //                    pErrorDesc = "Numero de caracteres permitidos 9";
        //                    iNumGuiaD.Value = "0";
        //                    pError = "1";
        //                }
        //                //else if (iNumeroGuia.ToString().Length > 0 && iNumeroGuia.ToString().Length < 9)
        //                //{
        //                //    iNumGuiaD.Value = iNumeroGuia.ToString().PadLeft(9, '0');
        //                //}

        //                var fecEnt = ((EditText)oForm.Items.Item("10").Specific).Value;
        //                var queryContractData = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetContractData.sql", GetType().Assembly), pNumCto);
        //                var resp = Record.Instance.Query(queryContractData).Execute().First();
        //                var pDato = resp["CreateDate"].ToString();
        //                var pFecha = "";
        //                var pFechaD = "";
        //                pFecha = pDato.Substring(6, 2) + "/" + pDato.Substring(4, 2) + "/" + pDato.Substring(0, 4);
        //                pFechaD = fecEnt.Substring(6, 2) + "/" + fecEnt.Substring(4, 2) + "/" + fecEnt.Substring(0, 4);
        //                if (Convert.ToDateTime(pFecha) < Convert.ToDateTime(pFechaD))
        //                {
        //                    pErrorDesc = "Error: No puede seleccionar una fecha anterior a la Fecha del Contrato..!! ";
        //                    pError = "1";
        //                }
        //                if (pError == "1")
        //                {
        //                    B1Connections.SboApp.SetStatusBarMessage(pErrorDesc);
        //                    BubbleEvent = false; return;
        //                }

        //            }
        //        }
        //        if (pVal.FormTypeEx == "180")
        //        {
        //            if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED && pVal.BeforeAction == false)
        //            {
        //                SAPbouiCOM.Form oForm = B1Connections.SboApp.Forms.Item(FormUID);
        //                oForm.Items.Item("U_BGT_FecFact").Enabled = false;
        //                oForm.Items.Item("U_BGT_ContAsoc").Enabled = false;
        //                oForm.Items.Item("U_NUM_GUIA").Enabled = false;
        //                oForm.Items.Item("U_BGT_DevProv").Enabled = false;
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        //Log.Info("Excp (SboApp_ItemEvent) => " + ex.Message);
        //    }
        //}
    }
}


//namespace bagant
//{
//    class Program : B1AddOn
//    {
//        public static class Globals
//        {
//            public static string pFormD { get; set; }
//        }

//        public static class Frd
//        {
//            public static SAPbouiCOM.Form FormRD { get; set; }
//        }

//        public string pFormDN = "";
//        public SAPbouiCOM.Form FormR;

//        public void LoadProgram(Assembly asm, string connStr, int appid)
//        {
//            SetAssembly(asm);
//            loadConnection(connStr, appid);
//            loadResources(connStr, appid, asm, "bagant.Menu.xml.addMenus.xml");

//            B1Connections.SboApp.SetStatusBarMessage("Addin Rental AF Iniciado", SAPbouiCOM.BoMessageTime.bmt_Medium, false);
//            B1Connections.SboApp.MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(sboApp_MenuItemEvent);
//        }

//        private void sboApp_MenuItemEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
//        {
//            var pDatoD = true;
//            var pError = "0";
//            var pNumCto = "";
//            var pErrorDesc = "";

//            if (pVal.MenuUID == "RemRowC")
//            {
//                if (Globals.pFormD == "2002122020")
//                {
//                    try
//                    {
//                        SAPbouiCOM.Form FormR = Frd.FormRD;
//                        var pValorD = FormR.Items.Item("Item_43").Specific;
//                        string pDatoN = pValorD.Value;

//                        var pNumCtoD = ((EditText)FormR.Items.Item("Item_45").Specific).Value.ToString();
//                        var pItem = "";
//                        var matrix = (SAPbouiCOM.Matrix)FormR.Items.Item("Item_62").Specific;
//                        int selrow1 = matrix.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
//                        if (selrow1 != -1)
//                        {
//                            FormR.Mode = BoFormMode.fm_UPDATE_MODE;
//                            pItem = matrix.Columns.Item(1).Cells.Item(selrow1).Specific.Value;
//                            if (pNumCtoD != "" && pItem != "")
//                            {
//                                var queryContractData = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetMovmentByItemDelRow.sql", GetType().Assembly), pNumCtoD, pItem);
//                                var resp = Record.Instance.Query(queryContractData).Execute().First();
//                                if (resp.Count > 0)
//                                {
//                                    var pDato = resp["Articulo"].ToString();
//                                    if (pDato == pItem)
//                                    {
//                                        pErrorDesc = "No es posible Eliminar partidas porque se tienen articulos asignados en el Contrato..!!";
//                                        B1Connections.SboApp.SetStatusBarMessage(pErrorDesc, BoMessageTime.bmt_Medium);
//                                        pDatoD = false;
//                                    }
//                                    else
//                                    {
//                                        //matrix.AddRow();
//                                        FormR.Mode = BoFormMode.fm_UPDATE_MODE;
//                                    }
//                                }
//                            }
//                        }
//                    }
//                    catch (System.Exception ex)
//                    {
//                        Log.Info("Excp (SboApp_ItemEvent) => " + ex.Message);
//                        pDatoD = false;
//                    }


//                }
//            }

//            if (pVal.MenuUID == "DuplicarCont")
//            {
//                if (Globals.pFormD == "2002122020")
//                {
//                    pErrorDesc = "Menu..";
//                    try
//                    {
//                        var pUser = B1Connections.SboApp.Company.UserName;
//                        SAPbouiCOM.Form FormR = Frd.FormRD;
//                        //FormR = B1Connections.SboApp.Forms.Item(Globals.pFormD);
//                        //FormR = new B1Forms(Globals.pFormD);
//                        var pNumCtoD = ((EditText)FormR.Items.Item("Item_45").Specific).Value.ToString();
//                        FormR.Mode = BoFormMode.fm_ADD_MODE;
//                        var queryContractData = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetDataContractAll.sql", GetType().Assembly), pNumCtoD, pUser);
//                        var resp = Record.Instance.Query(queryContractData).Execute().First();
//                        var pDato = resp["U_BGT_OrdComp"].ToString();

//                        //var queryUser = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetDataContractAll.sql", GetType().Assembly), pNumCtoD);
//                        //var respUser = Record.Instance.Query(queryContractData).Execute().First();


//                        ((EditText)FormR.Items.Item("Item_8").Specific).Value = pDato;
//                        var matrix = (SAPbouiCOM.Matrix)FormR.Items.Item("Item_62").Specific;
//                        matrix.Clear();
//                        string pFechaDN = "";
//                        pFechaDN = DateTime.Now.Year.ToString();
//                        if (DateTime.Now.Month.ToString().Length == 2)
//                        {
//                            pFechaDN += DateTime.Now.Month.ToString();
//                        }
//                        else
//                        {
//                            pFechaDN += "0" + DateTime.Now.Month.ToString();
//                        }
//                        if (DateTime.Now.Day.ToString().Length == 2)
//                        {
//                            pFechaDN += DateTime.Now.Day.ToString();
//                        }
//                        else
//                        {
//                            pFechaDN += "0" + DateTime.Now.Day.ToString();
//                        }

//                        ((EditText)FormR.Items.Item("Item_14").Specific).Value = pFechaDN;

//                        ((EditText)FormR.Items.Item("Item_6").Specific).Value = resp["U_BGT_CardCode"].ToString();

//                        ((EditText)FormR.Items.Item("Item_18").Specific).Value = resp["U_BGT_CodHolding"].ToString();

//                        SAPbouiCOM.ComboBox oComboBox21 = FormR.Items.Item("Item_21").Specific;
//                        oComboBox21.Select(resp["U_BGT_CntcPerson"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

//                        SAPbouiCOM.ComboBox oComboBox23 = FormR.Items.Item("Item_23").Specific;
//                        oComboBox23.Select(resp["U_BGT_DirecFE"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

//                        SAPbouiCOM.ComboBox oComboBox25 = FormR.Items.Item("Item_25").Specific;
//                        oComboBox25.Select(resp["U_BGT_DestObra"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

//                        SAPbouiCOM.ComboBox oComboBox27 = FormR.Items.Item("Item_27").Specific;
//                        oComboBox27.Select(resp["U_BGT_Territor"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

//                        FormR.Items.Item("Item_78").Enabled = true;
//                        FormR.Items.Item("Item_94").Enabled = true;
//                        FormR.Items.Item("Item_88").Enabled = true;
//                        FormR.Items.Item("Item_108").Enabled = true;

//                        SAPbouiCOM.ComboBox oComboBox78 = FormR.Items.Item("Item_78").Specific;
//                        oComboBox78.Select(resp["U_BGT_PriceList"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

//                        SAPbouiCOM.ComboBox oComboBox94 = FormR.Items.Item("Item_94").Specific;
//                        oComboBox94.Select(resp["U_BGT_CodSurc"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

//                        SAPbouiCOM.ComboBox oComboBox98 = FormR.Items.Item("Item_98").Specific;
//                        oComboBox98.Select(resp["U_BGT_PerFact"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

//                        SAPbouiCOM.ComboBox oComboBox88 = FormR.Items.Item("Item_88").Specific;
//                        oComboBox88.Select(resp["U_BGT_TipCalc"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

//                        ((EditText)FormR.Items.Item("Item_108").Specific).Value = resp["U_BGT_CicloFact"].ToString();

//                        SAPbouiCOM.ComboBox oComboBox43 = FormR.Items.Item("Item_43").Specific;
//                        oComboBox43.Select(resp["U_BGT_Status"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

//                        ((EditText)FormR.Items.Item("Item_37").Specific).Value = resp["U_BGT_DurCont"].ToString();
//                        ((EditText)FormR.Items.Item("Item_39").Specific).Value = resp["U_BGT_DocNumOF"].ToString();

//                        SAPbouiCOM.ComboBox oComboBox47 = FormR.Items.Item("Item_47").Specific;
//                        oComboBox47.Select(resp["U_BGT_DistObra"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

//                        SAPbouiCOM.ComboBox oComboBox79 = FormR.Items.Item("Item_79").Specific;
//                        oComboBox79.Select(resp["U_BGT_Transporte"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

//                        SAPbouiCOM.ComboBox oComboBox57 = FormR.Items.Item("Item_57").Specific;
//                        oComboBox57.Select(resp["Series"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

//                        ((EditText)FormR.Items.Item("Item_51").Specific).Value = resp["U_BGT_NumOport"].ToString();
//                        ((EditText)FormR.Items.Item("Item_55").Specific).Value = resp["U_BGT_ActRel"].ToString();

//                        SAPbouiCOM.ComboBox oComboBox84 = FormR.Items.Item("Item_84").Specific;
//                        oComboBox84.Select(resp["U_BGT_Categor"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

//                        ((EditText)FormR.Items.Item("Item_53").Specific).Value = resp["U_BGT_Project"].ToString();
//                        ((EditText)FormR.Items.Item("Item_31").Specific).Value = resp["U_BGT_ProjectName"].ToString();
//                        ((EditText)FormR.Items.Item("Item_95").Specific).Value = resp["U_BGT_NomComis"].ToString();
//                        ((EditText)FormR.Items.Item("Item_41").Specific).Value = resp["U_BGT_SlpName"].ToString();
//                        ((EditText)FormR.Items.Item("Item_33").Specific).Value = resp["OwnerD"].ToString();
//                        ((EditText)FormR.Items.Item("Item_74").Specific).Value = resp["U_BGT_Comments"].ToString();
//                        ((EditText)FormR.Items.Item("Item_65").Specific).Value = resp["U_BGT_ValRepEqOb"].ToString();
//                        ((EditText)FormR.Items.Item("Item_67").Specific).Value = resp["U_BGT_ValRepCont"].ToString();
//                        ((EditText)FormR.Items.Item("Item_69").Specific).Value = resp["U_BGT_ValGarant"].ToString();
//                        ((EditText)FormR.Items.Item("Item_71").Specific).Value = resp["U_BGT_SubPesObra"].ToString();
//                        ((EditText)FormR.Items.Item("Item_73").Specific).Value = resp["U_BGT_SubPesEnt"].ToString();

//                        var table = (SAPbouiCOM.Matrix)FormR.Items.Item("Item_62").Specific;
//                        var bdlevel = 1;
//                        FormR.DataSources.DBDataSources.Item(bdlevel).InsertRecord(FormR.DataSources.DBDataSources.Item(bdlevel).Size);
//                        FormR.DataSources.DBDataSources.Item(bdlevel).Offset = FormR.DataSources.DBDataSources.Item(bdlevel).Size - 1;
//                        table.AddRow(1);
//                        for (int i = 1; i <= table.RowCount; i++)
//                            table.SetCellWithoutValidation(table.RowCount, "#", i.ToString());
//                    }
//                    catch (System.Exception ex)
//                    {
//                        Log.Info("Excp (SboApp_ItemEvent) => " + ex.Message);
//                        pDatoD = false;
//                    }
//                    pDatoD = false;
//                }
//            }

//            if(pVal.MenuUID == "1282")
//            {
//                if(Globals.pFormD == "2002122020")
//                {
//                    SAPbouiCOM.Form FormR = Frd.FormRD;
//                    //B1Framework.B1Frame.B1Base.SetFieldDb("1", "U_BGT_CicloFact", "@BGT_OCTR");
//                    //if (FormR.Mode == BoFormMode.fm_ADD_MODE)
//                    //{B
//                    FormR.Items.Item("Item_78").Enabled = true;
//                    FormR.Items.Item("Item_94").Enabled = true;
//                    FormR.Items.Item("Item_88").Enabled = true;
//                    FormR.Items.Item("Item_108").Enabled = true;
//                    //FormR.Items.Item("Item_45").Enabled = true;

//                    //}
//                    pDatoD = true;
//                }
//            }

//            if (pVal.MenuUID == "5907")
//            {
//                if (Globals.pFormD == "140")
//                {
//                    try
//                    {
//                        FormR.Items.Item("U_BGT_FecFact").Enabled = false;
//                        FormR.Items.Item("U_BGT_ContAsoc").Enabled = false;
//                        FormR.Items.Item("U_NUM_GUIA").Enabled = false;
//                        FormR.Items.Item("U_BGT_DevProv").Enabled = false;

//                        pNumCto = ((EditText)FormR.Items.Item("U_BGT_ContAsoc").Specific).Value;
//                        var fecFact = ((EditText)FormR.Items.Item("U_BGT_FecFact").Specific).Value;

//                        if (string.IsNullOrEmpty(fecFact) || fecFact.Equals("0") || fecFact.Equals("0.0"))
//                        {
//                            pErrorDesc = "No puede dejar el campo de Fecha de Facturacion Vacio.";
//                            pError = "1";
//                        }


//                        var iNumGuiaD = ((EditText)FormR.Items.Item("U_NUM_GUIA").Specific);
//                        var iNumeroGuia = ((EditText)FormR.Items.Item("U_NUM_GUIA").Specific).Value;
//                        int valor = 0;
//                        if (!int.TryParse(iNumeroGuia.ToString(), out valor))
//                        {
//                            pErrorDesc = "Solo se admite numeros";
//                            iNumGuiaD.Value = "0";
//                            pError = "1";
//                        }

//                        if (iNumeroGuia.ToString().Length > 9)
//                        {
//                            pErrorDesc = "Numero de caracteres permitidos 9";
//                            iNumGuiaD.Value = "0";
//                            pError = "1";
//                        }
//                        //else if (iNumeroGuia.ToString().Length > 0 && iNumeroGuia.ToString().Length < 9)
//                        //{
//                        //    iNumGuiaD.Value = iNumeroGuia.ToString().PadLeft(9, '0');
//                        //}

//                        var fecEnt = ((EditText)FormR.Items.Item("10").Specific).Value;
//                        var queryContractData = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetContractData.sql", GetType().Assembly), pNumCto);
//                        var resp = Record.Instance.Query(queryContractData).Execute().First();
//                        var pDato = resp["CreateDate"].ToString();
//                        var pFecha = "";
//                        var pFechaD = "";
//                        pFecha = pDato.Substring(6, 2) + "/" + pDato.Substring(4, 2) + "/" + pDato.Substring(0, 4);
//                        pFechaD = fecEnt.Substring(6, 2) + "/" + fecEnt.Substring(4, 2) + "/" + fecEnt.Substring(0, 4);
//                        if (Convert.ToDateTime(pFecha) < Convert.ToDateTime(pFechaD))
//                        {
//                            pErrorDesc = "Error: No puede seleccionar una fecha anterior a la Fecha del Contrato..!! ";
//                            pError = "1";
//                            pDatoD = false;
//                        }
//                        if (pError == "1")
//                        {
//                            B1Connections.SboApp.SetStatusBarMessage(pErrorDesc);
//                            pDatoD = false;
//                        }
//                    }
//                    catch (System.Exception ex)
//                    {
//                        Log.Info("Excp (SboApp_ItemEvent) => " + ex.Message);
//                        pDatoD = false;
//                    }

//                }
//                if(pError == "0")
//                {
//                    pDatoD = true;
//                }
//            }
//            BubbleEvent = pDatoD;
//        }

//    }
//}
