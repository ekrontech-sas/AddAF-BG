using B1Framework.B1Frame;
using B1Framework.RecordSet;
using SAPbobsCOM;
using System;

namespace bagant.Services
{
    class SAPServices
    {
        public void CreateFixedAsset(string invtryItemCode)
        {
            try
            {
                var item = (Items)B1Connections.DiCompany.GetBusinessObject(BoObjectTypes.oItems);
                if (item.GetByKey(invtryItemCode))
                {
                    var serieSql = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetSeriesItemsToAsset.sql", GetType().Assembly), item.Series);
                    var respSerie = Record.Instance.Query(serieSql).Execute().First();

                    var asset = (Items)B1Connections.DiCompany.GetBusinessObject(BoObjectTypes.oItems);
                        asset.ItemType = ItemTypeEnum.itFixedAssets;
                        asset.Series = int.Parse(respSerie["U_BGT_SerieAF"]);
                        asset.ItemName = item.ItemName;
                        asset.ItemsGroupCode = item.ItemsGroupCode;
                        asset.AssetClass = respSerie["U_BGT_ClassAF"];

                    var assetResp = asset.Add();
                    var assetMsg = B1Connections.DiCompany.GetLastErrorDescription();
                    var assetCreated = B1Connections.DiCompany.GetNewObjectKey();

                    if (assetResp == 0)
                    {
                        item.UserFields.Fields.Item("U_AGT_ItemAsoc").Value = assetCreated;
                        item.Update();

                        Log.Info($"Activo fijo creado exitosamente {assetCreated}");
                        B1Connections.SboApp.SetStatusBarMessage("Activo fijo creado exitosamente", SAPbouiCOM.BoMessageTime.bmt_Medium, false);

                        if( asset.GetByKey(assetCreated) )
                        {
                            asset.UserFields.Fields.Item("U_AGT_ItemAsoc").Value = invtryItemCode;
                            asset.Update();
                        }
                    }
                    else
                    {
                        Log.Info($"Error al crear el actvo fijo => {assetResp} - {assetMsg}");
                        B1Connections.SboApp.SetStatusBarMessage($"Error al crear el actvo fijo => {assetMsg}");
                    }
                }
            } 
            catch(Exception ex)
            {
                Log.Error($"Exception (CreateFixedAsset) => {ex.Message}");
            }
        }

        public void CreateInventoryItem(string assetItemCode)
        {
            try
            {
                var itemAssets = (Items)B1Connections.DiCompany.GetBusinessObject(BoObjectTypes.oItems);
                if (itemAssets.GetByKey(assetItemCode))
                {
                    var serieSql = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetSeriesItemsToCreate.sql", GetType().Assembly), itemAssets.Series);
                    var respSerie = Record.Instance.Query(serieSql).Execute().First();

                    var itemInvtry = (Items)B1Connections.DiCompany.GetBusinessObject(BoObjectTypes.oItems);
                        itemInvtry.Series = int.Parse(respSerie["U_BGT_SerieItem"]);
                        itemInvtry.ItemName = itemAssets.ItemName;
                        itemInvtry.ItemsGroupCode = itemAssets.ItemsGroupCode;
                        itemInvtry.ManageBatchNumbers = BoYesNoEnum.tYES;

                    var assetResp = itemInvtry.Add();
                    var assetMsg = B1Connections.DiCompany.GetLastErrorDescription();
                    var itmCreated = B1Connections.DiCompany.GetNewObjectKey();

                    if (assetResp == 0)
                    {
                        itemAssets.UserFields.Fields.Item("U_AGT_ItemAsoc").Value = itmCreated;
                        itemAssets.Update();

                        Log.Info($"Articulo de inventario creado exitosamente {itmCreated}");
                        B1Connections.SboApp.SetStatusBarMessage("Articulo de inventario creado exitosamente", SAPbouiCOM.BoMessageTime.bmt_Medium, false);

                        if (itemAssets.GetByKey(itmCreated))
                        {
                            itemAssets.UserFields.Fields.Item("U_AGT_ItemAsoc").Value = assetItemCode;
                            itemAssets.Update();
                        }
                    }
                    else
                    {
                        Log.Info($"Error al crear el articulo de inventario => {assetResp} - {assetMsg}");
                        B1Connections.SboApp.SetStatusBarMessage($"Error al crear el articulo de inventario => {assetMsg}");
                    }
                }
            } 
            catch(Exception ex)
            {
                Log.Error($"Exception (CreateInventoryItem) => {ex.Message}");
            }
        }

