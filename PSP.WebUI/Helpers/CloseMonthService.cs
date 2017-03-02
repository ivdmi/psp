using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using PSP.Domain;
using PSP.Domain.Abstract;
using PSP.Domain.Concrete;
using PSP.Domain.Service;
using PSP.WebUI.Models;

namespace PSP.WebUI.Helpers
{
    public class CloseMonthService
    {
        public CloseMonthService(IRepository repository)
        {
            _repository = repository;
        }
        
        private IRepository _repository;

        
        public List<string> GetFactoryList(int Year, int Month)
        {
            IList<events> List = _repository.Events.Where(p => p.Date.Year == Year && p.Date.Month == Month).ToList();

            List<string> uniqFactories = new List<string>();
            foreach (events Event in List)
            {
                foreach (string S in Event.FactoryList.Split(';'))
                {
                    if ("" != S)
                    {
                        string[] V = S.Split(',');

                        // Контроль и обработка ошибок, если в сообщении аудитор вставил символ ";"         !!!!!
                        if (V.Count() > 2)
                            uniqFactories.Add(V[0]);
                    }
                }
            }
            uniqFactories = uniqFactories.Distinct().OrderBy(P => P).ToList();

            var strings = _repository.CompanyAnalysis.Where(p => p.Date.Year == Year && p.Date.Month == Month).Select(p => p.Company);
            var removeItemsList = uniqFactories.Where(p => strings.Contains(p));
            uniqFactories = uniqFactories.Except(removeItemsList).ToList();

            return uniqFactories;
        }

        public List<GridViewDataAuditorCloseMonthRowInfo> GetAuditorsTable(int Year, int Month, List<string> factories, string selectedFactory = null)
        {
            if (factories == null || selectedFactory.IsNullOrWhiteSpace())
                    return new List<GridViewDataAuditorCloseMonthRowInfo>();

            string _selectedFactory = selectedFactory.ToLower();
            var Start = new DateTime(Year, Month, 1);

            int DaysInMonth = DateTime.DaysInMonth(Year, Month);

            var End = new DateTime(Year, Month, DaysInMonth);

            events[] List = _repository.Events.Where(
                p => p.Date >= Start && p.Date <= End && 
                p.FactoryList.ToLower().Contains(_selectedFactory)).ToArray();

            var UserWorkMinutes = new Dictionary<string, int>();

            foreach (events Evt in List)
            {
                int Minutes = 0;

                string[] FactoryList = Evt.FactoryList.Split(';');
                foreach (string S in FactoryList)
                {
                    string[] Items = S.Split(',');

                    // Контроль и обработка ошибок, если в сообщении аудитор вставил символ ";"         !!!!!
                    if (Items.Count() < 3)
                    {
                        continue;
                    }

                    if (Items[0].ToLower().Equals(_selectedFactory))
                    {
                        DateTime From;
                        DateTime To;
                        if (!DateTime.TryParse(Items[1], out From))
                        {
                            continue;
                        }
                        if (!DateTime.TryParse(Items[2], out To))
                        {
                            continue;
                        }
                        TimeSpan Span = (To - From);
                        Minutes += Convert.ToInt32(Span.TotalMinutes);
                    }
                }

                if (0 != Minutes)
                {
                    string Id = Evt.UserID;
                    if (UserWorkMinutes.ContainsKey(Id))
                        UserWorkMinutes[Id] += Minutes;
                    else
                        UserWorkMinutes.Add(Id, Minutes);
                }
            }

            var usersDictionary = new Dictionary<string, string>();
            users[] usersArray = new UsersService(_repository).GetAllUsers().ToArray();
            foreach (users user in usersArray)
            {
                usersDictionary[user.ID] = user.Name;
            }

            List<GridViewDataAuditorCloseMonthRowInfo> auditorCloseMonthList = new List<GridViewDataAuditorCloseMonthRowInfo>();

            foreach (KeyValuePair<string, int> Pair in UserWorkMinutes)
            {
                GridViewDataAuditorCloseMonthRowInfo Row = new GridViewDataAuditorCloseMonthRowInfo();
                Row.Name = usersDictionary[Pair.Key];
                Row.Hours = string.Format("{0:00}:{1:00}", Pair.Value / 60, Pair.Value % 60);
                Row.CostOneHour = 0;
                Row.CostTotal = 0;
                auditorCloseMonthList.Add(Row);
            }

            return auditorCloseMonthList;
        }

        //public CloseMonthModel GetUpdatedCloseData(CloseMonthModel closeMonthData)
        //{
        //    CloseMonthModel _closeMonthData = closeMonthData;
        //    Factories = GetFactoryList();
        //    if (Factories.Count > 0)
        //    {
        //        _closeMonthData.AuditorsMonthList = GetAuditorsTable(Factories);
        //    }
        //    else
        //    {
        //        closeMonthData.AuditorsMonthList = new List<GridViewDataAuditorCloseMonthRowInfo>();
        //    }
        //    return closeMonthData;
        //}

        //private void CloseMonthExecute()
        //{
        //    var Date = new DateTime(Convert.ToInt32(YearFromSpin.Value), Convert.ToInt32(MonthFromSpin.Value), 1);
        //    var TableRecord = new companyanalysis
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        Company = FactoryComboBox.Text,
        //        Date = Date,
        //        Income = Convert.ToInt32(MoneySpin.Value),
        //        Costs = ReportGridView.Rows.Sum(RowInfo => Convert.ToInt32(RowInfo.Cells[3].Value)),
        //        //Auditors = Builder.ToString()
        //    };
        //    TableRecord.Result = TableRecord.Income - TableRecord.Costs;
        //    TableRecord.Save();


        //    decimal Dp = MoneySpin.Value;
        //    foreach (GridViewDataRowInfo Info in ReportGridView.Rows)
        //    {

        //        var A = new AuditorAnalysi();
        //        A.Id = Guid.NewGuid().ToString();

        //        var Auditor = Info.Cells[0].Value as string;
        //        A.Auditor = Auditor;

        //        string[] Hour = ((string)Info.Cells[1].Value).Split(':');
        //        int H = Convert.ToInt32(Hour[0]);
        //        int M = Convert.ToInt32(Hour[1]);

        //        A.ConstForClient = Convert.ToInt32(Dp / H);

        //        int Sum = Convert.ToInt32(Info.Cells[3].Value);
        //        A.Cost = Sum;

        //        A.Salary = A.ConstForClient - A.Cost;

        //        A.Date = Date;

        //        A.Factory = FactoryComboBox.Text;

        //        A.Save();
        //    }


        //    Close();
        //    DialogResult = DialogResult.OK;
        //}
    }
}