using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDbStorage.Data.Extentions
{
    public class GenericDto<T> where T : class
    {
        public T Payload { get; set; }
    }
}
