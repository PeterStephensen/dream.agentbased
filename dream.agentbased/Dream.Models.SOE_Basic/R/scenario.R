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

hpfilter      = function(x, mu = 100) {
  y = x
  n <- length(y)          # number of observations
  I <- diag(n)            # creates an identity matrix
  D <- diff(I,lag=1,d=2)  # second order differences
  d <- solve(I + mu * crossprod(D) , y) # solves focs
  d
}


#-------------------------
dd = d[is.na(d$marketWage),]
b_ids=unique(dd$Scenario)

d = d %>% filter(!(Scenario %in% b_ids)) 

d$Employment = as.numeric(d$Employment)

#-------------------------

# SPECIELT!!!!!!!!!!!!!!!!!!!!!!!!!!!
#-----------------------------------
#d$Employment = as.numeric(d$Employment)
#d = d[!is.na(d$Employment),]
#d = d %>% filter(Scenario!=101) 
#-----------------------------------



db = d %>% filter(Run=="Base") 

#----------------------------------------------------

#bcol = rgb(0.5,0.8,0.5)
bcol = rgb(0.9,0.9,0.9)

lcol = rgb(0.3,0.5,0.99)
lwd = 2.0

lo = function(x)
{
  z = sort(x)
  mean(z[1:3]
  )}

up = function(x)
{
  z = sort(x, decreasing = T)
  mean(z[1:3])
}


pplot = function(zz, sMain, ylab="Relative change", s_ylim=c(1,1), abs_ylim=c(0,0))
{
  
  col = rgb(0.5,0.7,0.7)
  mx=max(zz$up)
  mn=min(zz$lo)
  if(abs_ylim==c(0,0))
  {
    ylim=c(s_ylim[1]*mn,s_ylim[2]*mx)
  }else
  {
    ylim=abs_ylim
  }
  
  plot(zz$Time/12, (zz$up), type="l", col=bcol, ylim=ylim, xlab="Year", cex.axis=0.8, 
       ylab=ylab, main=paste(sMain," - ",ss[shk], "shock"), cex.main=0.8)
  box(col=col)
  axis(1, col=col, col.ticks=col, cex.axis=0.8)
  axis(2, col=col, col.ticks=col, cex.axis=0.8)
  lines(zz$Time/12, zz$lo, col=bcol)
  polygon(c(zz$Time/12, rev(zz$Time/12)), c(zz$lo, rev(zz$up)), col=bcol, lty=0)
  abline(h=0, col=col, lty=2)
  lines(zz$Time/12, zz$mean, lwd=lwd, col=lcol)
  
}

vert_lin = function(t)
{
  for(v in seq(from=0, to=max(t), by=10))
    abline(v=v, col="gray")
}

pplot2 = function(t,x, main="", s_miny=0, s_maxy=1.2)
{
  plot(t, x, type="l", main=main, ylim=max(x)*c(s_miny,s_maxy), ylab="", xlab="Year")
  abline(h=0)
  vert_lin(t)
  lines(t, x)
  
}

#----------------------------------------------------
pdf(paste0(o_dir, "/base.pdf"))

shk = 1
#r_col = rgb(1,0.3,0.3)
r_col = rgb(0.9,0.9,0.2)

#------------------------

d4 = db %>% filter(Scenario==ids[1])
plot(d4$Time/12, d4$expSharpeRatio, type="l", main="Sharpe Ratio", xlab="Year", ylab="", ylim=c(-0.1, 0.1))
abline(h=0)
vert_lin(d4$Time/12)


zz = db %>% group_by(Time) %>%
  dplyr::summarize(mean=mean(expSharpeRatio), up=up(expSharpeRatio), lo=lo(expSharpeRatio))

pplot(zz, "Sharpe Ratio", ylab = "", abs_ylim = c(-0.1, 0.1))
lines(d4$Time/12, d4$expSharpeRatio)

#lines(dd2$Time/12, dd2$expSharpeRatio, lwd=2, col=r_col)
#lines(dd2$Time/12, dd2$expSharpeRatio, lwd=0.5, col=rgb(0.7,0.7,0.7))

#------------------------

pplot2(d4$Time/12, d4$nFirms, main="Number of firms")


zz = db %>% group_by(Time) %>%
  dplyr::summarize(mean=mean(nFirms), up=up(nFirms), lo=lo(nFirms))

pplot(zz, "Number of firms", ylab = "", s_ylim = c(0.9, 1.1))
#lines(dd2$Time/12, dd2$nFirms, lwd=2, col=r_col)
#lines(dd2$Time/12, dd2$nFirms, lwd=0.5, col=rgb(0.7,0.7,0.7))

#------------------------

zz = db %>% group_by(Time) %>%
  dplyr::summarize(mean=mean(Sales), up=up(Sales), lo=lo(Sales))

pplot(zz, "Sales", ylab = "", s_ylim = c(0.9, 1.1))
#lines(dd2$Time/12, dd2$Sales, lwd=2, col=r_col)
#lines(dd2$Time/12, dd2$Sales, lwd=0.5, col=rgb(0.7,0.7,0.7))

#------------------------

zz = db %>% group_by(Time) %>%
  dplyr::summarize(mean=mean(Employment), up=up(Employment), lo=lo(Employment))

pplot(zz, "Employment", ylab = "", s_ylim = c(0.95, 1.05))
#lines(dd2$Time/12, dd2$Employment, lwd=2, col=r_col)
#lines(dd2$Time/12, dd2$Employment, lwd=0.5, col=rgb(0.7,0.7,0.7))

#------------------------

