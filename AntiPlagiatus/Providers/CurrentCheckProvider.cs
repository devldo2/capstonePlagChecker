using AntiPlagiatus.Models;
using AntiPlagiatus.Models.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AntiPlagiatus.Providers
{
    public class CurrentCheckProvider
    {
        private string checkKey;
        private int lastCheckId = 0;
        private int lastReportId = 0;
        private ReportItem lastReport;
        private string content = "allows you to work chika bubi sha, easily, and conveniently.";

        public event EventHandler<Status> ReportStatusChanged;
        public string Content
        {
            get => content;
            set
            {
                if (this.content != value)
                {
                    this.content = value;
                    this.lastReport = null;
                    this.checkKey = null;
                    this.ReportStatus = Status.None;
                }
            }
        }
        public bool IsProcessing { get; private set; }
        public List<IgnoreRule> IgnoreRules { get; set; } = new List<IgnoreRule>();
        public ReportItem GetLastReport => lastReport;
        public Status ReportStatus { get; private set; }
        public async Task<CheckResponse> Check()
        {
            this.IsProcessing = true;
            this.ReportStatus = Status.InProgress;
            CheckResponse responseResult = null;
            this.IgnoreRules = this.IgnoreRules ?? new List<IgnoreRule>();
            this.ReportStatusChanged?.Invoke(this, this.ReportStatus);

            ++lastCheckId;
            //  this.checkKey = "12163549";
            responseResult = await WebPlagiatusApiProvider.CheckText(this.Content, lastCheckId.ToString(), this.IgnoreRules);
            if (string.IsNullOrEmpty(responseResult.result.error_msg))
                this.checkKey = responseResult.result.key;
            else
            {
                this.IsProcessing = false;
                this.ReportStatus = Status.NoAPIResult;
                this.ReportStatusChanged?.Invoke(this, this.ReportStatus);
            }


            return responseResult;
        }
        public void ReuseHistoryItem(HistoryItem report)
        {
            this.Content = report.Content.Text;
            this.IgnoreRules = report.IgnoreRules;
        }
        public async Task<ReportItem> ProcessReport()
        {
            ReportItem currentReport = this.lastReport;
            if (currentReport == null && !string.IsNullOrEmpty(this.checkKey))
            {
                currentReport = new ReportItem();
                bool isOperationFinished = false;
                // var key = "12163549";
                ++lastReportId;
                this.ReportStatus = Status.ReportInProgress;
                string errorMessage = null;
                do
                {
                    var reportResponse = await WebPlagiatusApiProvider.GetReport(this.checkKey, lastReportId.ToString());
                    if (reportResponse?.result != null)
                    {
                        if (!string.IsNullOrEmpty(reportResponse.result.error_msg))
                        {
                            this.ReportStatus = Status.Failed;
                            errorMessage = reportResponse.result.error_msg;
                            isOperationFinished = true;
                            //-1 — не хватает символов на счету;
                            //-2 — не хватает денежных средств на счету;
                            //-5 — ошибка подключения к БД;
                            //-10 — получен неверный ключ;
                            //-11 — ошибка авторизации по токену;
                            //-13 — ошибка при проверке поля text;
                            //-14 — ошибка при проверке поля title;
                            //-17 — ошибка добавления работы;
                            //-21 — текст не найден;
                            //-67 — not enough symbols – недостаточно символов на счету, минимальное количество – 100 000.

                            // throw new Exception("error code: " + report.result.error + " | " + "error message: " + report.result.error_msg);
                        }
                        else
                            switch (reportResponse.result.status)
                            {
                                case "done":
                                    this.ReportStatus = Status.Success;
                                    currentReport.Date = DateTime.Now;
                                    currentReport.Equality = reportResponse.result.report.equality;
                                    currentReport.Rewrite = reportResponse.result.report.rewrite;
                                    var resultDomains = reportResponse.result.report.layers_by_domain;
                                    if (resultDomains?.Count > 0)
                                        currentReport.Domains.AddRange(resultDomains.Select(domain => new Domain()
                                        {
                                            Uri = domain.domain,
                                            Equality = domain.equality,
                                            Rewrite = domain.rewrite,
                                            Layers = domain.layers.Select(layer => new LayerByDomain()
                                            {
                                                Uri = layer.uri,
                                                Equality = layer.equality,
                                                Rewrite = layer.rewrite,
                                                Words = layer.words
                                            }).ToList()
                                        }));
                                    currentReport.CharactersNumber = reportResponse.result.report.len;
                                    currentReport.WordCount = reportResponse.result.report.word_count;
                                    isOperationFinished = true;
                                    break;
                                case "error":
                                    this.ReportStatus = Status.UknownError;
                                    currentReport.ErrorMessage = "Uknown error during processing the report";
                                    isOperationFinished = true;
                                    break;
                                case "not found":
                                    this.ReportStatus = Status.NotFound;
                                    isOperationFinished = true;
                                    break;
                                case "in progress":
                                    this.ReportStatus = Status.ReportInProgress;
                                    break;
                                default:
                                    break;
                            }
                    }
                    else this.ReportStatus = Status.NoAPIResult;

                    this.ReportStatusChanged?.Invoke(this, this.ReportStatus);
                    if (!isOperationFinished) await Task.Delay(5000);
                }
                while (!isOperationFinished);

                currentReport.Status = this.ReportStatus;
                this.lastReport = currentReport;
            }
            this.IsProcessing = false;
            return currentReport;
        }
        public void SetLastReport(ReportItem existsReport)
        {
            lastReport = existsReport;
            this.ReportStatus = existsReport.Status;
            this.ReportStatusChanged?.Invoke(this, this.ReportStatus);
        }
    }
}
