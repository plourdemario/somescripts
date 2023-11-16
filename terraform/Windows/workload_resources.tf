data "azurerm_subnet" "workload-subnet" {
    name                      = "AZ-CL-VDI-PERSONAL-WORKLOAD-SUBNET"
    resource_group_name       = "SPOKE-VDI-PRD-RG"
    virtual_network_name      = var.spoke-vnet-name
}