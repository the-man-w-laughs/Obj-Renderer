using Domain.ObjClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contracts.Parser
{
    public interface IObjFileParcer
    {
        Obj ParseObjFile(string filePath);
        Obj ParseObjStream(Stream stream);
        Obj LoadObj(IEnumerable<string> data);
    }
}