        public void CreateCapitalizationAsset(string tableHead, string tableDetail, int docEntryOpch)
        {
            try
            {
                var assetQuery = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetFixedFromDocument.sql", GetType().Assembly), tableHead, tableDetail, docEntryOpch);
                var assetResp = Record.Instance.Query(assetQuery).Execute().All();

                var doc = (AssetDocumentService)B1Connections.DiCompany.GetCompanyService().GetBusinessService(ServiceTypes.AssetCapitalizationService);
                var docAsset = (AssetDocument)doc.GetDataInterface(AssetDocumentServiceDataInterfaces.adsAssetDocument);
                    docAsset.DocumentType = AssetDocumentTypeEnum.adtAppreciation;

                for (int i = 0; i < assetResp.Length; i++)
                {
                    var line = docAsset.AssetDocumentLineCollection.Add();
                        line.AssetNumber = assetResp[i]["Asset"];
                        line.Quantity = double.Parse(assetResp[i]["Quantity"]);
                        line.TotalLC = double.Parse(assetResp[i]["Price"]);
                }

                var result = doc.Add(docAsset);

                AddMovementAssetData(result.Code.ToString(), "OACQ", "ACQ1", "1470000049");

                B1Connections.SboApp.MessageBox("Se creo la capitalizacion para el articulo.");
            }
            catch (Exception ex)
            {
                Log.Error("Exception (CreateCapitalizationAsset) => "+ex.Message);
            }
        }

        public void CreateDevolutionCapitalizationAsset(string tableHead, string tableDetail, int docEntryOpch)
        {
            try
            {
                var assetQuery = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetFixedFromDocument.sql", GetType().Assembly), tableHead, tableDetail, docEntryOpch);
                var assetResp = Record.Instance.Query(assetQuery).Execute().All();

                var doc = (AssetDocumentService)B1Connections.DiCompany.GetCompanyService().GetBusinessService(SAPbobsCOM.ServiceTypes.AssetCapitalizationCreditMemoService);
                var docAsset = (AssetDocument)doc.GetDataInterface(SAPbobsCOM.AssetDocumentServiceDataInterfaces.adsAssetDocument);

                for (int i = 0; i < assetResp.Length; i++)
                {
                    var lineDoc = docAsset.AssetDocumentLineCollection.Add();
                        lineDoc.AssetNumber = assetResp[i]["Asset"];
                        lineDoc.Quantity = double.Parse(assetResp[i]["Quantity"]);
                        lineDoc.TotalLC = double.Parse(assetResp[i]["Price"]);
                }

                var result = doc.Add(docAsset);
           
                AddMovementAssetData(result.Code.ToString(), "OACD", "ACD1", "1470000060");

                B1Connections.SboApp.MessageBox("Se creo la nota de credito de capitalizacion para el articulo.");
            }
            catch (Exception ex)
            {
                Log.Error("Exception (CreateCapitalizationAsset) => " + ex.Message);
            }
        }

