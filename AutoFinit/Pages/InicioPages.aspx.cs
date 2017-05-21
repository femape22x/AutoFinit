using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AutoFinit
{
    public partial class Formulario_web11 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnLeer_Click(object sender, EventArgs e)
        {
            // Se llama al método que realiza el proceso
            try
            {
                GuardarArchivo();
                throw new ArgumentException("Se ha guardado correctamente el archivo...");
            }
            catch (Exception ex)
            {
                //throw new ArgumentException(ex.Message);
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>");
                sb.Append("window.onload=function(){");
                sb.Append("alert('");
                sb.Append(ex.Message);
                sb.Append("')};");
                sb.Append("</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
            }
            Response.Redirect("~/Pages/AutomatasFinitos.aspx");
        }

        private void GuardarArchivo()
        {
            string rutaDestino = @"C:\MisArchivos";
            string rutaArchivo = String.Empty;
            string extensionFile = String.Empty;

            // Se valida la existencia del directorio de destino
            try
            {
                if (!Directory.Exists(rutaDestino))
                    Directory.CreateDirectory(rutaDestino);
            }
            catch (IOException ex)
            {
                throw new ArgumentException(ex.Message);
            }

            // Se valida la selección del archivo
            if (fluArchivo.HasFile)
            {
                rutaArchivo = Path.Combine(fluArchivo.PostedFile.ToString(), fluArchivo.FileName);

                // Se valida la extención del archivo
                extensionFile = Path.GetExtension(rutaArchivo);
                if (!HasValidExtension(extensionFile))
                {
                    throw new ArgumentException("Se ha seleccionado un tipo de archivo incorrecto. Solo admite archivos .XML y .TXT");
                }

                // Se guarda el archivo
                try
                {
                    string NewDocument = @"C:\MisArchivos\archivoXML.txt";
                    if (File.Exists(NewDocument))
                    {
                        File.Delete(NewDocument);
                    }
                    string path = Path.Combine(rutaDestino, "archivoXML.txt");
                    fluArchivo.SaveAs(path);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
            else
            {
                // En caso de dar click y no seleccionar ningún archivo
                throw new ArgumentException("No Se ha seleccionado ningún archivo.");
            }
        }

        private bool HasValidExtension(string extensionFile)
        {
            return extensionFile.Equals(".txt") || extensionFile.Equals(".xml");
        }
    }
}