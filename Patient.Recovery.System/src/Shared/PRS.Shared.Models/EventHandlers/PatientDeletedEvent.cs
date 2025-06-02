using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRS.Shared.Models.EventHandlers
{
    public class PatientDeletedEvent
    {
        public int PatientId { get; set; }
    }
}