        public void CheckUDOContract(string docEntry, string tableHead, string type)
        {
            if (type == "15")
            {
                var entrega = (Documents) B1Connections.DiCompany.GetBusinessObject(BoObjectTypes.oDeliveryNotes);
                if (entrega.GetByKey(int.Parse(docEntry)))
                {
                    var nroContract = entrega.UserFields.Fields.Item("U_BGT_ContAsoc").Value;
                    var pNumGuia = entrega.UserFields.Fields.Item("U_NUM_GUIA").Value;
                    if (string.IsNullOrEmpty(nroContract))
                        return;

                    var queryCheck = (string)B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.CheckIfHasMovement.sql", GetType().Assembly), tableHead, nroContract);
                    var respCheck = Record.Instance.Query(queryCheck).Execute().First();
                    var queryContract = (string)B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetDocEntryContract.sql", GetType().Assembly), nroContract);
                    var respContract = Record.Instance.Query(queryContract).Execute().First();

                    if (int.Parse(respCheck["Cant"]) == 1)
                    {
                        var udo = new UDOServices("AGT_CTR");
                        udo.LoadDocumentHeader("DocEntry", respContract["DocEntry"]);
                        udo.UpdateHeaderDocument(DateTime.Now, "U_BGT_DocDateDesp");
                        udo.Update();

                        udo = new UDOServices("AGT_CTR");
                        udo.LoadDocumentHeader("DocEntry", respContract["DocEntry"]);
                        udo.UpdateHeaderDocument("A", "U_BGT_Status");
                        udo.Update();
                    }

                    //SetLinesUpdate(respContract["DocEntry"], docEntry);
                    var queryCheckPre = (string)B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetEntregaPreliminarNoGuia.sql", GetType().Assembly), nroContract, pNumGuia, int.Parse(docEntry));
                    var respCheckPre = Record.Instance.Query(queryCheckPre).Execute().First();
                    if (int.Parse(respCheckPre["TotalNoGuia"]) == 1)
                    {
                        //SetLinesUpdate(respContract["DocEntry"], docEntry);
                        Services.UDT_Table.RemoveRecordUDT("BGT_RECEPMAT", int.Parse(respCheckPre["Code"]));
                        //var entregaPre = (Documents)B1Connections.DiCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                        //if (entregaPre.GetByKey(int.Parse(docEntry)))
                        //{
                        //    entregaPre.Remove();
                        //}
                    }
                    else
                    {
                        SetLinesUpdate(respContract["DocEntry"], docEntry);
                    }
                }
            }
            if (type == "112")
            {
                var entrega = (Documents)B1Connections.DiCompany.GetBusinessObject(BoObjectTypes.oDrafts);
                if (entrega.GetByKey(int.Parse(docEntry)))
                {
                    var nroContract = entrega.UserFields.Fields.Item("U_BGT_ContAsoc").Value;
                    var pNumGuia = entrega.UserFields.Fields.Item("U_NUM_GUIA").Value;
                    if (string.IsNullOrEmpty(nroContract))
                        return;

                    var queryCheck = (string)B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.CheckIfHasMovement.sql", GetType().Assembly), tableHead, nroContract);
                    var respCheck = Record.Instance.Query(queryCheck).Execute().First();
                    var queryContract = (string)B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetDocEntryContract.sql", GetType().Assembly), nroContract);
                    var respContract = Record.Instance.Query(queryContract).Execute().First();
                    
                    if (int.Parse(respCheck["Cant"]) == 1)
                    {
                        var udo = new UDOServices("AGT_CTR");
                        udo.LoadDocumentHeader("DocEntry", respContract["DocEntry"]);
                        udo.UpdateHeaderDocument(DateTime.Now, "U_BGT_DocDateDesp");
                        udo.Update();

                        udo = new UDOServices("AGT_CTR");
                        udo.LoadDocumentHeader("DocEntry", respContract["DocEntry"]);
                        udo.UpdateHeaderDocument("A", "U_BGT_Status");
                        udo.Update();
                    }
                    var queryCheckPre = (string)B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetEntregaPreliminarNoGuia.sql", GetType().Assembly), nroContract, pNumGuia, -1);
                    var respCheckPre = Record.Instance.Query(queryCheckPre).Execute().First();
                    if (int.Parse(respCheckPre["TotalNoGuia"]) == 1)
                    {
                        SetLinesUpdatePre(respContract["DocEntry"], docEntry);
                    }
                }
            }
            else if( type == "16" )
            {
                var devolucion = (Documents)B1Connections.DiCompany.GetBusinessObject(BoObjectTypes.oReturns);
                if (devolucion.GetByKey(int.Parse(docEntry)))
                {
                    var nroContract = devolucion.UserFields.Fields.Item("U_BGT_ContAsoc").Value;
                    if (string.IsNullOrEmpty(nroContract))
                        return;

                    var queryContract = (string)B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetDocEntryContract.sql", GetType().Assembly), nroContract);
                    var respContract = Record.Instance.Query(queryContract).Execute().First();

                    SetLinesUpdateDevolucion(respContract["DocEntry"], docEntry);
                }
            } 

        }

