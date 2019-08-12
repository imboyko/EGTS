using System;
using System.Collections.Generic;
using System.Text;

namespace Telematics.EGTS
{
    /// <summary>
    /// Подзапись предназначена для передачи на ТП информации об инфраструктуре на стороне АС, о составе, состоянии и параметрах блоков и модулей АС. 
    /// Данная подзапись является опциональной, и разработчик АС сам принимает решение о необходимости заполнения полей и отправки данной подзаписи. 
    /// Одна подзапись описывает один модуль. В одной записи может передаваться последовательно несколько таких подзаписей, что позволяет передать данные об отдельных составляющих всей аппаратной части АС и периферийного оборудования
    /// </summary>
    sealed class EGTS_SR_MODULE_DATA : IEgtsDataSubrecord
    {
        private byte _MT;
        private uint _VID;
        private ushort _FWV;
        private ushort _SWV;
        private byte _MD;
        private byte _ST;
        private string _SRN;
        private string _DSCR;

        public EgtsSubrecordType Type => EgtsSubrecordType.EGTS_SR_MODULE_DATA;
    }
}
