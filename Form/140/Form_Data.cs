using B1Framework.B1Frame;
using B1Framework.RecordSet;
using bagant.Services;
using SAPbouiCOM;
using System.Xml;
using System;
using static bagant.Program;

namespace bagant.Form._140
{
    class Form_Data : B1Form
    {

        public Form_Data()
        {
            FormType = "140";
            Program.Globals.pFormD = "140";
            //Form = new B1Forms(FormType);
            //Frd.FormRD = Form;
            //Frd.FormRDN = Form;
        }

        //[B1Listener(BoEventTypes.et_MENU_CLICK, true)]
        //public virtual void OnBeforeMenuClick(BusinessObjectInfo pVal)
        //{
        //    Globals.pFormD = "140";
        //}

        //[B1Listener(BoEventTypes.et_FORM_DATA_LOAD, true)]
        //public virtual void OnBeforeFormDataLoad(BusinessObjectInfo pVal)
        //{
        //    Globals.pFormD = "140";
        //    Form = new B1Forms(pVal.FormUID);
        //    Frd.FormRD=Form;
        //    Frd.FormRDN = Form;
        //    return;
        //}

        //[B1Listener(BoEventTypes.et_FORM_ACTIVATE, false)]
        //public virtual void OnBefireFormActivate(BusinessObjectInfo pVal)
        //{
        //    Program.Globals.pFormD = "140";
        //    return;
        //}

        //[B1Listener(BoEventTypes.et_FORM_LOAD, false)]
        //public virtual void OnBeforeFormLoad(BusinessObjectInfo pVal)
        //{
        //    Program.Globals.pFormD = "140";
        //}

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnBeforeFormDataAdd(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            Program.Globals.pFormD = "140";
            Frd.FormRDN = Form;
            if (pVal.ActionSuccess)
            {
                var xml = new XmlDocument();
                xml.LoadXml(pVal.ObjectKey);

                var sapServ = new Services.SAPServices();

                if (pVal.Type == "15")
                {
                    sapServ.AddMovementData(xml.InnerText, "ODLN", "DLN1", "15");
                    sapServ.CheckUDOContract(xml.InnerText, "ODLN", "15");
                }
                else if (pVal.Type == "112")
                {
                    sapServ.AddMovementData(xml.InnerText, "ODRF", "DRF1", "112");
                    sapServ.CheckUDOContract(xml.InnerText, "ODRF", "112");
                }

            }
            return;
        }

        //[B1Listener(BoEventTypes.et_FORM_DATA_ADD, true)]
        //public virtual void OnBeforeFormDataAdd(BusinessObjectInfo pVal)
        //{
        //    Form = new B1Forms(pVal.FormUID);
        //    if (pVal.ActionSuccess)
        //    {
        //        Globals.pFormD = "140";
        //        var pErrorDesc = ";";
        //        var pError = "";
        //        bool pDatoD = true;
        //        //try
        //        //{
        //        Form.Items.Item("U_BGT_FecFact").Enabled = false;
        //        Form.Items.Item("U_BGT_ContAsoc").Enabled = false;
        //        Form.Items.Item("U_NUM_GUIA").Enabled = false;
        //        Form.Items.Item("U_BGT_DevProv").Enabled = false;

        //        var pNumCto = ((EditText)Form.Items.Item("U_BGT_ContAsoc").Specific).Value;
        //        var fecFact = ((EditText)Form.Items.Item("U_BGT_FecFact").Specific).Value;

        //        if (string.IsNullOrEmpty(fecFact) || fecFact.Equals("0") || fecFact.Equals("0.0"))
        //        {
        //            pErrorDesc = "No puede dejar el campo de Fecha de Facturacion Vacio.";
        //            pError = "1";
        //        }


        //        var iNumGuiaD = ((EditText)Form.Items.Item("U_NUM_GUIA").Specific);
        //        var iNumeroGuia = ((EditText)Form.Items.Item("U_NUM_GUIA").Specific).Value;
        //        int valor = 0;
        //        if (!int.TryParse(iNumeroGuia.ToString(), out valor))
        //        {
        //            pErrorDesc = "Solo se admite numeros";
        //            iNumGuiaD.Value = "0";
        //            pError = "1";
        //        }

        //        if (iNumeroGuia.ToString().Length > 9)
        //        {
        //            pErrorDesc = "Numero de caracteres permitidos 9";
        //            iNumGuiaD.Value = "0";
        //            pError = "1";
        //        }
        //        //else if (iNumeroGuia.ToString().Length > 0 && iNumeroGuia.ToString().Length < 9)
        //        //{
        //        //    iNumGuiaD.Value = iNumeroGuia.ToString().PadLeft(9, '0');
        //        //}

        //        var fecEnt = ((EditText)Form.Items.Item("10").Specific).Value;
        //        var queryContractData = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetContractData.sql", GetType().Assembly), pNumCto);
        //        var resp = Record.Instance.Query(queryContractData).Execute().First();
        //        var pDato = resp["CreateDate"].ToString();
        //        var pFecha = "";
        //        var pFechaD = "";
        //        pFecha = pDato.Substring(6, 2) + "/" + pDato.Substring(4, 2) + "/" + pDato.Substring(0, 4);
        //        pFechaD = fecEnt.Substring(6, 2) + "/" + fecEnt.Substring(4, 2) + "/" + fecEnt.Substring(0, 4);
        //        if (Convert.ToDateTime(pFecha) < Convert.ToDateTime(pFechaD))
        //        {
        //            pErrorDesc = "Error: No puede seleccionar una fecha anterior a la Fecha del Contrato..!! ";
        //            pError = "1";
        //        }
        //        if (pError == "1")
        //        {
        //            B1Connections.SboApp.SetStatusBarMessage(pErrorDesc);
        //            pDatoD = false;
        //        }
        //        //}
        //        //catch (System.Exception ex)
        //        //{
        //        //    Log.Info("Excp (SboApp_ItemEvent) => " + ex.Message);
        //        //    pDatoD = false;
        //        //}

