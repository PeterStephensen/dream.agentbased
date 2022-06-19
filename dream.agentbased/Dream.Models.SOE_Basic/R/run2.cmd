echo off

for /l %%i in (1 1 11) do (
	for /l %%x in (1 1 4) do (
		echo %%i %%x
		start ..\bin\Debug\net6.0\Dream.Models.SOE_Basic
        	ping 127.0.0.1 -n 4 > nul 
		start ..\bin\Debug\net6.0\Dream.Models.SOE_Basic 1
        	ping 127.0.0.1 -n 4 > nul 
		start ..\bin\Debug\net6.0\Dream.Models.SOE_Basic 2
        	ping 127.0.0.1 -n 4 > nul 
		start ..\bin\Debug\net6.0\Dream.Models.SOE_Basic 3
        	ping 127.0.0.1 -n 4 > nul 
	)
       	ping 127.0.0.1 -n 110 > nul 
)

pause