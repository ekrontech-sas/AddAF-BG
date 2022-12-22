using B1Framework.B1Frame;
using B1Framework.RecordSet;
using bagant.Services;
using SAPbouiCOM;

namespace bagant.Form._2002122020
{
    class Button_1 : B1Item
    {
        public Button_1()
        {
            FormType = "2002122020";
            ItemUID = "1";
        }

        //[B1Listener(BoEventTypes.et_CLICK, true)]
        //public virtual bool OnBeforeClick(ItemEvent pVal)
        //{
        //    Form = new B1Forms(pVal.FormUID);
        //    var pDato = true;
        //    if (Form.Mode == BoFormMode.fm_UPDATE_MODE)
        //    {
        //        var permQuery = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetPermUsers.sql", GetType().Assembly), B1Connections.DiCompany.UserSignature);
        //        var resp = Record.Instance.Query(permQuery).Execute().First();
        //        var branch = Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("U_BGT_CodSurc", 0).ToString().Trim();
        //        if (!resp["Name"].ToString().Equals(branch))
        //        {
        //            B1Connections.SboApp.SetStatusBarMessage("No puede modificar el contrato de otra sucursal.");
        //            pDato = false;
        //        }
        //    }

        //    if (Form.Mode == BoFormMode.fm_ADD_MODE)
        //    {
        //        if (B1Connections.SboApp.MessageBox("Desea crear el contrato ?", 1, "Si", "No") == 1)
        //        {
        //            if (!PassValidations(Form))
        //            {
        //                pDato = false;
        //            }
        //        }
        //        else
        //        {
        //            pDato = false;
        //        }
        //    }

