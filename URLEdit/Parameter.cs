using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLEdit
{
    public class Parameter
    {
        public string Name {  get; set; }
        public string Value {  get; set; }
        public Parameter()
        {
            Name = String.Empty;
            Value = String.Empty;
        }
        public Parameter(string _name, string _value)
        {
            Name=_name;
            Value = _value;
        }
    }
}
