@echo off
cd /d %~dp0
set PATH="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE";%~dp0;%PATH%
set net35=net35\ObjectVisualization.sln
set net40=net40\ObjectVisualization.sln


echo �S�\�����[�V�����̃A�Z���u���o�[�W�����̃A�b�v
AssemblyVersionUp.exe /build

echo �\�����[�V�����ʂ̃r���h
devenv %net35% /clean
devenv %net35% /rebuild debug
if errorlevel 1 pause

devenv %net40% /clean
devenv %net40% /rebuild debug
if errorlevel 1 pause

echo �\�����[�V�����ʂ�dll���
DllCollector.exe


echo Please input any keys...

