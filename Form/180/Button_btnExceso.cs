using B1Framework.B1Frame;
using B1Framework.RecordSet;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Form._180
{
    class Button_btnExceso : B1Item
    {
        public Button_btnExceso()
        {
            FormType = "180";
            ItemUID = "btnExceso";
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, false)]
        public virtual void OnAfterItemPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            B1Connections.SboApp.Menus.Item("2052").Activate();

            try
            {
                var query = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetSerieReturnDevoEx.sql"), B1Connections.DiCompany.UserSignature);
                var resp = Record.Instance.Query(query).Execute().First();
                var serie = resp["Series"];

                var formOrigUDF = B1Connections.SboApp.Forms.Item(Form.UDFFormUID);
                var cardCode = Form.DataSources.DBDataSources.Item("ORDN").GetValue("CardCode", 0).ToString().Trim();
                var numCont = Form.DataSources.DBDataSources.Item("ORDN").GetValue("U_BGT_ContAsoc", 0).ToString().Trim();
                var docEntry = Form.DataSources.DBDataSources.Item("ORDN").GetValue("DocEntry", 0).ToString().Trim();

                var FormExceso = B1Connections.SboApp.Forms.ActiveForm;
                    FormExceso.Freeze(true);

                try
                {
                    FormExceso.DataSources.UserDataSources.Add("idenExceso", BoDataType.dt_SHORT_TEXT, 10);
                    FormExceso.DataSources.UserDataSources.Item("idenExceso").Value = "1";

                    var FormExcesoUDF = B1Connections.SboApp.Forms.Item(FormExceso.UDFFormUID);
                    var querySerie = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetSerieFromContract.sql"), numCont, "'C_EX', 'G_EX', 'M_EX', 'U_EX'");
                    var respSerie = Record.Instance.Query(querySerie).Execute().First();

                    if (!string.IsNullOrEmpty(respSerie["Serie"]))
                        ((ComboBox)FormExceso.Items.Item("88").Specific).Select(respSerie["Serie"], BoSearchKey.psk_ByValue);

                    new CommonTaskSapUI().CopyUDFFieldsForm(new B1Forms(Form.UDFFormUID), new B1Forms(FormExceso.UDFFormUID));
                    var DevProc = B1Connections.SboApp.Forms.Item(FormExceso.UDFFormUID).Items.Item("U_BGT_DevProv");
                    FormExceso.Items.Item("U_BGT_ContAsoc").Enabled = true;
                    DevProc.Enabled = true;
                    ((EditText)FormExceso.Items.Item("4").Specific).Value = cardCode;
                    ((EditText)FormExceso.Items.Item("U_BGT_ContAsoc").Specific).Value = numCont;
                    //((EditText)B1Connections.SboApp.Forms.Item(FormExceso.UDFFormUID).Items.Item("U_BGT_DevProv").Specific).Value = docEntry;
                    ((EditText)DevProc.Specific).Value = docEntry;
                    ((ComboBox)FormExceso.Items.Item("88").Specific).Select(serie, BoSearchKey.psk_ByValue);
                    FormExceso.Items.Item("U_BGT_ContAsoc").Enabled = false;
                    DevProc.Enabled = false;

                    var shipToCodeOrig = Form.DataSources.DBDataSources.Item("ORDN").GetValue("ShipToCode", 0).ToString();
                    var payToCodeOrig = Form.DataSources.DBDataSources.Item("ORDN").GetValue("PayToCode", 0).ToString();
                    var proyectOrig = Form.DataSources.DBDataSources.Item("ORDN").GetValue("Project", 0).ToString();

                    FormExceso.Items.Item("114").Click();
                    ((ComboBox)FormExceso.Items.Item("40").Specific).Select(shipToCodeOrig, BoSearchKey.psk_ByValue);
                    ((ComboBox)FormExceso.Items.Item("226").Specific).Select(payToCodeOrig, BoSearchKey.psk_ByValue);
                    FormExceso.Items.Item("138").Click();
                    ((EditText)FormExceso.Items.Item("157").Specific).Value = proyectOrig;
                    FormExceso.Items.Item("112").Click();
                }
                finally
                {
                    FormExceso.Freeze(false);
                }
            }
            catch(Exception ex)
            {
                Log.Info("(OnAfterItemPressed) Exception => " + ex.Message);
            }

            Form.Close();
        }

    }
}
