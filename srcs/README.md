# 構成図

- BuildTools
	- AssemblyVersionUp
		- AssemblyVersionUp
		- AssemblyVersionUp.sln
	- DllCollector
		- DllCollector
		- DllCollector.sln
- net35
	- ObjectVisualization
	- ObjectVisualization.sln
- net40
	- ObjectVisualization
	- ObjectVisualization.sln
- AssemblyVersionUp.exe
- build.bat
- DllCollector.exe
- README.md

# BuildTools

AssemblyVersionUp.exe + DllCollector.exe のソースコードです。

# build.bat + AssemblyVersionUp.exe + DllCollector.exe

コマンドライン上で、アセンブリバージョンのアップ、ビルド、dll回収、をするためのプログラムです。

# net35 + net40

本ライブラリのソースコード。環境が無くてもビルドできるようにソリューション単位で、.NET Framework 3.5、4 系向けに分かれています。
基本的には、同一ソースの2重管理で運用します。
