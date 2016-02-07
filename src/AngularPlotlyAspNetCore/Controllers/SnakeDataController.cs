﻿using System.Collections.Generic;
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

        [HttpGet("RegionBarChart/{region}")]
        public GeographicalCountries GetBarChartDataForRegion(string region)
        {
            return _snakeDataRepository.GetBarChartDataForRegion(region);
        }

        [HttpGet("AddAllData")]
        public IActionResult AddAllData()
        {
            _snakeDataRepository.AddAllData();
            return Ok();
        }
    }
}
