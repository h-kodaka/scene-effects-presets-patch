.PHONY: dll clean

CONFIGURATION ?= Release
PROJECT := SceneEffectsPresetsWindowMemory.csproj
DOTNET_ARGS := -c $(CONFIGURATION)
ifneq ($(KKS_PATH),)
DOTNET_ARGS += /p:KKSPath="$(KKS_PATH)"
endif

dll:
	dotnet build $(DOTNET_ARGS) $(PROJECT)

clean:
	dotnet clean -c $(CONFIGURATION) $(PROJECT)
	rm -rf bin obj
