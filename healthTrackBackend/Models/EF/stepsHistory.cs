//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace healthTrackBackend.Models.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class stepsHistory
    {
        public int id { get; set; }
        public int steps { get; set; }
        public System.DateTime recordDate { get; set; }
        public int userId { get; set; }
        public bool active { get; set; }
    
        [Newtonsoft.Json.JsonIgnore] public virtual user user { get; set; }
    }
}