        private void SetLinesUpdate(string docEntryContrato, string docEntryEntrega)
        {
            double valRepEqOb = 0, valRepCont = 0, subPesoOb = 0, subPesoEnt = 0;
            var queryEntregado = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetCantEntregado.sql", GetType().Assembly), docEntryEntrega);
            Log.Info("Entreg => "+queryEntregado);
            var respContract = Record.Instance.Query(queryEntregado).Execute().All();

            for (int i = 0; i < respContract.Length; i++) 
            {
                try
                {
                    var udo = new UDOServices("AGT_CTR", "BGT_CTR1");
                        udo.LoadDocumentHeader("DocEntry", docEntryContrato);
                        udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"])-1, "U_BGT_CantEntreg", respContract[i]["CantEnt"]);
                        udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"])-1, "U_BGT_CantEntr", respContract[i]["CantPend"]);
                        udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"])-1, "U_BGT_CantObra", respContract[i]["CantObra"]);
                        udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"])-1, "U_BGT_SubTotObr", respContract[i]["SubObra"]);
                        udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"])-1, "U_BGT_SubTotEnt", respContract[i]["SubEntr"]);
                    udo.Update();

                    valRepEqOb += (double.Parse(respContract[i]["CantObra"])*double.Parse(respContract[i]["PrecVta"]));
                    valRepCont += (double.Parse(respContract[i]["CantContr"]) * double.Parse(respContract[i]["PrecVta"]));
                    subPesoOb += (double.Parse(respContract[i]["CantObra"]) * double.Parse(respContract[i]["Peso"]));
                    subPesoEnt += (double.Parse(respContract[i]["Pendiente"]) * double.Parse(respContract[i]["Precio"]));
                }
                catch(Exception ex)
                {
                    Log.Info("Exception (SetLinesUpdate) => " + ex.Message);
                }
            }

            var udo2 = new UDOServices("AGT_CTR", "BGT_CTR1");
                udo2.LoadDocumentHeader("DocEntry", docEntryContrato);

                Log.Info("valRepEqOb " + valRepEqOb.ToString());
                Log.Info("valRepCont " + valRepCont.ToString());
                Log.Info("subPesoOb " + subPesoOb.ToString());
                Log.Info("subPesoEnt " + subPesoEnt.ToString());

                udo2.UpdateHeaderDocument(valRepEqOb.ToString(), "U_BGT_ValRepEqOb");
                udo2.UpdateHeaderDocument(valRepCont.ToString(), "U_BGT_ValRepCont");
                udo2.UpdateHeaderDocument(subPesoOb.ToString(), "U_BGT_SubPesObra");
                udo2.UpdateHeaderDocument(subPesoEnt.ToString(), "U_BGT_SubPesEnt");
                udo2.Update();
        }

        private void SetLinesUpdatePre(string docEntryContrato, string docEntryEntrega)
        {
            double valRepEqOb = 0, valRepCont = 0, subPesoOb = 0, subPesoEnt = 0;
            var queryEntregado = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetCantEntregadoPre.sql", GetType().Assembly), docEntryEntrega);
            Log.Info("Entreg => " + queryEntregado);
            var respContract = Record.Instance.Query(queryEntregado).Execute().All();

            for (int i = 0; i < respContract.Length; i++)
            {
                try
                {
                    var udo = new UDOServices("AGT_CTR", "BGT_CTR1");
                    udo.LoadDocumentHeader("DocEntry", docEntryContrato);
                    udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"]) - 1, "U_BGT_CantEntreg", respContract[i]["CantEnt"]);
                    udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"]) - 1, "U_BGT_CantEntr", respContract[i]["CantPend"]);
                    udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"]) - 1, "U_BGT_CantObra", respContract[i]["CantObra"]);
                    udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"]) - 1, "U_BGT_SubTotObr", respContract[i]["SubObra"]);
                    udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"]) - 1, "U_BGT_SubTotEnt", respContract[i]["SubEntr"]);
                    udo.Update();

                    valRepEqOb += (double.Parse(respContract[i]["CantObra"]) * double.Parse(respContract[i]["PrecVta"]));
                    valRepCont += (double.Parse(respContract[i]["CantContr"]) * double.Parse(respContract[i]["PrecVta"]));
                    subPesoOb += (double.Parse(respContract[i]["CantObra"]) * double.Parse(respContract[i]["Peso"]));
                    subPesoEnt += (double.Parse(respContract[i]["Pendiente"]) * double.Parse(respContract[i]["Precio"]));
                }
                catch (Exception ex)
                {
                    Log.Info("Exception (SetLinesUpdate) => " + ex.Message);
                }
            }

            var udo2 = new UDOServices("AGT_CTR", "BGT_CTR1");
            udo2.LoadDocumentHeader("DocEntry", docEntryContrato);

            Log.Info("valRepEqOb " + valRepEqOb.ToString());
            Log.Info("valRepCont " + valRepCont.ToString());
            Log.Info("subPesoOb " + subPesoOb.ToString());
            Log.Info("subPesoEnt " + subPesoEnt.ToString());

            udo2.UpdateHeaderDocument(valRepEqOb.ToString(), "U_BGT_ValRepEqOb");
            udo2.UpdateHeaderDocument(valRepCont.ToString(), "U_BGT_ValRepCont");
            udo2.UpdateHeaderDocument(subPesoOb.ToString(), "U_BGT_SubPesObra");
            udo2.UpdateHeaderDocument(subPesoEnt.ToString(), "U_BGT_SubPesEnt");
            udo2.Update();
        }