zz = db %>% group_by(Time) %>%
  dplyr::summarize(mean=mean(marketWage/marketPrice), up=up(marketWage/marketPrice), lo=lo(marketWage/marketPrice))

pplot(zz, "Real wage", ylab = "", s_ylim = c(0.9, 1.1))
#lines(dd2$Time/12, dd2$marketWage / dd2$marketPrice, lwd=2, col=r_col)
#lines(dd2$Time/12, dd2$marketWage / dd2$marketPrice, lwd=0.5, col=rgb(0.7,0.7,0.7))

#------------------------

zz = db %>% group_by(Time) %>%
  dplyr::summarize(mean=mean(marketPrice), up=up(marketPrice), lo=lo(marketPrice))

pplot(zz, "Price", ylab = "", s_ylim = c(0.9, 1.1))
#lines(dd2$Time/12, dd2$marketPrice, lwd=2, col=r_col)
#lines(dd2$Time/12, dd2$marketPrice, lwd=0.5, col=rgb(0.7,0.7,0.7))

#------------------------

zz = db %>% group_by(Time) %>%
  dplyr::summarize(mean=mean(marketWage), up=up(marketWage), lo=lo(marketWage))

pplot(zz, "Wage", ylab = "", s_ylim = c(0.9, 1.1))
#lines(dd2$Time/12, dd2$marketWage, lwd=2, col=r_col)
#lines(dd2$Time/12, dd2$marketWage, lwd=0.5, col=rgb(0.7,0.7,0.7))

#------------------------

dev.off()


#----------------------------------------------------
pdf(paste0(o_dir, "/base_rel.pdf"))

shk = 1
#r_col = rgb(1,0.3,0.3)
r_col = rgb(0.9,0.9,0.2)


#------------------------

zz = db %>% group_by(Time) %>%
  dplyr::summarize(mean=mean(expSharpeRatio), up=up(expSharpeRatio)-mean, lo=lo(expSharpeRatio)-mean, mean=0)

pplot(zz, "Sharpe Ratio", ylab = "", s_ylim = c(1.6, 1.6))
#lines(dd2$Time/12, dd2$expSharpeRatio, lwd=2, col=r_col)
#lines(dd2$Time/12, dd2$expSharpeRatio, lwd=0.5, col=rgb(0.7,0.7,0.7))

#------------------------

zz = db %>% group_by(Time) %>%
  dplyr::summarize(mean=mean(nFirms), up=up(nFirms)/mean, lo=lo(nFirms)/mean, mean=1)

pplot(zz, "Number of firms", ylab = "", s_ylim = c(0.9, 1.1))
#lines(dd2$Time/12, dd2$nFirms, lwd=2, col=r_col)
#lines(dd2$Time/12, dd2$nFirms, lwd=0.5, col=rgb(0.7,0.7,0.7))

#------------------------

zz = db %>% group_by(Time) %>%
  dplyr::summarize(mean=mean(Sales), up=up(Sales)/mean, lo=lo(Sales)/mean, mean=1)

pplot(zz, "Sales", ylab = "", s_ylim = c(0.9, 1.1))
#lines(dd2$Time/12, dd2$Sales, lwd=2, col=r_col)
#lines(dd2$Time/12, dd2$Sales, lwd=0.5, col=rgb(0.7,0.7,0.7))

#------------------------

zz = db %>% group_by(Time) %>%
  dplyr::summarize(mean=mean(Employment), up=up(Employment)/mean, lo=lo(Employment)/mean, mean=1)

pplot(zz, "Employment", ylab = "", s_ylim = c(0.95, 1.05))
#lines(dd2$Time/12, dd2$Employment, lwd=2, col=r_col)
#lines(dd2$Time/12, dd2$Employment, lwd=0.5, col=rgb(0.7,0.7,0.7))

#------------------------

zz = db %>% group_by(Time) %>%
  dplyr::summarize(mean=mean(marketWage/marketPrice), up=up(marketWage/marketPrice)/mean, lo=lo(marketWage/marketPrice)/mean, mean=1)

pplot(zz, "Real wage", ylab = "", s_ylim = c(0.9, 1.1))
#lines(dd2$Time/12, dd2$marketWage / dd2$marketPrice, lwd=2, col=r_col)
#lines(dd2$Time/12, dd2$marketWage / dd2$marketPrice, lwd=0.5, col=rgb(0.7,0.7,0.7))

#------------------------

zz = db %>% group_by(Time) %>%
  dplyr::summarize(mean=mean(marketPrice), up=up(marketPrice)/mean, lo=lo(marketPrice)/mean, mean=1)

pplot(zz, "Price", ylab = "", s_ylim = c(0.9, 1.1))
#lines(dd2$Time/12, dd2$marketPrice, lwd=2, col=r_col)
#lines(dd2$Time/12, dd2$marketPrice, lwd=0.5, col=rgb(0.7,0.7,0.7))

#------------------------

zz = db %>% group_by(Time) %>%
  dplyr::summarize(mean=mean(marketWage), up=up(marketWage)/mean, lo=lo(marketWage)/mean, mean=1)

pplot(zz, "Wage", ylab = "", s_ylim = c(0.9, 1.1))
#lines(dd2$Time/12, dd2$marketWage, lwd=2, col=r_col)
#lines(dd2$Time/12, dd2$marketWage, lwd=0.5, col=rgb(0.7,0.7,0.7))

#------------------------

dev.off()

#----------------------------------------------------
pdf(paste0(o_dir, "/shocks.pdf"))

par(mfrow=c(2,2))


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




















