using AntiPlagiatusServer.Data.Entities;

namespace AntiPlagiatusServer.Data
{
    public class OperationReportRepository : IOperationReportRepository
    {
        private IDbContext dbContext;
        private IRepository<OperationReport> operationReportRepository;
        public OperationReportRepository(IDbContext context)
        {
            dbContext = context;
        }

        public IRepository<OperationReport> OperationReports
        {
            get
            {
                if (operationReportRepository == null)
                    operationReportRepository = new Repository<OperationReport>(dbContext);
                return operationReportRepository;
            }
        }
    }
}
