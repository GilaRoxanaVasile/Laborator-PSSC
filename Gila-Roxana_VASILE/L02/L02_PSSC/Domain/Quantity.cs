using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L02_PSSC.Domain
{

    [AsChoice]
    public static partial class Quantity
    {
        public interface IQuantity {
            public string GetQ();
        }
        public record QKg() : IQuantity
        {
            private double kg { get; }

            string IQuantity.GetQ()
            {
                return kg.ToString();
            }

            public QKg(double kg) : this()
            {
                this.kg = kg;
            }
            public string GetQ2()
            {
                return kg.ToString();
            }
        }
        public record QUnit() : IQuantity
        {
            private double unit { get; }
            public QUnit(double unit) : this()
            {
                this.unit = unit;
            }
            public string GetQ()
            {
                return unit.ToString();
            }
        }
    }
}
