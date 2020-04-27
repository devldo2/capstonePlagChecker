using AntiPlagiatusServer.Models.DTO;
using System.Collections.Generic;

namespace AntiPlagiatusServer.Services
{
    public interface IOperationReportService
    {
        void AddReport(string userToken, OperationReportModel report);
        void DeleteReport(string userToken, OperationReportModel report);
        IEnumerable<OperationReportModel> GetAllHistory(string userToken);
        void ClearAll(string userToken);
    }
}
