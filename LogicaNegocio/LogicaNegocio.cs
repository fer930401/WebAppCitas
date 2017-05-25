using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using AccesoDatos;

namespace LogicaNegocio
{
    public class LogicaNegocioCls
    {
        AccesoDatos.AccesoDatosCls datos;

        public LogicaNegocioCls()
        {
            datos = new AccesoDatosCls();
        }

        public List<Entidades.sp_webAppSalaCbo_Result> obtCboSala(string ef_cve, string movimiento, string extra1, string extra2, string extra3, string extra4)
        {
            return datos.obtCboSala(ef_cve, movimiento, extra1, extra2, extra3, extra4);
        }

        public string accesoSistema(string ef_cve, string user_cve, string password)
        {
            /*string pass;
            pass = datos.accesoSistema(ef_cve, user_cve);

            if (pass.Equals(password))
            {
                return "1";
            }
            else
            {
                return "0";
            }*/
            /* mod 24/01/2017 - se agrego la funcion del login de los usuarios */
            return datos.accesoSistema(ef_cve, user_cve, password);
        }

        public Entidades.xcdconapl_cl obtInfPantalla(string ef_cve, string sp_cve, string tipdoc_cve, string spd_cve)
        {
            return datos.obtInfPantalla(ef_cve, sp_cve, tipdoc_cve, spd_cve);
        }

        public string obtInfRoles(string tipdoc_cve, string sp_cve, string spd_cve, string prm1, string prm2)
        {
            return datos.obtInfRoles(tipdoc_cve, sp_cve, spd_cve, prm1, prm2);
        }

        public List<Entidades.qcomare1_Result> obtCboArea(string ef_cve, string prg_cve)
        {
            return datos.obtCboArea(ef_cve, prg_cve);
        }

        public List<Entidades.qcomWebHora_Result> obtCboHora(string tipo_doc, DateTime fecha, string refer, string hora, string ef_cve, short id_pant, DateTime fec_salida)
        {
            return datos.obtCboHora(tipo_doc, fecha, refer, hora, ef_cve, id_pant, fec_salida);
        }
        public List<Entidades.qcomWebHoraReagenda_Result> obtCboHoraReagenda(string tipo_doc, string fecha, string refer, string hora, string ef_cve, short id_pant, string fec_salida, int? num_fol)
        {
            return datos.obtCboHoraReagenda(tipo_doc, fecha, refer, hora, ef_cve, id_pant, fec_salida, num_fol);
        }
        public List<Entidades.qcomsala1_Result> obtCboSala(string prg_cve, string movimiento)
        {
            return datos.obtCboSala(prg_cve, movimiento);
        }

        public List<Entidades.qcomtiptra1_Result> obtCboTiptrab(string prg_cve)
        {
            return datos.obtCboTipTrab(prg_cve);
        }

        public Entidades.sp_InsertarEncabezadoCitas_Result insEncabezado(string ef_cve, string tipodoc_cve, DateTime fec_ultact, DateTime fec_prom, string imp_letra, string fec_letra, string refer, string id_ultact, short plazo, string suc_aten, string obs, string pr5, string pr1, string uuid)
        {
            return datos.insEncabezado(ef_cve, tipodoc_cve, fec_ultact, fec_prom, imp_letra, fec_letra, refer, id_ultact, plazo, suc_aten, obs, pr5, pr1, uuid);
        }

        public Entidades.sp_InsertarDetalleCitas_Result insDetalle(string ef_cve, string tipo_doc, int num_fol, string desc_op, string lote_num, DateTime lote_fec, string art_tip, string sku_cve)
        {
            return datos.insDetalle(ef_cve, tipo_doc, num_fol, desc_op, lote_num, lote_fec, art_tip, sku_cve);
        }

        public Entidades.sp_AutValCita_Result autValCita(string ef_cve, string tipo_doc, int num_fol, int cve_status, int sw_terminado, int sw_si_no, string id_ultact, string obs, DateTime fec_ultact, DateTime fec_prom, string refer)
        {
            return datos.autValCita(ef_cve, tipo_doc, num_fol, cve_status, sw_terminado, sw_si_no, id_ultact, obs, fec_ultact, fec_prom, refer);
        }
        public List<string> qcomFiltra(string tipo_doc, string refer, DateTime fecha)
        {
           return  datos.qcomFiltra(tipo_doc, refer, fecha);
        }

        public List<string> sp_HoraFin(DateTime fecha, string tipo_doc, string refer)
        {
            return datos.sp_HoraFin(fecha, tipo_doc, refer);
        }
        public string consHoraSelec(string tipo_doc, string sp_cve, string spd_cve)
        {
            return datos.consHoraSelec(tipo_doc, sp_cve, spd_cve);
        }

        public List<Entidades.sp_FiltraHoraSalida_Result> sp_FiltraHoraS(string tipo_doc, DateTime fecha, string refer, string horaE)
        {
            return datos.sp_FiltraHoraS(tipo_doc, fecha, refer, horaE);
        }

        public List<Entidades.sp_consCitas_Result> consCitas(string tipo_doc, string rol_user, string cve_user, DateTime fecha)
        {
            return datos.consCitas(tipo_doc, rol_user, cve_user, fecha);
        }        

        public List<Entidades.sp_ConsCitasCalendario_Result> consCitasCal()
        {
            return datos.consCitasCal();
        }
    }
}
