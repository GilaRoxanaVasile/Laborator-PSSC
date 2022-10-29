using CSharp.Choices;
using LanguageExt;
using static LanguageExt.Prelude;
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
        public interface IQuantity {
            public decimal GetQ();
        }
        public record QKg() : IQuantity
        {
            private decimal kg { get; }
            public QKg(decimal kg) : this()
            {
                this.kg = kg;
            }
            public decimal GetQ()
            {
                return kg;
            }
        }
        public record QUnit() : IQuantity
        {
            private decimal unit { get; }
            public QUnit(decimal unit) : this()
            {
                this.unit = unit;
            }
            public decimal GetQ()
            {
                return unit;
            }
            public static Option<QUnit> TryParseQuantity(string qryString)
            {
                if (decimal.TryParse(qryString, out decimal numericQuantity) && IsValid(numericQuantity))
                {
                    return Some<QUnit>(new(numericQuantity));
                }
                else
                {
                    return None;
                }
            }

        }

        private static bool IsValid(decimal numericGrade) => numericGrade > 0;
        /*
        public static Option<QUnit> TryParseQuantity(string qryString)
        {
            if(decimal.TryParse(qryString, out decimal numericQuantity)&&IsValid(numericQuantity))
            {
                return Some<QUnit>(new(numericQuantity));
            }
            else
            {
                return None;
            }
        }
        */
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

