using System;
using System.Collections.Generic;
using System.Text;

namespace App_XML
{
   public class Studente
   {
        public string NomeCompleto { get; set; }
      
        public int Presenze { get; set; }
        public DateTime DataDiNascita { get; set; }
        
        public  override string ToString()
        {
            return $"{NomeCompleto}";
        }
    }
}
