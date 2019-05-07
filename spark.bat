@echo off
rem find the amps_spark.jar
if not exist "%~DP0\dist\lib\amps_spark.jar" goto errorcurdir
rem Locate a Java Runtime.  Use JAVA_HOME over PATH if possible
if not exist "%JAVA_HOME%\bin\java.exe" goto usepath
"%JAVA_HOME%\bin\java" -jar dist\lib\amps_spark.jar %*
if %ERRORLEVEL% EQU 0 goto done
if %ERRORLEVEL% EQU 9009 goto errorJava
goto unexpectedError
echo ** Warning: Couldn't find java in %JAVA_HOME%\bin -- trying PATH
:usepath
java -jar "%~DP0\dist\lib\amps_spark.jar" %*
if %ERRORLEVEL% EQU 0 goto done
if %ERRORLEVEL% EQU 9009 goto errorJava
echo ERROR: An unexpected error occured inside Spark.  
echo        An internal error message describing the problem should
echo        be displayed above.
exit /b 1
:errorcurdir
echo ERROR: Cannot find dist\lib\amps_spark.jar.  Please run spark from the directory containing spark.bat.
exit /b 1
:errorJava
echo ERROR: Couldn't find java executable.  
echo        Set JAVA_HOME or include java.exe in your path. Exiting.
:unexpectedError
exit /b 1
:done

