using B1Framework.B1Frame;
using bagant.Services;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Xml;

namespace bagant.Form._180
{
    class Form_Data : B1Form
    {
        public Form_Data()
        {
            FormType = "180";
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        public virtual void OnAfterFormDataAdd(BusinessObjectInfo pVal)
        {
            if (pVal.ActionSuccess)
            {
                var xml = new XmlDocument();
                xml.LoadXml(pVal.ObjectKey);

                var returDoc = (Documents) B1Connections.DiCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oReturns);
                    returDoc.GetByKey(int.Parse(xml.InnerText));

                var sapServ = new SAPServices();
                if ( string.IsNullOrEmpty(returDoc.UserFields.Fields.Item("U_BGT_DevProv").Value.ToString()))
                {
                    sapServ.AddMovementData(xml.InnerText, "ORDN", "RDN1", "16");
                    sapServ.CheckUDOContract(xml.InnerText, "ORDN", "16");
                }
                else
                {
                    sapServ.AddMovementData(xml.InnerText, "ORDN", "RDN1", "116");
                }
            }
        }

        [B1Listener(BoEventTypes.et_FORM_LOAD, true)]
        public virtual bool OnAfterFormLoad(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var btn = Form.Items.Item("10000330");
            var itemsXml = B1Util.Formato(GetEmbeddedResource("bagant.Form._180.xml.Update.xml"), pVal.FormUID, btn.Top, btn.Left-111, btn.Height);

            B1Connections.SboApp.LoadBatchActions(itemsXml);

            try
            {
                var txtCopy = Form.Items.Item("16");
                /*
                var lblCopy = Form.Items.Item("17");
                var lblContrato = Form.Items.Add("lblCont", BoFormItemTypes.it_STATIC);
                ((StaticText)lblContrato.Specific).Caption = "Numero de Contrato";
                lblContrato.Left = lblCopy.Left;
                lblContrato.Top = lblCopy.Top - 30;
                lblContrato.Width = 130;

                var txtContrato = Form.Items.Add("txtCont", BoFormItemTypes.it_EDIT);
                txtContrato.Left = txtCopy.Left;
                txtContrato.Top = txtCopy.Top - 30;
                ((EditText)txtContrato.Specific).DataBind.SetBound(true, "ORDN", "U_BGT_ContAsoc");
                txtContrato.Width = 100;
                */

                var linkContrato = Form.Items.Add("linkCont", BoFormItemTypes.it_LINKED_BUTTON);
                    linkContrato.Top = txtCopy.Top - 20;
                    linkContrato.Left = txtCopy.Left + txtCopy.Width + 1;
                    linkContrato.LinkTo = "U_BGT_ContAsoc";
                    linkContrato.RightJustified = true;
            }
            catch(Exception ex)
            {
                Log.Info("Exception => " + ex.Message);
            }

            return true;
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, true)]
        public virtual bool OnBeforeItemPressed(ItemEvent pVal)
        {
            SAPbouiCOM.Form oForm = B1Connections.SboApp.Forms.Item(pVal.FormTypeEx);
            oForm.Items.Item("U_BGT_FecFact").Enabled = false;
            oForm.Items.Item("U_BGT_ContAsoc").Enabled = false;
            oForm.Items.Item("U_NUM_GUIA").Enabled = false;
            oForm.Items.Item("U_BGT_DevProv").Enabled = false;
            return true;
        }


    }
}
