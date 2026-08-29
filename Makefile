.PHONY: dll clean install

CONFIGURATION ?= Release
PROJECT := SceneEffectsPresetsPatch.csproj
DOTNET_ARGS := -c $(CONFIGURATION)
ifneq ($(KKS_PATH),)
DOTNET_ARGS += '/p:KKSPath=$(KKS_PATH)'
endif
KKS_PLUGINS_DIR ?= C:/illusion/kks/BepInEx/plugins/00_Add
DLL := bin/$(CONFIGURATION)/SceneEffectsPresetsPatch.dll

dll:
	dotnet build $(DOTNET_ARGS) $(PROJECT)

install: dll
	mkdir -p "$(KKS_PLUGINS_DIR)"
	cp "$(DLL)" "$(KKS_PLUGINS_DIR)/"

clean:
	dotnet clean -c $(CONFIGURATION) $(PROJECT)
	rm -rf bin obj
