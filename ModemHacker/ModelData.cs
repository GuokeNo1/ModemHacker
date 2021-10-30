using System;
using System.Collections.Generic;
using System.Text;

namespace ModemHacker
{
    class ModelData
    {
        public string name = "";
        public Type ClassData;
        public bool useUser = false;
        public ModelData(Type data) {
            this.ClassData = data;
            this.name = data.GetField("modem").GetValue("NoName").ToString();
            this.useUser = (bool)data.GetField("needUser").GetValue(false);

        }
        public override string ToString()
        {
            return name;
        }
    }
}
