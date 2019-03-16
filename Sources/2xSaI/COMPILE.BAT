gcc main.cpp 2xSaI.cpp -o 2xSaI.exe -O3 -m486 -lalleg
del *.o

rem Compress the exe file
upx 2xSaI.exe 
