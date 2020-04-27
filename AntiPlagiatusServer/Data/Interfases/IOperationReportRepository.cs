using AntiPlagiatusServer.Data.Entities;

namespace AntiPlagiatusServer.Data
{
    //TODO
    public interface IOperationReportRepository
    {
        IRepository<OperationReport> OperationReports { get; }
    }
}
