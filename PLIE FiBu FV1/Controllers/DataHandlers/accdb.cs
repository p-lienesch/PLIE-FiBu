using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace PLIE_FiBu_FV1.Controllers.DataHandlers
{
    class Accdb : Controllers.DataHandlers.Template
    {
        //Fields
        string path;
        bool valid;
        OleDbDataReader reader;
        OleDbConnection connection;
        //Methods
        public bool DatabaseIsValid()
        {
            if (valid)
            {
                //Check if File exists if not, create them
                if (!System.IO.File.Exists(path))
                {
                    System.IO.File.Create(path);
                }
                //Check if requestet Tables exists. If not, create them
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Delete()
        {
            connection.Close();
            valid = false;
        }
        public override List<object> Read(ClassType class_type)
        {
            //AuxVariables
            List<object> result;
            string table_name;
            //Run Method
            result = new List<object>();
            if (OpenConnection())
            {
                if ((table_name = GetTableName(class_type)) != "")
                {
                    reader = new OleDbCommand("SELECT * FROM "+table_name+";", connection).ExecuteReader();
                    while (reader.Read())
                    {
                        object temp = ReaderToObject(class_type);
                        if (temp != new object())
                        {
                            result.Add(temp);
                        }
                    }
                }
                CloseConnection();
            }
            return result;
        }
        private object ReaderToObject(ClassType class_type)
        {
            switch (class_type)
            {

                case ClassType.account:
                    Models.Account temp = new Models.Account(true);
                    temp.SetID(Convert.ToInt32(reader["ID"]));
                    temp.SetUpperAccountID(Convert.ToInt32(reader["UpperAccountID"]));
                    temp.SetName(reader["TheName"].ToString());
                    temp.SetName2(reader["TheName2"].ToString());
                    temp.SetAccessible(Convert.ToBoolean(reader["Accessible"]));
                    temp.SetAsset(Convert.ToBoolean(reader["Asset"]));
                    temp.SetConsisted(Convert.ToBoolean(reader["Consisted"]));
                    return temp;
                case ClassType.accounting_record:
                    Models.AccountingRecord temp2 = new Models.AccountingRecord(true);
                    temp2.SetID(Convert.ToInt32(reader["ID"]));
                    temp2.SetName(reader["TheDescription"].ToString());
                    temp2.SetName2(reader["TheDescription2"].ToString());
                    temp2.SetDate(Convert.ToDateTime(reader["TheDate"]));
                    temp2.SetPosted(Convert.ToBoolean(reader["Posted"]));
                    return temp2;
                case ClassType.accounting_record_line:
                    Models.AccountingRecordLine temp3 = new Models.AccountingRecordLine(true, 0);
                    temp3.SetEntryNo(Convert.ToInt32(reader["ID"]));
                    temp3.SetDescription(reader["TheDescription"].ToString());
                    temp3.SetDescription2(reader["TheDescription2"].ToString());
                    temp3.SetAccountingRecordID(Convert.ToInt32(reader["AccountingRecordID"]));
                    temp3.SetAccountID(Convert.ToInt32(reader["AccountID"]));
                    temp3.SetAmount(Convert.ToInt32(reader["Amount"]));
                    return temp3;
                default:
                    return new object();
            }
        }
        private string GetTableName(ClassType class_type)
        {
            switch (class_type)
            {
                case ClassType.account:
                    return "Account";
                case ClassType.accounting_record:
                    return "AccountingRecord";
                case ClassType.accounting_record_line:
                    return "AccountingRecordLine";
                default:
                    return "";
            }
        }
        public override object Read(int primary_key, ClassType class_type)
        {
            //AuxVariables
            object result;
            string table_name;
            //Run Method
            result = new object();
            if (OpenConnection())
            {
                if ((table_name = GetTableName(class_type)) != "")
                {
                    reader = new OleDbCommand("SELECT * FROM " + table_name + " WHERE ID = "+primary_key+";", connection).ExecuteReader();
                    while (reader.Read())
                    {
                        object temp = ReaderToObject(class_type);
                        if (temp != new object())
                        {
                            result = temp;
                        }
                    }
                }
                CloseConnection();
            }
            return result;

        }
        public override object Write(object obj, bool delete)
        {
            //AuxVariables
            object result;
            string table_name;
            Controllers.ClassType class_type;
            OleDbCommand command;
            //Run Method
            result = obj;
            if (OpenConnection())
            {
                if ((table_name = 
                      GetTableName(
                        new Controllers.StorageController("","").
                          GetClassTypeOfObject(obj))) != "")
                {
                    command = new OleDbCommand();
                    command.Connection = connection;
                    class_type = new Controllers.StorageController("", "").
                                   GetClassTypeOfObject(obj);
                    switch (class_type)
                    {
                        case ClassType.account:
                            result = WriteAccount(obj, delete, command);
                            break;
                        case ClassType.accounting_record:
                            result = WriteAccountingRecord(obj, delete, command);
                            break;
                        case ClassType.accounting_record_line:
                            result = WriteAccountingRecordLine(obj, delete, command);
                            break;
                        default:
                            break;
                    }
                }
                CloseConnection();
            }
            return result;
        }
        private object WriteAccount(object obj, bool delete, OleDbCommand command)
        {
            //AuxVariables
            Models.Account temp;
            object result;
            //Run Method
            result = obj;
            if (obj is Models.Account)
            {
                temp = (Models.Account)obj;
                if (temp.GetCurrObjectStatus() == Models.ObjectStatus.created)
                {

                }
                else if (temp.GetCurrObjectStatus() == Models.ObjectStatus.saved)
                {
                    if (delete)
                    {

                    }
                    else
                    {

                    }
                }
                if (temp.GetCurrObjectStatus() == Models.ObjectStatus.created |
                    temp.GetCurrObjectStatus() == Models.ObjectStatus.created)
                {
                    command.ExecuteNonQuery();
                }
            }
            return result;
        }
        private object WriteAccountingRecord(object obj, bool delete, OleDbCommand command)
        {
            //AuxVariables
            Models.AccountingRecord temp;
            object result;
            //Run Method
            result = obj;
            if (obj is Models.AccountingRecord)
            {
                temp = (Models.AccountingRecord)obj;
                if (temp.GetCurrObjectStatus() == Models.ObjectStatus.created)
                {

                }
                else if (temp.GetCurrObjectStatus() == Models.ObjectStatus.saved)
                {
                    if (delete)
                    {

                    }
                    else
                    {

                    }
                }
                if (temp.GetCurrObjectStatus() == Models.ObjectStatus.created |
                    temp.GetCurrObjectStatus() == Models.ObjectStatus.created)
                {
                    command.ExecuteNonQuery();
                }
            }
            return result;
        }
        private object WriteAccountingRecordLine(object obj, bool delete, OleDbCommand command)
        {
            //AuxVariables
            Models.AccountingRecordLine temp;
            object result;
            //Run Method
            result = obj;
            if (obj is Models.AccountingRecordLine)
            {
                temp = (Models.AccountingRecordLine)obj;
                if (temp.GetCurrObjectStatus() == Models.ObjectStatus.created)
                {
                     
                }
                else if (temp.GetCurrObjectStatus() == Models.ObjectStatus.saved)
                {
                    if (delete)
                    {

                    }
                    else
                    {

                    }
                }
                if (temp.GetCurrObjectStatus() == Models.ObjectStatus.created |
                    temp.GetCurrObjectStatus() == Models.ObjectStatus.created)
                {
                    command.ExecuteNonQuery();
                }
            }
            return result;
        }
        private Models.ObjectStatus GetCurrObjectStatus(object obj, Controllers.ClassType class_type)
        {
            switch (class_type)
            {
                case ClassType.account:
                    return ((Models.Account)obj).GetCurrObjectStatus();
                case ClassType.accounting_record:
                    return ((Models.AccountingRecord)obj).GetCurrObjectStatus();
                case ClassType.accounting_record_line:
                    return ((Models.AccountingRecordLine)obj).GetCurrObjectStatus();
                default:
                    return Models.ObjectStatus.none;
            }
        }
        private bool OpenConnection()
        {
            //AuxVariables
            bool result;
            //Run Method
            result = false;
            if (DatabaseIsValid())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;" +
                                                     "Data Source=" + path + ";" +
                                                     "Persist Security Info=False;");
                    connection.Open();
                    result = true;
                }
                else if (connection.State == System.Data.ConnectionState.Open)
                {
                    result = true;
                }
            }
            return result;
        }
        private void CloseConnection()
        {
            connection.Close();
            reader.Close();
        }
        //Constructors
        public Accdb(string path)
        {
            valid = true;
            this.path = path;
            DatabaseIsValid();

        }
    }
}
