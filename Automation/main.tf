terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.30.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "tfstate"
    storage_account_name = "tfstatehlsh3"
    container_name       = "tfstate"
    key                  = "terraform.tfstate"
  }
}

locals {
  project_name = "FootballAnalytics"
}

resource "random_string" "suffix" {
  length  = 6
  special = false
  upper   = false
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "${local.project_name}-rg-${random_string.suffix.result}"
  location = "westeurope"
}

resource "azurerm_service_plan" "plan" {
  name                = "${local.project_name}-plan-${random_string.suffix.result}"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku_name            = "F1"
  os_type             = "Windows"
}

resource "azurerm_windows_web_app" "api" {
  name                = "${local.project_name}-api-${random_string.suffix.result}"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_service_plan.plan.location
  service_plan_id     = azurerm_service_plan.plan.id

  site_config {
    application_stack {
      current_stack  = "dotnet"
      dotnet_version = "v6.0"
    }
    always_on = false
  }
}

output "website_address" {
  value = "https://${azurerm_windows_web_app.api.default_hostname}/"
}