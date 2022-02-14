rm(list=ls())
library(dplyr)

#install.packages("ContourFunctions")


if(Sys.info()['nodename'] == "C1709161")    # PSP's machine
{
  o_dir = "C:/test/Dream.AgentBased.MacroModel"  
}
if(Sys.info()['nodename'] == "VDI00316")    # Fjernskrivebord
{
  o_dir = "C:/Users/B007566/Documents/Output"  
}

d_report = read.delim(paste0(o_dir,"/household_reports.txt"))

d_report = d_report %>% arrange(ID)
ids=unique(d_report$ID)
n = length(ids)


pdf(paste0(o_dir,"/household_reports.pdf"))


cols=palette()

#ddd = d_report %>% filter(ID==1345)

dec = function(x,n=3)
{
  z = 10^n
  round(z*x)/z
}


par(mfrow=c(2,1))

for(i in 1:n)
{
  #i=36
  dr = d_report %>% filter(ID==ids[i])
  

  mx = max(dr$Productivity)
  plot(dr$Age/12, dr$Productivity, type="l", xlab="Age", ylab="Productivity", ylim=c(0,1.1*mx))
  abline(h=0)
  
  mx = max(dr$Consumption)
  mn = min(dr$Consumption)
  plot(dr$Age/12, dr$Consumption, type="l", xlab="Age", ylab="Consumption", ylim=c(mn, 1.1*mx))
  abline(h=0)
  
}

dev.off()
