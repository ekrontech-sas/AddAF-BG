using B1Framework.B1Frame;
using SAPbouiCOM;
using System;

namespace bagant.Services
{
    class MatrixServices : B1Base
    {
        public void deleteRow(string matrixUid, string formUid)
        {
            var _form = new B1Forms(formUid);
            var matrix = ((SAPbouiCOM.Matrix)_form.Items.Item(matrixUid).Specific);

            for (int i = matrix.RowCount; i >= 1; i--)
                if (matrix.IsRowSelected(i))
                    matrix.DeleteRow(i);

            var numerationUID = matrix.Columns.Item(0).UniqueID;
            for (int i = 1; i <= matrix.RowCount; i++)
                ((EditText)matrix.GetCellSpecific(numerationUID, i)).Value = i.ToString();
        }

    }
}
