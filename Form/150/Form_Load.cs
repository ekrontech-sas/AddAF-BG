using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._150
{
    class Form_Load : B1Form
    {
        public Form_Load()
        {
            FormType = "150";
        }

        [B1Listener(BoEventTypes.et_FORM_LOAD, true)]
        public virtual bool OnBeforeLoad(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            CreateStructure(pVal.FormUID);

            //SAPbouiCOM.Item oItem;
            //SAPbouiCOM.Item oNewItem = Form.Items.Add("Tab_ActivoFijo", BoFormItemTypes.it_FOLDER);
            //oItem = Form.Items.Item("uaf_0");
            //oNewItem.Top = oItem.Top;
            //oNewItem.Height = oItem.Height;
            //oNewItem.Width = oItem.Width;
            //oNewItem.Left = oItem.Left+oItem.Width;
            //SAPbouiCOM.Folder oFolderItem = oNewItem.Specific;
            //oFolderItem.Caption = "Resumen";
            ////var pFolderAF = Form.Items.Item("uaf_0").Specific;
            ////oItem.Specific.Add(oFolderItem);
            ////oFolderItem.GroupWith("uaf_0");
            //Form.PaneLevel = 1;
            return true;
        }

        private void CreateStructure(string formUID)
        {
            var item = Form.Items.Item("12");
            var itemsXml = B1Util.Formato(B1Util.GetEmbeddedResource("bagant.Form._150.xml.UpdateItem.xml", GetType().Assembly), formUID, item.Top+14, item.Left);
            B1Connections.SboApp.LoadBatchActions(itemsXml);
        }

    }
}
