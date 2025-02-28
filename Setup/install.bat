@echo off
xcopy "SimpleTool.addin" "C:\ProgramData\Autodesk\Revit\Addins\2024" /Y
xcopy "SimpleTool.dll" "C:\ProgramData\Autodesk\Revit\Addins\2024" /Y
echo Files copied successfully!
pause