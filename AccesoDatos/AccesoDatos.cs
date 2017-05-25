using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace AccesoDatos
{
    public class AccesoDatosCls
    {
        skytexEntities contexto;

        public AccesoDatosCls()
        {
            contexto = new skytexEntities();
        }

        public List<Entidades.sp_webAppSalaCbo_Result> obtCboSala(string ef_cve, string movimiento, string extra1, string extra2, string extra3, string extra4)
        {
            return (contexto.sp_webAppSalaCbo(ef_cve, movimiento, extra1, extra2, extra3, extra4)).ToList();
        }

        public string accesoSistema(string ef_cve, string user_cve, string password)
        {
            /* mod 24/01/2017 - se agrego la funcion del login de los usuarios */
            string nomUser;
            var result = (from usuario in contexto.xcuser
                          where
                            usuario.ef_cve.Equals(ef_cve) &&
                            usuario.user_cve.Equals(user_cve) &&
                            usuario.password.Equals(password)
                          select usuario).Count();

            if (result > 0)
            {
                nomUser = (from usuario in contexto.xcuser
                        where
                          usuario.ef_cve.Equals(ef_cve) &&
                          usuario.user_cve.Equals(user_cve) &&
                          usuario.password.Equals(password)
                        select usuario.nombre).FirstOrDefault();
                return nomUser;
            }
            else
            {
                return nomUser = null;
            }
        }

        public Entidades.xcdconapl_cl obtInfPantalla(string ef_cve, string sp_cve, string tipdoc_cve, string spd_cve)
        {
            return (from r in contexto.xcdconapl_cl
                    where
                        r.tipdoc_cve.Equals(tipdoc_cve) &&
                        r.sp_cve.Equals(sp_cve) &&
                        r.spd_cve.Equals(spd_cve) &&
                        r.prm1.Equals(ef_cve)
                    select r).FirstOrDefault();
        }


        public string obtInfRoles(string tipdoc_cve, string sp_cve, string spd_cve, string prm1, string prm2 )
        {
            string rol;

            rol = (from r in contexto.xcdconapl_cl
                    where
                        r.tipdoc_cve.Equals(tipdoc_cve) &&
                        r.sp_cve.Equals(sp_cve) &&
                        r.spd_cve.Equals(spd_cve) &&
                        r.prm1.Equals(prm1)&&
                        r.prm2.Equals(prm2)
                    select r.prm3.Trim()).FirstOrDefault();

            return rol;


        }


        /*IMG*/
        public List<Entidades.qcomare1_Result> obtCboArea(string ef_cve, string prg_cve)
        {
            return (contexto.qcomare1(ef_cve, prg_cve)).ToList();
        }
        
        // 25/11/2016 Query para llenar combos de horas
        public List<Entidades.qcomWebHora_Result> obtCboHora(string tipo_doc, DateTime fecha, string refer, string hora, string ef_cve, short id_pant, DateTime fec_salida)
        {
            return (contexto.qcomWebHora(tipo_doc, fecha, refer, hora, ef_cve, id_pant, fec_salida)).ToList();
        }
        public List<Entidades.qcomWebHoraReagenda_Result> obtCboHoraReagenda(string tipo_doc, string fecha, string refer, string hora, string ef_cve, short id_pant, string fec_salida, int? num_fol)
        {
            return (contexto.qcomWebHoraReagenda(tipo_doc, fecha, refer, hora, ef_cve, id_pant, fec_salida, num_fol)).ToList();
        }
        public List<Entidades.qcomsala1_Result> obtCboSala(string prg_cve, string movimiento)
        {
            return (contexto.qcomsala1(prg_cve, movimiento)).ToList();
        }

        public List<Entidades.qcomtiptra1_Result> obtCboTipTrab(string prg_cve)
        {
            return (contexto.qcomtiptra1(prg_cve)).ToList();
        }

        public Entidades.sp_InsertarEncabezadoCitas_Result insEncabezado(string ef_cve, string tipodoc_cve, DateTime fec_ultact, DateTime fec_prom, string imp_letra, string fec_letra, string refer, string id_ultact, short plazo, string suc_aten, string obs, string pr5, string pr1, string uuid)
        {
            return (contexto.sp_InsertarEncabezadoCitas(ef_cve, tipodoc_cve, fec_ultact, fec_prom, imp_letra, fec_letra, refer, id_ultact, plazo, suc_aten, obs, pr5, pr1, uuid)).FirstOrDefault();                
        }

        public Entidades.sp_InsertarDetalleCitas_Result insDetalle(string ef_cve, string tipo_doc, int num_fol, string desc_op, string lote_num, DateTime lote_fec, string art_tip, string sku_cve)
        {
            return (contexto.sp_InsertarDetalleCitas(ef_cve, tipo_doc, num_fol, desc_op, lote_num, lote_fec, art_tip, sku_cve)).FirstOrDefault(); 
        }

        public Entidades.sp_AutValCita_Result autValCita(string ef_cve, string tipo_doc, int num_fol, int cve_status, int sw_terminado, int sw_si_no, string id_ultact, string obs, DateTime fec_ultact, DateTime fec_prom, string refer)
        {
            return (contexto.sp_AutValCita(ef_cve, tipo_doc, num_fol, cve_status, sw_terminado, sw_si_no, id_ultact, obs, fec_ultact, fec_prom, refer)).FirstOrDefault();
        }

        public List<string> qcomFiltra(string tipo_doc, string refer, DateTime fecha)
        {             
            return (contexto.qcomFiltra(tipo_doc, refer, fecha)).ToList();
        }

        public List<string> sp_HoraFin(DateTime fecha, string tipo_doc, string refer)
        {
            return (contexto.sp_HoraFin(fecha, tipo_doc, refer)).ToList();
        }

        public string consHoraSelec(string tipo_doc, string sp_cve, string spd_cve)
        {
            string cons = (from r in contexto.xcdconapl_cl
                        where
                            r.tipdoc_cve.Equals(tipo_doc) &&
                            r.sp_cve.Equals(sp_cve) &&
                            r.spd_cve.Equals(spd_cve)
                        select r.prm1.Trim()).FirstOrDefault();
            return cons;
        }

         public List<Entidades.sp_FiltraHoraSalida_Result> sp_FiltraHoraS(string tipo_doc, DateTime fecha, string refer, string horaE)
        {
            return (contexto.sp_FiltraHoraSalida(tipo_doc, fecha, refer, horaE)).ToList();
        }

         public List<Entidades.sp_consCitas_Result> consCitas(string tipo_doc, string rol_user, string cve_user, DateTime fecha)
         {
             return (contexto.sp_consCitas(tipo_doc, rol_user, cve_user, fecha)).ToList();
         }


         public List<Entidades.sp_ConsCitasCalendario_Result> consCitasCal()
         {
             return (contexto.sp_ConsCitasCalendario().ToList());
         }

    }
}
