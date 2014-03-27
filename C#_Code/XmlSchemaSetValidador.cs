using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;

namespace ContentAdmin
{
    public class XmlSchemaSetValidador : IXMLSchemaSetValidador
    {
        private string ErrorPrincipal = "";
        private string Error = "";
        private string Advertencias = "";
        private Int32 TotErrores = 0, TotAdvertencias = 0;

        public bool CompruebaXMLvsXSD(string targetNamespace, string UriXSD, string UriXML)
        {
            try
            {
                XmlReaderSettings booksSettings = new XmlReaderSettings();
                booksSettings.Schemas.Add(targetNamespace, UriXSD);
                booksSettings.ValidationType = ValidationType.Schema;
                booksSettings.ValidationEventHandler += new ValidationEventHandler(booksSettingsValidationEventHandler);
                StreamReader reader = new StreamReader(UriXML, System.Text.Encoding.UTF8);
                XmlReader Lec = XmlReader.Create(reader, booksSettings);
                while (Lec.Read()) { }
                return true;
            }
            catch (Exception ex)
            {
                ErrorPrincipal = "Error en CompruebaXMLvsXSD: " + ex.Message;
                return false;
            }
        }

        private void booksSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            long Linea = e.Exception.LineNumber;
            long Posicion = e.Exception.LinePosition;
            if (e.Severity == XmlSeverityType.Warning)
            {
                Advertencias += "Advertencia: " + e.Message + "--> Linea : " + Linea + ", Posición : " + Posicion + "\n";
                TotAdvertencias += 1;
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                Error += "Error: " + e.Message + "--> Linea : " + Linea + ", Posición : " + Posicion + "\n";
                TotErrores += 1;
            }
        }

        public Int32 TotalErrores() { return TotErrores; }
        public Int32 TotalAdvertencias() { return TotAdvertencias; }
        public string GetErrores() { return Error; }
        public string GetAdvertencias() { return Advertencias; }
        public string GetErrorPrincipal() { return ErrorPrincipal; }
    }
}