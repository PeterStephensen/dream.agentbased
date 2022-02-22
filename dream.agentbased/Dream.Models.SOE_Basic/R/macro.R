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

vert_lin = function(t)
{
  for(v in seq(from=0, to=max(t), by=10))
    abline(v=v, col="gray")
}

pplot = function(t,x, main="", s_miny=0, s_maxy=1.2)
{
  plot(t, x, type="l", main=main, ylim=max(x)*c(s_miny,s_maxy), ylab="", xlab="Year")
  abline(h=0)
  vert_lin(t)
  lines(t, x)
  
}

#pplot(d$Time, d$nFirms)

#s = spectrum(d$Employment, log="no")
#x = s$freq
#y = s$spec
#plot(x,y, type="l")
#abline(h=0)

pdf(paste0(o_dir, "\\macro.pdf"))

pplot(d$Time/12, d$nFirms, main="Number of firms")

pplot(d$Time/12, d$nFirmClosed, main="New (red) and Closed firms")
lines(d$Time/12, d$nFirmNew, col="red", lwd=2)

plot(d$Time/12, d$expSharpeRatio, type="l", main="Sharpe Ratio", xlab="Year", ylab="", ylim=c(-0.2, 0.2))
abline(h=0)
vert_lin(d$Time/12)


#pplot(d$Time/12, d$Production/(d$nLaborSupply - d$nUnemployed), main="Productivity per head")

#pplot(d$Time/12, d$Production/d$Employment, main="Productivity per productivity unit")

pplot(d$Time/12, d$Production/d$nFirms, main="Productivity per firm")
lines(d$Time/12, 6+10*d$expSharpeRatio, lty=2)


pplot(d$Time/12, d$Sales, main="Sales")
#pplot(d_yr$Year, d_yr$Sales, main="Sales (Yearly)")

pplot(d$Time/12, d$Employment, main="Employment")
#pplot(d_yr$Year, d_yr$Employment, main="Employment (Yearly)")

pplot(d$Time/12, d$nUnemployed/d$nLaborSupply, main="Unemployment rate", s_maxy = 1.5)
lines(d$Time/12, 0.03-0.2*d$expSharpeRatio, lty=2)
abline(h=0.03, lty=2)

pplot(d$Time/12, d$marketWage/d$marketPrice, main="Real wage")
#pplot(d_yr$Year, d_yr$Wage/d_yr$Price, main="Real wage (Yearly)")

pplot(d$Time/12, d$marketPrice, main="Price")
#pplot(d_yr$Year, d_yr$Price, main="Price (Yearly)")

pplot(d$Time/12, d$marketWage, main="Wage")
#pplot(d_yr$Year, d_yr$Wage, main="Wage (Yearly)")

pplot(d$Time/12, d$nHouseholds, main="Number of households (Red: In labor force)", s_miny = 0.5)
lines(d$Time/12, d$nLaborSupply, col="red")

dev.off()







