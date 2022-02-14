using System;

namespace Dream.Models.SOE_Basic
{
    class Program
    {
        static void Main(string[] args)
        {

            Settings settings = new Settings();

            //Firms
            settings.NumberOfFirms = 300*1;

            settings.FirmParetoMinPhi = 0.5; 
            settings.FirmPareto_k = 2.5;  // k * (1 - alpha) > 1     

            settings.FirmAlpha = 0.5;
            settings.FirmFi = 2;
            

            //-----
            settings.FirmWageMarkup = 0.05; 
            settings.FirmWageMarkupSensitivity = 2.0;
            settings.FirmWageMarkdown = 0.07;
            settings.FirmWageMarkdownSensitivity = 2.0;

            settings.FirmWageMarkupInZone = 0.05; //0.0----------------
            settings.FirmWageMarkupSensitivityInZone = 2.0;
            settings.FirmWageMarkdownInZone = 0.07; 
            settings.FirmWageMarkdownSensitivityInZone = 2.0;

            settings.FirmProbabilityRecalculateWage = 0.8;
            
            //-----
            settings.FirmPriceMarkup = 0.08;
            settings.FirmPriceMarkupSensitivity = 2.0;
            settings.FirmPriceMarkdown = 0.05;  
            settings.FirmPriceMarkdownSensitivity = 5.0; //2.0

            settings.FirmPriceMarkupInZone = 0.08;  //0.0---------------------------
            settings.FirmPriceMarkupSensitivityInZone = 1.0;
            settings.FirmPriceMarkdownInZone = 0.05;  //0.0
            settings.FirmPriceMarkdownSensitivityInZone = 2.0; 

            settings.FirmPriceMechanismStart = 12 * 1;
            settings.FirmProbabilityRecalculatePrice = 0.8;

            //-----
            settings.FirmComfortZoneEmployment = 0.05;
            settings.FirmComfortZoneSales = 0.05;

            //-----
            settings.FirmDefaultProbabilityNegativeProfit = 0.1;
            settings.FirmDefaultStart = 12*5;
            settings.FirmNegativeProfitOkAge = 12*2;

            settings.FirmExpectationSmooth = 0.4;
            settings.FirmMaxEmployment = 700;

            settings.FirmProfitLimitZeroPeriod = (2040 - 2014) * 12;

            //settings.FirmProductivityGrowth = 0.02;

            // Households
            settings.NumberOfHouseholdsPerFirm = 10000/300;
            settings.HouseholdNumberFirmsSearchJob = 5;     //(int)(0.05 * settings.NumberOfFirms); // Vigtig for unemplDuration
            settings.HouseholdNumberFirmsSearchShop = 10;    // (int)(0.05 * settings.NumberOfFirms);
            settings.HouseholdProbabilityQuitJob = 0.01;   
            settings.HouseholdProbabilitySearchForJob = 0.01;
            settings.HouseholdProbabilitySearchForShop = 0.01;//*************************
            settings.HouseholdProductivityLogSigmaInitial = 0.6;
            settings.HouseholdProductivityLogMeanInitial = -0.5 * Math.Pow(settings.HouseholdProductivityLogSigmaInitial, 2); // Sikrer at forventet produktivitet er 1
            settings.HouseholdProductivityErrorSigma = 0.02;

            
            settings.HouseholdPensionAge = 67 * 12;
            settings.HouseholdStartAge = 18 * 12;

            // Statistics
            settings.StatisticsInitialMarketPrice = 2.0;
            settings.StatisticsInitialInterestRate = Math.Pow(1 + 0.05, 1.0 / 12) - 1; // 5% p.a.

            settings.StatisticsFirmReportSampleSize = 0.15;
            settings.StatisticsExpectedSharpeRatioSmooth = 0.7;

            // R-stuff
            if (Environment.MachineName == "C1709161") // PSP's maskine
            {
                settings.ROutputDir = @"C:\test\Dream.AgentBased.MacroModel";
                settings.RExe = @"C:\Program Files\R\R-4.0.3\bin\x64\R.exe";
            }
            
            if (Environment.MachineName == "VDI00316") // Fjernskrivebord
            {
                settings.ROutputDir = @"C:\Users\B007566\Documents\Output";
                settings.RExe = @"C:\Users\B007566\Documents\R\R-4.1.2\bin\x64\R.exe";
            }
                      
            // Time and randomseed           
            settings.StartYear = 2014;
            settings.EndYear = 2377;
            settings.PeriodsPerYear = 12;

            settings.StatisticsOutputPeriode = (2075 - 2014) * 12;
            settings.StatisticsGraphicsPlotInterval = 12 * 1;

            //settings.ShockPeriod = (2100 - 2014) * 12;


            //settings.RandomSeed = 123;
            //settings.FirmNumberOfNewFirms = 1;

            settings.BurnInPeriod1 = (2035 - 2014) * 12;
            settings.BurnInPeriod2 = (2050 - 2014) * 12;
            settings.StatisticsWritePeriode = (2075 - 2014) * 12;

            //settings.BurnInPeriod1 = 1;
            ////settings.BurnInPeriod2 = 112 * 5;
            //settings.FirmProfitLimitZeroPeriod = 1;
            //settings.FirmDefaultStart = 1;
            //settings.LoadDatabase = true;

            var t0 = DateTime.Now;
                        
            // Run the simulation
            new Simulation(settings, new Time(0, (1 + settings.EndYear - settings.StartYear) * settings.PeriodsPerYear - 1));

            Console.WriteLine(DateTime.Now - t0);
            //Console.ReadLine();

        }
    }
}
