﻿using NamirniceDelivery.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NamirniceDelivery.Web.ViewModels.Kupac
{
    public class SpremljeneNamirniceViewModel
    {
        public List<NamirnicaPodruznica> SpremljeneNamirniceList { get; set; }
        public List<NamirniceDelivery.Data.Entities.Podruznica> SpremljenePodruzniceList { get; set; }
    }
}
