using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;

namespace PLIE_FiBu_FV1.Controllers.DataHandlers
{
    class accdb : Controllers.StorageController
    {
        //Fields
        string path;
        OleDbDataAdapter reader;
        OleDbConnection connection;
        //Methods
        public override object Read(Int32 primary_key, ClassType class_type)
        {
            if (DataBaseCheck(path))
            {

            }
        }
        public override List<object> Read(ClassType class_type)
        {
            if (DataBaseCheck(path))
            {

            }
        }
        public override object Write(object obj)
        {
            if (DataBaseCheck(path))
            {

            }
        }
        public bool DataBaseCheck(string path)
        {
            if (File.Exists(path))
            {
                this.path = path;
                return true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("This Database does not exists.",
                                                     "Error",
                                                     System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }
        public bool SetPath(string path)
        {
            if (path != "")
            {
                this.path = path;
                return true;
            }
            else
            {
                return false;
            }
        }
        //Constructors
        public accdb(string path)
        {
            this.path = path;
        }
    }
}
