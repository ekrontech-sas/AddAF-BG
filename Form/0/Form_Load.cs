using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._0
{
    class Form_Load : B1Form
    {
        public Form_Load()
        {
            FormType = "0";
        }

        [B1Listener(BoEventTypes.et_FORM_LOAD, false)]
        public virtual void OnBeforeLoad(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            if ( _2002122020.Button_Item_99.ProvieneContrato )
            {
                Form.DataSources.UserDataSources.Add("isCont", BoDataType.dt_SHORT_TEXT, 1);
                Form.DataSources.UserDataSources.Item("isCont").Value = "1";

                var itemLbl = (StaticText)Form.Items.Item("7").Specific;
                var origLeft = itemLbl.Item.Left;
                    itemLbl.Item.Left = -200;

                Form.DataSources.UserDataSources.Add("dtIni", BoDataType.dt_DATE);
                Form.DataSources.UserDataSources.Add("dtFin", BoDataType.dt_DATE);

                Form.Items.Add("lblFecIn", BoFormItemTypes.it_STATIC);
                Form.Items.Add("txtFecIn", BoFormItemTypes.it_EDIT);
                Form.Items.Add("lblFecFin", BoFormItemTypes.it_STATIC);
                Form.Items.Add("txtFecFin", BoFormItemTypes.it_EDIT);

                var lblIn = (StaticText)Form.Items.Item("lblFecIn").Specific;
                    lblIn.Caption = "Rango de Fecha de Inicio";
                    lblIn.Item.Width = 150;
                    lblIn.Item.Left = origLeft;
                    lblIn.Item.Top = itemLbl.Item.Top;

                var txtIn = (EditText)Form.Items.Item("txtFecIn").Specific;
                    txtIn.Item.Width = 100;
                    txtIn.Item.Left = lblIn.Item.Left+ lblIn.Item.Width+ 10;
                    txtIn.Item.Top = itemLbl.Item.Top;

                var lblFin = (StaticText)Form.Items.Item("lblFecFin").Specific;
                    lblFin.Caption = "Rango de Fecha de Fin";
                    lblFin.Item.Width = 150;
                    lblFin.Item.Left = origLeft;
                    lblFin.Item.Top = itemLbl.Item.Top + 20;

                var txtFin = (EditText)Form.Items.Item("txtFecFin").Specific;
                    txtFin.Item.Width = 100;
                    txtFin.Item.Left = origLeft + lblFin.Item.Width + 10;
                    txtFin.Item.Top = itemLbl.Item.Top + 20;

                txtIn.DataBind.SetBound(true, "", "dtIni");
                txtFin.DataBind.SetBound(true, "", "dtFin");
            }
            return;
        }

        //[B1Listener(BoEventTypes.et_FORM_DATA_ADD, false)]
        //public virtual void OnAfterFormDataAdd(ItemEvent pVal)
        //{
        //    Form = new B1Forms(pVal.FormUID);
        //}

        }
}
