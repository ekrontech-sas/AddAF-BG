using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bagant.Form._2010012021
{
    class Button_Item_33 : B1Item
    {
        public Button_Item_33()
        {
            FormType = "2010012021";
            ItemUID = "Item_33";
        }

        [B1Listener(BoEventTypes.et_CLICK, true)]
        public virtual bool OnBeforeClick(ItemEvent pVal)
        {
            B1Connections.SboApp.SetStatusBarMessage("Comenzando proceso de validacion, por favor espere...", BoMessageTime.bmt_Medium, false);
            Form = new B1Forms(pVal.FormUID);

            var listado = new Dictionary<string, int>();
            var grid = (Grid)Form.Items.Item("Item_35").Specific;


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
                    Form.Freeze(false);
                    Form.PaneLevel = 2;
                    Form.Items.Item("Item_35").Enabled = true;
                    Form.Items.Item("Item_6").Enabled = true;
                    return false;
                }
            }

            B1Connections.SboApp.SetStatusBarMessage("Proceso finalizado", BoMessageTime.bmt_Medium, false);

            return true;
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, true)]
        public virtual bool OnBeforeItemPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            try
            {
                Form.Freeze(true);

                if (Form.PaneLevel == 2)
                {
                    return new CommonTaskSapUI().ValidateHeaderDevolutionFinded(Form);
                }
                if (Form.PaneLevel == 3)
                {
                    return new CommonTaskSapUI().ValidateDetailDevolutionFinded(Form);
                }

                return true;
            }
            finally
            {
                Form.Freeze(false);
            }
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterItemPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            Int32 pDatoEnt = 0;
            try
            {
                Form.Freeze(true);
                Form.PaneLevel += 1;
                Form.Items.Item("Item_2").Enabled = true;

                if (Form.PaneLevel == 2)
                {
                    new CommonTaskSapUI().SetDeliveryToReturnHeaderFinded(Form);
                }
                else if (Form.PaneLevel == 3)
                {

                    bool dDato = true;
                    var listado = new Dictionary<string, int>();
                    var grid = (Grid)Form.Items.Item("Item_35").Specific;
                    int pEntra = 0;

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
                                    pEntra = 1;
                                    if (!listado.ContainsKey(nroCont))
                                        listado.Add(nroCont, 1);
                                    else
                                        listado[nroCont] += 1;
                                }
                            }
                        }
                    }
                    if(pEntra ==1)
                    {
                        for (int i = 0; i < listado.Count; i++)
                        {
                            var index = listado.ElementAt(i).Key;
                            if (listado[index] > 1)
                            {
                                B1Connections.SboApp.SetStatusBarMessage("No puede seleccionar mas de un codigo de transporte para el contrato nro " + index);
                                Form.Freeze(false);
                                Form.PaneLevel = 2;
                                Form.Items.Item("Item_35").Enabled = true;
                                Form.Items.Item("Item_6").Enabled = true;
                                return;
                            }
                        }
                    }


                    double valor1 = 0;
                    double valor2 = 0;
                    var grid1 = (Grid)Form.Items.Item("Item_35").Specific;
                    var pDatoDev = "";
                    var pDatoSal = "";
                    for (int i = 0; i < grid1.DataTable.Rows.Count; i++)
                    {
                        if (((string)grid.DataTable.GetValue("Opt", i)).ToString() != "")
                        {
                            if (((string)grid.DataTable.GetValue("Opt", i)).Equals("Y"))
                            {
                                if (grid1.DataTable.GetValue("Cantidad a Devolver", i) != null)
                                {
                                    pDatoEnt = 1;
                                    pDatoDev = (string)grid1.DataTable.GetValue("Cantidad a Devolver", i).ToString();
                                    pDatoSal = (string)grid1.DataTable.GetValue("Cantidad en Obra", i).ToString();
                                    if (pDatoDev != "")
                                    {
                                        valor1 = Convert.ToDouble((string)grid1.DataTable.GetValue("Cantidad a Devolver", i).ToString());
                                        valor2 = Convert.ToDouble((string)grid1.DataTable.GetValue("Cantidad en Obra", i).ToString());
                                        if (valor1 > valor2)
                                        {
                                            dDato = false;
                                            B1Connections.SboApp.SetStatusBarMessage($"Mayor Cantidad a Devolver que Cantidad en Obra en fila: {i + 1}");
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (pDatoEnt == 0)
                    {
                        dDato = false;
                        Form.Freeze(false);
                        Form.PaneLevel = 2;
                        Form.Items.Item("Item_35").Enabled = true;
                        Form.Items.Item("Item_6").Enabled = true;
                        B1Connections.SboApp.SetStatusBarMessage("Error: Debe de seleccionar una partida para generar la Devolucion..!! ");
                        return;
                    }
                    else
                    {
                        dDato = true;
                    }

                    if (dDato == true)
                    {
                        new CommonTaskSapUI().ShowResumeData(Form);
                        Form.Items.Item("Item_33").Enabled = true;
                        return;
                    }
                    else
                    {
                        Form.Freeze(false);
                        Form.PaneLevel = 2;
                        Form.Items.Item("Item_35").Enabled = true;
                        Form.Items.Item("Item_6").Enabled = true;
                        return;
                    }
                }
                else if (Form.PaneLevel == 4)
                {
                    bool dDato = new CommonTaskSapUI().ValidateDetailDevolutionFinded(Form);
                    if (dDato == true)
                    {
                        new CommonTaskSapUI().CreateReturnForm(Form);
                        Form.Items.Item("Item_33").Enabled = false;
                        return;
                    }
                    else
                    {
                        Form.Freeze(false);
                        Form.PaneLevel = 3;
                        Form.Items.Item("Item_33").Enabled = true;
                        Form.Items.Item("Item_6").Enabled = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                if(pDatoEnt==0)
                {
                    B1Connections.SboApp.SetStatusBarMessage("Error: Debe de seleccionar una partida para generar la Devolucion..!! ");
                    Form.Freeze(false);
                    Form.PaneLevel = 2;
                    Form.Items.Item("Item_35").Enabled = true;
                    Form.Items.Item("Item_6").Enabled = true;
                    return;
                }
                Log.Info("(Exception) SetTableData => " + ex.Message);
            }
            finally
            {
                Form.Freeze(false);
            }
        }
        //[B1Listener(BoEventTypes.et_CLICK, true)]
        //public virtual bool OnBeforeClick(ItemEvent pVal)
        //{
        //    B1Connections.SboApp.SetStatusBarMessage("Comenzando proceso de validacion, por favor espere...", BoMessageTime.bmt_Medium, false);
        //    Form = new B1Forms(pVal.FormUID);

        //    var listado = new Dictionary<string, int>();
        //    var grid = (Grid)Form.Items.Item("Item_35").Specific;


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

        //    for (int i = 0; i < listado.Count; i++)
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

        //[B1Listener(BoEventTypes.et_ITEM_PRESSED, true)]
        //public virtual bool OnBeforeItemPressed(ItemEvent pVal)
        //{
        //    Form = new B1Forms(pVal.FormUID);

        //    try
        //    {
        //        Form.Freeze(true);

        //        if (Form.PaneLevel == 2)
        //            return new CommonTaskSapUI().ValidateHeaderDevolutionFinded(Form);
        //        if (Form.PaneLevel == 3)
        //            return new CommonTaskSapUI().ValidateDetailDevolutionFinded(Form);

        //        return true;
        //    }
        //    finally
        //    {
        //        Form.Freeze(false);
        //    }
        //}

        //[B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        //public virtual void OnAfterItemPressed(ItemEvent pVal)
        //{
        //    Form = new B1Forms(pVal.FormUID);

        //    try
        //    {
        //        Form.Freeze(true);
        //        Form.PaneLevel += 1;
        //        Form.Items.Item("Item_2").Enabled = true;

        //        if (Form.PaneLevel == 2)
        //            new CommonTaskSapUI().SetDeliveryToReturnHeaderFinded(Form);
        //        else if (Form.PaneLevel == 3 )
        //            new CommonTaskSapUI().ShowResumeData(Form);
        //        else if (Form.PaneLevel == 4) 
        //        { 
        //            new CommonTaskSapUI().CreateReturnForm(Form);
        //            Form.Items.Item("Item_33").Enabled = false;
        //            return;
        //        }
        //    }
        //    finally
        //    {
        //        Form.Freeze(false);
        //    }
        //}

    }
}
