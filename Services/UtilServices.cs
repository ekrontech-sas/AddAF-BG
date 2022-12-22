using B1Framework.B1Frame;
using SAPbouiCOM;
using System;

namespace bagant.Services
{
    class UtilServices
    {
        public static void AddFieldUdf(string tabla, bool isAutoIncrement = false, string code = null, params string[] valores)
        {
            var userDefinedTable = B1Connections.DiCompany.UserTables.Item(tabla);
            var error = "";
            try
            {
                if (code != null)
                {
                    if (!isAutoIncrement)
                        userDefinedTable.Code = code;

                    userDefinedTable.Name = code;
                }

                for (var i = 0; i < userDefinedTable.UserFields.Fields.Count; i++)
                {
                    var acxz = userDefinedTable.UserFields.Fields.Item(i).Name;
                    userDefinedTable.UserFields.Fields.Item(i).Value = valores[i];
                }

                userDefinedTable.Add();
                error = B1Connections.DiCompany.GetLastErrorDescription();
            }
            catch (Exception ex)
            {
                Log.Error("Excepcion AddFieldUdf ->> "+ex.Message);
            }
            finally
            {

            }
        }

        public static void SetCflConditions(B1Forms Form, string uidCfl, string[] alias, BoConditionOperation[] operation, string[] value)
        {
            Conditions conditions = null;
            Condition condition = null;
            try
            {
                conditions = Form.ChooseFromLists.Item(uidCfl).GetConditions();

                for (int i = 0; i < alias.Length; i++)
                {
                    condition = conditions.Add();
                    //condition.BracketOpenNum = i+1;
                    condition.Alias = alias[i];
                    condition.Operation = operation[i];
                    condition.CondVal = value[i];

                    if (i < alias.Length - 1)
                        condition.Relationship = BoConditionRelationship.cr_OR;

                    //condition.BracketCloseNum = i+1;
                }

                Form.ChooseFromLists.Item(uidCfl).SetConditions(conditions);
            }
            catch (Exception)
            {
            }
            finally
            {
                B1Connections.FlushMemory(condition);
                B1Connections.FlushMemory(conditions);
            }
        }

        public static void NewSetCflConditions(B1Forms Formulario, string uidCfl, string[] alias, BoConditionOperation[] operation, string[] value)
        {
            Conditions conditions = null;
            Condition condition = null;
            try
            {
                conditions = Formulario.ChooseFromLists.Item(uidCfl).GetConditions();

                for (int i = 0; i < alias.Length; i++)
                {
                    condition = conditions.Add();
                    condition.Alias = alias[i];
                    condition.Operation = operation[i];
                    condition.CondVal = value[i];

                    if (i < alias.Length - 1)
                        condition.Relationship = BoConditionRelationship.cr_OR;
                }

                Formulario.ChooseFromLists.Item(uidCfl).SetConditions(conditions);
            }
            catch (Exception)
            {
            }
            finally
            {
                B1Connections.FlushMemory(condition);
                B1Connections.FlushMemory(conditions);
            }
        }

        public static void NewSetCflConditionsR(SAPbouiCOM.Form Formulario, string uidCfl, string[] alias, BoConditionOperation[] operation, string[] value)
        {
            Conditions conditions = null;
            Condition condition = null;
            try
            {
                conditions = Formulario.ChooseFromLists.Item(uidCfl).GetConditions();

                for (int i = 0; i < alias.Length; i++)
                {
                    condition = conditions.Add();
                    condition.Alias = alias[i];
                    condition.Operation = operation[i];
                    condition.CondVal = value[i];

                    if (i < alias.Length - 1)
                        condition.Relationship = BoConditionRelationship.cr_OR;
                }

                Formulario.ChooseFromLists.Item(uidCfl).SetConditions(conditions);
            }
            catch (Exception)
            {
            }
            finally
            {
                B1Connections.FlushMemory(condition);
                B1Connections.FlushMemory(conditions);
            }
        }

        public string var_dump(object obj, int recursion)
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();

            // Protect the method against endless recursion
            if (recursion < 5)
            {
                // Determine object type
                System.Type t = obj.GetType();

                // Get array with properties for this object
                System.Reflection.PropertyInfo[] properties = t.GetProperties();

                foreach (System.Reflection.PropertyInfo property in properties)
                {
                    try
                    {
                        // Get the property value
                        object value = property.GetValue(obj, null);

                        // Create indenting string to put in front of properties of a deeper level
                        // We'll need this when we display the property name and value
                        string indent = System.String.Empty;
                        string spaces = "|   ";
                        string trail = "|...";

                        if (recursion > 0)
                        {
                            indent = new System.Text.StringBuilder(trail).Insert(0, spaces, recursion - 1).ToString();
                        }

                        if (value != null)
                        {
                            // If the value is a string, add quotation marks
                            string displayValue = value.ToString();
                            if (value is string) displayValue = System.String.Concat('"', displayValue, '"');

                            // Add property name and value to return string
                            result.AppendFormat(" {0}{1} = {2}\n ", indent, property.Name, displayValue);

                            try
                            {
                                if (!(value is System.Collections.ICollection))
                                {
                                    // Call var_dump() again to list child properties
                                    // This throws an exception if the current property value
                                    // is of an unsupported type (eg. it has not properties)
                                    result.Append(var_dump(value, recursion + 1));
                                }
                                else
                                {
                                    // 2009-07-29: added support for collections
                                    // The value is a collection (eg. it's an arraylist or generic list)
                                    // so loop through its elements and dump their properties
                                    int elementCount = 0;
                                    foreach (object element in ((System.Collections.ICollection)value))
                                    {
                                        string elementName = System.String.Format(" {0}[{1}] ", property.Name, elementCount);
                                        indent = new System.Text.StringBuilder(trail).Insert(0, spaces, recursion).ToString();

                                        // Display the collection element name and type
                                        result.AppendFormat(" {0}{1} = {2}\n ", indent, elementName, element.ToString());

                                        // Display the child properties
                                        result.Append(var_dump(element, recursion + 2));
                                        elementCount++;
                                    }

                                    result.Append(var_dump(value, recursion + 1));
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            // Add empty (null) property to return string
                            result.AppendFormat(" {0}{1} = {2}\n ", indent, property.Name, "null");
                        }
                    }
                    catch
                    {
                        // Some properties will throw an exception on property.GetValue()
                        // I don't know exactly why this happens, so for now i will ignore them...
                    }
                }
            }

            return result.ToString();
        }
    }
}