        //    return pDato;
        //}

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, true)]
        public virtual bool OnBeforeItemPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var pDato = true;

            //B1Connections.SboApp.Menus.Item("1304").Activate();

            if (Form.Mode == BoFormMode.fm_UPDATE_MODE)
            {
                var permQuery = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetPermUsers.sql", GetType().Assembly), B1Connections.DiCompany.UserSignature);
                var resp = Record.Instance.Query(permQuery).Execute().First();
                var branch = Form.DataSources.DBDataSources.Item("@BGT_OCTR").GetValue("U_BGT_CodSurc", 0).ToString().Trim();
                if (!resp["Name"].ToString().Equals(branch))
                {
                    B1Connections.SboApp.SetStatusBarMessage("No puede modificar el contrato de otra sucursal.");
                    pDato = false;
                }
            }

            if ( Form.Mode == BoFormMode.fm_ADD_MODE )
            {
                if (!PassValidations(Form))
                {
                    pDato = false;
                }
                else
                {
                    if (B1Connections.SboApp.MessageBox("Desea crear el contrato ?", 1, "Si", "No") == 1)
                        pDato = true;
                    else
                        pDato = false;
                }
            }

            return pDato;
        }

        private bool PassValidations(B1Forms Form)
        {
            var cntctPerson = (ComboBox)Form.Items.Item("Item_21").Specific;
            var dirFe = (ComboBox)Form.Items.Item("Item_23").Specific;
            var destObra = (ComboBox)Form.Items.Item("Item_25").Specific;
            var territorio = (ComboBox)Form.Items.Item("Item_27").Specific;
            var listAlq = (ComboBox)Form.Items.Item("Item_78").Specific;
            var sucursal = (ComboBox)Form.Items.Item("Item_94").Specific;
            var tipoFact = (ComboBox)Form.Items.Item("Item_98").Specific;
            var tipCalc = (ComboBox)Form.Items.Item("Item_88").Specific;
            var ciclFact = (EditText)Form.Items.Item("Item_108").Specific;
            var distObra = (ComboBox)Form.Items.Item("Item_47").Specific;
            var trasnporte = (ComboBox)Form.Items.Item("Item_79").Specific;
            var categoria = (ComboBox)Form.Items.Item("Item_84").Specific;
            var codigoSN = (EditText)Form.Items.Item("Item_6").Specific;
            var mesesCont = (EditText)Form.Items.Item("Item_37").Specific;
            var proyecto = (EditText)Form.Items.Item("Item_53").Specific;
            var comision = (EditText)Form.Items.Item("Item_95").Specific;
            var empleado = (EditText)Form.Items.Item("Item_41").Specific;
            var propietario = (EditText)Form.Items.Item("Item_33").Specific;

            var pDatoRegresa = true;
            if (cntctPerson.Selected == null)
            {
                B1Connections.SboApp.SetStatusBarMessage("Tiene que seleccionar el campo \"Persona de Contacto\".");
                pDatoRegresa = false;
            }
            else
            {
                if (dirFe.Selected == null)
                {
                    B1Connections.SboApp.SetStatusBarMessage("Tiene que seleccionar el campo \"Direccion FE\".");
                    pDatoRegresa = false;
                }
                else
                {
                    if (destObra.Selected == null)
                    {
                        B1Connections.SboApp.SetStatusBarMessage("Tiene que seleccionar el campo \"Destino Obra\".");
                        pDatoRegresa = false;
                    }
                    else
                    {
                        if (territorio.Selected == null)
                        {
                            B1Connections.SboApp.SetStatusBarMessage("Tiene que seleccionar el campo \"Territorio\".");
                            pDatoRegresa = false;
                        }
                        else
                        {
                            if (listAlq.Selected == null)
                            {
                                B1Connections.SboApp.SetStatusBarMessage("Tiene que seleccionar el campo \"Lista de Alquiler\".");
                                pDatoRegresa = false;
                            }
                            else
                            {
                                if (sucursal.Selected == null)
                                {
                                    B1Connections.SboApp.SetStatusBarMessage("Tiene que seleccionar el campo \"Sucursal\".");
                                    pDatoRegresa = false;
                                }
                                else
                                {
                                    if (tipoFact.Selected == null)
                                    {
                                        B1Connections.SboApp.SetStatusBarMessage("Tiene que seleccionar el campo \"Tipo de Facturacion\".");
                                        pDatoRegresa = false;
                                    }
                                    else
                                    {
                                        if (tipCalc.Selected == null)
                                        {
                                            B1Connections.SboApp.SetStatusBarMessage("Tiene que seleccionar el campo \"Tipo de Calculo\".");
                                            pDatoRegresa = false;
                                        }
                                        else
                                        {
                                            if (distObra.Selected == null)
                                            {
                                                B1Connections.SboApp.SetStatusBarMessage("Tiene que seleccionar el campo \"Distancia de obra\".");
                                                pDatoRegresa = false;
                                            }
                                            else
                                            {
                                                if (trasnporte.Selected == null)
                                                {
                                                    B1Connections.SboApp.SetStatusBarMessage("Tiene que seleccionar el campo \"Transporte\".");
                                                    pDatoRegresa = false;
                                                }
                                                else
                                                {
                                                    if (categoria.Selected == null)
                                                    {
                                                        B1Connections.SboApp.SetStatusBarMessage("Tiene que seleccionar el campo \"Categoria\".");
                                                        pDatoRegresa = false;
                                                    }
                                                    else
                                                    {
                                                        if (string.IsNullOrEmpty(ciclFact.Value.Trim()))
                                                        {
                                                            B1Connections.SboApp.SetStatusBarMessage("Tiene que seleccionar el campo \"Ciclo de Facturacion\".");
                                                            pDatoRegresa = false;
                                                        }
                                                        else
                                                        {
                                                            if (string.IsNullOrEmpty(codigoSN.Value.Trim()))
                                                            {
                                                                B1Connections.SboApp.SetStatusBarMessage("Tiene que digitar el campo \"Codigo SN\".");
                                                                pDatoRegresa = false;
                                                            }
                                                            else
                                                            {
                                                                if (string.IsNullOrEmpty(mesesCont.Value.Trim()))
                                                                {
                                                                    B1Connections.SboApp.SetStatusBarMessage("Tiene que digitar el campo \"Meses duracion contrato\".");
                                                                    pDatoRegresa = false;
                                                                }
                                                                else
                                                                {
                                                                    if (string.IsNullOrEmpty(proyecto.Value.Trim()))
                                                                    {
                                                                        B1Connections.SboApp.SetStatusBarMessage("Tiene que digitar el campo \"Codigo de Obra\".");
                                                                        pDatoRegresa = false;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (string.IsNullOrEmpty(comision.Value.Trim()))
                                                                        {
                                                                            B1Connections.SboApp.SetStatusBarMessage("Tiene que digitar el campo \"Codigo de Comision\".");
                                                                            pDatoRegresa = false;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (string.IsNullOrEmpty(empleado.Value.Trim()))
                                                                            {
                                                                                B1Connections.SboApp.SetStatusBarMessage("Tiene que digitar el campo \"Empleado de Venta\".");
                                                                                pDatoRegresa = false;
                                                                            }
                                                                            else
                                                                            {
                                                                                if (string.IsNullOrEmpty(propietario.Value.Trim()))
                                                                                {
                                                                                    B1Connections.SboApp.SetStatusBarMessage("Tiene que digitar el campo \"Propietario\".");
                                                                                    pDatoRegresa = false;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return pDatoRegresa;
        }

    }
}
