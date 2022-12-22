using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Form
{
    class Form_RightClick : B1Event
    {
        [B1Listener(BoEventTypes.et_RIGHT_CLICK, true, new string[] { "2003112020", "2002122020", "2010012021", "2004012021","3002", "140" })]
        public bool OnBeforeRightClick(ContextMenuInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var pDato = "";
            if (Form.Mode == BoFormMode.fm_OK_MODE || Form.Mode == BoFormMode.fm_ADD_MODE || Form.Mode == BoFormMode.fm_UPDATE_MODE)
            {
                if (Form.TypeEx.Equals("2003112020") )
                {
                    if (pVal.ItemUID == "Item_7" || pVal.ItemUID == "Item_2" || pVal.ItemUID == "Item_17" || pVal.ItemUID == "Item_49" || pVal.ItemUID == "Item_50" || pVal.ItemUID == "Item_55" )
                    {
                        if (!B1Connections.SboApp.Menus.Exists("AddRow"))
                            AddSubMenu("AddRow", "Agregar Linea", 1);
                        if (!B1Connections.SboApp.Menus.Exists("RemRow"))
                            AddSubMenu("RemRow", "Eliminar Linea", 2);
                    }
                }
                else if( Form.TypeEx.Equals("2002122020") )
                {
                    var status = Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("U_BGT_Status", 0).Trim();
                    if (status.Equals("PL"))
                    {
                        Log.Info("Status => 31");
                        if (Form.Mode == BoFormMode.fm_OK_MODE)
                        {
                            Log.Info("Status => 32");
                            if (!B1Connections.SboApp.Menus.Exists("CancelDoc"))
                                AddSubMenu("CancelDoc", "Cancelar Contrato", 1);
                        }
                    }

                    if (pVal.ItemUID == "Item_62")
                    {
                        if (Form.Mode == BoFormMode.fm_ADD_MODE || Form.Mode == BoFormMode.fm_UPDATE_MODE)
                        {
                            if (!B1Connections.SboApp.Menus.Exists("AddRowC"))
                                AddSubMenu("AddRowC", "Agregar Linea", 1);
                            if (!B1Connections.SboApp.Menus.Exists("RemRowC"))
                                AddSubMenu("RemRowC", "Eliminar Linea", 2);
                        }
                    }

                    if (status.Equals("A"))
                    {
                        Log.Info("Status => 31");
                        if (Form.Mode == BoFormMode.fm_OK_MODE)
                        {
                            Log.Info("Status => 32");
                            if (B1Connections.SboApp.Menus.Exists("DuplicarCont1") == false)
                            {
                                AddSubMenu("DuplicarCont1", "Duplicar Contrato", 3);
                                if (Form.Mode == BoFormMode.fm_ADD_MODE)
                                {
                                    if (Form.Menu.Exists("DuplicarCont1"))
                                    {
                                        Form.Menu.RemoveEx("DuplicarCont1");
                                    }
                                }
                            }
                        }
                        if(Form.Mode == BoFormMode.fm_ADD_MODE)
                        {
                            Form.Menu.RemoveEx("DuplicarCont1");
                            if (!B1Connections.SboApp.Menus.Exists("AddRowC"))
                                AddSubMenu("AddRowC", "Agregar Linea", 1);
                            if (!B1Connections.SboApp.Menus.Exists("RemRowC"))
                                AddSubMenu("RemRowC", "Eliminar Linea", 2);
                        }
                    }
                    if (pVal.ItemUID == "Item_62" )
                    {
                        if (!B1Connections.SboApp.Menus.Exists("AddRowC"))
                            AddSubMenu("AddRowC", "Agregar Linea", 1);
                        if (!B1Connections.SboApp.Menus.Exists("RemRowC"))
                            AddSubMenu("RemRowC", "Eliminar Linea", 2);
                    }
                }
                else if (Form.TypeEx.Equals("2010012021"))
                {
                    if (pVal.ItemUID == "Item_42")
                    {
                        if (!B1Connections.SboApp.Menus.Exists("AddRowDa"))
                            AddSubMenu("AddRowDa", "Agregar Linea", 1);
                        if (!B1Connections.SboApp.Menus.Exists("RemRowDa"))
                            AddSubMenu("RemRowDa", "Eliminar Linea", 2);
                    }
                }
                else if (Form.TypeEx.Equals("3002"))
                {
                    try
                    {
                        Program.Globals.pFormD = "3002";
                        Form = new B1Forms(pVal.FormUID);
                        Program.Globals.pFormD = "3002";
                        Program.Globals.pRowD = pVal.Row.ToString();

                        SAPbouiCOM.Matrix grid = Form.Items.Item(pVal.ItemUID).Specific;
                        //var cantEntr = ""; ;
                        //for (int i = 1; i <= grid.Columns.Count; i++)
                        //{
                        //    cantEntr = grid.Columns.Item(i).Cells.Item(pVal.Row).Specific.Value;
                        //    if (cantEntr == "142")
                        //    {
                        //        var d = "";
                        //    }
                        //}
                        Program.Globals.DocEntryDR = grid.Columns.Item(1).Cells.Item(pVal.Row).Specific.Value;
                        Program.Globals.DocNumD = grid.Columns.Item(3).Cells.Item(pVal.Row).Specific.Value;
                        Program.Globals.NoGuiaD = grid.Columns.Item(74).Cells.Item(pVal.Row).Specific.Value;
                        Program.Globals.pNumCtoDR = grid.Columns.Item(201).Cells.Item(pVal.Row).Specific.Value;
                        Program.Frd.FormRD = Form;
                        if (pVal.ItemUID == "Item_3")
                        {
                            pDato = "1";
                            //if (!B1Connections.SboApp.Menus.Exists("AddRowDa"))
                            //    AddSubMenu("AddRowDa", "Agregar Linea", 1);
                            //if (!B1Connections.SboApp.Menus.Exists("RemRowDa"))
                            //    AddSubMenu("RemRowDa", "Eliminar Linea", 2);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        var error = ex.Message;
                    }
                }
                else if (Form.TypeEx.Equals("140"))
                {
                    try
                    {
                        Program.Globals.pFormD = "140";
                        //Form = new B1Forms("140");
                        Program.Globals.pFormD = "140";
                        Program.Frd.FormRD = Form;
                        Program.Globals.pEntrPre = "1";
                    }
                    catch (System.Exception ex)
                    {
                        var error = ex.Message;
                    }
                }
            }

            if (Form.TypeEx.Equals("2004012021"))
            {
                if (pVal.ItemUID == "Item_5")
                {
                    if (!B1Connections.SboApp.Menus.Exists("ExpandRow"))
                        AddSubMenu("ExpandRow", "Expandir", 1);
                    if (!B1Connections.SboApp.Menus.Exists("CllpseRow"))
                        AddSubMenu("CllpseRow", "Contraer", 2);
                }
            }

            return true;
        }

        [B1Listener(BoEventTypes.et_RIGHT_CLICK, false)]
        public virtual void OnAfterRightClick(ContextMenuInfo pVal)
        {
            DeleteSubMenu("AddRow", "RemRow", "AddRowC", "RemRowC", "CancelDoc", "AddRowDa", "RemRowDa", "ExpandRow", "CllpseRow", "DuplicarCont1");
        }
    }
}
