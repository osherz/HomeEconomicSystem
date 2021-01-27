﻿using System;
using System.Collections.Generic;

namespace HomeEconomicSystem.BE
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime DateTime{ get; set; }
        public virtual List<ProductTransaction> ProductTransactions { get; private set; }
    }
}