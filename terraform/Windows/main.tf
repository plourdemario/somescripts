module "workload-boot-diagnostic-sa" {

  resource-group-name = var.workload-rg-name
  sa-name             = var.boot-diag-storage-account-name
  spoke-spn           = var.spoke-spn

  # Required tags. Make sure to use a valid Billing-Code.
  tags        = var.tags
  VAULT_TOKEN = var.VAULT_TOKEN

  providers = {
    azurerm           = azurerm
    azurerm.south-hub = azurerm.south-hub
    vault             = vault
  }
}

module "wvd" {

  #Azure Variables
  resource-group-name      = var.workload-rg-name
  spoke-spn                = var.spoke-spn
  vnet-subnet-id           = data.azurerm_subnet.workload-subnet.id
  spoke-vnet-address-space = var.spoke-vnet-address-space
  subnet-address-space     = var.subnet-address-space
  workload-nsg-name        = var.workload-nsg-name

  #Hostpool variables
  hostpool-name                     = var.hostpool-name
  hostpool-type                     = var.hostpool-type
  hostpool-max-sessions             = var.hostpool-max-sessions
  hostpool-location                 = var.hostpool-location
  hostpool-lb-type                  = var.hostpool-lb-type
  hostpool-friendly-name            = var.hostpool-friendly-name
  hostpool-description              = var.hostpool-description
  hostpool-validate-environment     = var.hostpool-validate-environment
  hostpool-rdp-properties           = var.hostpool-rdp-properties
  hostpool-pd-assign-type           = var.hostpool-pd-assign-type
  hostpool-preferred-app-group-type = var.hostpool-preferred-app-group-type

  #VM Variables
  number-of-vms                = var.number-of-vms
  vm-name-count-offset         = var.vm-name-count-offset
  vm-name-prefix               = var.vm-name-prefix
  vm-size                      = var.vm-size
  admin-username               = data.vault_generic_secret.vm-build-id.data["adminuser"]
  admin-password               = data.vault_generic_secret.vm-build-id.data["adminpassword"]
  data-disks                   = var.data-disks
  source-image-reference       = var.source-image-reference
  timezone                     = var.timezone
  os-disk-storage-account-type = var.os-disk-storage-account-type
  os-disk-size-gb              = var.os-disk-size-gb
  enable-automatic-updates     = var.enable-automatic-updates
  plan                         = var.plan
  boot-diagnostics-storage-uri = module.workload-boot-diagnostic-sa.boot-diagnostics-storage-uri

  #Desktop App Group Variables
  create-desktop-app-group  = var.create-desktop-app-group
  dag-user-list             = var.dag-user-list
  dag-group-list            = var.dag-group-list
  dag-name                  = var.dag-name
  dag-friendly-name         = var.dag-friendly-name
  dag-description           = var.dag-description
  dag-ws-name               = var.dag-ws-name
  dag-ws-friendly-name      = var.dag-ws-friendly-name
  dag-ws-description        = var.dag-ws-description
  #rg-permissions-group      = var.rg-permissions-group

  #AD Variables
  vm-domain              = var.vm-domain
  vm-domain-oupath       = var.vm-domain-oupath
  vm-domain-joinuser     = data.vault_generic_secret.domain-join-credential.data[local.domain-join-user-key]
  vm-domain-joinpassword = data.vault_generic_secret.domain-join-credential.data[local.domain-join-pass-key]

  # Required tags. Make sure to use a valid Billing-Code.
  tags        = var.tags
  VAULT_TOKEN = var.VAULT_TOKEN

  providers = {
    azurerm = azurerm
    azuread = azuread
    vault   = vault
  }
}