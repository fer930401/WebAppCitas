using DayPilot.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using LogicaNegocio;

namespace WebAppCitas
{
    public class DataGeneratorScheduler
    {
        
        public static DataTable GetData()
        {
            LogicaNegocioCls logicaNegocio = new LogicaNegocioCls();
            List<Entidades.sp_ConsCitasCalendario_Result> conC = logicaNegocio.consCitasCal();
            DataTable dt;
            dt = new DataTable();
            dt.Columns.Add("start", typeof(DateTime));
            dt.Columns.Add("end", typeof(DateTime));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("column", typeof(string));
            dt.Columns.Add("status", typeof(string));

            DataRow dr;
            if (conC != null)
            {
                int i = 0;
                foreach (var elemento in conC)
                {
                    dr = dt.NewRow();
                    dr["id"] = elemento.num_fol;
                    dr["start"] = elemento.fec_ultact;
                    dr["end"] = elemento.fec_prom;
                    dr["name"] = elemento.imp_letra;
                    dr["column"] = elemento.spd_cve;
                    dr["status"] = elemento.cve_status;
                    dt.Rows.Add(dr);
                    i++;
                }
            }
            dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };
            return dt;
        }
    }
}