rm(list=ls())
library(dplyr)
#library(forecast)


if(Sys.info()['nodename'] == "C1709161")    # PSP's machine
{
  o_dir = "C:/test/Dream.AgentBased.MacroModel"  
}
if(Sys.info()['nodename'] == "VDI00316")    # Fjernskrivebord
{
  o_dir = "C:/Users/B007566/Documents/Output"  
}

files = list.files(paste0(o_dir, "\\Scenarios"), full.names = T)

d = read.delim(files[1])
for(i in 2:length(files))
{
  d = rbind(d, read.delim(files[i]))
}

yr0 = 12*(2100-2014)-1
d = d %>% filter(Time>yr0)
d$Time = d$Time - yr0 

d = d %>% arrange(Scenario)
ids=unique(d$Scenario)
n = length(ids)

ss=unique(d$Run)
n_ss = length(ss)

db = d %>% filter(Run=="Base") 

pdf(paste0(o_dir, "/shocks.pdf"))

par(mfrow=c(2,2))

bcol = rgb(0.5,0.8,0.5)
lcol = rgb(0.3,0.5,0.99)
lwd = 2.0

lo = function(x)
{
  z = sort(x)
  z[1:2]
}

up = function(x)
{
  z = sort(x, decreasing = T)
  z[1:2]
}


pplot = function(zz, sMain)
{
  
  col = rgb(0.5,0.7,0.7)
  mx=max(zz$up)
  mn=min(zz$lo)
  plot(zz$Time/12, zz$up, type="l", col=bcol, ylim=c(mn,mx), xlab="Year", cex.axis=0.8, 
       ylab="Relative change", main=paste(sMain," - ",ss[shk], "shock"), cex.main=0.8)
  box(col=col)
  axis(1, col=col, col.ticks=col, cex.axis=0.8)
  axis(2, col=col, col.ticks=col, cex.axis=0.8)
  lines(zz$Time/12, zz$lo, col=bcol)
  polygon(c(zz$Time/12, rev(zz$Time/12)), c(zz$lo, rev(zz$up)), col=bcol, lty=0)
  abline(h=0, col=col, lty=2)
  lines(zz$Time/12, zz$mean, lwd=lwd, col=lcol)
  
}

for(shk in 2:n_ss)
{
  #shk=4
  
  dc = d %>% filter(Run==ss[shk]) 
  
  dd = merge(dc, db, by=c("Scenario", "Time"))

  dd$dnFirms = dd$nFirms.x / dd$nFirms.y - 1 
  dd$dSales = dd$Sales.x  / dd$Sales.y - 1 
  dd$dEmployment = as.numeric(dd$Employment.x)  / as.numeric(dd$Employment.y) - 1 
  dd$dRealWage = (dd$marketWage.x  / dd$marketPrice.x ) / (dd$marketWage.y / dd$marketPrice.y) - 1 

  dd = dd[dd$dnFirms!=0,] # !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    
  if(F)
  {
    zids=unique(dd$Scenario)
    zn = length(ids)
    
    d2 = dd %>% filter(Scenario==zids[1]) %>% arrange(Time)
    plot(d2$Time/12, d2$dnFirms, type="l")
    for(i in 2:zn)
    {
      d2 = dd %>% filter(Scenario==zids[i]) %>% arrange(Time)
      lines(d2$Time/12, d2$dnFirms)
    }
  }

  
  #-----------------
  
  zz = dd %>% group_by(Time) %>%
    dplyr::summarize(mean=mean(dnFirms), up=up(dnFirms), lo=lo(dnFirms))

  pplot(zz, "Number of firms")

  #-----------------
  zz = dd %>% group_by(Time) %>%
    dplyr::summarize(mean=mean(dSales, na.rm = T), up=up(dSales), lo=lo(dSales))

  pplot(zz, "Sales")
  
  #-----------------
  zz = dd %>% group_by(Time) %>%
    dplyr::summarize(mean=mean(dEmployment, na.rm = T), up=up(dEmployment), lo=lo(dEmployment))
  
  pplot(zz, "Employment")
  
  #-----------------
  zz = dd %>% group_by(Time) %>%
    dplyr::summarize(mean=mean(dRealWage, na.rm = T), up=up(dRealWage), lo=lo(dRealWage))
  
  pplot(zz, "Real wage")
  
  #-----------------
  
}

dev.off()




















