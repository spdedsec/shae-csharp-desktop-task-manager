# Makefile — shae-csharp-desktop-task-manager
#
# SHAE Task Manager (C# / .NET). Console application.
#
# NOTE: The .NET SDK is not installed in this environment (only the runtime),
# so `make build`, `make test`, and `make release` cannot be run locally.
# See README.md and scripts/install.sh.

.PHONY: build test release clean install help

SRC_DIR  := src
PROJ     := $(SRC_DIR)
RELEASE  := shae-csharp-desktop-task-manager-v0.1.0.zip
DOTNET   ?= dotnet

help: ## Show this help
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-22s\033[0m %s\n", $$1, $$2}'

build: ## Build the project with dotnet
	@if command -v $(DOTNET) >/dev/null 2>&1; then \
		$(DOTNET) build $(PROJ); \
	else \
		echo "dotnet SDK not found on PATH. Skipping build."; \
		echo "Install the .NET SDK to run: make build"; \
		exit 1; \
	fi

test: build ## Build and run (no unit tests; compile is the check)
	@echo "Test OK: project compiles."

release: clean ## Create a release zip of src/
	@rm -f $(RELEASE)
	zip -r $(RELEASE) src scripts Makefile README.md DESIGN.md LICENSE docs
	@echo "Release artifact: $(RELEASE)"

clean: ## Remove build outputs
	rm -rf $(SRC_DIR)/bin $(SRC_DIR)/obj
	rm -f $(RELEASE)

install: ## Notes on installing (see scripts/install.sh)
	@./scripts/install.sh
