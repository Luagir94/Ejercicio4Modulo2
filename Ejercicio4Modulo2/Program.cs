using System.Globalization;

namespace Ejercicio4Modulo2
{
    internal class Program
    {
        static void Main(string[] args)
        {
       
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\data.txt";
            var context = new DBContext();
            var sr = new StreamReader(path);

            List<Ventas_Mensuales> ventas_validas = new List<Ventas_Mensuales>();
            List<Rechazos> rechazos = new List<Rechazos>();
            var parametria = context.Parametria.FirstOrDefault(p => p.id == 1)?.value;

            
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Ventas_Mensuales venta = new Ventas_Mensuales();


                    string fechaString = line.Substring(0, 10);
                    if (string.IsNullOrWhiteSpace(fechaString) || (fechaString != parametria))
                    {
                        Rechazos rechazo = new Rechazos()
                        {
                            error = "Fecha invalida",
                            registro_original = line
                        };
                        rechazos.Add(rechazo);
                        continue;
                    }

                    venta.fecha = DateTime.ParseExact(fechaString, "yyyy-MM-dd", CultureInfo.InvariantCulture);


                    venta.cod_vendedor = line.Substring(10, 3).Trim();
                    if (string.IsNullOrWhiteSpace(venta.cod_vendedor))
                    {
                        Rechazos rechazo = new Rechazos()
                        {
                            error = "Codigo de vendedor invalido",
                            registro_original = line
                        };
                        rechazos.Add(rechazo);
                        continue;
                    }


                    string ventaString = line.Substring(13, 11).Trim();
                    if (string.IsNullOrWhiteSpace(ventaString))
                    {
                        Rechazos rechazo = new Rechazos()
                        {
                            error = "Venta invalida",
                            registro_original = line
                        };
                        rechazos.Add(rechazo);
                        continue;
                    }

                    venta.venta = decimal.Parse(ventaString,
                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture);


                    string ventaEmpresaGrandeString = line.Substring(24, 1).Trim();
                    if (string.IsNullOrWhiteSpace(ventaEmpresaGrandeString) ||
                        (ventaEmpresaGrandeString != "S" && ventaEmpresaGrandeString != "N"))
                    {
                        Rechazos rechazo = new Rechazos()
                        {
                            error = "Venta a empresa grande invalida",
                            registro_original = line
                        };
                        rechazos.Add(rechazo);
                        continue;
                    }


                    venta.venta_empresa_grande = ventaEmpresaGrandeString == "S" ? true : false;

                    ventas_validas.Add(venta);

                }
            

            
            
                foreach (Ventas_Mensuales venta in ventas_validas)
                {
                    context.Ventas_Mensuales.Add(venta);
                }

                context.SaveChanges();

            

            
            
                foreach (Rechazos rechazo in rechazos)
                {
                    context.Rechazos.Add(rechazo);
                }

                context.SaveChanges();
            
            
            // 5. Listar todos los vendedores que hayan superado los 100.000 en el mes. Ejemplo: "El vendedor 001 vendio 250.000" 
            
            
                var mejoresVendedores = context.Ventas_Mensuales
                    .GroupBy(v => v.cod_vendedor)
                    .Select(v => new
                    {
                        cod_vendedor = v.Key,
                        total = v.Sum(v => v.venta)
                    })
                    .Where(v => v.total > 100000)
                    .ToList();

                foreach (var vendedor in mejoresVendedores)
                {
                    Console.WriteLine($"El vendedor {vendedor.cod_vendedor} vendio {vendedor.total}");
                }
            
            
            // 6. Listar todos los vendedores que NO hayan superado los 100.000 en el mes. Ejemplo: "El vendedor 001 vendio 90.000" 
            
            
            
                var peoresVendedores = context.Ventas_Mensuales
                    .GroupBy(v => v.cod_vendedor)
                    .Select(v => new
                    {
                        cod_vendedor = v.Key,
                        total = v.Sum(v => v.venta)
                    })
                    .Where(v => v.total <= 100000)
                    .ToList();

                foreach (var vendedor in peoresVendedores)
                {
                    Console.WriteLine($"El vendedor {vendedor.cod_vendedor} vendio {vendedor.total}");
                }
            
            // 7. Listar todos los vendedores que haya  vendido al menos una vez a una empresa grande. Solo listar los codigos de vendedor
            
            
                var vendedores = context.Ventas_Mensuales
                    .Where(v => v.venta_empresa_grande)
                    .Select(v => v.cod_vendedor)
                    .Distinct()
                    .ToList();

                foreach (var vendedor in vendedores)
                {
                    Console.WriteLine($"El vendedor {vendedor} vendio a una empresa grande");
                }
            
            
            // 8. Listar rechazos
            
            
            
                var rechazosdb = context.Rechazos.ToList();

                foreach (var rechazo in rechazosdb)
                {
                    Console.WriteLine($"Error: {rechazo.error} - Registro original: {rechazo.registro_original}");
                }
            
            

        }
    }
}
