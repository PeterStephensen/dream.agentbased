rm(list=ls())
library(dplyr)
#library(forecast)

hpfilter      = function(x, mu = 100) {
  y = x
  n <- length(y)          # number of observations
  I <- diag(n)            # creates an identity matrix
  D <- diff(I,lag=1,d=2)  # second order differences
  d <- solve(I + mu * crossprod(D) , y) # solves focs
  d
}


if(Sys.info()['nodename'] == "C1709161")    # PSP's machine
{
  o_dir = "C:/test/Dream.AgentBased.MacroModel"  
}
if(Sys.info()['nodename'] == "VDI00316")    # Fjernskrivebord
{
  o_dir = "C:/Users/B007566/Documents/Output"  
}


d = read.delim(paste0(o_dir, "\\macro.txt"))

yr0 = 12*(2100-2014)-1
d = d %>% filter(Time>yr0)
d$Time = d$Time - yr0 
d$Year = floor(d$Time/12)

d_yr = d %>% group_by(Year) %>%
       summarise(Sales=sum(Sales), Employment=sum(Employment), Price=mean(marketPrice), 
                 Wage=mean(marketWage))
maxyr = max(d_yr$Year)
d_yr = d_yr[d_yr$Year<maxyr & d_yr$Year>0,]


pplot = function(t,x, main)
{
  plot(t, x, type="l", main=main, ylim=c(0,max(x)), ylab="", xlab="Year")
  abline(h=0)  
}

pdf(paste0(o_dir, "\\macro.pdf"))

pplot(d$Time/12, d$nFirms, main="Number of firms")

plot(d$Time/12, d$expSharpeRatio, type="l", main="Sharpe Ratio", xlab="Year", ylab="", ylim=c(-0.2, 0.2))
abline(h=0)

pplot(d$Time/12, d$marketWage/d$marketPrice, main="Real wage")
pplot(d_yr$Year, d_yr$Wage/d_yr$Price, main="Real wage (Yearly)")

pplot(d$Time/12, d$Sales, main="Sales")
pplot(d_yr$Year, d_yr$Sales, main="Sales (Yearly)")

pplot(d$Time/12, d$Employment, main="Employment")
pplot(d_yr$Year, d_yr$Employment, main="Employment (Yearly)")

pplot(d$Time/12, d$marketPrice, main="Price")
pplot(d_yr$Year, d_yr$Price, main="Price (Yearly)")

pplot(d$Time/12, d$marketWage, main="Wage")
pplot(d_yr$Year, d_yr$Wage, main="Wage (Yearly)")

dev.off()



