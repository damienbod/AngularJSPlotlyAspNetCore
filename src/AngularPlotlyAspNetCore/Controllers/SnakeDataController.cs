using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularPlotlyAspNetCore.Models;
using Microsoft.AspNet.Mvc;

namespace AngularPlotlyAspNetCore.Controllers
{
    [Route("api/[controller]")]
    public class SnakeDataController : Controller
    {
        private ISnakeDataRepository _snakeDataRepository;

        public SnakeDataController(ISnakeDataRepository snakeDataRepository)
        {
            _snakeDataRepository = snakeDataRepository;
        }


        [HttpGet("GeographicalRegions")]
        public List<GeographicalRegion> GetGeographicalRegions()
        {
            return _snakeDataRepository.GetGeographicalRegions();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="datapoint"></param>
        /// <param name="proYearMonthDay">Y, M, D</param>
        /// <returns></returns>
        [HttpGet("LineData/{region}/{datapoint}")]
        public GeographicalCountries GetLineDataForMachine(string region, string datapoint)
        {
            return _snakeDataRepository.GetBarChartDataForRegion(region, datapoint);
        }


        [HttpGet("AddAllData")]
        public IActionResult AddAllData()
        {
            _snakeDataRepository.AddAllData();
            return Ok();
        }
    }
}
