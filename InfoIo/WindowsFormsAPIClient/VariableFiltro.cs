using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WindowsFormsAPIClient
{
    public class VariableFiltro : Variable 
    {

        List<string> v_ValoresFiltro = new List<string>();

        public List<string> FilterValues
        {
            get
            {
                return v_ValoresFiltro;
            }
            set
            {
                v_ValoresFiltro = value;
            }
        }

    }
}