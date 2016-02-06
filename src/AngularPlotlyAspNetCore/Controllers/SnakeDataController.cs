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


        [HttpGet("Machines")]
        public List<Machine> GetMachines()
        {
            return _snakeDataRepository.GetMachines();
        }

        // GET: api/values
        public OeeDataAverageAgg Get()
        {
            return _snakeDataRepository.GetOeeForAll();
        }

        [HttpGet("{machineName}")]
        public OeeDataAverageAgg GetForMachine(string machineName)
        { 
            var data = _snakeDataRepository.GetOeeForMachines(new List<string>() { machineName}).FirstOrDefault();
            data.MachineName = machineName;
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="datapoint"></param>
        /// <param name="proYearMonthDay">Y, M, D</param>
        /// <returns></returns>
        [HttpGet("LineData/{machineName}/{datapoint}/{proYearMonthDay}")]
        public OeeDataProUnit GetLineDataForMachine(string machineName, string datapoint, string proYearMonthDay)
        {
            return _snakeDataRepository.GetLineDataForMachine(machineName, datapoint, proYearMonthDay);
        }

        [HttpGet("CompareMachineOee/{machineName1}/{machineName2}")]
        public List<OeeDataAverageAgg> CompareMachineOee(string machineName1, string machineName2)
        {
            return _snakeDataRepository.GetOeeForMachines(new List<string>() { machineName1, machineName2 });
        }

        [HttpGet("AddAllData")]
        public IActionResult AddAllData()
        {
            _snakeDataRepository.AddAllData();
            return Ok();
        }
    }
}
