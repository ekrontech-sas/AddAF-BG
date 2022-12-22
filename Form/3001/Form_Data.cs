using B1Framework.B1Frame;
using SAPbouiCOM;

namespace bagant.Form._3001
{
    class Form_Load : B1Form
    {
        public Form_Load()
        {
            FormType = "3001";
        }

        [B1Listener(BoEventTypes.et_FORM_LOAD, false)]
        public virtual void OnBeforeLoad(ItemEvent pVal)
        {
            Form = new B1Forms(pVal.FormUID);
            return;
        }

    }
}
