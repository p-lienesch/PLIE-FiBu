using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLIE_FiBu_FV1.Models
{
    class Account : Models.Template
    {
        //Fields
        Int32 upper_account_id;
        string name, name2;
        bool accessible, asset, consisted;
        //Methods
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
        public bool SetUpperAccountID(Int32 upper_account_id)
        {
            //AuxVariables
            bool valid;
            //Run Method
            valid = false;
            foreach (Int32 possible_upper_account_id in GetPossibleUpperAccountIDs())
            {
                if (possible_upper_account_id == upper_account_id)
                {
                    valid = true;
                }
            }
            if (OnBeforeUpdate(Variable.upper_account_id) &
                this.upper_account_id != upper_account_id & 
                valid)
            {
                this.upper_account_id = upper_account_id;
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Int32> GetPossibleUpperAccountIDs()
        {
            //AuxVariables
            List<object> objects;
            List<Int32> result;
            //Run Method
            result = new List<int>();
            objects = Read(Controllers.ClassType.account);
            foreach (object obj in objects)
            {
                Models.Account temp = (Models.Account)obj;
                if (temp.GetAccessible() &
                    temp.GetAsset() == asset &
                    temp.GetConsisted() == consisted &
                    temp.GetUpperAccountID() == 0 &
                    temp.GetAccountingRecordIDs().Count == 0)
                {
                    result.Add(temp.GetID());
                }
            }
            return result;
        }
        public Int32 GetUpperAccountID()
        {
            return upper_account_id;
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
        public bool SetAccessible(bool accessible)
        {
            if (OnBeforeUpdate(Variable.accessible) &
                this.accessible != accessible)
            {
                this.accessible = accessible;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool GetAccessible()
        {
            return accessible;
        }
        public bool SetAsset(bool asset)
        {
            if (OnBeforeUpdate(Variable.asset) &
                this.asset != asset)
            {
                this.asset = asset;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool GetAsset()
        {
            return asset;
        }
        public bool SetConsisted(bool consisted)
        {
            if (OnBeforeUpdate(Variable.consisted) &
                this.consisted != consisted)
            {
                this.consisted = consisted;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool GetConsisted()
        {
            return consisted;
        }
        public List<Int32> GetLowerAccountIDs()
        {
            //AuxVariables
            List<object> objects;
            List<Int32> result;
            //Run Method
            result = new List<int>();
            objects = Read(Controllers.ClassType.account);
            foreach (object obj in objects)
            {
                Models.Account temp = (Models.Account)obj;
                if (temp.GetUpperAccountID() == primary_key)
                {
                    result.Add(temp.GetID());
                }
            }
            return result;
        }
        public List<Int32> GetAccountingRecordIDs()
        {

        }
        protected override bool OnAfterDelete(object obj)
        {
            return ((Models.Account)obj).GetCurrObjectStatus() == ObjectStatus.deleted;
        }
        protected override void OnAfterRead(object obj)
        {
            //AuxVariables
            Models.Account temp;
            //Run Method
            if (obj is Models.Account)
            {
                temp = (Models.Account)obj;
                if (temp.GetCurrObjectStatus() == ObjectStatus.temp)
                {
                    obj_status = ObjectStatus.saved;
                    primary_key = temp.GetID();
                    upper_account_id = temp.GetUpperAccountID();
                    name = temp.GetName();
                    name2 = temp.GetName2();
                    accessible = temp.GetAccessible();
                    asset = temp.GetAsset();
                    consisted = temp.GetConsisted();
                }
            }
        }
        protected override bool OnBeforeCreate()
        {
            return accessible & name != "";
        }
        protected override bool OnBeforeDelete()
        {
            return accessible & IsIndependent();
        }
        protected override bool OnBeforeUpdate(Variable variable)
        {
            switch (obj_status)
            {
                case ObjectStatus.temp:
                    switch (variable)
                    {
                        case Variable.primary_key:
                        case Variable.upper_account_id:
                        case Variable.name:
                        case Variable.name2:
                        case Variable.accessible:
                        case Variable.asset:
                        case Variable.consisted:
                            return true;
                        default:
                            return false;
                    }
                case ObjectStatus.created:
                case ObjectStatus.saved:
                    switch (variable)
                    {
                        case Variable.upper_account_id:
                        case Variable.name:
                        case Variable.name2:
                            return accessible;
                        case Variable.accessible:
                            return true;
                        case Variable.asset:
                        case Variable.consisted:
                            return accessible & IsIndependent();
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }
        private bool IsIndependent()
        {
            return GetLowerAccountIDs().Count == 0 & 
                   GetAccountingRecordIDs().Count == 0;
        }
        //Constructors
        public Account(bool temp) : base(temp)
        {

        }
        public Account(Int32 id) : base(id)
        {

        }
    }
}
