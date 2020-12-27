using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLIE_FiBu_FV1.Models
{
    class AccountingRecordLine : Models.Template
    {
        //Fields
        string description, description2;
        Int32 account_id, accounting_record_id;
        double amount;
        //Methods
        public bool SetAccountID(Int32 account_id)
        {
            if (OnBeforeUpdate(Variable.account_id) &
                this.account_id != account_id & 
                AmountAccontIDCheck(false,amount,account_id))
            {
                this.account_id = account_id;
                return true;
            }
            else
            {
                return false;
            }
        }
        public Int32 GetAccountID()
        {
            return account_id;
        }
        public bool SetAccountingRecordID(Int32 accounting_record_id)
        {
            if (OnBeforeUpdate(Variable.accounting_record_id) &
                this.accounting_record_id != accounting_record_id)
            {
                this.accounting_record_id = accounting_record_id;
                return true;
            }
            else
            {
                return false;
            }
        }
        public Int32 GetAccountingRecordID()
        {
            return accounting_record_id;
        }
        public bool SetEntryNo(Int32 entry_no)
        {
            if (OnBeforeUpdate(Variable.primary_key) &
                this.primary_key != entry_no)
            {
                this.primary_key = entry_no;
                return true;
            }
            else
            {
                return false;
            }
        }
        public Int32 GetEntryNo()
        {
            return primary_key;
        }
        public bool SetDescription(string description)
        {
            if (OnBeforeUpdate(Variable.name) &
                this.description != description)
            {
                this.description = description;
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetDescription()
        {
            return description;
        }
        public bool SetDescription2(string description2)
        {
            if (OnBeforeUpdate(Variable.name2) &
                this.description2 != description2)
            {
                this.description2 = description2;
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetDescription2()
        {
            return description2;
        }
        public bool SetAmount(double amount)
        {
            if (OnBeforeUpdate(Variable.amount) &
                this.amount != amount &
                AmountAccontIDCheck(true,amount,account_id))
            {
                this.amount = amount;
                return true;
            }
            else
            {
                return false;
            }
        }
        public double GetAmount()
        {
            return amount;
        }
        private bool AmountAccontIDCheck(bool amount_changed, double amount, Int32 account_id)
        {
            //AuxVariables
            double tmp_saldo;
            List<object> objects;
            //Run Method
            if (account_id != 0)
            {
                objects = Read(Controllers.ClassType.account);
                tmp_saldo = 0;
                foreach (object obj in objects)
                {
                    Models.Account temp = (Models.Account)obj;
                    if (temp.GetID() == account_id)
                    {
                        tmp_saldo = temp.GetSaldo();
                    }
                }
                return tmp_saldo + amount >= 0;
            }
            else
            {
                return true;
            }
        }
        protected override bool OnBeforeCreate()
        {
            return description != "";
        }
        protected override bool OnBeforeDelete()
        {
            return !GetPostetStateOfHdr();
        }
        protected override bool OnAfterDelete(object obj)
        {
            return ((Models.AccountingRecordLine)obj).GetCurrObjectStatus() == ObjectStatus.deleted;
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
                        case Variable.account_id:
                        case Variable.amount:
                        case Variable.accounting_record_id:
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
                        case Variable.account_id:
                        case Variable.amount:
                            return GetPostetStateOfHdr();
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }
        private bool GetPostetStateOfHdr()
        {
            //AuxVariables
            List<object> objects;
            bool result;
            //Run Method
            objects = Read(Controllers.ClassType.accounting_record);
            result = true;
            foreach (object obj in objects)
            {
                Models.AccountingRecord temp = (Models.AccountingRecord)obj;
                if (temp.GetID() == accounting_record_id)
                {
                    result = temp.GetPosted();
                }
            }
            return result;
        }
        protected override void OnAfterRead(object obj)
        {
            //AuxVariables
            Models.AccountingRecordLine temp;
            //Run Method
            if (obj is Models.AccountingRecordLine)
            {
                temp = (Models.AccountingRecordLine)obj;
                if (temp.GetCurrObjectStatus() == ObjectStatus.temp)
                {
                    obj_status = ObjectStatus.saved;
                    primary_key = temp.GetEntryNo();
                    accounting_record_id = temp.GetAccountingRecordID();
                    description = temp.GetDescription();
                    description2 = temp.GetDescription2();
                    account_id = temp.GetAccountID();
                    amount = temp.GetAmount();
                }
            }
        }
        //Constructors
        public AccountingRecordLine(bool temp, Int32 accounting_record_id) : base(temp)
        {
            if (!temp)
            {
                this.accounting_record_id = accounting_record_id;
            }
        }
        public AccountingRecordLine(Int32 entry_no) : base(entry_no)
        {

        }
    }
}
