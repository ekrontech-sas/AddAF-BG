using B1Framework.B1Frame;
using B1Framework.RecordSet;
using bagant.Services;
using SAPbobsCOM;
using SAPbouiCOM;
using System;

namespace bagant.Form._2002122020
{
    class EditText_Item_97 : B1Item
    {
        public EditText_Item_97()
        {
            FormType = "2002122020";
            ItemUID = "Item_97";
        }

        [B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, false)]
        public virtual void OnAfterChooseFromList(ItemEvent pVal)
        {
            if (((IChooseFromListEvent)pVal).SelectedObjects == null) return;

            Form = new B1Forms(pVal.FormUID);

            B1Connections.SboApp.SetStatusBarMessage("Cargando datos, por favor espere...", BoMessageTime.bmt_Medium, false);

            var ofDocEntry = ListChoiceListener(pVal, "DocEntry")[0].ToString();
            var ofDocNum = ListChoiceListener(pVal, "DocNum")[0].ToString();
            var category = ListChoiceListener(pVal, "U_BG_CATEGORIA")[0].ToString();

            var docOf = (Documents)B1Connections.DiCompany.GetBusinessObject(BoObjectTypes.oQuotations);
            if( docOf.GetByKey(int.Parse(ofDocEntry)) )
            {
                Form.Freeze(true);

                try
                {
                    var queryOF = B1Util.Formato(GetEmbeddedResource("bagant.Hana.GetValuesOF.sql"), ofDocEntry);
                    var respOF = Record.Instance.Query(queryOF).Execute().First();

                    Form.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("U_BGT_CardCode", 0, respOF["CardCode"]);
                    Form.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("U_BGT_CardName", 0, respOF["CardName"]);
                    Form.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("U_BGT_DocEntryOF", 0, ofDocEntry);
                    Form.DataSources.DBDataSources.Item("@BGT_OCTR").SetValue("U_BGT_DocNumOF", 0, ofDocNum);

                    new CommonTaskSapUI().SetValuesFromSNChoosed(Form, respOF["CardCode"]);
                    new CommonTaskSapUI().SetTableData(Form, ofDocEntry);
                    new CommonTaskSapUI().SetOpportunities(Form, ofDocEntry);
                    new CommonTaskSapUI().SetCategory(Form, category);
                }
                catch(Exception ex)
                {
                    Log.Error("Exception (OnAfterChooseFromList) => " + ex.Message);
                }
                finally
                {
                    Form.Freeze(false);
                    B1Connections.SboApp.SetStatusBarMessage("Carga de datos finalizada", BoMessageTime.bmt_Medium, false);
                }
            }
        }

    }
}
