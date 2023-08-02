using Excursionistas.Models;
using Excursionistas.Models.VModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Excursionistas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private List<Elemento> elementos = new List<Elemento>
        {
            new Elemento
            {
                Nombre = "E1",
                Peso = 5,
                Calorias = 3
            },
            new Elemento
            {
                Nombre = "E2",
                Peso = 3,
                Calorias = 5
            },
            new Elemento
            {
                Nombre = "E3",
                Peso = 5,
                Calorias = 2
            },
            new Elemento
            {
                Nombre = "E4",
                Peso = 1,
                Calorias = 8
            },
            new Elemento
            {
                Nombre ="E5",
                Peso = 2,
                Calorias = 3
            }
        };

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CalcularConjunto(int caloriasMinimas, int pesoMaximo)
        {
            List<Elemento> conjuntoElemnt = EncontrarConjunto(elementos, caloriasMinimas, pesoMaximo);
            VMElemento elemento = new VMElemento { conjuntoOpt = conjuntoElemnt };

            return View("Index", elemento);
        }

        private List<Elemento> EncontrarConjunto(List<Elemento> conjunto, int caloriasMinimas, int pesoMaximo)
        {
            List<Elemento> conj = new List<Elemento>();
            int mCalorias = 0;
            int menorPeso = Int32.MaxValue;


            // generar combinaciones posibles de elementos 
            int totalcombinacion = 1 << conjunto.Count;

            for (int i = 0; i < totalcombinacion; i++)
            {
                List<Elemento> combinacionActual = new List<Elemento>();
                int caloriasActual = 0;
                int pesoActual = 0;

                for (int j = 0; j < conjunto.Count; j++)
                {
                    if((i & (1 << j)) != 0)
                    {
                        combinacionActual.Add(conjunto[j]);
                        caloriasActual += conjunto[j].Calorias;
                        pesoActual += conjunto[j].Peso;
                    }
                }
                 // verificar si la combinacion cumple con los requisitos 
                if(caloriasActual >= caloriasMinimas && pesoActual <= pesoMaximo)
                {
                    // actualizar el conjunto encontrado
                    if(caloriasActual > mCalorias || (caloriasActual ==  mCalorias && pesoActual < menorPeso))
                    {
                        conj = new List<Elemento>(combinacionActual);
                        mCalorias = caloriasActual;
                        menorPeso = pesoActual;
                    }
                }
            }

            return conj;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}