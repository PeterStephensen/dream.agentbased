﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dream.AgentClass;
using Dream.IO;  

namespace Dream.Models.SOE_Basic
{
    public class Household : Agent
    {

        #region Private fields
        Simulation _simulation;
        Settings _settings;
        Time _time;
        Random _random;
        Statistics _statistics;
        int _age;
        Firm _firmEmployment=null, _firmShop=null;
        bool _unemp = false; // Primo: unemployed? 
        double _w; //Wage
        int _unempDuration = 0;
        double _productivity = 0;
        bool _initialHousehold = false;
        double _yr_consumption = 0;
        int _yr_employment = 0;
        bool _startFromDatabase = false;
        bool _report = false;
        double _consumption = 0;
        double _income = 0;

        #endregion


        #region Constructors
        public Household()
        {

            _simulation = Simulation.Instance;
            _settings = _simulation.Settings;
            _time = _simulation.Time;
            _random = _simulation.Random;
            _statistics = _simulation.Statistics;
            
            _productivity = 1;
            _age = _settings.HouseholdStartAge;

        }
        public Household(TabFileReader file) : this()
        {

            _age = file.GetInt32("Age");
            _productivity = file.GetDouble("Productivity");

            int firmEmploymentID = file.GetInt32("FirmEmploymentID");
            int firmShopID = file.GetInt32("FirmShopID");
            
            if(firmEmploymentID != -1)
                _firmEmployment = _simulation.GetFirmFromID(firmEmploymentID);                        

            if(firmShopID != -1)            
                _firmShop = _simulation.GetFirmFromID(firmShopID);

            if(_firmEmployment != null)
                _firmEmployment.Communicate(ECommunicate.Initialize, this);

            _startFromDatabase = true;
            
            if(_random.NextEvent(0.02))
                _report = true;

        }
        #endregion

        #region EventProc
        public override void EventProc(int idEvent)
        {
            switch (idEvent)
            {

                case Event.System.Start:  // Initial households
                    if(_startFromDatabase)
                    {
                        _initialHousehold = false;
                    }
                    else
                    {
                        _age = _settings.HouseholdStartAge + (int)(_random.NextDouble() * (_settings.HouseholdPensionAge - _settings.HouseholdStartAge - 1));
                        _productivity = Math.Exp(_random.NextGaussian(_settings.HouseholdProductivityLogMeanInitial, _settings.HouseholdProductivityLogSigmaInitial));
                        _initialHousehold = true;
                    }
                    break;

                case Event.System.PeriodStart:                    
                    _unemp = _firmEmployment == null;
                    _w = _unemp ? 0.0 : _firmEmployment.Wage;
                    _unempDuration = _unemp ? _unempDuration + 1 : 0;

                    if (_time.Now == 0)
                        _w = _simulation.Statistics.PublicMarketWage;

                    if(_time.Now % _settings.PeriodsPerYear==0)
                    {
                        _yr_consumption = 0;
                        _yr_employment = 0;
                    }

                    if (_firmEmployment != null)
                        _yr_employment++;

                    _income = _w * _productivity + _simulation.Statistics.PublicProfitPerHousehold;
                    
                    ReportToStatistics();

                    break;

                case Event.Economics.Update:
                    if(_age==_settings.HouseholdPensionAge)
                    {
                        if(_firmEmployment != null)
                        {
                            _firmEmployment.Communicate(ECommunicate.IQuit, this);
                            _firmEmployment = null;
                        }
                    }
                    else if(_age<_settings.HouseholdPensionAge)
                    {
                        if (_firmEmployment == null)  // If unemployed
                            SearchForJob();
                        else  // If employed
                        {
                            // If job is changed, it is from next period. 
                            if (_random.NextEvent(_settings.HouseholdProbabilitySearchForJob))
                                SearchForJob();

                            if (_random.NextEvent(_settings.HouseholdProbabilityQuitJob))
                            {
                                _firmEmployment.Communicate(ECommunicate.IQuit, this);
                                _firmEmployment = null;
                            }
                        }                    
                    }

                    if (_random.NextEvent(_settings.HouseholdProbabilitySearchForShop))
                        SearchForShop();

                    _consumption = BuyFromShop();
                    _yr_consumption += _consumption;
                    break;

                case Event.System.PeriodEnd:
                    if (_time.Now == _settings.StatisticsWritePeriode)
                        Write();
                    
                    if (_random.NextEvent(ProbabilityDeath()))
                    {
                        if(_firmEmployment!=null)
                            _firmEmployment.Communicate(ECommunicate.IQuit, this);
                        
                        RemoveThisAgent();
                        return;
                    }

                    if (!_initialHousehold)
                        if (_age < _settings.HouseholdPensionAge)
                            _productivity *= Math.Exp(_random.NextGaussian(0, _settings.HouseholdProductivityErrorSigma));
                        else
                            _productivity = 0;

                    _age++;
                    break;

                case Event.System.Stop:
                    break;

                default:
                    base.EventProc(idEvent);
                    break;
            }
        }
        #endregion

