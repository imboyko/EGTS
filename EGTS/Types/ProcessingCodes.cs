using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGTS.Types
{
    public enum ProcessingCode : uint
    {
        /// <summary>
        /// Completely done
        /// </summary>
        EGTS_PC_OK = 0U,

        /// <summary>
        /// In progress
        /// </summary>
        EGTS_PC_IN_PROGRESS = 1U,

        /// <summary>
        /// Unsupported protocol
        /// </summary>
        EGTS_PC_UNS_PROTOCOL = 128U,

        /// <summary>
        /// Decription error
        /// </summary>
        EGTS_PC_DECRYPT_ERROR = 129U,

        /// <summary>
        /// Processing denied 
        /// </summary>
        EGTS_PC_PROC_DENIED = 130U,

        /// <summary>
        /// Incorrect header format
        /// </summary>
        EGTS_PC_INC_HEADERFORM = 131U,

        /// <summary>
        /// Incorrect data format
        /// </summary>
        EGTS_PC_INC_DATAFORM = 132U,
        /// <summary>
        /// Unsupported type
        /// </summary>
        EGTS_PC_UNS_TYPE = 133U,

        /// <summary>
        /// Incorrect parameters number
        /// </summary>
        EGTS_PC_NOTEN_PARAMS = 134U,

        /// <summary>
        /// Attempt to retry processing
        /// </summary>
        EGTS_PC_DBL_PROC = 135U,

        /// <summary>
        /// Source data processing denied
        /// </summary>
        EGTS_PC_PROC_SRC_DENIED = 136U,

        /// <summary>
        /// Header CRC error
        /// </summary>
        EGTS_PC_HEADERCRC_ERROR = 137U,

        /// <summary>
        /// Data CRC error
        /// </summary>
        EGTS_PC_DATACRC_ERROR = 138U,

        /// <summary>
        /// Invalid data length
        /// </summary>
        EGTS_PC_INVDATALEN = 139U,

        /// <summary>
        /// Route not found
        /// </summary>
        EGTS_PC_ROUTE_NFOUND = 140U,

        /// <summary>
        /// Route closed
        /// </summary>
        EGTS_PC_ROUTE_CLOSED = 141U,

        /// <summary>
        /// Routing denied
        /// </summary>
        EGTS_PC_ROUTE_DENIED = 142U,

        /// <summary>
        /// Invalid address
        /// </summary>
        EGTS_PC_INVADDR = 143U,

        /// <summary>
        /// Retranslation data amount exceed
        /// </summary>
        EGTS_PC_TTLEXPIRED = 144U,

        /// <summary>
        /// No acknowledge
        /// </summary>
        EGTS_PC_NO_ACK = 145U,

        /// <summary>
        /// Object not found
        /// </summary>
        EGTS_PC_OBJ_NFOUND = 146U,

        /// <summary>
        /// Event not found
        /// </summary>
        EGTS_PC_EVNT_NFOUND = 147U,

        /// <summary>
        /// Service not found
        /// </summary>
        EGTS_PC_SRVC_NFOUND = 148U,

        /// <summary>
        /// Service denied
        /// </summary>
        EGTS_PC_SRVC_DENIED = 149U,

        /// <summary>
        /// Unknown service type
        /// </summary>
        EGTS_PC_SRVC_UNKN = 150U,

        /// <summary>
        /// Authorization denied
        /// </summary>
        EGTS_PC_AUTH_DENIED = 151U,

        /// <summary>
        /// Object already exist
        /// </summary>
        EGTS_PC_ALREADY_EXISTS = 152U,

        /// <summary>
        /// Identificator not found
        /// </summary>
        EGTS_PC_ID_NFOUND = 153U,

        /// <summary>
        /// Date or time incorrect
        /// </summary>
        EGTS_PC_INC_DATETIME = 154U,

        /// <summary>
        /// Input / output error
        /// </summary>
        EGTS_PC_IO_ERROR = 155U,

        /// <summary>
        /// Resources not available
        /// </summary>
        EGTS_PC_NO_RES_AVAIL = 156U,

        /// <summary>
        /// Module internal fault
        /// </summary>
        EGTS_PC_MODULE_FAULT = 157U,

        /// <summary>
        /// Module power fault
        /// </summary>
        EGTS_PC_MODULE_PWR_FLT = 158U,

        /// <summary>
        /// Module processor fault
        /// </summary>
        EGTS_PC_MODULE_PROC_FLT = 159U,

        /// <summary>
        /// Module software fault
        /// </summary>
        EGTS_PC_MODULE_SW_FLT = 160U,

        /// <summary>
        /// Module firmware fault
        /// </summary>
        EGTS_PC_MODULE_FW_FLT = 161U,

        /// <summary>
        /// Module input/output fault
        /// </summary>
        EGTS_PC_MODULE_IO_FLT = 162U,

        /// <summary>
        /// Module memory fault
        /// </summary>
        EGTS_PC_MODULE_MEM_FLT = 163U,

        /// <summary>
        /// Test failed
        /// </summary>
        EGTS_PC_TEST_FAILED = 164U,

        /// <summary>
        /// Unknown error
        /// </summary>
        EGTS_PC_UNKNOWN = 255U
    }

}
