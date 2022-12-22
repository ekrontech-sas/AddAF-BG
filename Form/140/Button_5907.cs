using B1Framework.B1Frame;
using B1Framework.RecordSet;
using SAPbouiCOM;
using bagant.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bagant.Form._140
{
    class Button_5907 : B1Item
    {
        public Button_5907()
        {
            FormType = "140";
            ItemUID = "5907";
        }

        //[B1Listener(BoEventTypes.et_MENU_CLICK, true)]
        //public virtual void OnAfterItemPressed(ItemEvent pVal)
        //{
        //    Form = new B1Forms(pVal.FormUID);
        //    var pNumCto = "";
        //    pNumCto = ((EditText)Form.Items.Item("U_BGT_ContAsoc").Specific).Value;
        //    //var fecFact = ((EditText)Form.Items.Item("U_BGT_FecFact").Specific).Value;

        //    //if (string.IsNullOrEmpty(fecFact) || fecFact.Equals("0") || fecFact.Equals("0.0"))
        //    //{
        //    //    B1Connections.SboApp.SetStatusBarMessage("No puede dejar el campo de Fecha de Facturacion Vacio.");
        //    //}


        //    //var iNumGuiaD = ((EditText)Form.Items.Item("U_NUM_GUIA").Specific);
        //    //var iNumeroGuia = ((EditText)Form.Items.Item("U_NUM_GUIA").Specific).Value;
        //    //int valor = 0;
        //    //if (!int.TryParse(iNumeroGuia.ToString(), out valor))
        //    //{
        //    //    B1Connections.SboApp.SetStatusBarMessage("Solo se admite numeros", BoMessageTime.bmt_Medium, true);
        //    //    iNumGuiaD.Value = "0";
        //    //    //return;
        //    //}

        //    //if (iNumeroGuia.ToString().Length > 9)
        //    //{
        //    //    B1Connections.SboApp.SetStatusBarMessage("Numero de caracteres permitidos 9", BoMessageTime.bmt_Medium, true);
        //    //    iNumGuiaD.Value = "0";
        //    //}
        //    //else if (iNumeroGuia.ToString().Length > 0 && iNumeroGuia.ToString().Length < 9)
        //    //{
        //    //    iNumGuiaD.Value = iNumeroGuia.ToString().ToString().PadLeft(9, '0');
        //    //}
        //    return;

        //}



    }
}
