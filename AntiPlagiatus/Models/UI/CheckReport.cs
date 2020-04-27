using System.Collections.Generic;
using System.Threading;

namespace AntiPlagiatus.Models
{
    public class CheckReport: Base
    {
        public int Id { get; private set; }
        public int CheckId { get; private set; }
        public string Progress { get; set; }
        public CancellationToken CancellationToken { get; private set; } = new CancellationToken();

        private CheckReport() { }
        public CheckReport(int id, int checkId)
        {
            this.Id = id;
            this.CheckId = checkId;
        }
    }
}