        public void SetLinesUpdatePreRemove(string docEntryContrato, string docEntryEntrega)
        {
            double valRepEqOb = 0, valRepCont = 0, subPesoOb = 0, subPesoEnt = 0;
            var queryEntregado = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetCantEntregadoPre.sql", GetType().Assembly), docEntryEntrega);
            Log.Info("Entreg => " + queryEntregado);
            var respContract = Record.Instance.Query(queryEntregado).Execute().All();

            for (int i = 0; i < respContract.Length; i++)
            {
                try
                {
                    var udo = new UDOServices("AGT_CTR", "BGT_CTR1");
                    udo.LoadDocumentHeader("DocEntry", docEntryContrato);
                    udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"]) - 1, "U_BGT_CantEntreg", respContract[i]["CantEnt"]);
                    udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"]) - 1, "U_BGT_CantEntr", respContract[i]["CantPend"]);
                    udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"]) - 1, "U_BGT_CantObra", respContract[i]["CantObra"]);
                    udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"]) - 1, "U_BGT_SubTotObr", respContract[i]["SubObra"]);
                    udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"]) - 1, "U_BGT_SubTotEnt", respContract[i]["SubEntr"]);
                    udo.Update();

                    valRepEqOb += (double.Parse(respContract[i]["CantObra"]) * double.Parse(respContract[i]["PrecVta"]));
                    valRepCont += (double.Parse(respContract[i]["CantContr"]) * double.Parse(respContract[i]["PrecVta"]));
                    subPesoOb += (double.Parse(respContract[i]["CantObra"]) * double.Parse(respContract[i]["Peso"]));
                    subPesoEnt += (double.Parse(respContract[i]["Pendiente"]) * double.Parse(respContract[i]["Precio"]));
                }
                catch (Exception ex)
                {
                    Log.Info("Exception (SetLinesUpdate) => " + ex.Message);
                }
            }

            var udo2 = new UDOServices("AGT_CTR", "BGT_CTR1");
            udo2.LoadDocumentHeader("DocEntry", docEntryContrato);

            Log.Info("valRepEqOb " + valRepEqOb.ToString());
            Log.Info("valRepCont " + valRepCont.ToString());
            Log.Info("subPesoOb " + subPesoOb.ToString());
            Log.Info("subPesoEnt " + subPesoEnt.ToString());

            udo2.UpdateHeaderDocument(valRepEqOb.ToString(), "U_BGT_ValRepEqOb");
            udo2.UpdateHeaderDocument(valRepCont.ToString(), "U_BGT_ValRepCont");
            udo2.UpdateHeaderDocument(subPesoOb.ToString(), "U_BGT_SubPesObra");
            udo2.UpdateHeaderDocument(subPesoEnt.ToString(), "U_BGT_SubPesEnt");
            udo2.Update();
        }


        public void SetLinesUpdateDevolucion(string docEntryContrato, string docEntryDevol)
        {
            var queryDevolucion = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetCantDevolucion.sql", GetType().Assembly), docEntryDevol);
            Log.Info("QueryDevol => " + queryDevolucion);
            var respContract = Record.Instance.Query(queryDevolucion).Execute().All();

            for (int i = 0; i < respContract.Length; i++)
            {
                try
                {
                    Log.Info("Cant Dev (CantPend) => " + respContract[i]["CantPend"]);
                    Log.Info("Cant Dev => (CantDev) " + respContract[i]["CantDev"]); 
                    Log.Info("Cant Dev => (CantDev) " + respContract[i]["CantObraCheck"]);


                    var udo = new UDOServices("AGT_CTR", "BGT_CTR1");
                        udo.LoadDocumentHeader("DocEntry", docEntryContrato);
                        udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"]) - 1, "U_BGT_CantDev", respContract[i]["CantDev"]);
                        udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"]) - 1, "U_BGT_CantObra", respContract[i]["CantObraCheck"]);
                        //udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"]) - 1, "U_BGT_CantEntr", respContract[i]["CantPend"]);
                        udo.UpdateLinesDocument(int.Parse(respContract[i]["U_BGT_LineNum"]) - 1, "U_BGT_SubTotObr", respContract[i]["SubObra"]);
                        udo.Update();
                }
                catch (Exception ex)
                {
                    Log.Info("Exception (SetLinesUpdateDevolucion) => " + ex.Message);
                }
            }
        }

