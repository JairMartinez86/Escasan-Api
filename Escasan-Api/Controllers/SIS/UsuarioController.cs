using Escasan_Api.Class;
using Escasan_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;

namespace Escasan_Api.Controllers
{
    public class UsuarioController : ApiController
    {
        //GET api/Get
        [HttpGet]
        public string Get(string usr, string pwd)
        {
            return Login(usr, pwd);
        }

        //GET api/Get
        [HttpGet]
        public string Get(string usr)
        {
            return Login(usr);
        }

        private string Login(string usr, string pwd)
        {

            string json = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {



                    var query = (from _q in _Conexion.Usuarios.AsEnumerable()
                                 where _q.Activo == true && _q.Usuario.Equals(usr) && _Conexion.Database.SqlQuery<string>($"SELECT [SIS].[Desencriptar]({"0x" + BitConverter.ToString(_q.Pass).Replace("-", "")}) AS Pass").Single().Equals(pwd)
                                 select new
                                 {
                                     Nombre = string.Concat(_q.Nombres, " ", _q.Apellidos),
                                     Fecha = DateTime.Now,
                                     IdVendedor = _q.IdVendedor
                                 }).ToList();

           
                    if(query.Count == 0)
                    {
                        json = Cls_Mensaje.Tojson(null, 0, string.Empty, "<p>Usuario y/o contraseña invalida.<p>", 1);
                    }
                    else
                    {
                        json = Cls_Mensaje.Tojson(query, query.Count, string.Empty, string.Empty, 0);
                    }



                }

            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }

        private string Login(string usr)
        {

            string json = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {

                    var query = (from _q in _Conexion.Usuarios.AsEnumerable()
                                 where _q.Activo == true && _q.Usuario.ToLower().Contains(usr.ToLower())
                                 orderby _q.Usuario.Length
                                 select new
                                 {
                                     Login = _q.Usuario,
                                     Nombre = string.Concat(_q.Nombres, " ", _q.Apellidos)
                                 }).Take(20).ToList();


                    json = Cls_Mensaje.Tojson(query, query.Count, string.Empty, string.Empty, 0);



                }

            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }
    }
}