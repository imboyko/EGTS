using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace TelematicsService.WCF
{
    [ServiceContract(Namespace = "http://1ckab.ru/telematics/")]
    interface IGeoService
    {
        [OperationContract]
        int GetCars();

    }
}
