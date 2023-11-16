# Azure Generic Settings
environment-flag     = "--env--"
spoke-name           = "--spoke-name--"
spoke-spn            = "CL-CT-DAC-PRD-SPN"
subnet-address-space = "10.56.56.0/22"
workload-name        = "az-vdi-chinww---true-env--"

tags = {
  Solution-Name = "Azure Virtual Desktop"
  Billing-Code  = "DG1318"
  Solution-ID   = "APM0003362"
  Expiry-Date   = "2030-12-31T23:59:00Z"
  Environment   = "--env--"
  Git-Repo      = "https://"
}

# VM Provisioning Flag
run-provisioning = true

# Boot Diagnostics Settings
boot-diag-storage-account-name = "az-vdi-chinww-boot"

# VM Settings
number-of-vms  = 1
vm-name-prefix = "AZ-VDI-CHINWW"
vm-size        = "Standard_D2s_v3"

# Hostpool Settings
hostpool-name = "AZ-CL-VDI-PERSONAL"

dag-name               = "AZ-CL-VDI-PERSONAL"
dag-friendly-name      = "AZ-CL-VDI-PERSONAL"
dag-description        = "CL Personal VDI"
dag-ws-name            = "az-cl-vdi-personal"
dag-ws-friendly-name   = "CL Virtual Desktop Workspace"
dag-ws-description     = "az-cl-vdi-personal workspace"

os-disk-size-gb = 130

dag-user-list = [
  ""
]


# VM Source Image Details
source-image-reference = {
  publisher = "MicrosoftWindowsServer"
  offer     = "WindowsServer"
  sku       = "2022-datacenter-azure-edition"
  version   = "latest"
}
