using AntiPlagiatusServer.Models.DTO;
using AntiPlagiatusServer.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AntiPlagiatusServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class HistoryController : ControllerBase
    {
        private IOperationReportService operationService;
        public HistoryController(IOperationReportService operationService)
        {
            this.operationService = operationService;
        }

        [HttpGet]
        [Route("GetAllHistory")]
        public async Task<ObjectResult> GetAllHistory()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
                {
                    var jsonContent = await reader.ReadToEndAsync();
                    var userModel = JsonConvert.DeserializeObject<UserModel>(jsonContent);
                    var historyCollection = this.operationService.GetAllHistory(userModel?.Token);
                    return new OkObjectResult(historyCollection);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.GetBaseException().Message);
            }
        }

        [HttpPost]
        [Route("SaveHistoryItem")]
        public async Task<IActionResult> SaveHistoryItem()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
                {
                    var jsonContent = await reader.ReadToEndAsync();
                    var report = JsonConvert.DeserializeObject<OperationReportModel>(jsonContent);
                    this.operationService.AddReport(report.UserToken, report);
                    return new OkResult();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.GetBaseException().Message);
            }
        }

        [HttpDelete]
        [Route("RemoveHistoryItem")]
        public async Task<IActionResult> RemoveHistoryItem()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
                {
                    var jsonContent = await reader.ReadToEndAsync();
                    var report = JsonConvert.DeserializeObject<OperationReportModel>(jsonContent);
                    this.operationService.DeleteReport(report.UserToken, report);
                    return new OkResult();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.GetBaseException().Message);
            }
        }

        [HttpDelete]
        [Route("CleanHistory")]
        public async Task<IActionResult> CleanHistory()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
                {
                    var jsonContent = await reader.ReadToEndAsync();
                    var userModel = JsonConvert.DeserializeObject<UserModel>(jsonContent);
                    this.operationService.ClearAll(userModel.Token);
                    return new OkResult();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.GetBaseException().Message);
            }
        }
    }
}