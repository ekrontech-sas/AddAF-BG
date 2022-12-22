using B1Framework.B1Frame;
using B1Framework.RecordSet;
using bagant.Services;
using SAPbouiCOM;
using System.Xml;

namespace bagant.Form._3002
{
    class Button_1 : B1Item
    {
        public static string RngIni = "";
        public static string RngFin = "";

        public Button_1()
        {
            FormType = "3002";
            ItemUID = "1";
        }

        [B1Listener(BoEventTypes.et_CLICK, false)]
        public virtual void OnAfterClick(ItemEvent pVal)
        {
            var frm = B1Connections.SboApp.Forms.Item(pVal.FormUID);
            if(pVal.ActionSuccess == true)
            {
                if(pVal.FormMode==2)
                {
                    if(pVal.FormTypeEx == "3002")
                    {
                        if (pVal.ItemUID == "1")
                        {
                            var pDocEntryCto = "0";
                            var pDocEntryDR = "0";
                            var docEntryEntrega = "";
                            var docEntryContrato = "";
                            var queryCheckPre = (string)B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetEntregaPreliminarNoGuiaRemove.sql", GetType().Assembly), Program.Globals.pNumCtoDR, Program.Globals.NoGuiaD, int.Parse(Program.Globals.DocEntryDR));
                            var respCheckPre = Record.Instance.Query(queryCheckPre).Execute().First();
                            if (int.Parse(respCheckPre["TotalNoGuia"]) > 0)
                            {
                                //SetLinesUpdatePreRemove(respCheckPre["DocEntryCto"], Program.Globals.DocEntryDR);
                                docEntryContrato = respCheckPre["DocEntryCto"];
                                docEntryEntrega = Program.Globals.DocEntryDR;
                                double valRepEqOb = 0, valRepCont = 0, subPesoOb = 0, subPesoEnt = 0;
                                var queryEntregado = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetCantEntregadoPreRemove.sql", GetType().Assembly), docEntryEntrega);
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
                                    catch (System.Exception ex)
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

                                Services.UDT_Table.RemoveRecordUDT("BGT_RECEPMAT", int.Parse(respCheckPre["Code"]));
                            }

                        }

                    }
                }
            }
            //if (_2002122020.Button_Item_99.ProvieneContrato)
            //{
            //    RngIni = ((EditText)Form.Items.Item("txtFecIn").Specific).Value;
            //    RngFin = ((EditText)Form.Items.Item("txtFecFin").Specific).Value;
            //}
            return;
        }

        public void SetLinesUpdatePreRemove(string docEntryContrato, string docEntryEntrega)
        {
            double valRepEqOb = 0, valRepCont = 0, subPesoOb = 0, subPesoEnt = 0;
            var queryEntregado = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetCantEntregadoPreRemove.sql", GetType().Assembly), docEntryEntrega);
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
                catch (System.Exception ex)
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
            return;
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_LOAD, false)]
        public virtual void OnBeforeFormDataAdd(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            //if (pVal.ActionSuccess)
            //{
            //    var xml = new XmlDocument();
            //    xml.LoadXml(pVal.ObjectKey);

            //    var sapServ = new SAPServices();

            //    if (pVal.Type == "15")
            //    {
            //        sapServ.AddMovementData(xml.InnerText, "ODLN", "DLN1", "15");
            //        sapServ.CheckUDOContract(xml.InnerText, "ODLN", "15");
            //    }
            //    else if (pVal.Type == "112")
            //    {
            //        sapServ.AddMovementData(xml.InnerText, "ODRF", "DRF1", "112");
            //        sapServ.CheckUDOContract(xml.InnerText, "ODRF", "112");
            //    }

            //}
            return;

        }

    }
}
