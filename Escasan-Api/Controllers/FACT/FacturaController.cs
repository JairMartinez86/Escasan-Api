using Escasan_Api.Class;
using Escasan_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;

namespace Escasan_Api.Controllers.FACT
{
    public class FacturaController : ApiController
    {

        //Factura/Cliente
        [HttpGet]
        public string Cliente(string CodBodega)
        {
            return _Cliente(CodBodega);
        }

        private string _Cliente(string CodBodega)
        {
            string json = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    var query = (from _q in _Conexion.Cliente
                                 join _b in _Conexion.Bodegas on _q.IdBodega equals _b.IdBodega into _q_t
                                 from _t in _q_t.DefaultIfEmpty()
                                 where _q.Estado.Equals("Activo")
                                 select new
                                 {
                                     IdCliente = _q.IdCliente,
                                     Codigo = _q.Codigo,
                                     Cliente = string.Concat(_q.Codigo, "  ", _q.Nombre),
                                     Ruc = _q.NoCedula,
                                     Telefono = string.Concat(_q.Telefono1, " ", _q.Telefono2, " ", _q.Telefono3),
                                     esEventual = (_t != null) ? true : false
                                 }).ToList();


                    json = Cls_Mensaje.Tojson(query, query.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }




        //Factura/Bodega
        [HttpGet]
        public string Bodega(string Usuario, bool esExportacion)
        {
            return _Bodega(Usuario, esExportacion);
        }

        private string _Bodega(string Usuario, bool esExportacion)
        {
            string json = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    var query = (from _q in _Conexion.Bodegas
                                 join _ub in _Conexion.UsuariosBodegas on _q.BodegaSuma equals _ub.IdBodega
                                 join _u in _Conexion.Usuarios on _ub.IdUsuario equals _u.IdUsuario
                                 where _u.Usuario.Equals(Usuario) && (esExportacion ? _q.BodegaExportacion : esExportacion)  == (esExportacion ? esExportacion: _q.BodegaExportacion)
                                select new
                                {
                                    IdBodega = _q.IdBodega,
                                    Codigo = _q.Codigo,
                                    Bodega = string.Concat(_q.Codigo, "  ",_q.Bodega),
                                    VendedorBod = _q.CodVendedor,
                                    ClienteBod = (int?)_Conexion.Cliente.FirstOrDefault(f => f.IdBodega == _q.IdBodega).IdCliente
                                }).ToList();


                    json = Cls_Mensaje.Tojson(query, query.Count, string.Empty, string.Empty, 0);
                }



            }
            catch (Exception ex)
            {
                json = Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
            }

            return json;
        }



        //Factura/Producto
        [HttpGet]
        public string Producto()
        {
            return _Producto();
        }

        private string _Producto()
        {
            string json = string.Empty;
            try
            {
                using (BalancesEntities _Conexion = new BalancesEntities())
                {
                    var query = (from _q in _Conexion.Productos
                                 where _q.Activo == true
                                 select new
                                 {
                                     IdProducto = _q.IdProducto,
                                     Codigo = _q.Codigo,
                                     Producto = _q.Producto
                                 }).ToList();


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