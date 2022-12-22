using B1Framework.B1Frame;
using B1Framework.RecordSet;
using bagant.Services;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bagant.Form._2004012021
{
    class Button_Item_33 : B1Item
    {
        public Button_Item_33()
        {
            FormType = "2004012021";
            ItemUID = "Item_33";
        }

        [B1Listener(BoEventTypes.et_CLICK, true)]
        public virtual bool OnBeforeItemPressed(ItemEvent pVal)
        {
            B1Connections.SboApp.SetStatusBarMessage("Comenzando proceso de validacion, por favor espere...", BoMessageTime.bmt_Medium, false);
            Form = new B1Forms(pVal.FormTypeEx);
            var listado = new Dictionary<string, int>();
            var grid = (Grid)Form.Items.Item("Item_5").Specific;
            //var gridFecFact = (Grid)Form.Items.Item("Item_37").Specific;
            //if(gridFecFact.Rows.Count>0)
            //{
            //    string fFecha = "";
            //    fFecha = gridFecFact.DataTable.GetValue("Fecha Facturacion", 0).ToString();
            //}
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                if (grid.GetDataTableRowIndex(i) != -1)
                {
                    var fila = grid.GetDataTableRowIndex(i);
                    if (((string)grid.DataTable.GetValue("Opt", fila)).Equals("Y"))
                    {
                        var nroCont = (string)grid.DataTable.GetValue("Numero Contrato", fila).ToString();
                        var codigoArticulo = (string)grid.DataTable.GetValue("Codigo Articulo", fila).ToString();

                        if (codigoArticulo.Contains("LOG"))
                        {
                            if (!listado.ContainsKey(nroCont))
                                listado.Add(nroCont, 1);
                            else
                                listado[nroCont] += 1;
                        }
                    }
                }
            }

            for (int i = 0; i < listado.Count; i++)
            {
                var index = listado.ElementAt(i).Key;
                if (listado[index] > 1)
                {
                    B1Connections.SboApp.SetStatusBarMessage("No puede seleccionar mas de un codigo de transporte para el contrato nro " + index);
                    return false;
                }
            }

            B1Connections.SboApp.SetStatusBarMessage("Proceso finalizado", BoMessageTime.bmt_Medium, false);
            
            return true;
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterItemPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            if (Form.PaneLevel + 1 == 4)
                Form.Items.Item(pVal.ItemUID).Enabled = false;

            Form.Items.Item("Item_2").Enabled = true;
            if (Form.PaneLevel + 1 == 2)
            {
                new CommonTaskSapUI().SetDeliveryFinded(Form);
            }

            if (Form.PaneLevel + 1 == 3)
            {
                var gridFecFact = (Grid)Form.Items.Item("Item_5").Specific;
                string pCleinte = gridFecFact.DataTable.GetValue("Nombre Cliente", 0).ToString();
                pCleinte = pCleinte.Substring(0, pCleinte.IndexOf(" -"));
                var queryContractData = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetCantGarantias.sql", GetType().Assembly), pCleinte);
                var resp = Record.Instance.Query(queryContractData).Execute().First();
                var pDato = resp["TotalGarantias"].ToString();
                if (pDato == "0")
                {
                    Form.Items.Item("Item_2").Enabled = true;
                    Form.Items.Item("Item_33").Enabled = true;
                    B1Connections.SboApp.SetStatusBarMessage("Error: No pueden Generar Entregas porque el cliente no cuenta con Garantias..!! ");
                    return;
                }
                else
                {
                    var grid = (Grid)Form.Items.Item("Item_5").Specific;
                    //var row = grid.GetDataTableRowIndex(pVal.Row);
                    //if (row != -1)
                    //{
                    double cantEntr = 0;
                    double stockPend = 0;
                    var item = "";
                    var pDatoE = 0;
                    for (int i = 0; i < grid.DataTable.Rows.Count; i++)
                    {
                        if (((string)grid.DataTable.GetValue("Opt", i)).Equals("Y"))
                        {
                            pDatoE = 1;
                            cantEntr = (double)grid.DataTable.GetValue("A Entregar", i);
                            stockPend = (double)grid.DataTable.GetValue("Stock Pendiente Contrato", i);
                            item = (string)grid.DataTable.GetValue("Codigo Articulo", i).ToString();

                            if (cantEntr > stockPend)
                            {
                                Form.Items.Item("Item_2").Enabled = true;
                                Form.Items.Item("Item_33").Enabled = true;
                                B1Connections.SboApp.SetStatusBarMessage("Error: La cantidad a entregar no puede ser mayor a la cantidad pendiente..!! ");
                                return;
                            }
                            else
                            {
                                if (cantEntr == 0)
                                {
                                    Form.Items.Item("Item_2").Enabled = true;
                                    Form.Items.Item("Item_33").Enabled = true;
                                    B1Connections.SboApp.SetStatusBarMessage("Error: La cantidad a entregar debe de ser mayor a 0..!! ");
                                    return;
                                }
                            }
                        }
                    }
                    if (pDatoE == 1)
                    {
                        //Program.Globals.pFormD = "140";
                        //Program.Frd.FormRD = new B1Forms("140");
                        new CommonTaskSapUI().SetDeliveryResume(Form);
                    }
                    else
                    {
                        Form.Items.Item("Item_2").Enabled = true;
                        Form.Items.Item("Item_33").Enabled = true;
                        B1Connections.SboApp.SetStatusBarMessage("Error: Debe de seleccionar una partida para generar la Entrega..!! ");
                        return;
                    }

                }
            }
            //B1Connections.SboApp.OpenForm((BoFormObjectEnum)112, "", "3252");
            if (Form.PaneLevel + 1 == 4)
            {
                string pProcesarEnt = "0";
                string fFecha = "";
                string fFechaContrato = "";
                string pNumeroContrato = "";
                string nroCont = "";
                var gridFecFact = (Grid)Form.Items.Item("Item_37").Specific;
                if (gridFecFact.Rows.Count > 0)
                {
                    for (int j = 0; j < gridFecFact.Rows.Count; j++)
                    {
                        fFecha = gridFecFact.DataTable.GetValue("Fecha Facturacion", j).ToString();
                        pNumeroContrato = gridFecFact.DataTable.GetValue("Numero Contrato", j).ToString();
                        var grid = (Grid)Form.Items.Item("Item_5").Specific;
                        var cell = gridFecFact.GetCellFocus();
                        for (int i = 0; i < grid.Rows.Count; i++)
                        {
                            nroCont = grid.DataTable.GetValue("Numero Contrato", i).ToString();
                            fFechaContrato = grid.DataTable.GetValue("Fecha Contrato", i).ToString();
                            if (nroCont == pNumeroContrato)
                            {
                                if (Convert.ToDateTime(fFecha.Substring(0, 10)) < Convert.ToDateTime(fFechaContrato.Substring(0, 10)))
                                {
                                    pProcesarEnt = "1";
                                }
                                break;
                            }
                        }
                        if (pProcesarEnt == "1")
                        {
                            break;
                        }

                    }
                }
                if (pProcesarEnt == "0")
                {
                    Program.Globals.pFormD = "140";
                    //Program.Frd.FormRD = new B1Forms("140");

                    new CommonTaskSapUI().CreateDeliveryForm(Form);
                    return;
                }
                else
                {
                    Form.Items.Item("Item_2").Enabled = true;
                    Form.Items.Item("Item_33").Enabled = true;
                    B1Connections.SboApp.SetStatusBarMessage("Error: No puede seleccionar una fecha anterior a la Fecha del Contrato..!! ");
                    return;
                }
            }

            Form.PaneLevel += 1;
        }

        //[B1Listener(BoEventTypes.et_CLICK, true)]
        //public virtual bool OnBeforeItemPressed(ItemEvent pVal)
        //{
        //    B1Connections.SboApp.SetStatusBarMessage("Comenzando proceso de validacion, por favor espere...", BoMessageTime.bmt_Medium, false);
        //    Form = new B1Forms(pVal.FormUID);
        //    var listado = new Dictionary<string, int>();
        //    var grid = (Grid)Form.Items.Item("Item_5").Specific;

        //    for (int i = 0; i < grid.Rows.Count; i++)
        //    {
        //        if (grid.GetDataTableRowIndex(i) != -1)
        //        {
        //            var fila = grid.GetDataTableRowIndex(i);
        //            if (((string)grid.DataTable.GetValue("Opt", fila)).Equals("Y"))
        //            {
        //                var nroCont = (string)grid.DataTable.GetValue("Numero Contrato", fila).ToString();
        //                var codigoArticulo = (string)grid.DataTable.GetValue("Codigo Articulo", fila).ToString();

        //                if (codigoArticulo.Contains("LOG"))
        //                {
        //                    if (!listado.ContainsKey(nroCont))
        //                        listado.Add(nroCont, 1);
        //                    else
        //                        listado[nroCont] += 1;
        //                }
        //            }
        //        }
        //    }

        //    for( int i=0; i<listado.Count; i++ )
        //    {
        //        var index = listado.ElementAt(i).Key;
        //        if (listado[index] > 1)
        //        {
        //            B1Connections.SboApp.SetStatusBarMessage("No puede seleccionar mas de un codigo de transporte para el contrato nro " + index);
        //            return false;
        //        }
        //    }

        //    B1Connections.SboApp.SetStatusBarMessage("Proceso finalizado", BoMessageTime.bmt_Medium, false);

        //    return true;
        //}

        //[B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        //public virtual void OnAfterItemPressed(ItemEvent pVal)
        //{
        //    Form = new B1Forms(pVal.FormUID);

        //    if (Form.PaneLevel+1 == 4)
        //        Form.Items.Item(pVal.ItemUID).Enabled = false;

        //    Form.Items.Item("Item_2").Enabled = true;
        //    if (Form.PaneLevel + 1 == 2)
        //        new CommonTaskSapUI().SetDeliveryFinded(Form);

        //    if (Form.PaneLevel + 1 == 3)
        //        new CommonTaskSapUI().SetDeliveryResume(Form);


        //    if (Form.PaneLevel + 1 == 4)
        //    {
        //        new CommonTaskSapUI().CreateDeliveryForm(Form);
        //        return;
        //    }

        //    Form.PaneLevel += 1;
        //}

    }
}
