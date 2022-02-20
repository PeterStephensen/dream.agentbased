echo off

for /l %%x in (1 1 40) do (
	echo %%x
	..\bin\Debug\net6.0\Dream.Models.SOE_Basic
	..\bin\Debug\net6.0\Dream.Models.SOE_Basic 1
	..\bin\Debug\net6.0\Dream.Models.SOE_Basic 3
)
