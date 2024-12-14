﻿using FarmFresh.Data.Models.Econt.DTOs;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class GetQuartersResponse : ResponseBase
    {
        public List<QuarterDTO>? Quarters { get; set; }
    }
}