        //        return pDatoD;
        //    }
        //}

        //[B1Listener(BoEventTypes.et_ITEM_PRESSED, true)]
        //public virtual bool OnBeforeItemPressed(ItemEvent pVal)
        //{
        //    //Form = new B1Forms(pVal.FormUID);

        //    //var fecFact = ((EditText)Form.Items.Item("U_BGT_FecFact").Specific).Value;

        //    //if (string.IsNullOrEmpty(fecFact) || fecFact.Equals("0") || fecFact.Equals("0.0"))
        //    //{
        //    //    B1Connections.SboApp.SetStatusBarMessage("No puede dejar el campo de Fecha de Facturacion Vacio.");
        //    //    return false;
        //    //}

        //    //return true;

        //    Form = new B1Forms(pVal.FormUID);
        //    bool pDatoRE = true;
        //    Program.Globals.pFormD = "140";
        //    if (Form.Mode == BoFormMode.fm_ADD_MODE)
        //    {
        //        Program.Globals.pFormD = "140";
        //        var pErrorDesc = ";";
        //        var pError = "-1";

        //        //try
        //        //{
        //        Form.Items.Item("U_BGT_FecFact").Enabled = false;
        //        Form.Items.Item("U_BGT_ContAsoc").Enabled = false;
        //        Form.Items.Item("U_NUM_GUIA").Enabled = true;
        //        Form.Items.Item("U_BGT_DevProv").Enabled = false;

        //        var pNumCto = ((EditText)Form.Items.Item("U_BGT_ContAsoc").Specific).Value;
        //        var fecFact = ((EditText)Form.Items.Item("U_BGT_FecFact").Specific).Value;

        //        if (string.IsNullOrEmpty(fecFact) || fecFact.Equals("0") || fecFact.Equals("0.0"))
        //        {
        //            pErrorDesc = "No puede dejar el campo de Fecha de Facturacion Vacio.";
        //            pError = "1";
        //            B1Connections.SboApp.SetStatusBarMessage(pErrorDesc);
        //            pDatoRE = false;
        //        }


        //        var iNumGuiaD = ((EditText)Form.Items.Item("U_NUM_GUIA").Specific);
        //        var iNumeroGuia = ((EditText)Form.Items.Item("U_NUM_GUIA").Specific).Value;
        //        int valor = 0;
        //        if (!int.TryParse(iNumeroGuia.ToString(), out valor))
        //        {
        //            pErrorDesc = "Solo se admite numeros en el Campo Número de Guia.";
        //            //iNumGuiaD.Value = "0";
        //            pError = "1";
        //            B1Connections.SboApp.SetStatusBarMessage(pErrorDesc);
        //            pDatoRE = false;
        //        }

        //        if (iNumeroGuia.ToString().Length > 9)
        //        {
        //            pErrorDesc = "Numero de caracteres permitidos 9";
        //            //iNumGuiaD.Value = "0";
        //            pError = "1";
        //            B1Connections.SboApp.SetStatusBarMessage(pErrorDesc);
        //            pDatoRE = false;
        //        }
        //        else
        //        {
        //            var fecEnt = ((EditText)Form.Items.Item("10").Specific).Value;
        //            var queryContractData = "";
        //            queryContractData = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetContractData.sql", GetType().Assembly), pNumCto);
        //            if (queryContractData != null)
        //            {
        //                var resp = Record.Instance.Query(queryContractData).Execute().First();
        //                var pDato = resp["CreateDate"].ToString();
        //                var pFecha = "";
        //                var pFechaD = "";
        //                pFecha = pDato.Substring(6, 2) + "/" + pDato.Substring(4, 2) + "/" + pDato.Substring(0, 4);
        //                pFechaD = fecEnt.Substring(6, 2) + "/" + fecEnt.Substring(4, 2) + "/" + fecEnt.Substring(0, 4);
        //                if (Convert.ToDateTime(pFecha) > Convert.ToDateTime(pFechaD))
        //                {
        //                    pErrorDesc = "Error: No puede seleccionar una fecha anterior a la Fecha del Contrato..!! ";
        //                    pError = "1";
        //                    B1Connections.SboApp.SetStatusBarMessage(pErrorDesc);
        //                    pDatoRE = false;
        //                }
        //            }

        //        }
        //        if (iNumeroGuia.ToString().Length > 0 && iNumeroGuia.ToString().Length < 9)
        //        {
        //            iNumGuiaD.Value = iNumeroGuia.ToString().PadLeft(9, '0');
        //            pDatoRE = true;
        //        }



        //        //}
        //        //catch (System.Exception ex)
        //        //{
        //        //    Log.Info("Excp (SboApp_ItemEvent) => " + ex.Message);
        //        //    pDatoD = false;
        //        //}
        //    }

        //    return pDatoRE;
        //}

    }

}
