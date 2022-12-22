using B1Framework.B1Frame;
using bagant.Services;
using SAPbouiCOM;
using System;

namespace bagant.Form._2010012021
{
    class Grid_Item_5 : B1Item
    {
        public Grid_Item_5()
        {
            FormType = "2010012021";
            ItemUID = "Item_5";
        }

        [B1Listener(BoEventTypes.et_DOUBLE_CLICK, false)]
        public virtual void OnAfterDoubleClick(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
        }

        [B1Listener(BoEventTypes.et_VALIDATE, true)]
        public virtual bool OnBeforeValidate(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);

            return true;
        }

        [B1Listener(BoEventTypes.et_MATRIX_LINK_PRESSED, true)]
        public virtual bool OnBeforeMatrixLinkPressed(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
    
            return true;
        }

    }
}
