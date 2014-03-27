using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentAdmin
{
    public interface IXMLSchemaSetValidador
    {
        bool CompruebaXMLvsXSD(string targetNamespace, string UriXSD, string UriXML);
        Int32 TotalErrores();
        Int32 TotalAdvertencias();
        string GetErrores();
        string GetAdvertencias();
        string GetErrorPrincipal();
    }
}
