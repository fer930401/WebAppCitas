using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppCitas
{
    public partial class Pases : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["numCita"].ToString() != null)
            {
                //Response.Write(Session["numCita"].ToString());
                //Response.Write(Session["acceso"].ToString());
                string refer = Session["acceso"].ToString();
                ReportDocument rep = new ReportDocument();
                if (refer.Equals("1") == true)
                {
                    rep.Load(Server.MapPath("Reportes/PAP.rpt"));
                }
                else if (refer.Equals("2") == true)
                {
                    rep.Load(Server.MapPath("Reportes/PAS.rpt"));
                }
                else if (refer.Equals("3") == true)
                {
                    rep.Load(Server.MapPath("Reportes/PTP.rpt"));
                }
                else if (refer.Equals("4") == true)
                {
                    rep.Load(Server.MapPath("Reportes/PAD.rpt"));
                }
                else
                {
                    rep.Load(Server.MapPath("Reportes/PAD.rpt"));
                }
                //rep.Load(Server.MapPath("Reportes/PAD.rpt"));
                //rep.DataSourceConnections[0].SetConnection("SQL", "skytex", true);
                //rep.DataSourceConnections[0].SetLogon("soludin_develop", "dinamico20");
                //rep.SetDatabaseLogon("soludin_develop", "dinamico20", "SQL", "skytex", false);
                rep.Refresh();
                //rep.SetDatabaseLogon("soludin_develop", "dinamico20", "192.168.18.96", "skytex", false);
                rep.SetDatabaseLogon("soludin_develop", "dinamico20", "skyhdev3", "develop", false);
                //rep.SetDatabaseLogon("soludin_develop", "dinamico20", "SQL", "skytex", false);
                rep.SetParameterValue("@ef_cve","001");
                rep.SetParameterValue("@tipdoc_cve","ltasis");
                rep.SetParameterValue("@num_fol", Int32.Parse(Session["numCita"].ToString()));
                //rep.SetParameterValue("@num_fol", 152);
                //rep.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("Activo/ReportName.pdf"));

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                //CrDiskFileDestinationOptions.DiskFileName = @"C:\Desarrollo\Desarrollo_web\Citas\Reportes\Pases\Acceso"+Session["numCita"].ToString()+".pdf";
                //CrDiskFileDestinationOptions.DiskFileName = "c:\\Acceso" + Session["numCita"].ToString() + ".pdf";
                CrExportOptions = rep.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }
                //rep.Export();
                
                //Response.ContentType = "application/pdf";
                ////Response.AppendHeader("Content-Disposition", "attachment; filename=Acceso"+Session["numCita"].ToString()+".pdf");
                rep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Acceso"+Session["numCita"].ToString()); // aqui importando el espacio de nombres               
            }
        }
    }
}