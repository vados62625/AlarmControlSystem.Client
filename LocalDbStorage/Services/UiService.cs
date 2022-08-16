using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AutoMapper;
using LocalDbStorage.Data.Interfaces;
using LocalDbStorage.Dto;
using LocalDbStorage.Repositories.Models;

namespace LocalDbStorage.Services
{
    public class UiService : IUiService
    {
        private readonly IDataService _dataService;
        public const int Day = 24;

        public UiService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<Dictionary<DateTime, double>> CountOfIncomingAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
        {
            var result = await _dataService.GetScopeIncomingAlarm(idWorkStation, startDate, endDate);

            return AverageCount(result.Select(c => c.Date).ToList());
        }

        public async Task<Dictionary<DateTime, int>> CountOfActiveAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
        {
            var result = await _dataService.GetScopeActiveAlarm(idWorkStation, startDate, endDate);

            return Count(result.Select(c => c.StartDate).ToList());
        }

        public async Task<Dictionary<DateTime, int>> CountOfSuppressedAlarms(int idWorkStation, DateTime startDate, DateTime endDate)
        {
            var result = await _dataService.GetScopeSuppressedAlarm(idWorkStation, startDate, endDate);

            return Count(result.Select(c => c.Date).ToList());
        }



        private Dictionary<DateTime, int> Count(List<DateTime> list)
        {
            Dictionary<DateTime, int> _count = new Dictionary<DateTime, int>();

            var groupDateTime = list.GroupBy(g => g.Date);

            groupDateTime = groupDateTime.OrderBy(u => u.Key).ToList();

            foreach (var g in groupDateTime)
            {
                _count.Add(g.Key, g.Count());
            }

            return _count;
        }

        private Dictionary<DateTime, double> AverageCount(List<DateTime> list)
        {
            Dictionary<DateTime, double> _count = new Dictionary<DateTime, double>();

            var groupDateTime = list.GroupBy(g => g.Date);

            groupDateTime = groupDateTime.OrderBy(u => u.Key).ToList();

            foreach (var g in groupDateTime)
            {
                _count.Add(g.Key, g.Count() / Day);
            }

            return _count;
        }


    }
}
