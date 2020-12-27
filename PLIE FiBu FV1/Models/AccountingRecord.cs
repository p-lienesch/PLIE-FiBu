using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLIE_FiBu_FV1.Models
{
    class AccountingRecord : Models.Template
    {
        //Fields
        string name, name2;
        DateTime date;
        bool posted;
        //Methods
        public bool SetDate(DateTime date)
        {
            if (OnBeforeUpdate(Variable.date) &
                this.date != date)
            {
                this.date = date;
                return true;
            }
            else
            {
                return false;
            }
        }
        public DateTime GetDate()
        {
            return date;
        }
        public bool SetID(Int32 id)
        {
            if (OnBeforeUpdate(Variable.primary_key) &
                this.primary_key != id)
            {
                this.primary_key = id;
                return true;
            }
            else
            {
                return false;
            }
        }
        public Int32 GetID()
        {
            return primary_key;
        }
        public bool SetName(string name)
        {
            if (OnBeforeUpdate(Variable.name) &
                this.name != name &
                CandidateIsUnice(name, this))
            {
                this.name = name;
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetName()
        {
            return name;
        }
        public bool SetName2(string name2)
        {
            if (OnBeforeUpdate(Variable.name2) &
                this.name2 != name2)
            {
                this.name2 = name2;
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetName2()
        {
            return name2;
        }
        public bool SetPosted(bool posted)
        {
            if (OnBeforeUpdate(Variable.posted) &
                posted)
            {
                this.posted = posted;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool GetPosted()
        {
            return posted;
        }
        public List<Int32> GetAccountingRecordLineEntryNos()
        {
            //AuxVariables
            List<object> objects;
            List<Int32> result;
            //Run Method
            result = new List<int>();
            objects = Read(Controllers.ClassType.accounting_record_line);
            foreach (object obj in objects)
            {
                Models.AccountingRecordLine temp = (Models.AccountingRecordLine)obj;
                if (temp.GetAccountingRecordID() == primary_key)
                {
                    result.Add(temp.GetEntryNo());
                }
            }
            return result;
        }
        public bool AccountIDExistsInLine(Int32 account_id)
        {
            //AuxVariables
            bool result;
            //Run Method
            result = false;
            foreach (Int32 line in GetAccountingRecordLineEntryNos())
            {
                Models.AccountingRecordLine temp = new AccountingRecordLine(line);
                if (temp.GetAccountID() == account_id)
                {
                    result = true;
                }
            }
            return result;
        }
        protected override bool OnBeforeCreate()
        {
            return name != "";
        }
        protected override bool OnBeforeDelete()
        {
            return !posted;
        }
        protected override bool OnAfterDelete(object obj)
        {
            return ((Models.AccountingRecord)obj).GetCurrObjectStatus() == ObjectStatus.deleted;
        }
        protected override bool OnBeforeUpdate(Variable variable)
        {
            switch (obj_status)
            {
                case ObjectStatus.temp:
                    switch (variable)
                    {
                        case Variable.primary_key:
                        case Variable.name:
                        case Variable.name2:
                        case Variable.date:
                            return !posted;
                        case Variable.posted:
                            return true;
                        default:
                            return false;
                    }
                case ObjectStatus.created:
                case ObjectStatus.saved:
                    switch (variable)
                    {
                        case Variable.name:
                        case Variable.name2:
                        case Variable.date:
                            return !posted;
                        case Variable.posted:
                            return true;
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }
        protected override void OnAfterRead(object obj)
        {
            //AuxVariables
            Models.AccountingRecord temp;
            //Run Method
            if (obj is Models.AccountingRecord)
            {
                temp = (Models.AccountingRecord)obj;
                if (temp.GetCurrObjectStatus() == ObjectStatus.temp)
                {
                    obj_status = ObjectStatus.saved;
                    primary_key = temp.GetID();
                    name = temp.GetName();
                    name2 = temp.GetName2();
                    date = temp.GetDate();
                    posted = temp.GetPosted();
                }
            }
        }
        //Constructors
        public AccountingRecord(bool temp) : base(temp)
        {

        }
        public AccountingRecord(Int32 id) : base(id)
        {

        }
    }
}
