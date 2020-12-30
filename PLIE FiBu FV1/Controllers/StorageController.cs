using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLIE_FiBu_FV1.Controllers
{
    public enum ClassType
    {
        none,
        account,
        accounting_record,
        accounting_record_line
    }
    class StorageController : Controllers.DataHandlers.Template
    {
        //Fields
        string settings_path, storage_type, storage_path;
        Controllers.DataHandlers.Accdb accdb;
        //Methods
        public ClassType GetClassTypeOfObject(object obj)
        {
            if (obj is Models.Account)
            {
                return ClassType.account;
            }
            else if (obj is Models.AccountingRecord)
            {
                return ClassType.accounting_record;
            }
            else if (obj is Models.AccountingRecordLine)
            {
                return ClassType.accounting_record_line;
            }
            else
            {
                return ClassType.none;
            }
        }
        public override List<object> Read(ClassType class_type)
        {
            switch (storage_type)
            {
                case "accdb":
                    return accdb.Read(class_type);
                default:
                    return new List<object>();
            }
        }
        public override object Read(int primary_key, ClassType class_type)
        {
            switch (storage_type)
            {
                case "accdb":
                    return accdb.Read(primary_key, class_type);
                default:
                    return new object();
            }
        }
        public override object Write(object obj, bool delete)
        {
            switch (storage_type)
            {
                case "accdb":
                    return accdb.Write(obj, delete);
                default:
                    return new object();
            }
        }
        public void SetStorageTypeValues(string type, string path)
        {
            //AuxVariables
            System.IO.StreamWriter writer;
            //Run Method
            if (!System.IO.File.Exists(settings_path))
            {
                System.IO.File.Create(settings_path);
            }
            writer = new System.IO.StreamWriter(settings_path);
            writer.WriteLine(type);
            writer.WriteLine(path);
            writer.Close();
        }
        private void GetStorageTypeValues()
        {
            //AuxVariables
            System.IO.StreamReader reader;
            string line;
            Int32 counter;
            //Run Method
            storage_path = "";
            storage_type = "";
            if (System.IO.File.Exists(settings_path))
            {
                counter = 0;
                reader = new System.IO.StreamReader(settings_path);
                while ((line = reader.ReadLine()) != null)
                {
                    if (counter == 0)
                    {
                        storage_type = line;
                    }
                    else if (counter == 1)
                    {
                        storage_path = line;
                    }
                    counter++;
                }
                reader.Close();
                if (storage_type == "" | storage_path == "")
                {
                    storage_path = "";
                    storage_type = "";
                }
            }
        }
        //Constructors
        public StorageController(string type, string path)
        {
            if (type != "" & path != "")
            {
                SetStorageTypeValues(type, path);
            }
            GetStorageTypeValues();
            settings_path = "StorageSettings.txt";
            switch (storage_type)
            {
                case "accdb":
                    accdb = new DataHandlers.Accdb(storage_path);
                    break;
                default:
                    break;
            }
        }
    }
}
