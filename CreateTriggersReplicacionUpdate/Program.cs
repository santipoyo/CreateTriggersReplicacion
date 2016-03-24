using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Collections;
using System.Linq;

namespace CreateTriggersReplicacion
{
    class Program
    {
        static String strRuta = @"C:\Users\milton.alvarez\Google Drive\Workdir\Grupo KFC\Replicación DBs\Scripts Replicación\pruebas insert asincronico dinamico v2.5.sql";
        static String strRuta2 = @"D:\Milton\Documents\Workdir\Tests\Triggers Test\";
        //static String strRuta = AppDomain.CurrentDomain.BaseDirectory;
        static List<String> storedProcedures = new List<string>();
        static List<String> comandos = new List<string>();
        static List<String> tablas = new List<string>();

        static Dictionary<String, String> tablasKeys = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            //Console.WriteLine(strRuta);

            CargarNombresTablas();

            //ListaArchivos();
            EscribirLista();

            Console.WriteLine("Presione cualquier tecla para finalizar.");
            Console.ReadKey();
            Environment.Exit(0);
        }

        private static void CargarNombresTablas()
        {
            try
            {
                tablasKeys.Add("EquipoRecursoHumano", "IDEmpleado");

                //tablasKeys.Add("Acceso_Pos", "IDAccesoPos");
                //tablasKeys.Add("Pantalla_Pos", "IDPantallaPos");
                //tablasKeys.Add("Perfil_Pos", "IDPerfilPos");
                //tablasKeys.Add("Users_Pos", "IDUsersPos");

                //tablasKeys.Add("Pisos", "IDPisos");
                //tablasKeys.Add("AreaPiso", "IDAreaPiso");
                //tablasKeys.Add("Mesa", "IDMesa");
                //tablasKeys.Add("Cliente_Reserva", "IDClienteReserva");

                //tablasKeys.Add("Estacion", "IDEstacion");
                //tablasKeys.Add("Control_Estacion", "IDControlEstacion");
                //tablasKeys.Add("Billete_Denominacion", "IDBilleteDenominacion");

                //tablasKeys.Add("Autorizacion", "IDAutorizacion");
                //tablasKeys.Add("Pregunta_Sugerida", "IDPreguntaSugerida");
                //tablasKeys.Add("Menu_Agrupacion", "IDMenuAgrupacion");
                //tablasKeys.Add("Menu", "IDMenu");

                //tablasKeys.Add("Red_Adquiriente", "IDRedAdquiriente");
                //tablasKeys.Add("Formapago", "IDFormapago");
                //tablasKeys.Add("Cliente", "IDCliente");
                //tablasKeys.Add("Tipo_Documento", "IDTipoDocumento");
                //tablasKeys.Add("Cabecera_Orden_Pedido", "IDCabeceraOrdenPedido");
                //tablasKeys.Add("Detalle_Orden_Pedido", "IDDetalleOrdenPedido");
                //tablasKeys.Add("CabeceraMotivoAnulacion", "IDCabeceraMotivoAnulacion");
                //tablasKeys.Add("Motivo_Anulacion", "IDMotivoAnulacion");
                //tablasKeys.Add("Tipo_Impresora", "IDTipoImpresora");
                //tablasKeys.Add("Impresora", "IDImpresora");
                //tablasKeys.Add("Canal_Impresion", "IDCanalImpresion");
                //tablasKeys.Add("Puertos", "IDPuertos");
                //tablasKeys.Add("Status", "IDStatus");

                //tablasKeys.Add("Tipo_Descuento", "IDTipoDescuento");
                //tablasKeys.Add("Descuentos", "IDDescuentos");

                //tablasKeys.Add("Config_Cadena", "IDConfigCadena");
                //tablasKeys.Add("Config_Restaurante", "IDConfigRestaurante");
                //tablasKeys.Add("Config_Formapago", "IDConfigFormapago");
                //tablasKeys.Add("Config_Plus", "IDConfigPlus");

                //tablasKeys.Add("Periodo", "IDPeriodo");
                //tablasKeys.Add("Clasificacion", "IDClasificacion");
                //tablasKeys.Add("Tipo_Facturacion", "IDTipoFacturacion");
                //tablasKeys.Add("Tipo_Mov", "IDTipoMovimiento");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ListaArchivos()
        {
            try
            {
                if (Directory.Exists(strRuta))
                {
                    String[] archivos = Directory.GetFiles(strRuta);

                    archivos = archivos.Where(p => p.Contains(".sql")).ToArray();

                    foreach (string item in archivos)
                    {
                        String archivo = item.Replace(strRuta, String.Empty);
                        storedProcedures.Add(archivo);
                        //Console.WriteLine(archivo);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void EscribirLista()
        {
            StringBuilder cadena = new StringBuilder();
            string[] lineasTR;
            List<String> listaLineasTR = new List<string>();

            try
            {
                DateTime fechaActual = DateTime.Now;

                foreach (KeyValuePair<String, String> item in tablasKeys)
                //foreach (String item in storedProcedures)
                {
                    comandos.Add("/*==============================================================*/");
                    comandos.Add("/*	Database Name:		MAXMANAGER								*/");
                    comandos.Add("/*	Database Objects:	Trigger									*/");
                    comandos.Add("/*	Updated by:			Milton S. Alvarez C.					*/");
                    comandos.Add("/*	Update Date:		" + fechaActual.ToString() + "						*/");
                    comandos.Add("/*==============================================================*/");
                    comandos.Add(Environment.NewLine);

                    comandos.Add("if exists (	select 1 ");
                    comandos.Add("			from	sysobjects ");
                    comandos.Add("			where	id = object_id('dbo.tr_replicaUpdate" + item.Key + "') ");
                    comandos.Add("					and type = 'TR') ");
                    comandos.Add("begin ");
                    comandos.Add("	drop trigger dbo.tr_replicaUpdate" + item.Key + "");
                    comandos.Add("end ");
                    comandos.Add("go ");
                    comandos.Add(Environment.NewLine);

                    comandos.Add("create trigger dbo.tr_replicaUpdate" + item.Key);
                    comandos.Add("on MaxManager.dbo." + item.Key);
                    comandos.Add("after update");
                    comandos.Add("as");

                    lineasTR = File.ReadAllLines(strRuta);

                    foreach (String linea in lineasTR.ToList())
                    {
                        listaLineasTR.Add(linea);
                    }
                    listaLineasTR.Add(String.Empty);
                    listaLineasTR.Add(String.Empty);
                    listaLineasTR.Add(String.Empty);

                    foreach (String linea in listaLineasTR)
                    {
                        comandos.Add(linea);
                    }
                    comandos.Add("go ");

                    File.WriteAllLines(strRuta2 + @"tr_replicaUpdate" + item.Key + ".sql", comandos.ToArray());

                    listaLineasTR.Clear();
                    comandos.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
