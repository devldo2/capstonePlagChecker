using AntiPlagiatusServer.Data;
using AntiPlagiatusServer.Data.Entities;
using AntiPlagiatusServer.Models.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AntiPlagiatusServer.Services
{
    public class OperationReportService : IOperationReportService
    {
        private IOperationReportRepository reportRepository;
        private IUserRepository userRepository;
        private IMapper mapper;
        public OperationReportService(IOperationReportRepository reportRep, IUserRepository userRep, IMapper mapp)
        {
            this.reportRepository = reportRep;
            this.userRepository = userRep;
            this.mapper = mapp;
        }
        public void AddReport(string userToken, OperationReportModel report)
        {
            if (report != null)
            {
                try
                {
                    var dbReport = mapper.Map<OperationReportModel, OperationReport>(report);
                    if (this.reportRepository.OperationReports.GetAll().Where(item => item.User.Token == userToken).FirstOrDefault(item => item.ReportId == report.ReportId) == null)
                    {
                        var user = userRepository.Users.GetAll().FirstOrDefault(item => item.Token == userToken);
                        if (user != null)
                        {
                            dbReport.User = user;
                            this.reportRepository.OperationReports.Add(dbReport);
                            this.reportRepository.OperationReports.Save();
                        }
                        else throw new FileNotFoundException("User doesn't exist in database.");
                    }
                    else throw new InvalidOperationException("Report has already exist in database");
                }
                catch (Exception ex)
                {
                    throw new Exception($"AddReport: {ex.Message}", ex.InnerException ?? ex);
                }
            }
        }
        public void DeleteReport(string userToken, OperationReportModel report)
        {
            if (report != null)
            {
                try
                {
                    var user = userRepository.Users.GetAll().FirstOrDefault(item => item.Token == userToken);
                    if (user != null)
                    {
                        var dbReport = this.reportRepository.OperationReports.GetAll().Where(item => item.User.Token == userToken).FirstOrDefault(item => item.ReportId == report.ReportId);
                        if (dbReport != null)
                        {
                            this.reportRepository.OperationReports.Delete(dbReport);
                            this.reportRepository.OperationReports.Save();
                        }
                        else throw new InvalidOperationException("Report can't be removed because it does not exist in database");
                    }
                    else throw new FileNotFoundException("User doesn't exist in database.");
                }
                catch (Exception ex)
                {
                    throw new Exception($"DeleteReport: {ex.Message}", ex.InnerException ?? ex);
                }
            }
        }
        public IEnumerable<OperationReportModel> GetAllHistory(string userToken)
        {
            IEnumerable<OperationReportModel> operations = null;
            try
            {
                var user = userRepository.Users.GetAll().FirstOrDefault(item => item.Token == userToken);
                if (user != null)
                {
                    var dbOperations = this.reportRepository.OperationReports.GetAll(source => source.Include(item => item.Content).Include(item => item.IgnoreRules).Include(item => item.Domains).ThenInclude(item => item.Layers));
                    operations = this.mapper.Map<IEnumerable<OperationReport>, List<OperationReportModel>>(dbOperations.Where(item => item.User.Token == userToken));
                }
                else throw new FileNotFoundException("User doesn't exist in database.");
            }
            catch (Exception ex)
            {
                throw new Exception($"GetAllHistory: {ex.Message}", ex.InnerException ?? ex);
            }
            return operations;
        }
        public void ClearAll(string userToken)
        {
            try
            {
                var user = userRepository.Users.GetAll().FirstOrDefault(item => item.Token == userToken);
                if (user != null)
                {
                    var userOperations = this.reportRepository.OperationReports.GetAll().Where(item => item.User.Token == userToken);
                    if (userOperations.Any())
                    {
                        this.reportRepository.OperationReports.RemoveRange(userOperations);
                        this.reportRepository.OperationReports.Save();
                    }
                }
                else throw new FileNotFoundException("User doesn't exist in database.");
            }
            catch (Exception ex)
            {
                throw new Exception($"ClearAll: {ex.Message}", ex.InnerException ?? ex);
            }

        }
    }
}