        public void AddMovementData(string docEntry, string tableHead, string tableDetail, string type)
        {
            try
            {
                var queryLote = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetDataTransacc.sql", GetType().Assembly), docEntry, tableHead, tableDetail, type);
                var respLote = Record.Instance.Query(queryLote).Execute().All();

                Log.Info("QueryLote => " + queryLote);
                Log.Info("type => " + type);
                for (int i = 0; i < respLote.Length; i++)
                {
                    UDT_Table.AddRecordUDT("BGT_RECEPMAT", int.Parse(respLote[i]["DocNum"]),
                                                                    new string[] { "U_BGT_ItemCode", "U_BGT_Quantity", "U_BGT_Batch", "U_BGT_DocEntry", "U_BGT_DocType", "U_BGT_WhsCode", "U_BGT_WhsCodeTo", "U_BGT_Contract" },
                                                                    new object[] { respLote[i]["ItemCode"], respLote[i]["Quantity"], respLote[i]["BatchNum"], docEntry, type, respLote[i]["WhsCodeFrom"], respLote[i]["WhsCodeTo"], respLote[i]["Contrato"] });
                }

                B1Connections.SboApp.SetStatusBarMessage("Guardado de la transaccion exitosamente", SAPbouiCOM.BoMessageTime.bmt_Medium, false);
            }
            catch(Exception ex)
            {
                Log.Error($"Exception (AddMovementData) => {ex.Message}");
            }
        }

        public void AddInvoiceMovement(string docEntry)
        {
            try
            {
                var queryData = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetDataMovementInvoice.sql", GetType().Assembly), docEntry);
                var respData = Record.Instance.Query(queryData).Execute().First();

                for (int i = 0; i < respData.Count; i++)
                {
                    UDT_Table.AddRecordUDT("BGT_FCRD", new string[] { "U_NroContrato", "U_TipFact", "U_FecFact", "U_CicFact", "U_CantFact", "U_NroDoc", "U_NroContInterno" },
                                                       new object[] { respData["Contrato"], respData["Tipo de Facturacion"], respData["Fecha Factura"], respData["Ciclo Facturacion"], respData["Cantidad Factura"], respData["Numero Documento"], respData["Contrato Interno"] });
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Exception (AddInvoiceMovement) => {ex.Message}");
            }
        }

        public void AddInvoiceData(string docEntry)
        {
            try
            {
                var inv = (Documents) B1Connections.DiCompany.GetBusinessObject(BoObjectTypes.oInvoices);
                if (inv.GetByKey(int.Parse(docEntry)))
                {
                    if (!string.IsNullOrEmpty(inv.UserFields.Fields.Item("U_BGT_ContAsoc").Value.ToString()))
                    {
                        var dt = DateTime.Now;
                        var start = new DateTime(dt.Year, dt.Month, 1); 
                        var end = start.AddMonths(1).AddDays(-1); 

                        UDT_Table.AddRecordUDT("BGT_DINV", new string[] { "U_BGT_FecFact",  "U_BGT_NumCont",                                        "U_BGT_MesFact",        "U_BGT_FecIni",     "U_BGT_FecFin",     "U_BGT_DocEntryInv" },
                                                           new object[] { DateTime.Now,     inv.UserFields.Fields.Item("U_BGT_ContAsoc").Value,     DateTime.Now.Month,     start,              end,                docEntry });
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Exception (AddInvoiceMovement) => {ex.Message}");
            }
        }

        public void AddMovementAssetData(string docEntry, string tableHead, string tableDetail, string type)
        {
            try
            {
                var queryLote = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetDataAssetTransacc.sql", GetType().Assembly), docEntry, tableHead, tableDetail, type);
                var respLote = Record.Instance.Query(queryLote).Execute().First();

                UDT_Table.AddRecordUDT("BGT_RECEPASSET", int.Parse(respLote["DocNum"]),
                                                                new string[] { "U_BGT_ItemAsset", "U_BGT_Quantity", "U_BGT_DocEntry", "U_BGT_DocType" },
                                                                new object[] { respLote["ItemCode"], respLote["Quantity"], docEntry, respLote["ObjType"] });

                B1Connections.SboApp.SetStatusBarMessage("Guardado de la transaccion exitosamente", SAPbouiCOM.BoMessageTime.bmt_Medium, false);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception (AddMovementAssetData) => {ex.Message}");
            }
        }


    }
}
