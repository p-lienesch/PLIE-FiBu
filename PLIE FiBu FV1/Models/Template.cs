using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLIE_FiBu_FV1.Models
{
    public enum ObjectStatus
    {
        none,
        temp,
        created,
        saved,
        deleted
    }
    public enum Variable
    {
        primary_key,
        upper_account_id,
        name,
        name2,
        accessible,
        asset,
        consisted,
        posted,
        date,
        account_id,
        amount,
        accounting_record_id
    }
    abstract class Template
    {
        //Fields
        Controllers.StorageController storage_controller;
        protected ObjectStatus obj_status;
        protected Int32 primary_key;
        //Methods
        protected abstract bool OnBeforeCreate();
        protected bool Create()
        {
            //AuxVariables
            bool result;
            //Run Method
            result = false;
            if (OnBeforeCreate() & 
                obj_status == ObjectStatus.created)
            {
                OnAfterRead(storage_controller.Write(this));
                if (GetCurrObjectStatus() == ObjectStatus.saved)
                {
                    result = true;
                }
            }
            return result;
        }
        protected abstract bool OnBeforeDelete();
        protected abstract bool OnAfterDelete(object obj);
        protected bool Delete()
        {
            //AuxVariables
            bool result;
            //Run Method
            result = false;
            if (OnBeforeDelete() &
                obj_status == ObjectStatus.saved)
            {
                object obj = storage_controller.Write(this);
                OnAfterDelete(obj);
            }
            return result;
        }
        protected abstract bool OnBeforeUpdate(Variable variable);
        protected void OnAfterUpdate()
        {
            if (obj_status == ObjectStatus.saved)
            {
                OnAfterRead(storage_controller.Write(this));
            }
        }
        protected List<object> Read(Controllers.ClassType class_type)
        {
            return storage_controller.Read(class_type);
        }
        public ObjectStatus GetCurrObjectStatus() 
        {
            return obj_status;
        }
        protected abstract void OnAfterRead(object obj);
        protected bool CandidateIsUnice(string candidate, object obj)
        {
            //AuxVariables
            bool result;
            List<object> objects;
            //Run Method
            result = true;
            if (candidate != "")
            {
                switch (storage_controller.GetClassTypeOfObject(obj))
                {
                    case Controllers.ClassType.account:
                        objects = Read(Controllers.ClassType.account);
                        foreach (object obj2 in objects)
                        {
                            Models.Account temp = (Models.Account)obj2;
                            if (temp.GetName() == candidate)
                            {
                                result = false;
                            }
                        }
                        break;
                    case Controllers.ClassType.accounting_record:
                        objects = Read(Controllers.ClassType.accounting_record);
                        foreach (object obj2 in objects)
                        {
                            Models.AccountingRecord temp = (Models.AccountingRecord)obj2;
                            if (temp.GetName() == candidate)
                            {
                                result = false;
                            }
                        }
                        break;
                    case Controllers.ClassType.accounting_record_line:
                        objects = Read(Controllers.ClassType.accounting_record_line);
                        foreach (object obj2 in objects)
                        {
                            Models.AccountingRecordLine temp = (Models.AccountingRecordLine)obj2;
                            if (temp.GetDescription() == candidate)
                            {
                                result = false;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
        //Constructors
        private void ConstructorCode()
        {
            storage_controller = new Controllers.StorageController("","");
        }
        public Template(bool temp)
        {
            ConstructorCode();
            if (temp)
            {
                obj_status = ObjectStatus.temp;
            }
            else
            {
                obj_status = ObjectStatus.created;
            }
        }
        public Template(Int32 primary_key)
        {
            ConstructorCode();
            this.primary_key = primary_key;
            OnAfterRead(storage_controller.Read(this.primary_key,
                                                storage_controller.GetClassTypeOfObject(this)));
        }
    }
}
