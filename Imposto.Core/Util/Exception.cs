﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Util
{
    [Serializable]
    public class Exception : System.Exception
    {
        public int Codigo { get; set; }
        public string Mensagem { get; set; }
    }
}
