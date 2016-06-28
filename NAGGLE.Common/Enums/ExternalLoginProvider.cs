//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Common.Enums
{
  //  [JsonConverter(typeof(StringEnumConverter))]
   public enum ExternalLoginProvider : byte
    {
        None = 0,

        Google = 1,

        Facebook = 2,

        Microsoft = 3
    }
}
