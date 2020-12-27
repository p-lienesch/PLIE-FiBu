﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PLIE_FiBu_FV1.Controllers
{
    public enum ClassType
    {
        none,
        account,
        accounting_record,
        accounting_record_line
    }
    class StorageController
    {
        //Fields
        string storage_type, storage_path;
        Controllers.DataHandlers.accdb accdb;
        //Methods
        public ClassType GetClassTypeOfObject(object obj)
        {
            if (obj is Models.Account)
            {
                return ClassType.account;
            }
            else
            {
                return ClassType.none;
            }
        }
        public virtual List<object> Read(ClassType class_type)
        {
            switch (storage_type)
            {
                case "accdb":
                    return accdb.Read(class_type);
                default:
                    return new List<object>();
            }
        }
        public virtual object Read(Int32 primary_key, ClassType class_type)
        {
            switch (storage_type)
            {
                case "accdb":
                    return accdb.Read(primary_key, class_type);
                default:
                    return new object();
            }
        }
        public virtual object Write(object obj)
        {
            switch (storage_type)
            {
                case "accdb":
                    return accdb.Write(obj);
                default:
                    return new object();
            }
        }
        //Constructors
        public StorageController(string type, string path)
        {
            //AuxVariables
            string setting_path, line;
            Int32 counter;
            StreamReader reader;
            //Run Method
            setting_path = "Storage.txt";
            storage_type = "";
            storage_path = "";
            if (type != "")
            {
                if (path != "")
                {
                    storage_type = type;
                    storage_path = path;
                }
                else
                {
                    //ToDo
                }
            }
            else
            {
                if (File.Exists(setting_path))
                {
                    counter = 0;
                    reader = new StreamReader(setting_path);
                    while ((line = reader.ReadLine()) != null)
                    {
                        switch (counter)
                        {
                            case 0:
                                storage_type = line;
                                break;
                            case 1:
                                storage_path = line;
                                break;
                            default:
                                break;
                        }
                        counter++;
                    }
                    reader.Close();
                }
            }
            if (storage_path == "" |
                storage_type == "")
            {
                System.Windows.Forms.MessageBox.Show("The Settings are not defined",
                                                     "Error",
                                                     System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Error);
            }
            else
            {
                switch (storage_type)
                {
                    case "accdb":
                        accdb = new DataHandlers.accdb(storage_path);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
