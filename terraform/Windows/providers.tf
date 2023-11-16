# Azurerm Providers
provider "azurerm" {
  features {}
  subscription_id = data.vault_generic_secret.vault-spoke-spn.data["subscription_id"]
  tenant_id       = data.vault_generic_secret.vault-spoke-spn.data["tenant_id"]
  client_id       = data.vault_generic_secret.vault-spoke-spn.data["client_id"]
  client_secret   = data.vault_generic_secret.vault-spoke-spn.data["client_secret"]
}
provider "azurerm" {
  features {}
  alias           = "south-hub"
  client_id       = data.vault_generic_secret.vault-south-hub-spn.data["client_id"]
  client_secret   = data.vault_generic_secret.vault-south-hub-spn.data["client_secret"]
  tenant_id       = data.vault_generic_secret.vault-south-hub-spn.data["tenant_id"]
  subscription_id = data.vault_generic_secret.vault-south-hub-spn.data["subscription_id"]
}
provider "azurerm" {
  features {}
  alias           = "north-hub"
  client_id       = data.vault_generic_secret.vault-north-hub-spn.data["client_id"]
  client_secret   = data.vault_generic_secret.vault-north-hub-spn.data["client_secret"]
  tenant_id       = data.vault_generic_secret.vault-north-hub-spn.data["tenant_id"]
  subscription_id = data.vault_generic_secret.vault-north-hub-spn.data["subscription_id"]
}

# Azuread Providers
provider "azuread" {
  # Whilst version is optional, we /strongly recommend/ using it to pin the version of the Provider to be used
  tenant_id     = data.vault_generic_secret.vault-aad-readonly.data["tenant_id"]
  client_id     = data.vault_generic_secret.vault-aad-readonly.data["client_id"]
  client_secret = data.vault_generic_secret.vault-aad-readonly.data["client_secret"]
}

# Hashi Vault Providers
provider "vault" {
  address         = "https://vault.cicd.canadalife.bz"
  token           = var.VAULT_TOKEN
  skip_tls_verify = "true"
}

