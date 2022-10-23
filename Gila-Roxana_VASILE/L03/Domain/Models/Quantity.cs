﻿using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static L02_PSSC.Domain.Quantity;

namespace L02_PSSC.Domain
{

    [AsChoice]
    public static partial class Quantity
    {
        public interface IQuantity { }
        public record QKg() : IQuantity
        {
            private decimal kg { get; }
            public QKg(decimal kg) : this()
            {
                this.kg = kg;
            }
            public string GetQ()
            {
                return kg.ToString();
            }
        }
        public record QUnit() : IQuantity
        {
            private decimal unit { get; }
            public QUnit(decimal unit) : this()
            {
                this.unit = unit;
            }
            public string GetQ()
            {
                return unit.ToString();
            }
        }

        private static bool IsValid(decimal numericGrade) => numericGrade > 0;

        public static bool TryParseQuantityKg(string qtyString, out QKg quantity)
        {
            bool isValid = false;
            quantity = null;
            if(decimal.TryParse(qtyString, out decimal numericQty))
            {
                if(IsValid(numericQty))
                {
                    isValid = true;
                    quantity = new(numericQty);
                }
            }
            return isValid;
        }

        public static bool TryParseQuantityUnit(string qtyString, out QUnit quantity)
        {
            bool isValid = false;
            quantity = null;
            if (decimal.TryParse(qtyString, out decimal numericQty))
            {
                if (IsValid(numericQty))
                {
                    isValid = true;
                    quantity = new(numericQty);
                }
            }
            return isValid;
        }

    }

}
