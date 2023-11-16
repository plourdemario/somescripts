####################################
# Azure Generic Settings
####################################
variable "location" {
  description = "Azure location example CanadaCentral"
  type        = string
  default     = "CanadaCentral"
}

variable "environment-flag" {
  type        = string
  default     = "PRD"
}

variable "spoke-name" {
  description = "Name of the Spoke"
  type        = string
}

variable "spoke-spn" {
  description = "Name of the Spoke Service Principal"
  type        = string
}

variable "subnet-address-space" {
  description = "subnet cidr block"
  type        = string
  default     = "10.56.56.0/22"
}

variable "workload-name" {
  description = "Workload Name"
  type        = string
}
variable "workload-rg-name" {
  description = "Workload Name"
  type        = string
  default     = "AZ-CL-VDI-PERSONAL-WORKLOAD-RG"
}
variable "workload-subnet-id" {
  description = "Workload Name"
  type        = string
  default     = "AZ-CL-VDI-PERSONAL-WORKLOAD-RG"
}
variable "workload-nsg-name" {
  description = "Workload NSG Name"
  type        = string
  default     = "AZ-CL-VDI-PERSONAL-WORKLOAD-NSG"
}
variable "spoke-vnet-name" {
  description = "spoke-vnet-name"
  type        = string
  default     = "SPOKE-VDI-PRD-VNT"
}
variable "spoke-vnet-address-space" {
  description = "spoke-vnet-address-space"
  type        = list(string)
  default     = ["10.56.48.0/20"]
}
variable "tags" {
  type = map(any)
}

variable "run-provisioning" {
  description = "Determines whether to run the Provisioning Job for the vms"
  type        = bool
  default     = true
}

########################################
# Boot Diagnostics Configuration Section
########################################

variable "boot-diag-storage-account-name" {
  type        = string
}

####################################
# WVD Hostpool Configuration Section
####################################

variable "hostpool-name" {
  type    = string
}

variable "hostpool-type" {
  type    = string
  default = "Personal"
}

variable "hostpool-max-sessions" {
  type    = number
  default = 999999
}

variable "hostpool-location" {
  type    = string
  default = "Canada Central"
}

variable "hostpool-lb-type" {
  type    = string
  default = "BreadthFirst"
}

variable "hostpool-friendly-name" {
  type    = string
  default = null
}

variable "hostpool-description" {
  type    = string
  default = null
}

variable "hostpool-validate-environment" {
  type    = bool
  default = false
}

variable "hostpool-rdp-properties" {
  type    = string
  default = "drivestoredirect:s:;audiomode:i:0;videoplaybackmode:i:1;redirectclipboard:i:0;redirectprinters:i:0;devicestoredirect:s:;redirectcomports:i:1;redirectsmartcards:i:0;usbdevicestoredirect:s:;enablecredsspsupport:i:1;use multimon:i:1;audiocapturemode:i:1;encode redirected video capture:i:1;redirected video capture encoding quality:i:2;camerastoredirect:s:*;"
}

variable "hostpool-pd-assign-type" {
  type    = string
  default = null
}

variable "hostpool-preferred-app-group-type" {
  type    = string
  default = null
}
##########################
# VM configuration section
##########################

variable "number-of-vms" {
  description = "Number of VMs to create"
  type        = number
}

variable "vm-name-count-offset" {
  description = "Number to start creating VMs at"
  type        = number
  default     = 1
}

variable "vm-name-prefix" {
  description = "First part of the VM name.  Will be appended with numeric variable"
  type        = string
}

variable "vm-size" {
  description = "Azure VM Size, ex Standard_B2ms"
  type        = string
}

variable "data-disks" {
  type = list(object({
    diskname             = string
    storage-account-type = string
    create-option        = string
    mountPoint           = string
    disk-size-gb         = number
    lun                  = number
  }))
  default = []
}

variable "source-image-reference" {
  description = "Plan required for the markeplace agreement"
  type = object({
    publisher   = string
    offer       = string
    sku         = string
    version     = string
  })
  default = {
    publisher   = "MicrosoftWindowsDesktop"
    offer       = "Windows-10"
    sku         = "win10-21h2-avd"
    version     = "latest"
  }
}

variable "install_french" {
  description = "Set to true in order to set French as the Default Language for the Hosts"
  type        = bool
  default     = false
}

variable "timezone" {
  description = "OS Timezone example Central Standard Time"
  type        = string
  default     = "Central Standard Time"
}

variable "os-disk-storage-account-type" {
  description = "Defines the type of storage account to be created. Valid options are Standard_LRS, Standard_ZRS, Standard_GRS, Standard_RAGRS, Premium_LRS."
  type        = string
  default     = "Premium_LRS"
}

variable "os-disk-size-gb" {
  description = "OS Disk Size"
  type        = number
  default     = 127
}

variable "enable-automatic-updates" {
  type        = bool
  default     = true
}

variable "plan" {
  description = "Plan required for the markeplace agreement"
  type = object( {
    product   = string
    publisher = string
    name      = string
  })
  default = {
    product   = ""
    publisher = ""
    name      = ""  
  }
}

###################################
# Desktop application group section
###################################

variable "create-desktop-app-group" {
  type        = bool
  default     = false
}

variable "dag-user-list" {
  type        = list(string)
  default     = []
}

variable "dag-group-list" {
  type        = list(string)
  default     = []
}

variable "dag-name" {
  type        = string
  default     = ""
}

variable "dag-friendly-name" {
  type        = string
  default     = ""
}

variable "dag-description" {
  type        = string
  default     = ""
}

variable "dag-ws-name" {
  type        = string
  default     = ""
}

variable "dag-ws-friendly-name" {
  type        = string
  default     = ""
}

variable "dag-ws-description" {
  type        = string
  default     = ""
}

#variable "rg-permissions-group" {
#  type        = list(string)
#  default     = ["DLG_AZURE_D&C-CONTRIBUTORS"]
#}

###############################
# Domain Configurations Section
###############################

variable "vm-domain" {
  type        = string
  default     = "gwl.bz"
}

variable "vm-domain-oupath" {
  type        = string
  default     = "OU=AZURE,OU=Windows 10 Live,OU=Workstations,DC=gwl,DC=bz"
}

#######################################
# HashiCorp Vault Configuration Section
#######################################

variable "VAULT_TOKEN" {
  default = ""
}