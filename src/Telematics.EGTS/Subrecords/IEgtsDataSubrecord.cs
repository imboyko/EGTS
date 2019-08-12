using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS
{
    /// <summary>Представляет подзапись данных уровня поддержки услуг</summary>
    public interface IEgtsDataSubrecord
    {
        /// <summary>Gets the type.</summary>
        /// <value>The type.</value>
        /// TODO Edit XML Comment Template for Type
        EgtsSubrecordType Type { get;}
    }
}
