using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dream.Models.SOE_Basic
{
    public class Settings
    {
        public int NumberOfHouseholdsPerFirm=0;
        public bool FirmStartNewFirms = false; 

        public int NumberOfFirms=0;
        /// <summary>
        /// Minimum productivity in pareto distribution
        /// </summary>
        public double FirmParetoMinPhi = 0.5;
        /// <summary>
        /// k-parameter in pareto productivity distribution
        /// </summary>
        public double FirmPareto_k = 2;

        /// <summary>
        /// Decreasing returns to scale parameter
        /// </summary>
        public double FirmAlpha = 0.8;
        /// <summary>
        /// Increasing returns to scale parameter
        /// </summary>
        public double FirmFi = 1.0;

        /// <summary>
        /// Wage markup if hard to find people (outside comfort zone)
        /// </summary>
        public double FirmWageMarkup = 0.001;
        public double FirmWageMarkupSensitivity = 1.0;

        /// <summary>
        /// Wage markup if hard to find people (in comfort zone)
        /// </summary>
        public double FirmWageMarkupInZone = 0.001;
        public double FirmWageMarkupSensitivityInZone = 1.0;


        /// <summary>
        /// Wage markdown if too many people (outside comfort zone)
        /// </summary>
        public double FirmWageMarkdown = 0.001;
        public double FirmWageMarkdownSensitivity = 1.0;

        /// <summary>
        /// Wage markdown if too many people (in comfort zone)
        /// </summary>
        public double FirmWageMarkdownInZone = 0.001;
        public double FirmWageMarkdownSensitivityInZone = 1.0;

        /// <summary>
        /// Price markup (outside comfort zone)
        /// </summary>
        public double FirmPriceMarkup = 0.001;
        public double FirmPriceMarkupSensitivity = 1.0;

        /// <summary>
        /// Price markup (in comfort zone)
        /// </summary>
        public double FirmPriceMarkupInZone = 0.001;
        public double FirmPriceMarkupSensitivityInZone = 1.0;


        /// <summary>
        /// Price markdown (outside comfort zone)
        /// </summary>
        public double FirmPriceMarkdown = 0.001;
        public double FirmPriceMarkdownSensitivity = 1.0;

        /// <summary>
        /// Price markdown (in comfort zone)
        /// </summary>
        public double FirmPriceMarkdownInZone = 0.001;
        public double FirmPriceMarkdownSensitivityInZone = 1.0;


        /// <summary>
        /// Periode when price mechanism starts
        /// </summary>
        public int FirmPriceMechanismStart = 0;

        /// <summary>
        /// Smoothing in expectations: xE(t) = b*xE(t-1) + (1-b)*x(t-1)
        /// </summary>
        public double FirmExpectationSmooth = 0.0;

        /// <summary>
        /// Maximum firm size
        /// </summary>
        public int FirmMaxEmployment = 10000000;

        /// <summary>
        /// If negative profit, the firm is closed with this probability
        /// </summary>
        public double FirmDefaultProbabilityNegativeProfit = 0.0;

        public int FirmNegativeProfitOkAge = 0;

        /// <summary>
        /// Exogeneous default risk
        /// </summary>
        public double FirmDefaultProbability = 0.0;

        /// <summary>
        /// Period where the posibility of default starts 
        /// </summary>
        public int FirmDefaultStart = 0;

        /// <summary>
        /// Period where the posibility of firings starts 
        /// </summary>
        public int FirmFiringsStart = 0;


        public double FirmProbabilityRecalculatePrice = 1.0;
       
        public double FirmProbabilityRecalculateWage = 1.0;

        /// <summary>
        /// Productivity growth p.a. in new firms
        /// </summary>
        public double FirmProductivityGrowth = 0.0;

        public int FirmNumberOfNewFirms = 0;

        /// <summary>
        /// Duration of startup periode (= Entry Cost)
        /// </summary>
        public int FirmStartupPeriod = -1;

        /// <summary>
        /// Employment during startup periode
        /// </summary>
        public int FirmStartupEmployment = 0;

        /// <summary>
        /// The firm is in its comport zone if actual employment diviates less from optimal 
        /// </summary>
        public double FirmComfortZoneEmployment = 0.0;

        /// <summary>
        /// The firm is in its comport zone if actual sales diviates less from optimal production 
        /// </summary>
        public double FirmComfortZoneSales = 0.0;

        /// <summary>
        /// From this period, the profit limit os zero
        /// </summary>
        public double FirmProfitLimitZeroPeriod = 0.0;



        //---------------------------------
        /// <summary>
        /// Number of firms contacted when searching for job
        /// </summary>
        public int HouseholdNumberFirmsSearchJob = 10;
        public int HouseholdNumberFirmsSearchShop = 5;
        public int HouseholdMaxNumberShops = 5;
        public double HouseholdProbabilityQuitJob = 0;
        public double HouseholdProbabilitySearchForJob = 0;
        public double HouseholdProbabilitySearchForShop = 0.01;
        public int HouseholdPensionAge = 0;
        public int HouseholdStartAge = 0;
        /// <summary>
        /// The number of new housholds each period
        /// </summary>
        public int HouseholdNewBorn = 0;

        /// <summary>
        /// Mean in log-normal productivity distribution (initial population)
        /// </summary>
        public double HouseholdProductivityLogMeanInitial = 0.0;
        /// <summary>
        /// Standard deviation in log-normal productivity distribution (initial population)
        /// </summary>
        public double HouseholdProductivityLogSigmaInitial = 0.3;
        /// <summary>
        /// Standard deviation in error term in dynamic productivity equation
        /// </summary>
        public double HouseholdProductivityErrorSigma= 0;

        /// <summary>
        /// Initial size of investor firm portefolio
        /// </summary>
        public int InvestorInitialInflow = 0;

        public double InvestorProfitSensitivity = 0;


        public double StatisticsInitialMarketPrice = 1.0;    
        public double StatisticsInitialMarketWage = 1.0;
        /// <summary>
        /// Interest rate usend to calulate discounted profits
        /// </summary>
        public double StatisticsInitialInterestRate = 0.0;

        /// <summary>
        /// The share of firms that is randomly picked to report monthly data 
        /// </summary>
        public double StatisticsFirmReportSampleSize = 0.0;

        /// <summary>
        /// The share of households that is randomly picked to report monthly data 
        /// </summary>
        public double StatisticsHouseholdReportSampleSize = 0.0;

        /// <summary>
        /// If this is x, graphics is plottet every x periods
        /// </summary>
        public int StatisticsGraphicsPlotInterval = 0;

        public int StatisticsGraphicsStartPeriod = 0;


        public int StatisticsOutputPeriode = -1;

        public double StatisticsExpectedSharpeRatioSmooth = 0.0;

        /// <summary>
        /// In this periode all agents are written to a data base
        /// </summary>
        public int StatisticsWritePeriode = -1;


        /// <summary>
        /// Seed for the random generator. Should be positive.
        /// </summary>
        public int RandomSeed = -1;

        public int StartYear = 0;
        public int EndYear = 10;
        public int PeriodsPerYear = 1;
        public int ShockPeriod = -1;
        public int BurnInPeriod1 = -1;
        public int BurnInPeriod2 = -1;
        public double Scale = 1;
        public bool LoadDatabase = false;

        public string RCodeDir = @"..\..\..\R";
        public string ROutputDir = "";
        public string RExe = "";


    }
}
