MSBuild.exe decode-time.sln /p:Configuration=Debug /p:Platform="Any CPU"
if errorlevel 1 goto :eof
copy test.xml test1.xml
bin\Debug\decode-time test1.xml
type test1.xml
