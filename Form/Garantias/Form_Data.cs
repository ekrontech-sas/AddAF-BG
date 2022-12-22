using B1Framework.B1Frame;
using B1Framework.RecordSet;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Form.UDO_FT_BAG_GARANTIAS
{
    class Form_Data : B1Form
    {
        public Form_Data()
        {
            FormType = "UDO_FT_BAG_GARANTIAS";
        }

        [B1Listener(BoEventTypes.et_FORM_DATA_LOAD, false)]
        public virtual void OnAfterFormDataLoad(BusinessObjectInfo pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            var pUser = "";
            if (pVal.ActionSuccess)
            {
                pUser = B1Connections.SboApp.Company.UserName;
                var queryContractData = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Hana.GetUserData.sql", GetType().Assembly), pUser);
                var resp = Record.Instance.Query(queryContractData).Execute().First();
                var pDato = resp["Permission"].ToString();
                var pBolP = true;
                var pBolNoP = false;
                if (pDato == "F")
                {
                    Form.Items.Item("0_U_G").Enabled = pBolP;
                    var matrixData = (SAPbouiCOM.Matrix)Form.Items.Item("0_U_G").Specific;
                    matrixData.Columns.Item("C_0_1").Editable = pBolP;
                    matrixData.Columns.Item("C_0_2").Editable = pBolP;
                    matrixData.Columns.Item("C_0_3").Editable = pBolP;
                    matrixData.Columns.Item("C_0_4").Editable = pBolP;
                    matrixData.Columns.Item("C_0_5").Editable = pBolP;
                    matrixData.Columns.Item("C_0_6").Editable = pBolP;
                    matrixData.Columns.Item("C_0_7").Editable = pBolP;
                }
                else
                {
                    Form.Items.Item("0_U_G").Enabled = pBolNoP;
                    var matrixData = (SAPbouiCOM.Matrix)Form.Items.Item("0_U_G").Specific;
                    matrixData.Columns.Item("C_0_1").Editable = pBolNoP;
                    matrixData.Columns.Item("C_0_2").Editable = pBolNoP;
                    matrixData.Columns.Item("C_0_3").Editable = pBolNoP;
                    matrixData.Columns.Item("C_0_4").Editable = pBolNoP;
                    matrixData.Columns.Item("C_0_5").Editable = pBolNoP;
                    matrixData.Columns.Item("C_0_6").Editable = pBolNoP;
                    matrixData.Columns.Item("C_0_7").Editable = pBolNoP;
                }

                //if (Form.Mode == BoFormMode.fm_OK_MODE)
                //{
                //    Form.Items.Item("0_U_G").Enabled = false;
                //    var matrixData = (SAPbouiCOM.Matrix)Form.Items.Item("0_U_G").Specific;
                //    matrixData.Columns.Item("C_0_1").Editable = false;
                //    matrixData.Columns.Item("C_0_2").Editable = false;
                //    matrixData.Columns.Item("C_0_3").Editable = false;
                //    matrixData.Columns.Item("C_0_4").Editable = false;
                //    matrixData.Columns.Item("C_0_5").Editable = false;
                //    matrixData.Columns.Item("C_0_6").Editable = false;
                //    matrixData.Columns.Item("C_0_7").Editable = false;
                //}
                //if (Form.Mode == BoFormMode.fm_ADD_MODE)
                //{
                //    Form.Items.Item("0_U_G").Enabled = true;
                //    var matrixData = (SAPbouiCOM.Matrix)Form.Items.Item("0_U_G").Specific;
                //    matrixData.Columns.Item("C_0_1").Editable = true;
                //    matrixData.Columns.Item("C_0_2").Editable = true;
                //    matrixData.Columns.Item("C_0_3").Editable = true;
                //    matrixData.Columns.Item("C_0_4").Editable = true;
                //    matrixData.Columns.Item("C_0_5").Editable = true;
                //    matrixData.Columns.Item("C_0_6").Editable = true;
                //    matrixData.Columns.Item("C_0_7").Editable = true;
                //}
            }
        }

        [B1Listener(BoEventTypes.et_ITEM_PRESSED, true)]
        public virtual bool OnBeforeItemPressed(ItemEvent pVal)
        {
            //Form = new B1Forms(pVal.FormUID);
            SAPbouiCOM.Form oForm = B1Connections.SboApp.Forms.Item(pVal.FormTypeEx);
            if (oForm.Mode == BoFormMode.fm_ADD_MODE && pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED && pVal.BeforeAction == true)
            {
                oForm.Items.Item("0_U_G").Enabled = true;
                var matrixData = (SAPbouiCOM.Matrix)oForm.Items.Item("0_U_G").Specific;
                matrixData.Columns.Item("C_0_1").Editable = true;
                matrixData.Columns.Item("C_0_2").Editable = true;
                matrixData.Columns.Item("C_0_3").Editable = true;
                matrixData.Columns.Item("C_0_4").Editable = true;
                matrixData.Columns.Item("C_0_5").Editable = true;
                matrixData.Columns.Item("C_0_6").Editable = true;
                matrixData.Columns.Item("C_0_7").Editable = true;

            }
            return true;
        }

    }
}
