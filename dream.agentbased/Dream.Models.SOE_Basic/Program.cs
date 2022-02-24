using System;

namespace Dream.Models.SOE_Basic
{
    class Program
    {
        static void Main(string[] args)
        {

            Settings settings = new();

            double scale = 1*1.0; // Scale the model up and down
            
            //Firms
            settings.NumberOfFirms = (int)(300 * scale);

            settings.FirmParetoMinPhi = 0.5; 
            settings.FirmPareto_k = 2.5;  // k * (1 - alpha) > 1     

            settings.FirmAlpha = 0.5;
            settings.FirmFi = 2;

            //-----
            double mark = 0.08; // SE HER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            double sens = 0.5;

            settings.FirmWageMarkup = 2 * mark; 
            settings.FirmWageMarkupSensitivity = sens;
            settings.FirmWageMarkdown = 2 * mark;  
            settings.FirmWageMarkdownSensitivity = sens;

            settings.FirmWageMarkupInZone = 1 * mark; 
            settings.FirmWageMarkupSensitivityInZone = sens;
            settings.FirmWageMarkdownInZone = 1 * mark;  
            settings.FirmWageMarkdownSensitivityInZone = sens;

            settings.FirmProbabilityRecalculateWage = 0.5; 
            settings.FirmProbabilityRecalculateWageInZone = 0.2; 

            //-----
            settings.FirmPriceMarkup = 2 * mark;  
            settings.FirmPriceMarkupSensitivity = sens; 
            settings.FirmPriceMarkdown = 0.5 * mark;             //Stable prices  
            settings.FirmPriceMarkdownSensitivity = 0.5 * sens;  //Stable prices 

            settings.FirmPriceMarkupInZone = mark;  
            settings.FirmPriceMarkupSensitivityInZone = sens;  
            settings.FirmPriceMarkdownInZone = 0.25* mark;                //Stable prices  
            settings.FirmPriceMarkdownSensitivityInZone = 0.25 * sens;    //Stable prices 

            settings.FirmPriceMechanismStart = 12 * 1;
            
            settings.FirmProbabilityRecalculatePrice = 0.5; 
            settings.FirmProbabilityRecalculatePriceInZone = 0.2; 

            //-----
            settings.FirmComfortZoneEmployment = 0.15;
            settings.FirmComfortZoneSales = 0.15;

            //-----
            settings.FirmDefaultProbabilityNegativeProfit = 0.1;
            settings.FirmDefaultStart = 12*5;
            settings.FirmNegativeProfitOkAge = 12*2;

            settings.FirmExpectationSmooth = 0.4;
            settings.FirmMaxEmployment = 700;

            settings.FirmVacanciesShare = 0.1;
            settings.FirmMinRemainingVacancies = 5;

            settings.FirmProfitLimitZeroPeriod = (2040 - 2014) * 12;

            //settings.FirmProductivityGrowth = 0.02;

            // Households
            settings.NumberOfHouseholdsPerFirm = 10000/300;
            settings.HouseholdNumberFirmsSearchJob = 4;     // Try 20!
            settings.HouseholdNumberFirmsSearchShop = 75;    //----------------------- 
            settings.HouseholdProbabilityQuitJob = 0.01;   
            settings.HouseholdProbabilitySearchForJob = 0.01;
            settings.HouseholdProbabilitySearchForShop = 0.01;
            settings.HouseholdProductivityLogSigmaInitial = 0.6;
            settings.HouseholdProductivityLogMeanInitial = -0.5 * Math.Pow(settings.HouseholdProductivityLogSigmaInitial, 2); // Sikrer at forventet produktivitet er 1
            settings.HouseholdProductivityErrorSigma = 0.02;
            settings.HouseholdNewBorn = (int)(15 * scale);
            
            settings.HouseholdPensionAge = 67 * 12;
            settings.HouseholdStartAge = 18 * 12;

            // Investor
            settings.InvestorInitialInflow = (int)(17 * scale);
            settings.InvestorProfitSensitivity = 5.0;   // Try 30 !!!!!!            
            
            // Statistics
            settings.StatisticsInitialMarketPrice = 2.0;
            settings.StatisticsInitialInterestRate = Math.Pow(1 + 0.05, 1.0 / 12) - 1; // 5% p.a.

            settings.StatisticsFirmReportSampleSize = 0.15;
            settings.StatisticsHouseholdReportSampleSize = 0.02;
            
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
            settings.EndYear = 2160;
            settings.PeriodsPerYear = 12;

            settings.StatisticsOutputPeriode = (2075 - 2014) * 12;
            settings.StatisticsGraphicsPlotInterval = 12 * 1;
            settings.StatisticsGraphicsStartPeriod = 12*150;   // SE HER !!!!!!!!!!!!!!!!!!!!!!!!!!!!
            
            if(args.Length == 1)
            {
                //settings.Shock = EShock.Tsunami;
                settings.Shock = (EShock)Int32.Parse(args[0]);
                settings.ShockPeriod = (2100 - 2014) * 12;
            }

            //settings.SaveScenario = true;

            //settings.RandomSeed = 123;
            //settings.FirmNumberOfNewFirms = 1;

            settings.BurnInPeriod1 = (2035 - 2014) * 12;
            settings.BurnInPeriod2 = (2050 - 2014) * 12;
            settings.StatisticsWritePeriode = (2075 - 2014) * 12;

            // !!!!! Remember some settings are changed in Simulation after BurnIn1 !!!!!

            //settings.BurnInPeriod1 = 1;
            ////settings.BurnInPeriod2 = 112 * 5;
            //settings.FirmProfitLimitZeroPeriod = 1;
            //settings.FirmDefaultStart = 1;
            //settings.LoadDatabase = true;

            var t0 = DateTime.Now;
                        
            // Run the simulation
            new Simulation(settings, new Time(0, (1 + settings.EndYear - settings.StartYear) * settings.PeriodsPerYear - 1));

            Console.Write("\n");
            Console.WriteLine(DateTime.Now - t0);
            //Console.ReadLine();

        }
    }
}
