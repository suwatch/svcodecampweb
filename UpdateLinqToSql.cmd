"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sqlmetal.exe" /code:LINQToSQL\svcodecamp.designer.cs /server:. /database:svcodecamp /namespace:CodeCampSV /context:CodeCampDataContext /views

cd CodeGenerator\bin\debug
CodeGenerator.exe -Verbose  -Server=.  -User=sa -Password=Zebra99   -TableToProcess="" -SqlServerCatalogName="svcodecamp"   -AccessNameSpace="CodeCampSV" -EntityNameSpace="CodeCampSV" -OverWriteFilesInAutoGen   -DataContextPrefix="CodeCamp"  -BaseDirectory="../../../DataAccess/ORM/"  -FileExclusionList="../../ExcludeFiles.txt"

cd ..
cd ..
cd ..
pause