        #region Internal methods
        #region BuyFromShop()
        double BuyFromShop()
        {

            if (_firmShop == null)
                _firmShop = _simulation.GetRandomFirm();
            
            double budget = _income; // KAN VÆRE NEGATIV !!!!!!!!!!!!!!!!!!!!!!!!!!!
            bool foundFirm = true;

            int i = 0;
            if (budget > 0)
                while(_firmShop.Communicate(ECommunicate.CanIBuy, budget / _firmShop.Price)!=ECommunicate.Yes)
                {
                    SearchForShop();
                    i++;
                    if (i > _settings.HouseholdMaxNumberShops)
                    {
                        foundFirm = false;
                        break;
                    }
                }
            
            return foundFirm ? budget / _firmShop.Price : 0.0;
        }
        #endregion

        #region SearchForJob()
        void SearchForJob()
        {

            double wageNow = _firmEmployment != null ? _firmEmployment.Wage : 0.0;
            
            var firms = _simulation.GetRandomFirms(_settings.HouseholdNumberFirmsSearchJob);
            firms = firms.OrderByDescending(x => x.Wage).ToList(); // Order by wage. Highest wage first

            foreach (Firm f in firms)
            {
                if(f.Wage > wageNow)
                    if (f.Communicate(ECommunicate.JobApplication, this) == ECommunicate.Yes)
                    {
                        if (_firmEmployment != null)
                            _firmEmployment.Communicate(ECommunicate.IQuit, this);

                        _firmEmployment = f;
                        break;
                    }
            }
        }
        #endregion

        #region SearchForShop()
        void SearchForShop()
        {

            List<Firm> firms = _simulation.GetRandomFirms(_settings.HouseholdNumberFirmsSearchShop);
            firms = firms.OrderBy(x => x.Price).ToList(); // Order by price. Lowes price first

            if (_firmShop == null || firms.First().Price < _firmShop.Price)  
                _firmShop = firms.First();

        }
        #endregion

        #region ProbabilityDeath()
        double ProbabilityDeath()
        {
            return Math.Pow(1 + Math.Exp(0.1 * _age / _settings.PeriodsPerYear - 10.0), 1.0/_settings.PeriodsPerYear) - 1;

        }
        #endregion

        #region ReportToStatistics()
        void ReportToStatistics()
        {
            if (_report)
            {
                
                _statistics.StreamWriterHouseholdReport.WriteLineTab(_settings.StartYear + 1.0 * _time.Now / _settings.PeriodsPerYear, 
                    this.ID, _productivity, _age, _consumption);

                _statistics.StreamWriterHouseholdReport.Flush();

            }


        }
        #endregion

        #endregion

        #region Communicate
        public ECommunicate Communicate(ECommunicate comID, object o)
        {
            switch (comID)
            {
                case ECommunicate.YouAreFired:
                    _firmEmployment = null;
                    return ECommunicate.Ok;
               
                case ECommunicate.Initialize:
                    _firmEmployment = (Firm)o;
                    return ECommunicate.Ok;
                
                default:
                    return ECommunicate.Ok;
            }
        }
        #endregion

        #region Write()
        void Write()
        {

            int firmEmploymentID = _firmEmployment != null ? _firmEmployment.ID : -1;
            int firmShopID = _firmShop != null ? _firmShop.ID : -1;

            _statistics.StreamWriterDBHouseholds.WriteLineTab(ID, _age, firmEmploymentID, firmShopID, _productivity);

        }
        #endregion

        #region Public proporties
        public int Age
        {
            get { return _age; }
        }
        /// <summary>
        /// True if unemployed primo
        /// </summary>
        public bool Unemployed
        {
            get { return _unemp; }
        }

        /// <summary>
        /// Duration of unemployment spell
        /// </summary>
        public int UnemploymentDuration
        {
            get { return _unempDuration; }
        }
        public double Productivity
        {
            get { return _productivity; }
        }
        public double YearConsumption
        {
            get { return _yr_consumption; }
        }
        public int YearEmployment
        {
            get { return _yr_employment; }
        }

        #endregion

    }
}