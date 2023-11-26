

using Contracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHomeController.Key;

namespace SmartHomeControllers.Controllers
{
    [ApiController]
    [Route("/SensorData/")]
    [ApiKey]
    public class SensorDataController : ControllerBase
    {
        private readonly ISensorDataService _sensorDataService;

        public SensorDataController(ISensorDataService sensorDataService)
        {
            _sensorDataService = sensorDataService;
        }

        /// <summary>
        /// Get the most recent sensor measurements for a home.
        /// </summary>
        [HttpGet]
        [Route("recent/{homeId:long}/{amount:int}")]
        public async Task<ActionResult<List<SensorData>>>
            GetRecentSensorMeasurements([FromRoute] long homeId, [FromRoute] int amount)
        {
            try
            {
                ICollection<SensorData> measurements = await _sensorDataService.GetRecentMeasurements(homeId, amount);
                return Ok(measurements);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get the last recorded sensor measurement for a home.
        /// </summary>
        [HttpGet]
        [Route("last/{homeId:long}")]
        public async Task<ActionResult<SensorData>>
            GetLastSensorMeasurement([FromRoute] long homeId)
        {
            try
            {
                SensorData measurement = await _sensorDataService.GetLastMeasurement(homeId);
                return Ok(measurement);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get sensor measurements for a home over a specified number of hours.
        /// </summary>
        [HttpGet]
        [Route("hourly/{homeId:long}/{hours:int}")]
        public async Task<ActionResult<List<SensorData>>>
            GetHourlySensorMeasurements([FromRoute] long homeId, [FromRoute] int hours)
        {
            try
            {
                ICollection<SensorData> measurements = await _sensorDataService.GetHourlyMeasurements(homeId, hours);
                return Ok(measurements);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get sensor measurements for a home over a specified number of days.
        /// </summary>
        [HttpGet]
        [Route("daily/{homeId:long}/{days:int}")]
        public async Task<ActionResult<List<SensorData>>>
            GetDailySensorMeasurements([FromRoute] long homeId, [FromRoute] int days)
        {
            try
            {
                ICollection<SensorData> measurements = await _sensorDataService.GetDailyMeasurements(homeId, days);
                return Ok(measurements);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get sensor measurements for a home over a specific month and year.
        /// </summary>
        [HttpGet]
        [Route("monthly/{homeId:long}/{month:int}/{year:int}")]
        public async Task<ActionResult<List<SensorData>>>
            GetMonthlySensorMeasurements([FromRoute] long homeId, [FromRoute] int month, [FromRoute] int year)
        {
            try
            {
                ICollection<SensorData> measurements = await _sensorDataService.GetMonthlyMeasurements(homeId, month, year);
                return Ok(measurements);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get sensor measurements for a home over a specific year.
        /// </summary>
        [HttpGet]
        [Route("yearly/{homeId:long}/{year:int}")]
        public async Task<ActionResult<List<SensorData>>>
            GetYearlySensorMeasurements([FromRoute] long homeId, [FromRoute] int year)
        {
            try
            {
                ICollection<SensorData> measurements = await _sensorDataService.GetYearlyMeasurements(homeId, year);
                return Ok(measurements);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Add a new sensor measurement to a home.
        /// </summary>
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult> AddSensorMeasurement([FromBody] SensorData sensorData, [FromRoute] long homeId)
        {
            try
            {
                await _sensorDataService.AddSensorMeasurement(sensorData, homeId);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Add a new sensor measurement to a home with a specific EUI (device identifier).
        /// </summary>
        [HttpPost]
        [Route("addWithEUI")]
        public async Task<ActionResult> AddSensorMeasurementWithEUI([FromBody] SensorData sensorData, [FromQuery] string EUI)
        {
            try
            {
                await _sensorDataService.AddSensorMeasurementWithEui(sensorData, EUI);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
