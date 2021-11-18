UnityDirectory := /Applications/Unity/Hub/Editor/2020.3.4f1/Unity.app
UnityCommand := $(UnityDirectory)/Contents/MacOS/Unity

build_webgl:
	$(UnityCommand) \
	  -batchmode \
	  -nographics \
	  -quit \
	  -projectPath $(PWD) \
	  -logFile - \
	  -executeMethod Build.WebglBuild
