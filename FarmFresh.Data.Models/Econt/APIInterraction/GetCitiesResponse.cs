﻿using FarmFresh.Data.Models.Econt.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class GetCitiesResponse : ResponseBase
    {
        public List<CityDTO>? Cities { get; set; }
    }
}
